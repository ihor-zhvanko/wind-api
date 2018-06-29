using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wind.Api.Workers
{
    public interface IWorker : IDisposable
    {
        void Start();
        void Stop();
    }

    public abstract class BaseWorker : IWorker
    {
        private Task _workerTask;
        private volatile bool _work;
        public int Timeout { get; protected set; }

        public BaseWorker()
        {
            Timeout = 10; //10 seconds. Default value
            _work = false;
        }

        public void Start()
        {
            _work = true;

            _workerTask = new Task(async () =>
            {
                while (_work)
                {
                    await Main();
                    //this simple trick won't block main process when it wants to exit
                    await Task.Delay(_work ? Timeout * 1000 : 0);
                }
            });

            _workerTask.Start();
        }

        protected abstract Task Main();

        public void Stop()
        {
            _work = false;
            _workerTask.Wait();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
