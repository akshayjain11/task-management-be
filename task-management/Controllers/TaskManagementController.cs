using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task_management.Interfaces;
using task_management.Models;
using task_management.Models.RequestModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace task_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Admin,Manager")]
    public class TaskManagementController : ControllerBase
    {

        private readonly ITaskManagementService _taskManagementService;
        public TaskManagementController(ITaskManagementService taskManagementService)
        {
            _taskManagementService = taskManagementService;
        }

        [HttpGet("get-all-task-list")]
        public async Task<List<TaskMaster>> GetTaskList()
        {
            return await _taskManagementService.GetTaskList();
        }

        [HttpGet("get-my-task-list")]
        [Authorize(Roles = "User,Manager")]
        public async Task<List<TaskMaster>> GetAssignToTaskList()
        {
            return await _taskManagementService.GetAssignToTaskList();
        }

        [HttpGet("get-tasks-createdbyme")]
        [Authorize(Roles = "Manager")]

        public async Task<List<TaskMaster>> GetCreatedByTaskList()
        {
            return await _taskManagementService.GetCreatedByTaskList();
        }
        [HttpGet("{taskId}")]
        public async Task<TaskMaster> GetTaskById(int taskId)
        {
            return await _taskManagementService.GetTaskById(taskId);
        }

        // POST api/<TaskManagementController>
        [HttpPost("add-task")]
        [Authorize(Roles = "Manager")]
        public async Task<TaskMaster> AddTask([FromBody] TaskMaster obj)
        {
            return await _taskManagementService.AddTask(obj);
        }

        // PUT api/<TaskManagementController>/5
        [Authorize(Roles = "User,Manager")]
        [HttpPost("update-task-status")]

        public bool UpdateTaskStatus([FromBody] TaskMasterRequest obj)
        {
            return _taskManagementService.UpdateTaskStatus(obj);
        }

        // DELETE api/<TaskManagementController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]

        public async Task<bool> DeleteTask(int id)
        {
            return await _taskManagementService.DeleteTaskById(id);
        }
    }
}
