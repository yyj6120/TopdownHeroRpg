using SJ.GameServer.Service.ValueObjects;

namespace SJ.GameServer.DataAccess.Entity
{
    public class EntityMapper
    {
        public static void Initialize()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Account, AccountVo>();
            });
        }
    }
}
