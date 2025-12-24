using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Contracts.Response;

namespace Industrial_AI_Ops.Core.Ports.UseCase;

public interface IDashboardService
{
    Task<Result<DashboardSummaryResponse>> GetDashboardSummary();
}