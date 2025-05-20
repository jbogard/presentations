# nsb-diagnostics-poc

This code example shows integration with OpenTelemetry and W3C trace context

It requires RabbitMQ and Zipkin (just use Docker for both) to run.

Values based on https://github.com/open-telemetry/opentelemetry-specification/pull/418

## Messaging attributes

| Attribute name |                          Notes and examples                            | Required? | NServiceBus Value |
| -------------- | ---------------------------------------------------------------------- | --------- | ----------------- |
| `messaging.system` | A string identifying the messaging system vendor such as `kafka`, `rabbitmq` or `activemq`. | Yes | ReadOnlySettings.Get<TransportDefinition>().GetType().Name.Replace("Transport", null).ToLowerInvariant() |
| `messaging.destination` | The message destination name, e.g. `MyQueue` or `MyTopic`. This might be equal to the span name but is required nevertheless. | Yes | ReadOnlySettings.LogicalAddress().ToString() |
| `messaging.destination_kind` | The kind of message destination: Either `queue` or `topic`. | Yes, if either of them applies. | Translated from the `MessageIntentEnum` in the message and `OutboundRoutingType` |
| `messaging.temp_destination` | A boolean that is `true` if the message destination is temporary. | If temporary (assumed to be `false` if missing). | |
| `messaging.protocol` | The name of the transport protocol such as `AMQP` or `MQTT`. | No | |
| `messaging.protocol_version` | The version of the transport protocol such as `0.9.1`. | No | |
| `messaging.url` | Connection string such as `tibjmsnaming://localhost:7222` or `https://queue.amazonaws.com/80398EXAMPLE/MyQueue`. | No | |
| `messaging.message_id` | A value used by the messaging system as an identifier for the message, represented as a string. | No | Message.MessageId |
| `messaging.conversation_id` | A value identifying the conversation to which the message belongs, represented as a string. Sometimes called "Correlation ID". | No | "NServiceBus.ConversationId" header |

avg(rate(messaging_client_server_duration_ms_count[5m]))

## Prometheus

To integrate with Prometheus, in the root run a Prometheus instance with Docker:

```powershell
docker run `
    -p 9090:9090 `
    -v ${PWD}/prometheus.yml:/etc/prometheus/prometheus.yml `
    prom/prometheus
```