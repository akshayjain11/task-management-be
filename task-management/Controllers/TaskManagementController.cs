using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task_management.Interfaces;
using task_management.Models;

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

        [HttpGet("get-task-list")]
        public List<TaskMaster> GetTaskList()
        {
            return _taskManagementService.GetTaskList();
        }

        [HttpGet("{id}")]
        public TaskMaster GetTaskById(int taskId)
        {
            return _taskManagementService.GetTaskById(taskId);
        }

        // POST api/<TaskManagementController>
        [HttpPost("add-task")]
        [Authorize(Roles = "Manager")]

        public TaskMaster AddTask([FromBody] TaskMaster obj)
        {
            return _taskManagementService.AddTask(obj);
        }

        // PUT api/<TaskManagementController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "User,Manager")]

        public bool UpdateTask(int id, [FromBody] TaskMaster obj)
        {
            return _taskManagementService.UpdateTask(obj);

        }

        // DELETE api/<TaskManagementController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]

        public bool DeleteTask(int id)
        {
            return _taskManagementService.DeleteTaskById(id);
        }
    }
}
