using task_management.Models;

namespace task_management.Interfaces
{
    public interface ITaskManagementService
    {
        public List<TaskMaster> GetTaskList();
        public TaskMaster GetTaskById(int taskId);
        public TaskMaster AddTask(TaskMaster obj);
        public bool UpdateTask(TaskMaster obj);
        public bool DeleteTaskById(int taskId);

    }
}
