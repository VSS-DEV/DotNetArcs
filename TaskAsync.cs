using System;
using System.Threading.Tasks;

namespace DotNetArcs
{
    public class AsyncTasker
    {
        /// <summary>
        /// Классы выполнения асинхронных задач
        /// </summary>
        /// <param name="ActionCommand">Заранее описанное событие которое должно быть выполнено</param>
        public AsyncTasker(Action ActionCommand,in bool NoWait=false)
        {
            using (Task ts = new Task(ActionCommand))
            {
                ts.Start();
                if (!NoWait)
                    ts.Wait();
            }
        }
    }
}