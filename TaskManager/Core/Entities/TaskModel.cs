using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.Models
{
    public class TaskModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
    }
}