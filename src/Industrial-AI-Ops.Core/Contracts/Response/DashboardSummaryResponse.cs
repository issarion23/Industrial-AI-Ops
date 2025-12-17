namespace Industrial_AI_Ops.Core.Contracts.Response;

public class DashboardSummaryResponse
{
    public int TotalEquipment { get; set; }
    public EquipmentStatusResponse EquipmentStatus { get; set; }

    public int CriticalAlerts { get; set; }

    public double AverageHealthScore { get; set; }

    public DateTime Timestamp { get; set; }
}

public class EquipmentStatusResponse
{
    public int Operational { get; set; }
    public int Warning { get; set; }
    public int Critical { get; set; }
    public int Offline { get; set; }
}