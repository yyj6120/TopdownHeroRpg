using System;
using System.Configuration;

namespace SJ.GameServer.Service
{
    public static class ServiceLocalDateTimeOffset
    {
        public static string serviceLocal;
        private const string timeFormat = "o";

        public static DateTimeOffset now
        {
            get
            {
                if (String.IsNullOrEmpty(serviceLocal))
                    serviceLocal = ConfigurationManager.AppSettings["ServiceLocal"];

                return DateTimeOffset.ParseExact(DateTimeOffset.Now.ToString(timeFormat), timeFormat, 
                    new System.Globalization.CultureInfo(serviceLocal, true), System.Globalization.DateTimeStyles.AssumeUniversal);
            }
        }
    }
}
