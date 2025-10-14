using Industrial_AI_Ops.Core.Models;
using Industrial_AI_Ops.Core.Ports;
using Microsoft.ML;
using Industrial_AI_Ops.ML.Models;

namespace Industrial_AI_Ops.ML;

public class MlModelTrainer
{
    private readonly MLContext _mlContext;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MlModelTrainer> _logger;
    private readonly string _modelsPath;

    private ITransformer? _pumpModel;
    private ITransformer? _compressorModel;
    private ITransformer? _turbineModel;
    private ITransformer? _maintenanceModel;

    public MlModelTrainer(IServiceScopeFactory scopeFactory, IConfiguration config, ILogger<MlModelTrainer> logger)
    {
        _mlContext = new MLContext(seed: 42);
        _scopeFactory = scopeFactory;
        _logger = logger;
        _modelsPath = config["ModelsPath"] ?? "./models";
        
        Directory.CreateDirectory(_modelsPath);
    }
    
    public async Task InitializeAllModelsAsync()
    {
        _logger.LogInformation("Initializing ML models locally (no Azure required)...");

        try
        {
            // Попытка загрузить существующие модели
            if (ModelsExist())
            {
                LoadAllModels();
                _logger.LogInformation("Loaded existing models from disk");
            }
            else
            {
                // Обучить новые модели локально
                await TrainAllModelsAsync();
                _logger.LogInformation("Trained new models locally");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing models");
        }
    }
    
    private async Task TrainAllModelsAsync()
    {
        await TrainPumpModelAsync();
        await TrainCompressorModelAsync();
        await TrainTurbineModelAsync();
        await TrainMaintenanceModelAsync();
        SaveAllModels();
    }
    
     public async Task TrainPumpModelAsync()
    {
        _logger.LogInformation("Training Pump anomaly detection model locally...");

        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        // Получаем данные из базы
        var data = await context.PumpSensorData
            .OrderByDescending(d => d.Timestamp)
            .Take(50000)
            .ToListAsync();

        // Если данных мало, генерируем синтетические
        if (data.Count < 1000)
        {
            _logger.LogWarning("Insufficient data, generating synthetic training data");
            data = GenerateSyntheticPumpData(10000);
        }

        // Конвертируем в ML формат
        var mlData = _mlContext.Data.LoadFromEnumerable(data.Select(d => new PumpMlInput
        {
            SuctionPressure = (float)(d.SuctionPressure ?? 5.0),
            DischargePressure = (float)(d.DischargePressure ?? 10.0),
            FlowRate = (float)(d.FlowRate ?? 100.0),
            Temperature = (float)(d.Temperature ?? 60.0),
            BearingTemperature = (float)(d.BearingTemperature ?? 70.0),
            VibrationX = (float)(d.VibrationX ?? 3.0),
            VibrationY = (float)(d.VibrationY ?? 3.0),
            VibrationZ = (float)(d.VibrationZ ?? 3.0),
            MotorCurrent = (float)(d.MotorCurrent ?? 50.0),
            PowerConsumption = (float)(d.PowerConsumption ?? 100.0),
            Rpm = (float)(d.Rpm ?? 1500.0),
            Efficiency = (float)(d.Efficiency ?? 80.0)
        }));

        // Pipeline для обнаружения аномалий (RandomizedPCA - unsupervised)
        var pipeline = _mlContext.Transforms.Concatenate("Features",
                nameof(PumpMlInput.SuctionPressure),
                nameof(PumpMlInput.DischargePressure),
                nameof(PumpMlInput.FlowRate),
                nameof(PumpMlInput.Temperature),
                nameof(PumpMlInput.BearingTemperature),
                nameof(PumpMlInput.VibrationX),
                nameof(PumpMlInput.VibrationY),
                nameof(PumpMlInput.VibrationZ),
                nameof(PumpMlInput.MotorCurrent),
                nameof(PumpMlInput.PowerConsumption),
                nameof(PumpMlInput.Rpm),
                nameof(PumpMlInput.Efficiency))
            .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(_mlContext.AnomalyDetection.Trainers.RandomizedPca(
                featureColumnName: "Features",
                rank: 10,
                ensureZeroMean: true));

        // Обучение модели ЛОКАЛЬНО (на сервере TCO)
        _pumpModel = pipeline.Fit(mlData);

        _logger.LogInformation("Pump model trained successfully");
    }

    private async Task TrainCompressorModelAsync()
    {
        _logger.LogInformation("Training Compressor anomaly detection model locally...");

        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        var data = await context.CompressorSensorData
            .OrderByDescending(d => d.Timestamp)
            .Take(50000)
            .ToListAsync();

        if (data.Count < 1000)
        {
            data = GenerateSyntheticCompressorData(10000);
        }

        var mlData = _mlContext.Data.LoadFromEnumerable(data.Select(d => new CompressorMlInput
        {
            InletPressure = (float)(d.InletPressure ?? 1.5),
            OutletPressure = (float)(d.OutletPressure ?? 6.0),
            InletTemperature = (float)(d.InletTemperature ?? 30.0),
            OutletTemperature = (float)(d.OutletTemperature ?? 85.0),
            MassFlowRate = (float)(d.MassFlowRate ?? 1000.0),
            VibrationAxial = (float)(d.VibrationAxial ?? 4.0),
            VibrationRadial = (float)(d.VibrationRadial ?? 4.0),
            BearingTemp1 = (float)(d.BearingTemp1 ?? 75.0),
            BearingTemp2 = (float)(d.BearingTemp2 ?? 75.0),
            PowerConsumption = (float)(d.PowerConsumption ?? 500.0),
            Rpm = (float)(d.Rpm ?? 3000.0),
            LubOilPressure = (float)(d.LubOilPressure ?? 3.5),
            SurgeMargin = (float)(d.SurgeMargin ?? 20.0)
        }));

        var pipeline = _mlContext.Transforms.Concatenate("Features",
                nameof(CompressorMlInput.InletPressure),
                nameof(CompressorMlInput.OutletPressure),
                nameof(CompressorMlInput.InletTemperature),
                nameof(CompressorMlInput.OutletTemperature),
                nameof(CompressorMlInput.MassFlowRate),
                nameof(CompressorMlInput.VibrationAxial),
                nameof(CompressorMlInput.VibrationRadial),
                nameof(CompressorMlInput.BearingTemp1),
                nameof(CompressorMlInput.BearingTemp2),
                nameof(CompressorMlInput.PowerConsumption),
                nameof(CompressorMlInput.Rpm),
                nameof(CompressorMlInput.LubOilPressure),
                nameof(CompressorMlInput.SurgeMargin))
            .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(_mlContext.AnomalyDetection.Trainers.RandomizedPca(
                featureColumnName: "Features",
                rank: 10,
                ensureZeroMean: true));

        _compressorModel = pipeline.Fit(mlData);

        _logger.LogInformation("Compressor model trained successfully");
    }
    
    public async Task TrainTurbineModelAsync()
    {
        _logger.LogInformation("Training Turbine anomaly detection model locally...");

        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        var data = await context.TurbineSensorData
            .OrderByDescending(d => d.Timestamp)
            .Take(50000)
            .ToListAsync();

        if (data.Count < 1000)
        {
            data = GenerateSyntheticTurbineData(10000);
        }

        var mlData = _mlContext.Data.LoadFromEnumerable(data.Select(d => new TurbineMlInput
        {
            InletPressure = (float)(d.InletPressure ?? 15.0),
            InletTemperature = (float)(d.InletTemperature ?? 450.0),
            ExhaustTemperature = (float)(d.ExhaustTemperature ?? 550.0),
            FuelGasFlowRate = (float)(d.FuelGasFlowRate ?? 2000.0),
            PowerOutput = (float)(d.PowerOutput ?? 25.0),
            Rpm = (float)(d.Rpm ?? 5000.0),
            VibrationBearing1 = (float)(d.VibrationBearing1 ?? 5.0),
            VibrationBearing2 = (float)(d.VibrationBearing2 ?? 5.0),
            BearingTemp1 = (float)(d.BearingTemp1 ?? 80.0),
            BearingTemp2 = (float)(d.BearingTemp2 ?? 80.0),
            LubOilPressure = (float)(d.LubOilPressure ?? 4.0),
            ThermalEfficiency = (float)(d.ThermalEfficiency ?? 32.0)
        }));

        var pipeline = _mlContext.Transforms.Concatenate("Features",
                nameof(TurbineMlInput.InletPressure),
                nameof(TurbineMlInput.InletTemperature),
                nameof(TurbineMlInput.ExhaustTemperature),
                nameof(TurbineMlInput.FuelGasFlowRate),
                nameof(TurbineMlInput.PowerOutput),
                nameof(TurbineMlInput.Rpm),
                nameof(TurbineMlInput.VibrationBearing1),
                nameof(TurbineMlInput.VibrationBearing2),
                nameof(TurbineMlInput.BearingTemp1),
                nameof(TurbineMlInput.BearingTemp2),
                nameof(TurbineMlInput.LubOilPressure),
                nameof(TurbineMlInput.ThermalEfficiency))
            .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(_mlContext.AnomalyDetection.Trainers.RandomizedPca(
                featureColumnName: "Features",
                rank: 10,
                ensureZeroMean: true));

        _turbineModel = pipeline.Fit(mlData);

        _logger.LogInformation("Turbine model trained successfully");
    }

    public async Task TrainMaintenanceModelAsync()
    {
        _logger.LogInformation("Training Maintenance prediction model locally...");

        // Генерируем синтетические данные для обучения предсказания отказов
        var trainingData = GenerateMaintenanceTrainingData(5000);

        var mlData = _mlContext.Data.LoadFromEnumerable(trainingData);

        // Regression для предсказания дней до отказа
        var pipeline = _mlContext.Transforms.Concatenate("Features",
                nameof(MaintenanceInput.HealthScore),
                nameof(MaintenanceInput.AnomalyScore),
                nameof(MaintenanceInput.DaysSinceLastMaintenance),
                nameof(MaintenanceInput.AvgVibration),
                nameof(MaintenanceInput.AvgTemperature),
                nameof(MaintenanceInput.OperatingHours))
            .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(_mlContext.Regression.Trainers.FastTree(
                labelColumnName: nameof(MaintenanceOutput.DaysToFailure),
                featureColumnName: "Features",
                numberOfLeaves: 20,
                numberOfTrees: 100,
                minimumExampleCountPerLeaf: 10,
                learningRate: 0.2));

        _maintenanceModel = pipeline.Fit(mlData);

        _logger.LogInformation("Maintenance model trained successfully");

        await Task.CompletedTask;
    }

    public void SaveAllModels()
    {
        _logger.LogInformation($"Saving models to {_modelsPath}...");

        if (_pumpModel != null)
            _mlContext.Model.Save(_pumpModel, null, Path.Combine(_modelsPath, "pump_model.zip"));

        if (_compressorModel != null)
            _mlContext.Model.Save(_compressorModel, null, Path.Combine(_modelsPath, "compressor_model.zip"));

        if (_turbineModel != null)
            _mlContext.Model.Save(_turbineModel, null, Path.Combine(_modelsPath, "turbine_model.zip"));

        if (_maintenanceModel != null)
            _mlContext.Model.Save(_maintenanceModel, null, Path.Combine(_modelsPath, "maintenance_model.zip"));

        _logger.LogInformation("All models saved successfully");
    }

    public void LoadAllModels()
    {
        _logger.LogInformation($"Loading models from {_modelsPath}...");

        _pumpModel = _mlContext.Model.Load(Path.Combine(_modelsPath, "pump_model.zip"), out _);
        _compressorModel = _mlContext.Model.Load(Path.Combine(_modelsPath, "compressor_model.zip"), out _);
        _turbineModel = _mlContext.Model.Load(Path.Combine(_modelsPath, "turbine_model.zip"), out _);
        _maintenanceModel = _mlContext.Model.Load(Path.Combine(_modelsPath, "maintenance_model.zip"), out _);

        _logger.LogInformation("All models loaded successfully");
    }

    private bool ModelsExist()
    {
        return File.Exists(Path.Combine(_modelsPath, "pump_model.zip")) &&
               File.Exists(Path.Combine(_modelsPath, "compressor_model.zip")) &&
               File.Exists(Path.Combine(_modelsPath, "turbine_model.zip")) &&
               File.Exists(Path.Combine(_modelsPath, "maintenance_model.zip"));
    }

    public ITransformer? GetPumpModel() => _pumpModel;
    public ITransformer? GetCompressorModel() => _compressorModel;
    public ITransformer? GetTurbineModel() => _turbineModel;
    public ITransformer? GetMaintenanceModel() => _maintenanceModel;

    // Synthetic data generation
    private List<PumpSensorData> GenerateSyntheticPumpData(int count)
    {
        var random = new Random(42);
        var data = new List<PumpSensorData>();

        for (int i = 0; i < count; i++)
        {
            var isAnomaly = random.NextDouble() < 0.1;
            data.Add(new PumpSensorData
            {
                Id = i,
                EquipmentId = 1,
                Timestamp = DateTime.UtcNow.AddHours(-i),
                SuctionPressure = 5.0 + random.NextDouble() * 2 + (isAnomaly ? random.NextDouble() * 3 : 0),
                DischargePressure = 10.0 + random.NextDouble() * 2 + (isAnomaly ? random.NextDouble() * 4 : 0),
                FlowRate = 100.0 + random.NextDouble() * 20 + (isAnomaly ? random.NextDouble() * 30 : 0),
                Temperature = 60.0 + random.NextDouble() * 10 + (isAnomaly ? random.NextDouble() * 15 : 0),
                BearingTemperature = 70.0 + random.NextDouble() * 10 + (isAnomaly ? random.NextDouble() * 20 : 0),
                VibrationX = 3.0 + random.NextDouble() * 2 + (isAnomaly ? random.NextDouble() * 5 : 0),
                VibrationY = 3.0 + random.NextDouble() * 2 + (isAnomaly ? random.NextDouble() * 5 : 0),
                VibrationZ = 3.0 + random.NextDouble() * 2 + (isAnomaly ? random.NextDouble() * 5 : 0),
                MotorCurrent = 50.0 + random.NextDouble() * 10,
                PowerConsumption = 100.0 + random.NextDouble() * 20,
                Rpm = 1500.0 + random.NextDouble() * 100,
                Efficiency = 80.0 + random.NextDouble() * 10 - (isAnomaly ? random.NextDouble() * 20 : 0)
            });
        }

        return data;
    }
}