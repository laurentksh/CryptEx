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

        public TimerManager(Action action)
        {
            this.action = action;
            startedAt = DateTime.Now;
            timer = new Timer(Execute, new AutoResetEvent(false), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        public void Execute(object state)
        {
            if ((DateTime.Now - startedAt).Seconds % 10 == 0) {
                action?.Invoke();
            }

            if ((DateTime.Now - startedAt).Seconds > 60) {
                timer.Dispose();
            }
        }
    }
}
