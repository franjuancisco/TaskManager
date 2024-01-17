using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManager.Core.Entities;
using TaskManager.Models;

namespace TaskManager.Core.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<TaskModel> GetAllTasks();
        List<PriorityModel> GetAllPriorities();
        TaskModel GetTaskById(int taskId);
        void AddTask(TaskModel task);
        void UpdateTask(TaskModel task);
        void RemoveTask(int taskId);

    }
}