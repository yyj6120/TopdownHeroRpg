using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace SJ.GameServer.DataAccess.RDB
{
    public static class ConnectionFactory
    {
        public static class ConnectionString
        {
            public static string Connection = ConfigurationManager.ConnectionStrings["InhaGame"].ConnectionString;
        }

        public static DbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString.Connection);
        }
    }
}
