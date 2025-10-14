namespace Industrial_AI_Ops.Core.Models;

public abstract class BaseSensorData
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}