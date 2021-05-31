using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer
{
    public class JobQueue
    {
        private const int MaxJobs = 10;
        private readonly Mutex _syncMutex;
        private readonly Queue<Job> _jobs;

        public JobQueue()
        {
            _jobs = new Queue<Job>();
            _syncMutex = new Mutex();
        }

        public bool TryTake(out Job job)
        {
            job = null;
            if (_jobs.Count > 0)
            {
                _syncMutex.WaitOne();

                if (_jobs.Count > 0)
                {
                    job = _jobs.Dequeue();
                }

                _syncMutex.ReleaseMutex();

                if (job != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryPublish(Job job)
        {
            if (_jobs.Count < MaxJobs)
            {
                _syncMutex.WaitOne();

                bool success = false;
                if (_jobs.Count < MaxJobs)
                {
                    _jobs.Enqueue(job);
                    success = true;
                }

                _syncMutex.ReleaseMutex();
                if (success)
                {
                    return true;
                }
            }

            return false;
        }
    }
}