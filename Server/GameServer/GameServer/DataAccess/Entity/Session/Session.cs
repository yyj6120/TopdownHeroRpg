using System;

namespace SJ.GameServer.DataAccess.Entity
{
    [Serializable]
    public class Session : EntityBase
    {
        public long accountId;
        public string username;
        public string authToken;
    }
}
