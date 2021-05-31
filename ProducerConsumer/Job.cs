using System;
using System.Threading;

namespace ProducerConsumer
{
    public class Job
    {
        private static int ID = 1;

        public int Id { get; set; }

        public Job()
        {
            this.Id = ID++;
        }

        public void Do()
        {
            // Simulate some Great Job by
            // calculation of sum of 100 million sequential inverse square roots
            double sum = 0;
            for (int i = 1; i <= 100_000_000; i++)
            {
                sum += 1 / Math.Sqrt(i);
            }
        }
    }
}