using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using NServiceBus;

#region windowsservicehosting
namespace Snippets6.Host
{
    class ProgramService : ServiceBase
    {
        IEndpointInstance endpointInstance;

        static void Main()
        {
            using (ProgramService service = new ProgramService())
            {
                if (Environment.UserInteractive)
                {
                    service.OnStart(null);
                    Console.WriteLine("Bus created and configured");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                    service.OnStop();
                    return;
                }
                Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            AsyncOnStart().GetAwaiter().GetResult();
        }

        async Task AsyncOnStart()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.EnableInstallers();
            endpointInstance = await Endpoint.Start(configuration);
        }

        protected override void OnStop()
        {
            AsyncOnStop().GetAwaiter().GetResult();
        }

        async Task AsyncOnStop()
        {
            if (endpointInstance != null)
            {
                await endpointInstance.Stop();
            }
        }
    }
}
#endregion