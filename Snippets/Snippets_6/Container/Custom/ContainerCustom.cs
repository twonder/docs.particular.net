﻿namespace Snippets6.Container.Custom
{
    using NServiceBus;
    using NServiceBus.Container;
    using NServiceBus.ObjectBuilder.Common;
    using NServiceBus.Settings;

    public class Usage
    {
        #region CustomContainers

        public void CustomContainerExtensionUsage()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();
            //Register the container in the configuration with '.UseContainer<T>()'
            configuration.UseContainer<MyContainer>();
        }

        // Create a class that implements 'ContainerDefinition' and returns your 'IContainer' implementation.
        public class MyContainer : ContainerDefinition
        {
            public override IContainer CreateContainer(ReadOnlySettings settings)
            {
                //Create a class that implements 'IContainer'
                return new MyObjectBuilder();
            }
        }

        #endregion
    }
}