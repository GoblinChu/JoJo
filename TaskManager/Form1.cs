using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class Form1 : Form
    {
        TaskManager taskMgr = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //初始设置任务管理器
            if (taskMgr == null)
            {
                taskMgr = new TaskManager();
            }
            init();
        }

        public void init()
        {
            for (var j = 0; j < 5; j++)
            {
                Task taskItem = new Task(Guid.NewGuid(), new Executer());
                taskItem.taskStatus += new TaskDelegate<ParaStatus>(StatusChange);
                taskItem.taskProcess += new TaskDelegate<ParaProcess>(taskProcess);
                taskMgr.AddTask(taskItem);
                ListViewItem lvi = new ListViewItem();
                for (int i = 0; i < 3; i++)
                {
                    lvi.SubItems.Add("");
                }
                lvi.SubItems[GetColumn("name")].Text = "task"+ j.ToString(); 
                lvi.SubItems[GetColumn("TaskId")].Text = taskItem.TaskId.ToString(); 
                lvi.SubItems[GetColumn("Status")].Text = taskItem.Status.ToString();
                lvi.SubItems[GetColumn("rate")].Text = "0"; 
                lvi.Tag = taskItem.TaskId.ToString(); //设置TAG
                lsv.Items.Add(lvi);

            }
            lsv.Refresh();
        }

        public void taskProcess(ParaProcess p)
        {
            //如果需要在安全的线程上下文中执行
            if (lsv.InvokeRequired)
            {
                lsv.Invoke(new TaskDelegate<ParaProcess>(taskProcess), p);
                return;
            }
            ParaProcess p1 = p;
            Task ac = GetTask(p1.TaskId);
            ListViewItem item = GetLsvItem(p1.TaskId);
            //设置提示信息
            item.SubItems[GetColumn("rate")].Text = p1.Rate.ToString();
        }

        public void StatusChange(ParaStatus paraStatus)
        {
            //如果需要在安全的线程上下文中执行
            if (lsv.InvokeRequired)
            {
                lsv.Invoke(new TaskDelegate<ParaStatus>(StatusChange), paraStatus);
                return;
            }
            //转换参数
            ParaStatus p = paraStatus;
            //取得指定任务
            Task downloader = GetTask(p.TaskId);
            if (downloader == null)
                return;

            //设置TaskItem
            ListViewItem item = GetLsvItem(p.TaskId);

            item.SubItems[GetColumn("Status")].Text = paraStatus.Status.ToString();
            
            
        } //end Start

        private delegate int delGetColumn(string columnName);
        /// <summary>
        /// 取得列所在位置
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public int GetColumn(string columnName)
        {
            if (lsv.InvokeRequired)
            {
                lsv.Invoke(new delGetColumn(GetColumn), columnName);
                return -1;
            }
            foreach (ColumnHeader item in lsv.Columns)
            {
                if (item.Tag.ToString() == columnName)
                    return item.Index;
            }
            return -1;
        }

        private delegate ListViewItem delGetLsvItem(Guid guid);
        public ListViewItem GetLsvItem(Guid guid)
        {
            if (lsv.InvokeRequired)
            {
                lsv.Invoke(new delGetLsvItem(GetLsvItem), guid);
                return null;
            }
            for (int i = 0; i < lsv.Items.Count; i++)
            {
                if (lsv.Items[i].Tag != null)
                {
                    if (new Guid((string)lsv.Items[i].Tag) == guid)
                    {
                        return lsv.Items[i];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据GUID值寻找对应的任务
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Task GetTask(Guid guid)
        {
            foreach (var i in taskMgr.Tasks)
            {
                if (i.TaskId == guid)
                    return i;
            }
            return null;
        }


        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsv.FocusedItem == null && lsv.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请选择！");
                return;
            }
            int count = lsv.Items.Count;
            for (int i = 0; i < count; i++)
            {
                ListViewItem item = lsv.Items[i];
                if (item.Selected == true)
                {
                    Task taskItem = GetTask(new Guid((string)item.Tag));
                    if (taskItem != null)
                    {
                        taskMgr.StartTask(taskItem);
                    }
                }
            }
        }

        private void suspToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsv.FocusedItem == null && lsv.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请选择！");
                return;
            }
            int count = lsv.Items.Count;
            for (int i = 0; i < count; i++)
            {
                ListViewItem item = lsv.Items[i];
                if (item.Selected == true)
                {
                    Task taskItem = GetTask(new Guid((string)item.Tag));
                    if (taskItem != null)
                    {
                        taskMgr.SuspendTask(taskItem);
                    }
                }
            }
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsv.FocusedItem == null && lsv.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请选择！");
                return;
            }
            int count = lsv.Items.Count;
            for (int i = 0; i < count; i++)
            {
                ListViewItem item = lsv.Items[i];
                if (item.Selected == true)
                {
                    Task taskItem = GetTask(new Guid((string)item.Tag));
                    if (taskItem != null)
                    {
                        taskMgr.ResumeTask(taskItem);
                    }
                }
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsv.FocusedItem == null && lsv.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请选择！");
                return;
            }
            int count = lsv.Items.Count;
            for (int i = 0; i < count; i++)
            {
                ListViewItem item = lsv.Items[i];
                if (item.Selected == true)
                {
                    Task taskItem = GetTask(new Guid((string)item.Tag));
                    if (taskItem != null)
                    {
                        taskMgr.StopTask(taskItem);
                    }
                }
            }
        }

        private void lsv_ContextMenuStripChanged(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (lsv.FocusedItem == null && lsv.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请选择！");
                return;
            }
            int count = lsv.Items.Count;
            for (int i = 0; i < count; i++)
            {
                ListViewItem item = lsv.Items[i];
                if (item.Selected == true)
                {
                    Task taskItem = GetTask(new Guid((string)item.Tag));
                    if (taskItem != null)
                    {
                        var status = taskItem.Status;
                        switch (status)
                        {
                            case TaskStatus.wait:
                            case TaskStatus.fail:
                            case TaskStatus.other:
                            case TaskStatus.stop:
                                startToolStripMenuItem.Enabled = true;
                                resumeToolStripMenuItem.Enabled = false;
                                suspToolStripMenuItem.Enabled = false;
                                stopToolStripMenuItem.Enabled = false;
                                break;
                            case TaskStatus.working:
                                startToolStripMenuItem.Enabled = false;
                                resumeToolStripMenuItem.Enabled = false;
                                suspToolStripMenuItem.Enabled = true;
                                stopToolStripMenuItem.Enabled = true;
                                break;
                            case TaskStatus.suspend:
                                startToolStripMenuItem.Enabled = false;
                                resumeToolStripMenuItem.Enabled = true;
                                suspToolStripMenuItem.Enabled = true;
                                stopToolStripMenuItem.Enabled = true;
                                break;
                            case TaskStatus.complete:
                                 startToolStripMenuItem.Enabled = true;
                                resumeToolStripMenuItem.Enabled = false;
                                suspToolStripMenuItem.Enabled = false;
                                stopToolStripMenuItem.Enabled = false;
                                break;

                        }
                    }
                }
            }
        }

       
    }
}
