using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProducerConsumer
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var queue = new JobQueue();
            var consumers = Enumerable.Range(0, 5).Select(_ => new Consumer(queue)).ToList();
            var producers = Enumerable.Range(0, 5).Select(_ => new Producer(queue)).ToList();

            var countdown = 3;
            while (countdown >= 0)
            {
                Console.Clear();
                Console.WriteLine($"Starting in: {countdown}");
                Thread.Sleep(1000);
                countdown--;
            }

            foreach (var consumer in consumers)
            {
                consumer.Start();
            }

            foreach (var producer in producers)
            {
                producer.Start();
            }

            while (true)
            {
                Console.Clear();
                PrintInfo(consumers, producers);
                Console.WriteLine("Press any key to exit...");

                Thread.Sleep(50);

                if (Console.KeyAvailable)
                {
                    break;
                }
            }


            foreach (var consumer in consumers)
            {
                consumer.Stop();
            }

            foreach (var producer in producers)
            {
                producer.Stop();
            }
            
            PrintInfo(consumers, producers);
        }

        private static void PrintInfo(List<Consumer> consumers, List<Producer> producers)
        {
            Console.WriteLine("Consumers: ");
            Console.WriteLine("-------------------");
            Console.WriteLine("|-Id-|----State----|");
            foreach (var consumer in consumers)
            {
                Console.WriteLine($"|{consumer.Id.ToString().PadRight(4, '-')}|{consumer.State.ToString().PadRight(13, '-')}|");
            }

            Console.WriteLine("-------------------");
            Console.WriteLine("Producers: ");
            Console.WriteLine("-------------------");
            Console.WriteLine("|-Id-|----State----|");
            foreach (var producer in producers)
            {
                Console.WriteLine($"|{producer.Id.ToString().PadRight(4, '-')}|{producer.State.ToString().PadRight(13, '-')}|");
            }

            Console.WriteLine("-------------------");
        }
    }
}