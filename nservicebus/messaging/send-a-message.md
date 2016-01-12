---
title: Sending messages
summary: Describes how to send messages
tags: []
redirects:
- nservicebus/how-do-i-send-a-message
---

NServiceBus supports sending different types of messages (see [Messages, Events and Commands](messages-events-commands.md)) to any endpoint. Messages can be sent from outside or inside the message processing pipeline:

## Outside the pipeline

Messages are sent outside the pipeline when the send operation is not related to an incoming message. Examples may be:

* Sending a command which is triggered by an HTML form submitted to an ASP.NET application.
* Publishing an event after an user clicked a button on the GUI (see [Publish and Handle an Event](publish-subscribe/publish-handle-event.md)).

A message is sent by directly using the instance of the endpoint:

snippet:BasicSend

Note: Up to NServiceBus version 5, `IBus` is automatically registered in the configured dependency injection container. In Version 6, `IBus` has been deprecated and replaced with `IEndpointInstance` for sends outside the pipeline. `IEndpointInstance` is not registered by default.


## Inside the pipeline

Messages are sent from within the message processing pipeline when they are sent from a message handler, a saga or some advanced extensibility points. This is code triggered by an incoming message and all send operations are linked to that incoming message. This allows transactions to rollback already sent messages when the message fails at some later stage in the pipeline.

Here's how you send a simple message from a message handler:

snippet:SendFromHandler

It's also possible to use an interface rather than a concrete class for a message:

snippet:BasicSendInterface

Note: Up to NServiceBus version 5, the operations are available on the `IBus` which can be accessed using constructor or property injection. From version 6 `IMessageHandlerContext` is passed in directly to provide access to bus operations.


### Immediate Dispatch

While its usually best to let NServiceBus [handle exceptions for you](/nservicebus/errors), there are some scenarios where you want to send messages out even though the incoming message is rolled back. One example would be sending a reply notifying that there was an issue processing the message.

In order to request immediate dispatch use the following syntax.

snippet:RequestImmediateDispatch

NOTE: By specifying immediate dispatch, outgoing messages are not [batched](/nservicebus/messaging/batched-dispatch.md) or enlisted in the current receive transaction even if the transport has support for it.

Version 6 and below also allows suppressing the ambient transaction in order to have the outgoing message sent immediately.

snippet:RequestImmediateDispatchUsingScope

This only works for transports that enlists the receive operation in a transaction scope like MSMQ or SqlServer in DTC mode. Should you use any other transport or disable the DTC this will no longer work and the outgoing message might be rolled back together with the incoming message. For this reason we've decided to deprecate this method and recommend users switch to the explicit immediate dispatch API mentioned above.
