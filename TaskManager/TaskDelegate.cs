using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskManager
{
    /// </summary>
    /// <param name="para">泛型参数</param>
    public delegate void TaskDelegate<T>(T para);

    /// <summary>
    /// 
    /// </summary>
    public class ParaStatus
    {
        public ParaStatus(Guid task) { TaskId = task; }
        public Guid TaskId { get; set; }
        public TaskStatus Status { get; set; }
    }    

    public class ParaInfo
    {
        public ParaInfo(Guid task) { TaskId = task; }
        public Guid TaskId { get; set; }
        public string Info { get; set; }
    }

    public class ParaProcess
    {
        public ParaProcess(Guid task) { TaskId = task; }
        public Guid TaskId { get; set; }
        public double Rate { get; set; }        
    }
}
