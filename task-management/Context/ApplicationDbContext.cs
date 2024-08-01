using Microsoft.EntityFrameworkCore;
using System.Data;
using task_management.Models;

namespace task_management.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<TaskMaster> TaskMasters { get; set; }

    }
}
