using Azure.Messaging.ServiceBus;
using System;

namespace AzureServiceBus_Topic_Receive
{
    class Program
    {
        private static string connection_string = "Endpoint=sb://nssbatinjune21.servicebus.windows.net/;SharedAccessKeyName=p1;SharedAccessKey=KQcIqmzeuqCRpJCiiTLqnfFhS2SFTY3w4Ia/HScZNMM=;EntityPath=topic1";
        private static string topic_name = "topic1";
        private static string subscription_name = "subscription1";
        static void Main(string[] args)
        {
            ServiceBusClient _client = new ServiceBusClient(connection_string);
            ServiceBusReceiver _receiver = _client.CreateReceiver(topic_name, subscription_name, new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

            var _messages = _receiver.ReceiveMessagesAsync(2);

            foreach (var _message in _messages.Result)
            {
                Console.WriteLine($"The Sequence number is {_message.SequenceNumber}");
                Console.WriteLine(_message.Body);

            }
            Console.ReadKey();
        }
    }
}
