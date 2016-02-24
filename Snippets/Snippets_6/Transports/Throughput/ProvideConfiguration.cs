﻿namespace Snippets6.Transports.Throughput
{
    using NServiceBus;

    class ConcurrencyConfiguration
    {
        void ConfigureFromCode()
        {
            #region TuningFromCode
            EndpointConfiguration configuration = new EndpointConfiguration();

            configuration.LimitMessageProcessingConcurrencyTo(5);
            #endregion
        }
        
    }
}