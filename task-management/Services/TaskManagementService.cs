using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using task_management.Context;
using task_management.Interfaces;
using task_management.Models;
using task_management.Models.RequestModels;
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
            _userUtil = userUtil;
        }

        public async Task<List<TaskMaster>> GetTaskList()
        {
            var TaskMasterlist = await _context.TaskMasters.ToListAsync();
            return TaskMasterlist;
        }
        public async Task<List<TaskMaster>> GetAssignToTaskList()
        {
            ClaimsPrincipal claimsPrincipal = _userUtil.GetUserDetailsFromToken();
            int currentUser = Convert.ToInt32(claimsPrincipal.FindFirst("id").Value);
            var TaskMasterlist = await _context.TaskMasters.Where(e => e.AssignTo == currentUser).ToListAsync();
            return TaskMasterlist;
        }
        public async Task<List<TaskMaster>> GetCreatedByTaskList()
        {
            ClaimsPrincipal claimsPrincipal = _userUtil.GetUserDetailsFromToken();
            int currentUser = Convert.ToInt32(claimsPrincipal?.FindFirst("id")?.Value);
            var TaskMasterlist = await _context.TaskMasters.Where(e => e.CreatedBy == currentUser).ToListAsync();
            return TaskMasterlist;
        }
        public async Task<TaskMaster> AddTask(TaskMaster obj)
        {
            obj.CreatedDate = DateTime.Now;
            obj.UpdatedDate = DateTime.Now;
            ClaimsPrincipal claimsPrincipal = _userUtil.GetUserDetailsFromToken();
            obj.CreatedBy = Convert.ToInt32(claimsPrincipal?.FindFirst("id")?.Value);
            await _context.TaskMasters.AddAsync(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> DeleteTaskById(int taskId)
        {
            ClaimsPrincipal claimsPrincipal = _userUtil.GetUserDetailsFromToken();
            int currentUser = Convert.ToInt32(claimsPrincipal?.FindFirst("id")?.Value);

            var record = _context.TaskMasters.FirstOrDefault(e => e.Id == taskId && e.CreatedBy == currentUser) ?? throw new Exception("Something went wrong");

            _context.TaskMasters.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TaskMaster> GetTaskById(int taskId)
        {
            return await _context.TaskMasters.Where(e => e.Id == taskId).FirstOrDefaultAsync() ?? throw new Exception("Something went Wrong");

        }

        public bool UpdateTaskStatus(TaskMasterRequest obj)
        {
            bool success = false;
            ClaimsPrincipal claimsPrincipal = _userUtil.GetUserDetailsFromToken();
            int currentUser = Convert.ToInt32(claimsPrincipal?.FindFirst("id")?.Value);

            var record = _context.TaskMasters.Where(e => e.Id == obj.Id && (e.CreatedBy == currentUser || e.AssignTo == currentUser)).FirstOrDefault() ?? throw new Exception("Something went wrong");

            record.UpdatedDate = DateTime.Now;
            record.status = obj.status;
            record.Description = obj.Description;

            _context.SaveChanges();
            success = true;
            return success;
        }
    }
}
