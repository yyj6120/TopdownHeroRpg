using System;

namespace SJ.GameServer.DataAccess.Entity
{
    public class Account : EntityBase
    {
        public long accountId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int gem { get; set; }
        public DateTimeOffset createdDate { get; set; }
    }
}
