using System.ComponentModel.DataAnnotations.Schema;

namespace Industrial_AI_Ops.Core.Models;

public abstract class BaseSensorData
{
    public string Id { get; set; }
    
    [ForeignKey(nameof(Equipment))]
    public string EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}