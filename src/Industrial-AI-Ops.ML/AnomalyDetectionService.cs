using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Models.ML;
using Industrial_AI_Ops.Core.Models.ML.Results;
using Industrial_AI_Ops.Core.Ports;
using Industrial_AI_Ops.Core.Ports.ML;
using Microsoft.Extensions.Logging;
using Microsoft.ML;

namespace Industrial_AI_Ops.ML;

public class AnomalyDetectionService : IAnomalyDetectionService
{
    private readonly MLContext _mlContext;
    private readonly ILogger<AnomalyDetectionService> _logger;
    private ITransformer? _pumpModel;
    private ITransformer? _compressorModel;
    private ITransformer? _turbineModel;
    private ITransformer? _maintenanceModel;
    private PredictionEngine<PumpMlInput, AnomalyPrediction>? _pumpEngine;
    private PredictionEngine<CompressorMlInput, AnomalyPrediction>? _compressorEngine;
    private PredictionEngine<TurbineMlInput, AnomalyPrediction>? _turbineEngine;
    private PredictionEngine<MaintenanceInput, MaintenanceOutput>? _maintenanceEngine;

    public AnomalyDetectionService(ILogger<AnomalyDetectionService> logger)
    {
        _mlContext = new MLContext(seed: 42);
        _logger = logger;
    }

    public void LoadModels(
        ITransformer pumpModel, 
        ITransformer compressorModel, 
        ITransformer turbineModel, 
        ITransformer? maintenanceModel = null)
    {
        _pumpModel = pumpModel;
        _compressorModel = compressorModel;
        _turbineModel = turbineModel;
        _maintenanceModel = maintenanceModel;

        _pumpEngine = _mlContext.Model.CreatePredictionEngine<PumpMlInput, AnomalyPrediction>(_pumpModel);
        _compressorEngine = _mlContext.Model.CreatePredictionEngine<CompressorMlInput, AnomalyPrediction>(_compressorModel);
        _turbineEngine = _mlContext.Model.CreatePredictionEngine<TurbineMlInput, AnomalyPrediction>(_turbineModel);
        
        if (_maintenanceModel != null)
        {
            _maintenanceEngine = _mlContext.Model.CreatePredictionEngine<MaintenanceInput, MaintenanceOutput>(_maintenanceModel);
        }

        _logger.LogInformation("All prediction engines created successfully");
    }

    public async Task<AnomalyResult> DetectPumpAnomalyAsync(PumpSensorData data)
    {
        if (_pumpEngine == null)
            throw new InvalidOperationException("Pump model not loaded");

        try
        {
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
                Confidence = CalculateConfidence(prediction.AnomalyScore),
                FeatureImportance = CalculatePumpFeatureImportance(input),
                AnomalyType = DeterminePumpAnomalyType(input, prediction.IsAnomaly),
                Recommendation = GeneratePumpRecommendation(input, prediction.IsAnomaly)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting pump anomaly for equipment {EquipmentId}", data.EquipmentId);
            throw;
        }
    }

    public async Task<AnomalyResult> DetectCompressorAnomalyAsync(CompressorSensorData data)
    {
        if (_compressorEngine == null)
            throw new InvalidOperationException("Compressor model not loaded");

        try
        {
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
                Confidence = CalculateConfidence(prediction.AnomalyScore),
                FeatureImportance = CalculateCompressorFeatureImportance(input),
                AnomalyType = DetermineCompressorAnomalyType(input, prediction.IsAnomaly),
                Recommendation = GenerateCompressorRecommendation(input, prediction.IsAnomaly)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting compressor anomaly for equipment {EquipmentId}", data.EquipmentId);
            throw;
        }
    }

    public async Task<AnomalyResult> DetectTurbineAnomalyAsync(TurbineSensorData data)
    {
        if (_turbineEngine == null)
            throw new InvalidOperationException("Turbine model not loaded");

        try
        {
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
                Confidence = CalculateConfidence(prediction.AnomalyScore),
                FeatureImportance = CalculateTurbineFeatureImportance(input),
                AnomalyType = DetermineTurbineAnomalyType(input, prediction.IsAnomaly),
                Recommendation = GenerateTurbineRecommendation(input, prediction.IsAnomaly)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting turbine anomaly for equipment {EquipmentId}", data.EquipmentId);
            throw;
        }
    }

    public async Task<MaintenancePredictionResult> PredictMaintenanceAsync(MaintenanceInput input)
    {
        if (_maintenanceEngine == null)
            throw new InvalidOperationException("Maintenance model not loaded");

        try
        {
            var prediction = _maintenanceEngine.Predict(input);

            var riskLevel = DetermineRiskLevel(prediction.DaysToFailure);
            var priority = DeterminePriority(prediction.DaysToFailure, input.AnomalyScore);

            return await Task.FromResult(new MaintenancePredictionResult
            {
                DaysToFailure = Math.Max(0, prediction.DaysToFailure),
                RiskLevel = riskLevel,
                Priority = priority,
                RecommendedAction = GenerateMaintenanceRecommendation(prediction.DaysToFailure, riskLevel),
                ConfidenceScore = CalculateMaintenanceConfidence(input)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error predicting maintenance");
            throw;
        }
    }

    // Feature Importance Calculations
    private Dictionary<string, float> CalculatePumpFeatureImportance(PumpMlInput input)
    {
        var avgVibration = (input.VibrationX + input.VibrationY + input.VibrationZ) / 3;
        var pressureDiff = input.DischargePressure - input.SuctionPressure;

        return new Dictionary<string, float>
        {
            { "vibration", NormalizeImportance(avgVibration, 0, 15) },
            { "bearing_temperature", NormalizeImportance(input.BearingTemperature, 50, 100) },
            { "pressure_differential", NormalizeImportance(pressureDiff, 0, 20) },
            { "efficiency", NormalizeImportance(input.Efficiency, 50, 100) },
            { "temperature", NormalizeImportance(input.Temperature, 40, 90) },
            { "motor_current", NormalizeImportance(input.MotorCurrent, 30, 80) }
        };
    }

    private Dictionary<string, float> CalculateCompressorFeatureImportance(CompressorMlInput input)
    {
        var avgVibration = (input.VibrationAxial + input.VibrationRadial) / 2;
        var avgBearingTemp = (input.BearingTemp1 + input.BearingTemp2) / 2;
        var compressionRatio = input.OutletPressure / Math.Max(input.InletPressure, 0.1f);

        return new Dictionary<string, float>
        {
            { "surge_margin", NormalizeImportance(input.SurgeMargin, 0, 40) },
            { "vibration", NormalizeImportance(avgVibration, 0, 12) },
            { "bearing_temp", NormalizeImportance(avgBearingTemp, 50, 100) },
            { "compression_ratio", NormalizeImportance(compressionRatio, 1, 8) },
            { "lub_oil_pressure", NormalizeImportance(input.LubOilPressure, 1, 6) },
            { "outlet_temperature", NormalizeImportance(input.OutletTemperature, 60, 120) }
        };
    }

    private Dictionary<string, float> CalculateTurbineFeatureImportance(TurbineMlInput input)
    {
        var avgVibration = (input.VibrationBearing1 + input.VibrationBearing2) / 2;
        var avgBearingTemp = (input.BearingTemp1 + input.BearingTemp2) / 2;

        return new Dictionary<string, float>
        {
            { "exhaust_temperature", NormalizeImportance(input.ExhaustTemperature, 400, 700) },
            { "vibration", NormalizeImportance(avgVibration, 0, 15) },
            { "efficiency", NormalizeImportance(input.ThermalEfficiency, 20, 40) },
            { "emissions", NormalizeImportance(input.NOxEmission, 20, 100) },
            { "bearing_temp", NormalizeImportance(avgBearingTemp, 60, 110) },
            { "lub_oil_pressure", NormalizeImportance(input.LubOilPressure, 2, 6) }
        };
    }

    // Anomaly Type Determination
    private string DeterminePumpAnomalyType(PumpMlInput input, bool isAnomaly)
    {
        if (!isAnomaly) return "Normal Operation";

        var avgVibration = (input.VibrationX + input.VibrationY + input.VibrationZ) / 3;
        
        if (avgVibration > 10) return "Bearing Failure Risk";
        if (input.BearingTemperature > 85) return "Overheating";
        if (input.Efficiency < 70) return "Performance Degradation";
        if (Math.Abs(input.DischargePressure - input.SuctionPressure) < 3) return "Low Pressure Differential";
        if (input.MotorCurrent > 70) return "High Motor Load";
        
        return "General Anomaly";
    }

    private string DetermineCompressorAnomalyType(CompressorMlInput input, bool isAnomaly)
    {
        if (!isAnomaly) return "Normal Operation";

        if (input.SurgeMargin < 10) return "Surge Risk";
        if (input.BearingTemp1 > 90 || input.BearingTemp2 > 90) return "Bearing Overheating";
        if (input.LubOilPressure < 2) return "Lubrication Issue";
        if (input.OutletTemperature > 110) return "High Discharge Temperature";
        if ((input.VibrationAxial + input.VibrationRadial) / 2 > 8) return "Excessive Vibration";
        
        return "General Anomaly";
    }

    private string DetermineTurbineAnomalyType(TurbineMlInput input, bool isAnomaly)
    {
        if (!isAnomaly) return "Normal Operation";

        if (input.ExhaustTemperature > 650) return "Combustion Anomaly";
        if (input.VibrationBearing1 > 12 || input.VibrationBearing2 > 12) return "Mechanical Issue";
        if (input.ThermalEfficiency < 28) return "Efficiency Loss";
        if (input.NOxEmission > 80) return "High Emissions";
        if (input.LubOilPressure < 2.5) return "Lubrication Problem";
        if (input.PowerOutput < 20) return "Low Power Output";
        
        return "General Anomaly";
    }

    // Recommendation Generation
    private string GeneratePumpRecommendation(PumpMlInput input, bool isAnomaly)
    {
        if (!isAnomaly) return "Continue normal operation. Monitor key parameters.";
        
        var avgVibration = (input.VibrationX + input.VibrationY + input.VibrationZ) / 3;
        
        if (avgVibration > 10) 
            return "URGENT: Schedule bearing inspection within 7 days. Risk of catastrophic failure.";
        if (input.BearingTemperature > 85) 
            return "Reduce load by 20% and check cooling system immediately. Schedule maintenance.";
        if (input.Efficiency < 70) 
            return "Performance degradation detected. Check impeller wear and seal condition.";
        if (input.MotorCurrent > 70) 
            return "High motor current detected. Check for mechanical binding or electrical issues.";
        
        return "Investigate anomaly. Consider scheduling preventive maintenance within 14 days.";
    }

    private string GenerateCompressorRecommendation(CompressorMlInput input, bool isAnomaly)
    {
        if (!isAnomaly) return "Continue normal operation. Monitor surge margin.";
        
        if (input.SurgeMargin < 10) 
            return "CRITICAL: Adjust operating point immediately to avoid surge. Risk of damage.";
        if (input.BearingTemp1 > 90 || input.BearingTemp2 > 90) 
            return "Check lubrication system and bearing condition immediately. Schedule inspection.";
        if (input.LubOilPressure < 2) 
            return "Low lubrication pressure detected. Check oil level and pump operation urgently.";
        if (input.OutletTemperature > 110) 
            return "High discharge temperature. Check intercooler performance and reduce load.";
        
        return "Schedule comprehensive inspection within 14 days. Monitor closely.";
    }

    private string GenerateTurbineRecommendation(TurbineMlInput input, bool isAnomaly)
    {
        if (!isAnomaly) return "Continue normal operation. Monitor exhaust temperature.";
        
        if (input.ExhaustTemperature > 650) 
            return "Check combustion system, fuel quality, and turbine blades. Schedule hot section inspection.";
        if (input.VibrationBearing1 > 12 || input.VibrationBearing2 > 12) 
            return "URGENT: Schedule rotor inspection. Check alignment and bearing condition.";
        if (input.ThermalEfficiency < 28) 
            return "Efficiency loss detected. Check for fouling, leaks, or degraded components.";
        if (input.NOxEmission > 80) 
            return "High emissions detected. Check combustion tuning and fuel injection system.";
        if (input.LubOilPressure < 2.5) 
            return "Low oil pressure. Check lubrication system immediately to prevent bearing damage.";
        
        return "Monitor closely and plan maintenance within 21 days.";
    }

    private string GenerateMaintenanceRecommendation(float daysToFailure, string riskLevel)
    {
        return riskLevel switch
        {
            "Critical" => $"URGENT: Equipment failure predicted within {(int)daysToFailure} days. Schedule immediate maintenance.",
            "High" => $"High risk: Schedule maintenance within {(int)daysToFailure} days to prevent failure.",
            "Medium" => $"Moderate risk: Plan maintenance within {(int)daysToFailure} days. Continue monitoring.",
            "Low" => $"Low risk: Routine maintenance recommended within {(int)daysToFailure} days.",
            _ => "Continue normal operation with regular monitoring."
        };
    }

    // Helper Methods
    private float CalculateConfidence(float anomalyScore)
    {
        return Math.Min(100, Math.Abs(anomalyScore) * 10);
    }

    private float CalculateMaintenanceConfidence(MaintenanceInput input)
    {
        var dataQuality = (input.HealthScore > 0 ? 25f : 0) +
                         (input.AnomalyScore >= 0 ? 25f : 0) +
                         (input.DaysSinceLastMaintenance > 0 ? 25f : 0) +
                         (input.OperatingHours > 0 ? 25f : 0);
        return dataQuality;
    }

    private float NormalizeImportance(float value, float min, float max)
    {
        return Math.Clamp((value - min) / (max - min) * 100, 0, 100);
    }

    private string DetermineRiskLevel(float daysToFailure)
    {
        return daysToFailure switch
        {
            < 7 => "Critical",
            < 30 => "High",
            < 90 => "Medium",
            _ => "Low"
        };
    }

    private string DeterminePriority(float daysToFailure, float anomalyScore)
    {
        if (daysToFailure < 7 || anomalyScore > 8) return "Urgent";
        if (daysToFailure < 30 || anomalyScore > 5) return "High";
        if (daysToFailure < 90) return "Medium";
        return "Low";
    }

    public bool AreModelsLoaded()
    {
        return _pumpEngine != null && _compressorEngine != null && _turbineEngine != null;
    }
}

