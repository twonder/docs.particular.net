﻿namespace Snippets5.Transports.SqlServer
{
    using System;
    using NServiceBus;

    public class SqlServerConfigurationSettings
    {
        

        void CallbackReceiverMaxConcurrency()
        {
            #region sqlserver-CallbackReceiverMaxConcurrency 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .CallbackReceiverMaxConcurrency(10);

            #endregion
        }
        void TimeToWaitBeforeTriggeringCircuitBreaker()
        {
            #region sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .TimeToWaitBeforeTriggeringCircuitBreaker(TimeSpan.FromMinutes(3));

            #endregion
        }
        void PauseAfterReceiveFailure()
        {
            #region sqlserver-PauseAfterReceiveFailure 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .PauseAfterReceiveFailure(TimeSpan.FromSeconds(15));

            #endregion
        }

        void DisableSecondaries()
        {
            #region sqlserver-config-disable-secondaries 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .DisableCallbackReceiver();

            #endregion
        }

    }
}