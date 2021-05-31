using System;
using System.Threading;

namespace ProducerConsumer
{
    public class Consumer
    {
        private static int ID = 0;

        public int Id { get; set; }

        private readonly JobQueue _jobQueue;

        private readonly Thread _thread;

        private volatile bool _shouldStop;
        
        public ConsumerState State { get; set; }

        public Consumer(JobQueue jobQueue)
        {
            Id = ID++;
            _jobQueue = jobQueue;
            _thread = new Thread(ConsumeRoutine);
            State = ConsumerState.Stopped;
        }

        private void ConsumeRoutine()
        {
            while (!_shouldStop)
            {
                State = ConsumerState.Taking;
                if (_jobQueue.TryTake(out var job))
                {
                    State = ConsumerState.Performing;
                    job.Do();
                }

                State = ConsumerState.Waiting;
                Thread.Sleep(100);
            }

            _shouldStop = false;
            
            State = ConsumerState.Stopped;
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _shouldStop = true;
        }
    }
}