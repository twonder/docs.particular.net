namespace Snippets6.Conventions
{
    using System;
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region MessageConventions

            EndpointConfiguration configuration = new EndpointConfiguration();
            ConventionsBuilder conventions = configuration.Conventions();
            conventions.DefiningCommandsAs(t => t.Namespace == "MyNamespace.Messages.Commands");
            conventions.DefiningEventsAs(t => t.Namespace == "MyNamespace.Messages.Events");
            conventions.DefiningMessagesAs(t => t.Namespace == "MyNamespace.Messages");
            conventions.DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"));
            conventions.DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"));
            conventions.DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"));
            conventions.DefiningTimeToBeReceivedAs(t =>
                t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(30) : TimeSpan.MaxValue);

            #endregion
        }
    }
}