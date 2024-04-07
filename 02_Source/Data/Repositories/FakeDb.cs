using Common.Constants;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public static class FakeDb
    {
        // Use hard set Id instead of generating new id to be easy to test
        public static List<TaskModel> tasks = new List<TaskModel>
        {
        new TaskModel
            {
                Id = "6d7a2d26-5e3b-4f4f-9a1b-4de46b74a510",
                Title = "Task 1",
                Description = "Install and configure necessary software for development environment setup",
                Priority = PriorityEnum.High,
                DueDate = DateTime.Today.AddDays(1)
            },
            new TaskModel
            {
                Id = "eeb7f313-07a3-4e91-8e12-45109ba2088d",
                Title = "Task 2",
                Description = "Develop authentication system using JWT for user authentication",
                Priority = PriorityEnum.High,
                DueDate = DateTime.Today.AddDays(2)
            },
            new TaskModel
            {
                Id = "ccd6dc3a-7fe3-4e76-8b5b-c23fd45651f1",
                Title = "Task 3",
                Description = "Create database schema for the application",
                Priority = PriorityEnum.Medium,
                DueDate = DateTime.Today.AddDays(3)
            },
            new TaskModel
            {
                Id = "cf325d2c-f1c4-4185-b36d-d499f1f6e63c",
                Title = "Task 4",
                Description = "Design and implement user interface for the application",
                Priority = PriorityEnum.Medium,
                DueDate = DateTime.Today.AddDays(4)
            },
            new TaskModel
            {
                Id = "0fc13089-91de-4e5e-8ec5-70cb418e2b70\"",
                Title = "Task 5",
                Description = "Write business logic and application services",
                Priority = PriorityEnum.Medium,
                DueDate = DateTime.Today.AddDays(5)
            },
            new TaskModel
            {
                Id = "0f58b027-f30b-40ee-a035-5b5a4622d6b0",
                Title = "Task 6",
                Description = "Write unit tests to ensure code quality and reliability",
                Priority = PriorityEnum.Low,
                DueDate = DateTime.Today.AddDays(6)
            },
            new TaskModel
            {
                Id = "7c86a17e-dcb1-4a9e-b375-4ed39abde77e",
                Title = "Task 7",
                Description = "Perform integration testing to ensure seamless operation of the application",
                Priority = PriorityEnum.Low,
                DueDate = DateTime.Today.AddDays(7)
            },
            new TaskModel
            {
                Id = "7a52c0ec-7ef6-4a5a-84c6-b682055f7832",
                Title = "Task 8",
                Description = "Deploy application to production server and ensure it's running smoothly",
                Priority = PriorityEnum.High,
                DueDate = DateTime.Today.AddDays(8)
            },
            new TaskModel
            {
                Id = "8532d8cf-3788-4e44-9337-7f7e47e5377d",
                Title = "Task 9",
                Description = "Document codebase and APIs for better maintainability and future reference",
                Priority = PriorityEnum.Low,
                DueDate = DateTime.Today.AddDays(9)
            },
            new TaskModel
            {
                Id = "2a9c5421-5208-4d3d-8f9d-58b67635b121",
                Title = "Task 10",
                Description = "Relax",
                Priority = PriorityEnum.High,
                DueDate = DateTime.Today.AddDays(10)
            }
        };
    }
}
