## What is the difference between Azure Event Hub and Event Grid?
The noticeable difference between them is that Event Hubs are accepting only endpoints for the ingestion of data and they don’t provide a mechanism for sending data back to publishers. On the other hand, Event Grid sends HTTP requests to notify events that happen in publishers.

Event Grid is an eventing backplane that enables event-driven, reactive programming. It uses a publish-subscribe model. Publishers emit events, but have no expectation about which events are handled. Subscribers decide which events they want to handle.

## When to Use
### Event Hubs
 - This service can be used when your application deals with the series of events and when you think your application might need a massive scale at least in the future, say a million events and to handle the data that also comes along with the event.

### Event Grid
- This service can be used when your application deals with discrete events. Predominantly, when there is a need for your application to work in a publisher/subscriber model and to handle event but not the data, unlike Event Hubs.

## Real-time scenario
### Event Hubs
- Azure Event Hub is used more for the telemetry scenarios. Let’s say if every component that’s been used in the Enterprise for this e-commerce application emits telemetry data like Log4Net or Log4J and you want to capture it, then Azure Event Hub can be used. A great example is Azure Application Insights, under the hood it uses Azure Event Hub to capture the telemetry information.

### Event Grid
- When you’re actually shipping and moving stuff, where you are reacting to things like item has been pulled from the shelf, item was bought to postal area, item has been shipped, or this shipment was rejected and so on, in this case instead of doing a central control workflow you would do in an Event-based model running active workflows for each product item.  Here you are reacting to real changes in real-time. Azure Event Grid is ideal for these kinds of reactive scenarios. 
