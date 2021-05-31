using System;
using System.Threading;

namespace ProducerConsumer
{
    public class Producer
    {
        private static int ID = 0;
        
        public int Id { get; set; }
        
        private readonly JobQueue _jobQueue;

        private readonly Thread _thread;

        private volatile bool _shouldStop;

        public ProducerState State { get; set; }
        
        public Producer(JobQueue jobQueue)
        {
            Id = ID++;
            _jobQueue = jobQueue;
            _thread = new Thread(ProduceRoutine);
            State = ProducerState.Stopped;
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _shouldStop = true;
        }

        private void ProduceRoutine()
        {
            while (!_shouldStop)
            {
                State = ProducerState.Producing;
                var job = new Job();
                Thread.Sleep(100);
                
                State = ProducerState.Publishing;
                while (!_jobQueue.TryPublish(job))
                {
                    Thread.Sleep(100);
                }

                State = ProducerState.Waiting;

                Thread.Sleep(200);
            }

            _shouldStop = false;
            State = ProducerState.Stopped;
        }
    }
}