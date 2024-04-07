using API;
using AutoFixture;
using Common.Constants;
using Common.Objects;
using Data.Repositories;
using Domain.Filters;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Repositories
{
    [TestClass]
    public class TaskRepositoryTest
    {

        private readonly Fixture _fixture;

        public TaskRepositoryTest()
        {
            _fixture = new Fixture();
        }

        #region GetByIdAsync
        [TestMethod]
        public async Task GetByIdAsync_Success()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var expectedResult = _fixture.Create<TaskModel>();
            expectedResult.Id = id;

            var mockTasks = _fixture.CreateMany<TaskModel>().ToList();
            mockTasks.Add(expectedResult);
            var taskRepository = new TaskRepository(mockTasks);

            // Action
            TaskModel? result = await taskRepository.GetByIdAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public async Task GetByIdAsync_Fail_NotFoundTask()
        {
            // Arrange
            var notExistId = Guid.NewGuid().ToString();

            var mockTasks = _fixture.CreateMany<TaskModel>().ToList();
            var taskRepository = new TaskRepository(mockTasks);

            // Action
            var result = await taskRepository.GetByIdAsync(notExistId);

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsync_Success()
        {
            // Arrange
            var mockTasks = _fixture.CreateMany<TaskModel>().ToList();
            var expectedResult = mockTasks;

            var taskRepository = new TaskRepository(mockTasks);

            // Action
            IEnumerable<TaskModel> result = await taskRepository.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }
        #endregion

        #region GetListAsync
        [TestMethod]
        public async Task GetListAsync_Success()
        {
            // Arrange
            var mockTasks = _fixture.CreateMany<TaskModel>().ToList();
            var expectedResult = mockTasks;

            var taskRepository = new TaskRepository(mockTasks);

            // Action
            PagedDto<TaskModel> result = await taskRepository.GetListAsync(new TaskFilter());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Count(), result.Data.Count);
        }
        #endregion

        #region InsertAsync
        [TestMethod]
        public async Task InsertAsync_Success()
        {
            // Arrange
            var mockTasks = _fixture.CreateMany<TaskModel>().ToList();

            var taskRepository = new TaskRepository(mockTasks);

            var expectedResult = _fixture.Create<TaskModel>();


            // Action
            TaskModel? result = await taskRepository.InsertAsync(expectedResult);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result);
        }
        #endregion

        #region UpdateAsync
        [TestMethod]
        public async Task UpdateAsync_Success()
        {
            // Arrange
            var mockID = Guid.NewGuid().ToString();
            var taskToUpdate = new TaskModel
            {
                Id = mockID,
                Title = "Updated Title",
                Description = "Updated Description",
                Priority = PriorityEnum.Medium,
                DueDate = DateTime.Now.AddDays(3)
            };

            var existingTask = new TaskModel
            {
                Id = mockID,
                Title = "Original Title",
                Description = "Original Description",
                Priority = PriorityEnum.Low,
                DueDate = DateTime.Now.AddDays(1)
            };

            var mockTasks = _fixture.CreateMany<TaskModel>().ToList();
            mockTasks.Add(existingTask);
            var taskRepository = new TaskRepository(mockTasks);

            // Action
            int result = await taskRepository.UpdateAsync(taskToUpdate);

            // Assert
            Assert.AreEqual(1, result);

            var updatedTask = mockTasks.SingleOrDefault(t => t.Id == mockID);
            Assert.AreEqual(taskToUpdate.Title, updatedTask.Title);
            Assert.AreEqual(taskToUpdate.Description, updatedTask.Description);
            Assert.AreEqual(taskToUpdate.Priority, updatedTask.Priority);
            Assert.AreEqual(taskToUpdate.DueDate, updatedTask.DueDate);
        }

        [TestMethod]
        public async Task UpdateAsync_Fail_NotFoundTask()
        {
            // Arrange
            var mockTasks = _fixture.CreateMany<TaskModel>().ToList();
            var taskRepository = new TaskRepository(mockTasks);

            var taskToUpdate = _fixture.Create<TaskModel>();

            // Action
            var result = await taskRepository.UpdateAsync(taskToUpdate);

            // Assert
            Assert.AreEqual(0, result);
        }
        #endregion


        [TestMethod]
        public async Task DeleteAsync_Success()
        {
            // Arrange
            var taskToDelete = _fixture.Create<TaskModel>();
            string idToDelete = Guid.NewGuid().ToString();
            taskToDelete.Id = idToDelete;

            var mockTasks = _fixture.CreateMany<TaskModel>().ToList();
            mockTasks.Add(taskToDelete);
            var taskRepository = new TaskRepository(mockTasks);

            // Action
            var result = await taskRepository.DeleteAsync(idToDelete);
            
            // Assert
            Assert.IsTrue(!mockTasks.Contains(taskToDelete));
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task DeleteAsync_Fail_NotFoundTask()
        {
            // Arrange
            var notExistId = Guid.NewGuid().ToString();

            var mockTasks = _fixture.CreateMany<TaskModel>().ToList();
            var taskRepository = new TaskRepository(mockTasks);

            // Action
            var result = await taskRepository.DeleteAsync(notExistId);

            // Assert
            Assert.AreEqual(0, result);
        }

        
    }
}
