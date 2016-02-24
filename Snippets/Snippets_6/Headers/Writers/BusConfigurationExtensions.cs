namespace Snippets6.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NServiceBus;

    public static class BusConfigurationExtensions
    {
        public static void SetTypesToScan(this EndpointConfiguration busConfiguration, IEnumerable<Type> typesToScan)
        {
            MethodInfo methodInfo = typeof(EndpointConfiguration).GetMethod("TypesToScanInternal",BindingFlags.NonPublic|BindingFlags.Instance);
            methodInfo.Invoke(busConfiguration, new object[]
            {
                typesToScan
            });
        }
    }
}