using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using task_management.Context;
using task_management.Interfaces;
using task_management.Models;
using task_management.Utils;

namespace task_management.Services
{
    public class TaskManagementService : ITaskManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private UserUtils _userUtil;

        public TaskManagementService(ApplicationDbContext context, IConfiguration configuration, UserUtils userUtil)
        {
            _context = context;
            _configuration = configuration;
            _userUtil=userUtil;
        }

        public List<TaskMaster> GetTaskList()
        {
            var TaskMasterlist = _context.TaskMasters.ToList();
            return TaskMasterlist;
        }

        TaskMaster ITaskManagementService.AddTask(TaskMaster obj)
        {
            obj.CreatedDate = DateTime.Now;
            obj.UpdatedDate = DateTime.Now;
            ClaimsPrincipal claimsPrincipal = _userUtil.GetUserDetailsFromToken();
            obj.CreatedBy =Convert.ToInt32(claimsPrincipal.FindFirst("id").Value);
                    var addedTask = _context.TaskMasters.Add(obj);
            _context.SaveChanges();
            return addedTask.Entity;
        }

        bool ITaskManagementService.DeleteTaskById(int taskId)
        {
            throw new NotImplementedException();
        }

        TaskMaster ITaskManagementService.GetTaskById(int taskId)
        {
            var taskMaster = _context.TaskMasters.Where(e=>e.Id==taskId).FirstOrDefault();
            return taskMaster;
        }

        bool ITaskManagementService.UpdateTask(TaskMaster obj)
        {
            throw new NotImplementedException();
        }
    }
}
