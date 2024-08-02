using task_management.Models;
using task_management.Models.RequestModels;

namespace task_management.Interfaces
{
    public interface ITaskManagementService
    {
        public Task<List<TaskMaster>> GetTaskList();
        public Task<List<TaskMaster>> GetAssignToTaskList();
        public Task<List<TaskMaster>> GetCreatedByTaskList();
        public Task<TaskMaster> GetTaskById(int taskId);
        public Task<TaskMaster> AddTask(TaskMaster obj);
        public bool UpdateTaskStatus(TaskMasterRequest obj);
        public Task<bool> DeleteTaskById(int taskId);

    }
}
