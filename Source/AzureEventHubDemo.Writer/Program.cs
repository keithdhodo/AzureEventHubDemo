// <copyright file="Program.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

// Demo code from: https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-send
namespace AzureEventHubDemo.Writer
{
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Producer;
    using AzureEventHubDemo.Writer.Models;
    using System;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class Program
    {
        // connection string to the Event Hubs namespace
        private const string connectionString = "Endpoint=sb://eventhubkhododemo.servicebus.windows.net/;SharedAccessKeyName=SendPolicy;SharedAccessKey=QfV2ZKub+eptz10Fq1+jZW6B1GDdy715WM+CBecccB0=";

        // name of the event hub
        private const string eventHubName = "eventhubkhododemo";

        // number of events to be sent to the event hub
        private const int batchMaximumSizeInBites = 999999;
        private static int count = 0;
        private static int total = 0;

        // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when events are being published or read regularly.
        private static EventHubProducerClient producerClient;

        // https://willvelida.medium.com/building-a-simple-streaming-app-with-azure-cosmos-db-event-hubs-and-azure-functions-3dd033979faf
        public static async Task Main()
        {
            // Create a producer client that you can use to send events to an event hub
            producerClient = new EventHubProducerClient(connectionString, eventHubName);

            // Create a batch of events
            var createBatchOptions = new CreateBatchOptions()
            {
                MaximumSizeInBytes = batchMaximumSizeInBites,
            };
            var eventBatch = await producerClient.CreateBatchAsync(createBatchOptions);

            while (true)
            {
                try
                {
                    var createdEvent = GenerateEvent();

                    if (!eventBatch.TryAdd(createdEvent))
                    {
                        // Use the producer client to send the batch of events to the event hub
                        await producerClient.SendAsync(eventBatch);
                        Console.WriteLine($"A batch of {count} events has been published.");
                        eventBatch = await producerClient.CreateBatchAsync(createBatchOptions);
                        count = 0;

                        // Add the event we couldn't before
                        if (!eventBatch.TryAdd(createdEvent))
                        {
                            // if it is too large for the batch
                            throw new Exception($"Event {total} is too large for the batch and cannot be sent.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                //finally
                //{
                //    await producerClient.DisposeAsync();
                //}
            }

            //await producerClient.DisposeAsync();
        }

        private static EventData GenerateEvent()
        {
            var driverProfile = GenerateDriverProfile();

            return new EventData(ObjectToByteArray(driverProfile));
        }

        private static DriverProfile GenerateDriverProfile()
        {
            count++;

            return new DriverProfile()
            {
                DriverId = Guid.NewGuid(),
                Name = Faker.Name.FullName(Faker.NameFormats.StandardWithMiddle),
                Area = Faker.Country.Name(),
                Bio = Faker.Lorem.Paragraph(),
                Followers = Faker.RandomNumber.Next(),
                Count = total++,
            };
        }

        /// <summary>
        /// Convert an object to a byte array.
        /// </summary>
        /// <param name="obj">https://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp</param>
        /// <returns>https://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp</returns>
        public static byte[] ObjectToByteArray(object obj)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            var json = JsonSerializer.Serialize(obj, options);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}