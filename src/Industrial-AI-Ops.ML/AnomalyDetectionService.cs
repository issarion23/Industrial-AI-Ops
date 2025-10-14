using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.ML.Models;
using Industrial_AI_Ops.ML.Models.Results;
using Microsoft.ML;

namespace Industrial_AI_Ops.ML;

public class AnomalyDetectionService
{
    private readonly MLContext _mlContext;
    private ITransformer? _pumpModel;
    private ITransformer? _compressorModel;
    private ITransformer? _turbineModel;
    private PredictionEngine<PumpMlInput, AnomalyPrediction>? _pumpEngine;
    private PredictionEngine<CompressorMlInput, AnomalyPrediction>? _compressorEngine;
    private PredictionEngine<TurbineMlInput, AnomalyPrediction>? _turbineEngine;

    public AnomalyDetectionService()
    {
        _mlContext = new MLContext(seed: 42);
    }

    public void LoadModels(ITransformer pumpModel, ITransformer compressorModel, ITransformer turbineModel)
    {
        _pumpModel = pumpModel;
        _compressorModel = compressorModel;
        _turbineModel = turbineModel;

        _pumpEngine = _mlContext.Model.CreatePredictionEngine<PumpMlInput, AnomalyPrediction>(_pumpModel);
        _compressorEngine = _mlContext.Model.CreatePredictionEngine<CompressorMlInput, AnomalyPrediction>(_compressorModel);
        _turbineEngine = _mlContext.Model.CreatePredictionEngine<TurbineMlInput, AnomalyPrediction>(_turbineModel);
    }

    public async Task<AnomalyResult> DetectPumpAnomalyAsync(PumpSensorData data)
    {
        if (_pumpEngine == null)
            throw new InvalidOperationException("Pump model not loaded");

        var input = new PumpMlInput
        {
            SuctionPressure = (float)(data.SuctionPressure ?? 0),
            DischargePressure = (float)(data.DischargePressure ?? 0),
            FlowRate = (float)(data.FlowRate ?? 0),
            Temperature = (float)(data.Temperature ?? 0),
            BearingTemperature = (float)(data.BearingTemperature ?? 0),
            VibrationX = (float)(data.VibrationX ?? 0),
            VibrationY = (float)(data.VibrationY ?? 0),
            VibrationZ = (float)(data.VibrationZ ?? 0),
            MotorCurrent = (float)(data.MotorCurrent ?? 0),
            PowerConsumption = (float)(data.PowerConsumption ?? 0),
            Rpm = (float)(data.Rpm ?? 0),
            Efficiency = (float)(data.Efficiency ?? 0)
        };

        var prediction = _pumpEngine.Predict(input);

        return await Task.FromResult(new AnomalyResult
        {
            IsAnomaly = prediction.IsAnomaly,
            AnomalyScore = prediction.AnomalyScore,
            Confidence = Math.Abs(prediction.AnomalyScore),
            FeatureImportance = CalculatePumpFeatureImportance(input),
            AnomalyType = DeterminePumpAnomalyType(input),
            Recommendation = GeneratePumpRecommendation(input, prediction.IsAnomaly)
        });
    }

    public async Task<AnomalyResult> DetectCompressorAnomalyAsync(CompressorSensorData data)
    {
        if (_compressorEngine == null)
            throw new InvalidOperationException("Compressor model not loaded");

        var input = new CompressorMlInput
        {
            InletPressure = (float)(data.InletPressure ?? 0),
            OutletPressure = (float)(data.OutletPressure ?? 0),
            InletTemperature = (float)(data.InletTemperature ?? 0),
            OutletTemperature = (float)(data.OutletTemperature ?? 0),
            MassFlowRate = (float)(data.MassFlowRate ?? 0),
            VibrationAxial = (float)(data.VibrationAxial ?? 0),
            VibrationRadial = (float)(data.VibrationRadial ?? 0),
            BearingTemp1 = (float)(data.BearingTemp1 ?? 0),
            BearingTemp2 = (float)(data.BearingTemp2 ?? 0),
            PowerConsumption = (float)(data.PowerConsumption ?? 0),
            Rpm = (float)(data.Rpm ?? 0),
            LubOilPressure = (float)(data.LubOilPressure ?? 0),
            SurgeMargin = (float)(data.SurgeMargin ?? 0)
        };

        var prediction = _compressorEngine.Predict(input);

        return await Task.FromResult(new AnomalyResult
        {
            IsAnomaly = prediction.IsAnomaly,
            AnomalyScore = prediction.AnomalyScore,
            Confidence = Math.Abs(prediction.AnomalyScore),
            FeatureImportance = CalculateCompressorFeatureImportance(input),
            AnomalyType = DetermineCompressorAnomalyType(input),
            Recommendation = GenerateCompressorRecommendation(input, prediction.IsAnomaly)
        });
    }

    public async Task<AnomalyResult> DetectTurbineAnomalyAsync(TurbineSensorData data)
    {
        if (_turbineEngine == null)
            throw new InvalidOperationException("Turbine model not loaded");

        var input = new TurbineMlInput
        {
            InletPressure = (float)(data.InletPressure ?? 0),
            InletTemperature = (float)(data.InletTemperature ?? 0),
            ExhaustTemperature = (float)(data.ExhaustTemperature ?? 0),
            FuelGasFlowRate = (float)(data.FuelGasFlowRate ?? 0),
            PowerOutput = (float)(data.PowerOutput ?? 0),
            Rpm = (float)(data.Rpm ?? 0),
            VibrationBearing1 = (float)(data.VibrationBearing1 ?? 0),
            VibrationBearing2 = (float)(data.VibrationBearing2 ?? 0),
            BearingTemp1 = (float)(data.BearingTemp1 ?? 0),
            BearingTemp2 = (float)(data.BearingTemp2 ?? 0),
            LubOilPressure = (float)(data.LubOilPressure ?? 0),
            ThermalEfficiency = (float)(data.ThermalEfficiency ?? 0),
            NOxEmission = (float)(data.NOxEmission ?? 0)
        };

        var prediction = _turbineEngine.Predict(input);

        return await Task.FromResult(new AnomalyResult
        {
            IsAnomaly = prediction.IsAnomaly,
            AnomalyScore = prediction.AnomalyScore,
            Confidence = Math.Abs(prediction.AnomalyScore),
            FeatureImportance = CalculateTurbineFeatureImportance(input),
            AnomalyType = DetermineTurbineAnomalyType(input),
            Recommendation = GenerateTurbineRecommendation(input, prediction.IsAnomaly)
        });
    }

    private Dictionary<string, float> CalculatePumpFeatureImportance(PumpMlInput input)
    {
        return new Dictionary<string, float>
        {
            { "vibration", (input.VibrationX + input.VibrationY + input.VibrationZ) / 3 },
            { "bearing_temperature", input.BearingTemperature },
            { "pressure_differential", input.DischargePressure - input.SuctionPressure },
            { "efficiency", input.Efficiency }
        };
    }

    private Dictionary<string, float> CalculateCompressorFeatureImportance(CompressorMlInput input)
    {
        return new Dictionary<string, float>
        {
            { "surge_margin", input.SurgeMargin },
            { "vibration", (input.VibrationAxial + input.VibrationRadial) / 2 },
            { "bearing_temp", (input.BearingTemp1 + input.BearingTemp2) / 2 },
            { "compression_ratio", input.OutletPressure / Math.Max(input.InletPressure, 0.1f) }
        };
    }

    private Dictionary<string, float> CalculateTurbineFeatureImportance(TurbineMlInput input)
    {
        return new Dictionary<string, float>
        {
            { "exhaust_temperature", input.ExhaustTemperature },
            { "vibration", (input.VibrationBearing1 + input.VibrationBearing2) / 2 },
            { "efficiency", input.ThermalEfficiency },
            { "emissions", input.NOxEmission }
        };
    }

    private string DeterminePumpAnomalyType(PumpMlInput input)
    {
        var avgVibration = (input.VibrationX + input.VibrationY + input.VibrationZ) / 3;
        if (avgVibration > 10) return "Bearing Failure Risk";
        if (input.BearingTemperature > 85) return "Overheating";
        if (input.Efficiency < 70) return "Performance Degradation";
        return "General Anomaly";
    }

    private string DetermineCompressorAnomalyType(CompressorMlInput input)
    {
        if (input.SurgeMargin < 10) return "Surge Risk";
        if (input.BearingTemp1 > 90 || input.BearingTemp2 > 90) return "Bearing Overheating";
        if (input.LubOilPressure < 2) return "Lubrication Issue";
        return "General Anomaly";
    }

    private string DetermineTurbineAnomalyType(TurbineMlInput input)
    {
        if (input.ExhaustTemperature > 650) return "Combustion Anomaly";
        if (input.VibrationBearing1 > 12 || input.VibrationBearing2 > 12) return "Mechanical Issue";
        if (input.ThermalEfficiency < 28) return "Efficiency Loss";
        return "General Anomaly";
    }

    private string GeneratePumpRecommendation(PumpMlInput input, bool isAnomaly)
    {
        if (!isAnomaly) return "Continue normal operation";
        
        var avgVibration = (input.VibrationX + input.VibrationY + input.VibrationZ) / 3;
        if (avgVibration > 10) return "Schedule bearing inspection within 7 days";
        if (input.BearingTemperature > 85) return "Reduce load and check cooling system";
        return "Investigate anomaly, consider maintenance";
    }

    private string GenerateCompressorRecommendation(CompressorMlInput input, bool isAnomaly)
    {
        if (!isAnomaly) return "Continue normal operation";
        
        if (input.SurgeMargin < 10) return "URGENT: Adjust operating point to avoid surge";
        if (input.BearingTemp1 > 90 || input.BearingTemp2 > 90) return "Check lubrication system immediately";
        return "Schedule inspection within 14 days";
    }

    private string GenerateTurbineRecommendation(TurbineMlInput input, bool isAnomaly)
    {
        if (!isAnomaly) return "Continue normal operation";
        
        if (input.ExhaustTemperature > 650) return "Check combustion system and fuel quality";
        if (input.VibrationBearing1 > 12) return "Schedule rotor inspection";
        return "Monitor closely, plan maintenance";
    }
}