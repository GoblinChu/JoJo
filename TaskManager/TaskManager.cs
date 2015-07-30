using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading;

namespace TaskManager
{
    public class TaskManager
    {
        public List<Task> Tasks = new List<Task>();
        /// <summary>
        /// 添加任务
        /// </summary>
        public Task AddTask(Task task)
        {
            try
            {
                Tasks.Add(task);

                task.Wait();
            }
            catch (Exception e)
            {

            }
            return task;
        }
        public void StartTask(Task task)
        {
            task.Start();
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="downloader"></param>
        public void StopTask(Task task)
        {
            task.Stop();
        }

        public void SuspendTask(Task task)
        {
            task.Suspend();
        }

        public void ResumeTask(Task task)
        {
            task.Resume();
        }

        /// <summary>
        /// 删除任务(自动终止未停止的任务)
        /// </summary>
        /// <param name="downloader"></param>
        public void DeleteTask(Task task, bool deleteFile)
        {
            //先停止任务
            task.Stop();

            while (task.Status == TaskStatus.working)
            {
                Thread.Sleep(50);
            }

            //从任务列表中删除任务
            if (Tasks.Contains(task))
            {
                Tasks.Remove(task);
            }
        }

    }
}
