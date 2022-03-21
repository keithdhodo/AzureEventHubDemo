// <copyright file="Program.cs" company="Keith D. Hodo">
// Copyright (c) Keith D. Hodo. All rights reserved.
// </copyright>

// Demo code from: https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-dotnet-standard-getstarted-send
namespace AzureEventHubDemo.Writer
{
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Producer;
    using AzureEventHubDemo.Writer.Classes;
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;

    public class Program
    {
        // connection string to the Event Hubs namespace
        private const string connectionString = "<Writer Connection String>";

        // name of the event hub
        private const string eventHubName = "eventhubkhododemo";

        // number of events to be sent to the event hub
        private const int numOfEvents = 1800;

        // The Event Hubs client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when events are being published or read regularly.
        private static EventHubProducerClient producerClient;

        public static async Task Main()
        {
            // Create a producer client that you can use to send events to an event hub
            var producerClient = new EventHubProducerClient(connectionString, eventHubName);

            while (true)
            {
                // Create a batch of events
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

                for (int i = 1; i <= numOfEvents; i++)
                {
                    if (!eventBatch.TryAdd(GenerateEvent()))
                    {
                        // if it is too large for the batch
                        throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
                    }
                }

                try
                {
                    // Use the producer client to send the batch of events to the event hub
                    await producerClient.SendAsync(eventBatch);
                    Console.WriteLine($"A batch of {numOfEvents} events has been published.");
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
            return new DriverProfile()
            {
                Name = Faker.Name.FullName(Faker.NameFormats.StandardWithMiddle),
                Area = Faker.Country.Name(),
                Bio = Faker.Lorem.Paragraph(),
                Followers = Faker.RandomNumber.Next(),
            };
        }

        /// <summary>
        /// Convert an object to a byte array.
        /// </summary>
        /// <param name="obj">https://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp</param>
        /// <returns>https://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp</returns>
        public static byte[] ObjectToByteArray(Object obj)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}