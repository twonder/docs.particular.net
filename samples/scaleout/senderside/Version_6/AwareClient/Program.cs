﻿using System;
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
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region Logical-Routing

        endpointConfiguration.Routing()
            .UnicastRoutingTable
            .RouteToEndpoint(typeof(DoSomething), "Server");

        #endregion

        #region File-Based-Routing

        endpointConfiguration.Routing()
            .UseFileBasedEndpointInstanceMapping("routes.xml");

        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            await endpoint.Send(new DoSomething());
            Console.WriteLine("Message Sent");
        }
        await endpoint.Stop();
    }
}