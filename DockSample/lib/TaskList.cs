using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockSample.lib
{
    public class Tasks
    {
        public string title { get; set; }
        public string description { get; set; }
        public string creation_date { get; set; }
        public string due_date { get; set; }
        public bool completed { get; set; }
    }
    public class TaskList
    {
        public string list_name { get; set; }
        public string creation_date { get; set; }
        public string due_date { get; set; }
        public List<Tasks> tasks { get; set; }
    }

    public class TaskConfig
    {
        public List<TaskList> task_list { get; set; }
    }
}
