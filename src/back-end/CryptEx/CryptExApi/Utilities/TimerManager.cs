using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CryptExApi.Utilities
{
    public class TimerManager
    {
        private readonly Timer timer;
        private readonly Action action;
        private readonly DateTime startedAt;
        private readonly CancellationToken token;

        public TimerManager(Action action, CancellationToken token)
        {
            this.action = action;
            this.token = token;
            startedAt = DateTime.Now;
            timer = new Timer(Execute, new AutoResetEvent(false), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        public void Execute(object state)
        {
            if (token.IsCancellationRequested) {
                timer.Dispose();
                return;
            }

            if ((DateTime.Now - startedAt).Seconds % 10 == 0) {
                action?.Invoke();
            }

            if ((DateTime.Now - startedAt).Seconds >= 60) {
                timer.Dispose();
            }
        }
    }
}
