using Industrial_AI_Ops.Core.Contracts.Response;

namespace Industrial_AI_Ops.Core.Ports.UseCase;

public interface IMlModelManagementService
{
    MlModelStatusResponse GetModelsStatus();
    Task InitializeModels();
    Task RetrainModels();
}