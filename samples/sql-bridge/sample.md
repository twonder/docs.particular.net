---
title: SQL Bridge
summary: This sample shows how to setup a SQL subscriber so it can subscribe to events from a MSMQ publisher.
related:
- nservicebus/transports
- nservicebus/msmq
- nservicebus/sqlserver
---

## Sql Bridge (Transport Integration)

WARNING: This sample has been deprecated. Please refer to the [MsmqToSqlRelay Sample](/samples/msmqtosqlrelay/) 

This sample shows how to setup a sql subscriber so it can subscribe to events from a MSMQ publisher. The solution comprises of these 5 projects.

NOTE: This sample uses NHibernate persistence. It uses a database called, `PersistenceForMsmqTransport` for MSMQ transport endpoints and a different database called, `PersistenceForSqlTransport` for SQL Transport endpoints.


## Shared

The event that will be published by `MsmqPublisher`

snippet:event


## MsmqPublisher


### The publisher configuration.

snippet:publisher-config


### The publish loop

snippet:publisher-loop


### Additional entry to the list of subscribers

Add a new entry in the Subscriptions collection for the new queue specified in the app.config to the list of subscribers in the MsmqPublisher's subscription storage.

Run a this script to add the new entry:

```
Use PersistenceForMsmqTransport
Go

INSERT INTO Subscription
       ([SubscriberEndpoint]
       ,[MessageType]
       ,[Version]
       ,[TypeName])
 VALUES
       ('SqlMsmqTransportBridge@MachineName',
       'Shared.SomethingHappened,0.0.0.0',
       '0.0.0.0',
       'Shared.SomethingHappened')
GO
```

## MsmqSubscriber

Subscribes to the events from the `MsmqPublisher`


### The Msmq Subscriber configuration.

snippet:msmqsubscriber-config


### The Msmq Subscriber handler.

snippet:msmqsubscriber-handler


## SqlBridge

This endpoint is setup to read messages that arrive in MSMQ via an `IAdvancedSatellite`.


### The bridge configuration

snippet:bridge-config


### The Satellite

snippet:satellite

Note: Since `SqlBridge` is not using native msmq transport you will need to manually create the `SqlMsmqTransportBridge` queue.


## How does the advanced satellite work

- It uses a MSMQ Dequeue strategy to read messages from its Input queue.
- References the `Shared` message schema dll.
- The input queue is defined as `SqlMsmqTransportBridge` in the `InputAddress` property of the satellite.
- The MSMQ dequeue strategy is set here for reading messages from the queue (MSMQ).
- The satellite will automatically process any message that is received in that queue (MSMQ).
- The satellite will publish the received event. Since this endpoint uses SqlTransport, it will publish to its Sql queues.


## SqlSubscriber

Receives events from the SqlBridge. The endpoint address is the sql bridge address and not the original publisher's address.


### The Sql Subscriber configuration

snippet:sqlsubscriber-config


### The event handler

snippet:sqlsubscriber-handler


## Summary

1. Creating a new transactional queue that the MSMQ publisher will be sending its events to.
2. In the original MSMQ Publisher's Subscriptions storage, in addition to its list of all its current subscribers, has an additional entry for the queue that the SqlBridge will be listening to.
3. The Sql bridge endpoint (setup to read from that input queue) processes that message and publishes the event.
4. The Sql Subscriber, subscribes to the SqlBridge.

Note: The steps of creating the queue and adding the additional subscription message in the subscriptions queue of the publisher can be automated for deployment.
