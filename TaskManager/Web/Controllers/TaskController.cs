using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.Core.Repositories;
using TaskManager.Implementation;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController()
        {
            _taskRepository = new TaskRepository();
        }
        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository)); ;
        }

        public ActionResult Index() { 

            List<TaskModel> tasks = _taskRepository.GetAllTasks().ToList();
            ViewBag.Priorities = _taskRepository.GetAllPriorities();
            return View(tasks);
        }

        public ActionResult Create()
        {
            ViewBag.Priorities = _taskRepository.GetAllPriorities();
            return View();
        }

        [HttpPost]
        public ActionResult Create(TaskModel task)
        {
            if (ValidateTaskModel(task)) { 
                _taskRepository.AddTask(task);
                return RedirectToAction("Index");
            }
            ViewBag.Priorities = _taskRepository.GetAllPriorities();
            return View(task);
        }

        // Acción para mostrar el formulario de edición de tarea
        public ActionResult Edit(int id)
        {
            ViewBag.Priorities = _taskRepository.GetAllPriorities();
            var task = _taskRepository.GetTaskById(id);
            if (task == null) throw new Exception("Tarea no encontrada");
            
            return View(task);
        }

        public ActionResult Detail(int taskId)
        {
            ViewBag.Priorities = _taskRepository.GetAllPriorities();
            var task = _taskRepository.GetTaskById(taskId);
            if (task == null) throw new Exception("Tarea no encontrada");

            return PartialView("_TaskDetails", task);
        }

        // Acción para procesar el formulario de edición de tarea
        [HttpPost]
        public ActionResult Edit(TaskModel task)
        {
            if (ValidateTaskModel(task))
            {
                _taskRepository.UpdateTask(task);
                return RedirectToAction("Index");
            }
            return View(task);
        }

        // Acción para desactivar una tarea
        public ActionResult Deactivate(int id)
        {
            _taskRepository.RemoveTask(id);
            return RedirectToAction("Index");
        }

        public bool ValidateTaskModel(TaskModel task)
        {
            if (task.Priority!=0 && task.Priority!=null && task.Description.Length > 4)
            {
                return true;
            }
            return true;
        }

        //public ActionResult ShowReport()
        //{
        //    var reportViewer = new ReportViewer
        //    {
        //        ProcessingMode = ProcessingMode.Remote,
        //        SizeToReportContent = true,
        //        Width = Unit.Percentage(100),
        //        Height = Unit.Percentage(100),
        //    };

        //    var connectionString = "Data Source=TuServidor;Initial Catalog=TuBaseDeDatos;Integrated Security=True";
        //    reportViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials(connectionString);

        //    ViewBag.ReportViewer = reportViewer;

        //    return View();
        //}
    }
}