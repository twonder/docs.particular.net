﻿namespace Snippets6
{
    using System;
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.Settings;
    using NServiceBus.Transports;

    public class TransportTransactions
    {
        public void Unreliable()
        {
            #region TransactionsDisable
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.None);
            #endregion
        }

        public void TransportTransactionReceiveOnly()
        {
            #region TransportTransactionReceiveOnly
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.ReceiveOnly);
            #endregion
        }

        public void TransportTransactionAtomicSendsWithReceive()
        {
            #region TransportTransactionAtomicSendsWithReceive
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.SendsAtomicWithReceive);
            #endregion
        }

        public void TransportTransactionScope()
        {
            #region TransportTransactionScope
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.TransactionScope);
            #endregion
        }

        public void TransportTransactionsWithScope()
        {
            #region TransactionsWrapHandlersExecutionInATransactionScope
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UnitOfWork()
                .WrapHandlersInATransactionScope();
            #endregion
        }

        public void CustomTransactionIsolationLevel()
        {
            #region CustomTransactionIsolationLevel
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(isolationLevel: IsolationLevel.RepeatableRead);
            #endregion
        }

        public void CustomTransactionTimeout()
        {
            #region CustomTransactionTimeout
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseTransport<MyTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(timeout: TimeSpan.FromSeconds(30));
            #endregion
        }
    }

    public class MyTransport : TransportDefinition
    {
        protected override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
        {
            throw new NotImplementedException();
        }

        public override string ExampleConnectionStringForErrorMessage { get; }
    }

    internal static class MyTransportConfigurationExtensions
    {
        public static void TransactionScopeOptions(this TransportExtensions<MyTransport> transportExtensions, TimeSpan? timeout = null, IsolationLevel? isolationLevel = null)
        {
        }
    }
}