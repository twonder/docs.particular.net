﻿using System;
using System.Threading.Tasks;
using NServiceBus;

#region Server-Handler
public class DoSomethingHandler : IHandleMessages<DoSomething>
{
    public Task Handle(DoSomething message, IMessageHandlerContext context)
    {
        Console.WriteLine("Message received.");
        return Task.FromResult(0);
    }
}
#endregion
