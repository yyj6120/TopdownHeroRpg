using System;
using System.Linq;
using System.Reflection;
using SJ.GameServer.Utilities;
using SJ.GameServer.DataAccess.Entity;

namespace SJ.GameServer
{
    public class GameServer
    {
        public bool isWebHosted;

        public GameServer()
        {

        }

        public void Initialize()
        {
            try
            {
                isWebHosted = IsWebHosted();
                EntityMapper.Initialize();
            }
            catch (Exception ex)
            {
            }
        }

        private bool IsWebHosted()
        {
            try
            {
                var webAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("System.Web"));

                foreach (var webAssembly in webAssemblies)
                {
                    var hostingEnvironmentType = webAssembly.GetType("System.Web.Hosting.HostingEnvironment");
                    if (hostingEnvironmentType != null)
                    {
                        var isHostedProperty = hostingEnvironmentType.GetProperty("IsHosted",
                            BindingFlags.GetProperty | BindingFlags.Static | BindingFlags.Public);

                        if (isHostedProperty != null)
                        {
                            object result = isHostedProperty.GetValue(null, null);
                            if (result is bool)
                                return (bool)result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        static public GameServer Instance { get { return Singleton<GameServer>.Instance; } }
    }
}
