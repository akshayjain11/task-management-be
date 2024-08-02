using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            var temp= DateTime.Now;
            obj.CreatedDate = temp;
            obj.UpdatedDate = temp;
            ClaimsPrincipal claimsPrincipal = _userUtil.GetUserDetailsFromToken();
            obj.CreatedBy = Convert.ToInt32(claimsPrincipal?.FindFirst("id")?.Value);

            if (!obj.Attachment.IsNullOrEmpty())
            {
                if(this.WriteImageFile(string.Format(@"{0}\${1}", "C:\\Images", "image" + temp.ToString().Replace(":", "")), obj.Attachment))
                    throw new Exception("Error occured while storing images on server.Please contact to your administrator");
            }
            obj.Attachment = "image"+ temp.ToString().Replace(":", "");

             await _context.TaskMasters.AddAsync(obj);

            


            await _context.SaveChangesAsync();
            return obj;
        }
        private bool WriteImageFile(string filePath, String base64String)
        {
            var bytes = Convert.FromBase64String(base64String);
            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }
            return false;
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
            TaskMaster obj= await _context.TaskMasters.Where(e => e.Id == taskId).FirstOrDefaultAsync() ?? throw new Exception("Something went Wrong");

            if (obj.Attachment.IsNullOrEmpty())
                return obj;

            string filePath = @"C:\Images\$" + obj.Attachment;

            if (System.IO.File.Exists(filePath))
            {
                // Get the file extension and determine the content type
                var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();

                // Read the file bytes
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                obj.Attachment= Convert.ToBase64String(fileBytes);
            }
            return obj;
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
