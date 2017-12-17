using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;

namespace SJ.GameServer.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ServiceHost> hosts = new List<ServiceHost>();

            try
            {
                // 멀티코어 JIT 유효화
                System.Runtime.ProfileOptimization.SetProfileRoot(Environment.CurrentDirectory);
                System.Runtime.ProfileOptimization.StartProfile("App.JIT.Profile");

#if DEBUG
                var gameServerDllName = "Debug-SJ.GameServer";
#else
                var gameServerDllName = "SJ.GameServer";
#endif
                var section = ConfigurationManager.GetSection("system.serviceModel/services") as ServicesSection;
                foreach (ServiceElement element in section.Services)
                {
                    var serviceType = Type.GetType(element.Name + "," + gameServerDllName);
                    var host = new ServiceHost(serviceType);

                    host.Open();
                    hosts.Add(host);

                    foreach (ServiceEndpoint ep in host.Description.Endpoints)
                        Console.WriteLine("Endpoint Name: {0}, Address: {1}", ep.Name, ep.Address);
                }

                Console.WriteLine("\nService Start Time: " + DateTime.Now.ToString());
                Console.WriteLine("Press any key to quit.");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
                Console.WriteLine("*** Service Error ***");
            }
            finally
            {
                foreach (ServiceHost host in hosts)
                {
                    if (host.State == CommunicationState.Opened)
                        host.Close();
                    else
                        host.Abort();
                }
            }
        }
    }
}
