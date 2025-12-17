using Industrial_AI_Ops.Core.Models;

namespace Industrial_AI_Ops.Core.Contracts;

public abstract record BaseSensorDataDto
{
    public string Id { get; set; }
    public string EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}