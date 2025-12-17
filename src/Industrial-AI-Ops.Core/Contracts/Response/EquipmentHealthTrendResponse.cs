namespace Industrial_AI_Ops.Core.Contracts.Response;

public class EquipmentHealthTrendResponse
{
    public string EquipmentId { get; set; }
    public string EquipmentName { get; set; }
    public double CurrentHealthScore { get; set; }
    public string Trend { get; set; }
    public int PeriodDays { get; set; }
    public DateTime Timestamp { get; set; }
}