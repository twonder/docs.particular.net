---
title: Multi-database support
summary: How to configure SQL Server transport to use multiple instances of the database and route messages between them.
tags:
- SQL Server
- Transports
---

The SQL Server transport allows you to select, on per-endpoint basis, where the table queues should be created. The selection can be done on multiple levels:

 * different schemas in a single database
 * different databases in a single SQL Server instance
 * different SQL Server instances

The transport will route messages to destination endpoints based on the configuration. If no specific configuration has been provided for a particular destination endpoint, the transport assumes the destination has the same configuration (schema, database and instance name/address) as the sending endpoint. If this assumption turns out to be false (the transport cannot connect to destination queue), an exception is thrown immediately. There is no store-and-forward mechanism on the transport level (and hence -- no dead-letter queue).

NOTE: If the destination endpoint uses different database or server instance, sending a message to it might cause the transaction to escalate to a distributed transaction. Usually it is not a desired effect and one can use NServiceBus Outbox to avoid it.

## Single database

By default SQL Server transport uses a single instance of the SQL Server to maintain all the queues for all endpoints of a system. In order to send a message, an endpoint needs to connect to the (usually remote) database server and execute a SQL command. The message is delivered directly to the destination queue without any store-and-forward mechanism. Such a simplistic approach can only be only used for small-to-medium size systems because of the need to store everything in a single database. Using schemas to distingiush logical data stores might be a good compromise for mid-size projects. The upside is it does not require Distributed Transaction Coordinator (MS DTC). Another advantage is the ability to take a snapshot of entire system state (all the queues) by backing up a database. This is even more useful if the business data are stored in the same database.


## Database-per-endpoint

In a more complex scenario endpoints use separate databases and DTC is involved. Due to lack of store-and-forward mechanism, if a remote endpoint's database or DTC infrastructure is down, the endpoint cannot send messages to it. This potentially renders the endpoint (and all endpoints transitively depending on it) unavailable.


## Database-per-endpoint with store-and-forward

In order to overcome this limitation a higher level store-and-forward mechanism needs to be used. The Outbox feature can be used to effectively implement a distributed decoupled architecture where:
 * Each endpoint has its own database where it stores both the queues and the user data
 * Messages are not sent immediately when calling `Bus.Send()` but are added to the *outbox* that is stored in the endpoint's own database. After completing the handling logic the messages in the *outbox* are forwarded to their destination databases
 * Should one of the forward operations fail, it will be retried by means of [standard NServiceBus retry mechanism](/nservicebus/errors/automatic-retries.md). This might result in some messages being sent more than once but it is not a problem because the outbox automatically handles the deduplication of incoming messages based on their ID.

## Current endpoint

SQL Server transport defaults to `dbo` schema and uses `NServiceBus/Transport` connection string from the configuration file to connect to the database. The default schema can be changed using following API

snippet:sqlserver-multidb-current-endpoint-schema

or by providing additional `Queue Schema` parameter in the connection string

snippet:sqlserver-multidb-current-endpoint-schema-config

The second approach has precedence over the first one.

The other parameters (database and instance name/address) can be changed in code using the connection string API

snippet:sqlserver-multidb-current-endpoint-connection-string

NOTE: `Queue Schema` parameter can also be used in the connection string provided via code.

NOTE: Starting with `V1.2.3` of the `SQL Server Transport` the `Queue Schema` parameter is supported only when used used in the connection string provided via code or via configuration.

NOTE: Unlike in the SQL Server transport, the connection string configuration API in NServiceBus core favors code over config which means that if you configure connection string both in `app.config` and via the `ConnectionString()` method, the latter will win.


## Other endpoints

If a particular remote endpoint requires customization of any part of the routing (schema, database or instance name/address), appropriate values have to be provided either via code or via configuration convention.


### Push mode

In the push mode the whole collection of endpoint connection information objects is passed during configuration time.

snippet:sqlserver-multidb-other-endpoint-connection-push


### Pull mode

The pull mode can be used when specific information is not available at configuration time. One can pass a `Func<String, ConnectionInfo>` that will be used by the SQL Server transport to resolve connection information at runtime.

snippet: sqlserver-multidb-other-endpoint-connection-pull


### Configuration

Endpoint-specific connection information is discovered by reading the connection strings from the configuration file with `NServiceBus/Transport/{name of the endpoint in the message mappings}` naming convention. If such a connection string is found, it is used for a given endpoint and this setting has precedence over the code-provided connection information.

Given the following mappings:

snippet:sqlserver-multidb-messagemapping

and the following connection strings:

snippet:sqlserver-multidb-connectionstrings

the messages sent to `billing` will go to database `Billing` on server `DbServerB` while the messages to `sales` will go to the database and server set by default i.e. `MyDefaultDB` on server `DbServerA`.


### ServiceControl

ServiceControl requires the `error` and `audit` queues to be present in a single database. In a system where there are multiple databases, used by various endpoints, this can be achieved by redirecting the queues using the following configuration endpoint configuration:

snippet:sqlserver-multidb-redirect-audit-error

For ServiceControl to retrying a message, after processing failure, it needs to have the same endpoint to connection string mappings as user endpoints e.g.

snippet:sqlserver-multidb-sc

Users need to make sure the connection string mapping is kept in sync between all the endpoints and ServiceControl.
