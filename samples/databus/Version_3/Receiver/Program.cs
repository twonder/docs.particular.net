using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{

    static void Main()
    {
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.DataBus.Receiver");
        configure.DefaultBuilder();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        configure.FileShareDataBus("..\\..\\..\\storage");
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus
                .Start(() => configure.ForInstallationOn<Windows>().Install());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}