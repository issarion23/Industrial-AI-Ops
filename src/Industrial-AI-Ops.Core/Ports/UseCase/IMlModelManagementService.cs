using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Contracts.Response;

namespace Industrial_AI_Ops.Core.Ports.UseCase;

public interface IMlModelManagementService
{
    Result<MlModelStatusResponse> GetModelsStatus();
    Task<Result> InitializeModels();
    Task<Result> RetrainModels();
}