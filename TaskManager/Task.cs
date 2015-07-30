using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TaskManager
{
    /// <summary>
    /// 
    /// </summary>
    public class Task
    {
        public TaskDelegate<ParaStatus> taskStatus;
        public TaskDelegate<ParaInfo> taskInfo;
        public TaskDelegate<ParaProcess> taskProcess;

        private TaskStatus _status = TaskStatus.wait;
        private IExecuter _Executer { get; set; }
        private Guid _taskId;
        private Thread thread;
        private string _name;

        public Task(Guid id, IExecuter Executer)
        {
            _taskId = id;
            _Executer = Executer;
            _Executer.TaskId = id;
            _Executer.Task = this;
        }

        public void Wait()
        {
            ChangeStatus(TaskStatus.wait);
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        
        public Guid TaskId
        {
            get
            {
                return _taskId;
            }
        }
        
        public TaskStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }        

        public void Start()
        {
            if (thread == null)
            {
                thread = new Thread(_Executer.Do);
            }
            else if (thread.ThreadState == ThreadState.Stopped)
            {
                thread = new Thread(_Executer.Do);
            }
            thread.Start();
            ChangeStatus(TaskStatus.working);
        }
        public void ChangeStatus(TaskStatus s)
        {
            Status = s;
            ParaStatus ps = new ParaStatus(TaskId);
            ps.Status = Status;
            if (taskStatus != null) taskStatus(ps);
        }
        public void Stop()
        {
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch { }
                }
            }
            ChangeStatus(TaskStatus.stop);
        }
        public void Suspend()
        {
            try
            {
                thread.Suspend();
            }
            catch { }
            ChangeStatus(TaskStatus.suspend);
        }
        public void Resume()
        {
            if (thread.IsAlive)
            {
                thread.Resume();
            }
            ChangeStatus(TaskStatus.working);
        }
    }
}
