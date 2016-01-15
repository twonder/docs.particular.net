---
title: Authoring a custom transport
summary: How to author a custom NServiceBus transport in NServiceBus Version 6
tags: []
redirects:
 - nservicebus/authoring-custom-nservicebus-transport
---

NServiceBus requires a mechanism to transport messages between endpoints as discussed in [Transports in NServiceBus](/nservicebus/transports/).

While a variety of transport technologies are supported out of the box by NServiceBus (for example MSMQ, SQL Server, RabbitMQ, Azure Storage Queues or Azure Service Bus) you sometimes may want to plug in your own, or unsupported, transport infrastructure. Just like our awesome community already did in order to [support AmazonSQS and OracleAQ](/platform/extensions#transports).

This guide will explain the various tasks involved in writing a custom transport. 

## Transport Definition

## Transport Configuration

## Receive Infrastructure

## Dispatch Infrastructure

## Subscription Infrastructure