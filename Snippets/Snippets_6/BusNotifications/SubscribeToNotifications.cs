﻿// ReSharper disable UnusedParameter.Local
namespace Snippets6.BusNotifications
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Faults;

    #region SubscribeToErrorsNotifications

    public class SubscribeToNotifications :
        IWantToRunWhenBusStartsAndStops
    {
        BusNotifications busNotifications;

        public SubscribeToNotifications(BusNotifications busNotifications)
        {
            this.busNotifications = busNotifications;
        }

        public Task Start(IMessageSession session)
        {
            ErrorsNotifications errors = busNotifications.Errors;
            errors.MessageHasBeenSentToSecondLevelRetries += (sender, retry) => LogToConsole(retry);
            errors.MessageHasFailedAFirstLevelRetryAttempt += (sender, retry) => LogToConsole(retry);
            errors.MessageSentToErrorQueue += (sender, retry) => LogToConsole(retry);
            return Task.FromResult(0);
        }

        void LogToConsole(FailedMessage failedMessage)
        {
            Console.WriteLine("Mesage sent to error queue");
        }

        void LogToConsole(SecondLevelRetry secondLevelRetry)
        {
            Console.WriteLine("Mesage sent to SLR. RetryAttempt:" + secondLevelRetry.RetryAttempt);
        }

        void LogToConsole(FirstLevelRetry firstLevelRetry)
        {
            Console.WriteLine("Mesage sent to FLR. RetryAttempt:" + firstLevelRetry.RetryAttempt);
        }

        public Task Stop(IMessageSession session)
        {
            return Task.FromResult(0);
        }
    }

    #endregion
}

