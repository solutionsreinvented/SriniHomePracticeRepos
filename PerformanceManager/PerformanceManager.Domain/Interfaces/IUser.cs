﻿using PerformanceManager.Domain.Enums;

namespace PerformanceManager.Domain.Interfaces
{
    public interface IUser
    {
        int Id { get; set; }

        string FullName { get; set; }

        string Password { get; set; }

        UserRole UserRole { get; set; }
    }
}