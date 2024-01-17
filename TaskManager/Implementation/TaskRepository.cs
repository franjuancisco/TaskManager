using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TaskManager.Core.Entities;
using TaskManager.Core.Repositories;
using TaskManager.Models;

namespace TaskManager.Implementation
{
    public class TaskRepository:ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["TaskDbConnection"].ConnectionString; ;
        }

        public IEnumerable<TaskModel> GetAllTasks()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetTasks", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<TaskModel> tasks = new List<TaskModel>();
                        while (reader.Read())
                        {
                            tasks.Add(MapToTaskModel(reader));
                        }
                        return tasks;
                    }
                }
            }
        }

        public List<PriorityModel> GetAllPriorities()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetPriorityList", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<PriorityModel> tasks = new List<PriorityModel>();
                        while (reader.Read())
                        {
                            tasks.Add(MapToPriorityModel(reader));
                        }
                        return tasks;
                    }
                }
            }
        }

        public TaskModel GetTaskById(int taskId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetTaskById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TaskId", taskId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapToTaskModel(reader);
                        }
                        return null;
                    }
                }
            }
        }

        public void AddTask(TaskModel task)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("AddTask", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    AddTaskParameters(command, task);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateTask(TaskModel task)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UpdateTask", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    UpdateTaskParameters(command, task);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveTask(int taskId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("RemoveTask", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TaskId", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }

        private TaskModel MapToTaskModel(SqlDataReader reader)
        {
            return new TaskModel
            {
                ID = Convert.ToInt32(reader["Id"]),
                Description = reader["Description"].ToString(),
                CreationDate = Convert.ToDateTime(reader["CreationDate"]),
                Status = reader["Status"].ToString(),
                Priority = Convert.ToInt32(reader["Priority"])
            };
        }

        private PriorityModel MapToPriorityModel(SqlDataReader reader)
        {
            return new PriorityModel
            {
                ID = Convert.ToInt32(reader["Id"]),
                PriorityName = reader["PriorityName"].ToString()
            };
        }

        private void AddTaskParameters(SqlCommand command, TaskModel task)
        {
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Priority", task.Priority);
        }
        private void UpdateTaskParameters(SqlCommand command, TaskModel task)
        {
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@Priority", task.Priority);
            command.Parameters.AddWithValue("@TaskId", task.ID);
        }


    }
}