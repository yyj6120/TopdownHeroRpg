using System;

namespace SJ.GameServer.Service.ValueObjects
{
    [Serializable]
    public class AccountVo : ValueObjectBase
    {
        public long userId;
        public int gem;
    }
}
