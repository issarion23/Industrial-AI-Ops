using Industrial_AI_Ops.Core.Contracts;
using Industrial_AI_Ops.Core.Models;
using Mapster;

namespace Industrial_AI_Ops.Core.Common.Mapper;

public class RegisterMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EquipmentDto, Equipment>()
            .RequireDestinationMemberSource(true);
    }
}