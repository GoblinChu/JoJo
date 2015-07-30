using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TaskManager
{
    public class Executer : IExecuter
    {
        public void Execute()
        {

        }
        #region IExecute
        public Guid TaskId { get; set; }

        public Task Task { get; set; }

        public void Do()
        {
            for (int i = 1; i <= 100; i++)
            {
                if (Task.taskProcess != null)
                {
                    Thread.Sleep(1000);
                    ParaProcess pp = new ParaProcess(TaskId);
                    pp.Rate = (double)i / 100;
                    Task.taskProcess(pp); 
                }
            }
            if (Task.taskStatus != null)
            {
                Task.ChangeStatus(TaskStatus.complete);
            }
        }
        #endregion
    }
}
