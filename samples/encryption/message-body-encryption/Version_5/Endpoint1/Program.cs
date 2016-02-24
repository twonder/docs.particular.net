﻿using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.MessageBodyEncryption.Endpoint1");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        #region RegisterMessageEncryptor
        busConfiguration.RegisterMessageEncryptor();
        #endregion
        IStartableBus startableBus = Bus.Create(busConfiguration);
        using (IBus bus = startableBus.Start())
        {
            CompleteOrder completeOrder = new CompleteOrder
                                          {
                                              CreditCard = "123-456-789"
                                          };
            bus.Send("Samples.MessageBodyEncryption.Endpoint2", completeOrder);
            Console.WriteLine("Message sent");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}