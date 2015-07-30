using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskManager
{
    public enum TaskStatus
    {
        wait = 0,
        working = 1,
        stop = 2,
        suspend = 3,
        complete = 4,
        fail = 5,
        other = 99
    }
}
