using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskManager
{
    public interface IExecuter
    {
        Guid TaskId { get; set; }

        Task Task { get; set; }
        void Do();
    }
}
