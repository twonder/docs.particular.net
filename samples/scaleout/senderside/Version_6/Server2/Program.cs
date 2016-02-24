﻿using System;
using System.Configuration;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.EndpointName("Server");
        string discriminator = ConfigurationManager.AppSettings["InstanceId"];
        endpointConfiguration.ScaleOut().InstanceDiscriminator(discriminator);
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
        await endpoint.Stop();
    }
}