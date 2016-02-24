﻿namespace Snippets4
{
    using NServiceBus;
    using NServiceBus.Unicast.Config;

    public class BasicUsageOfIBus
    {
        void Send()
        {
            #region BasicSend
            ConfigUnicastBus configUnicastBus = Configure.With().UnicastBus();
            IBus bus = configUnicastBus.CreateBus().Start();

            bus.Send(new MyMessage());
            #endregion
        }

        #region SendFromHandler

        public class MyMessageHandler : IHandleMessages<MyMessage>
        {
            IBus bus;

            public MyMessageHandler(IBus bus)
            {
                this.bus = bus;
            }

            public void Handle(MyMessage message)
            {
                bus.Send(new OtherMessage());
            }
        }
        #endregion

        void SendInterface()
        {
            IBus bus = null;

            #region BasicSendInterface
            bus.Send<IMyMessage>(m => m.MyProperty = "Hello world");
            #endregion
        }

        void SetDestination()
        {
            IBus bus = null;

            #region BasicSendSetDestination
            bus.Send(Address.Parse("MyDestination"), new MyMessage());
            #endregion
        }

        void ThisEndpoint()
        {
            IBus bus = null;

            #region BasicSendToAnyInstance
            bus.SendLocal(new MyMessage());
            #endregion
        }

        public class MyMessage
        {
        }

        public class OtherMessage
        {
        }

        interface IMyMessage
        {
            string MyProperty { get; set; }
        }
    }
}