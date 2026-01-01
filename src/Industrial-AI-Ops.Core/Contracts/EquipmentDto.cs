using Industrial_AI_Ops.Core.Models;

namespace Industrial_AI_Ops.Core.Contracts;

public record EquipmentDto(
    string Id,
    string Name,
    EquipmentType Type,
    string Location,
    EquipmentStatus Status,
    DateTime InstallationDate,
    DateTime? LastMaintenanceDate,
    double HealthScore = 100.0
);