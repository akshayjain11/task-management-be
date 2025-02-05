﻿using System.Data;
using task_management.Models;
using task_management.Models.RequestModels;

namespace task_management.Interfaces
{
    public interface IAuthService
    {
        User AddUser(User user);
        string Login(LoginRequest loginRequest);
        Role AddRole(Role role);
        bool AssignRoleToUser(AddUserRole obj);
    }
}
