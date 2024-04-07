using API.Controllers;
using AutoFixture;
using AutoMapper;
using Business.Services;
using Common.Objects;
using Dtos.Task;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace UnitTest.Controllers
{
    [TestClass]
    public class TaskControllerTest
    {
        private readonly Fixture _fixture;
        private readonly IMapper _mapper;
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly TaskController _taskController;

        public TaskControllerTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _fixture = new Fixture();
            _taskServiceMock = new Mock<ITaskService>();
            _taskController = new TaskController( _configurationMock.Object, _taskServiceMock.Object);
        }

        #region Insert
        [TestMethod]
        public async Task Insert_Success()
        {
            // Arrange
            var expectedResult = _fixture.Create<TaskDto>();
            var requestDto = _fixture.Create<TaskRequestDto>();

            _taskServiceMock.Setup(c => c.InsertTaskAsync(It.IsAny<TaskRequestDto>())).ReturnsAsync(expectedResult);

            // Action
            ActionResult<TaskDto> result = await _taskController.Insert(requestDto);
            var objectResult = (ObjectResult)result.Result;
            TaskDto dtoResult = (TaskDto)objectResult.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, objectResult.StatusCode);
        }

        // For checking ModelState validation because ModelState.IsValid always returns true in unit test
        private void SimulateValidation(object model, ControllerBase controller)
        {
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string errorKey = validationResult.MemberNames.Count() > 0 ? validationResult.MemberNames.FirstOrDefault() : "Bad Request";
                controller.ModelState.AddModelError(errorKey, validationResult.ErrorMessage);
            }
        }

        [TestMethod]
        public async Task Insert_Fail_EmptyTitle()
        {
            // Arrange
            var invalidDto = new TaskRequestDto()
            {
                Title = String.Empty,
                Description = "",
                Priority = "High",
                DueDate = DateTime.Today.AddDays(1)
            };

            // Action
            SimulateValidation(invalidDto, _taskController);
            var result = await _taskController.Insert(invalidDto);
            var objectResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Insert_Fail_EmptyPriority()
        {
            // Arrange
            var invalidDto = new TaskRequestDto()
            {
                Title = "New Task",
                Description = "Description for new task",
                Priority = "",
                DueDate = DateTime.Today.AddDays(1)
            };

            // Action
            SimulateValidation(invalidDto, _taskController);
            var result = await _taskController.Insert(invalidDto);
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Insert_Fail_InvalidPriority()
        {
            // Arrange
            var invalidDto = new TaskRequestDto()
            {
                Title = "New Task",
                Description = "Description for new task",
                Priority = "InVaLid Priority",
                DueDate = DateTime.Today.AddDays(1)
            };

            // Action
            SimulateValidation(invalidDto, _taskController);
            var result = await _taskController.Insert(invalidDto);
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Insert_Fail_InvalidDueDate()
        {
            // Arrange
            var invalidDto = new TaskRequestDto()
            {
                Title = "New Task",
                Description = "Description for new task",
                Priority = "High",
                DueDate = DateTime.Today.AddDays(-1)
            };

            // Action
            SimulateValidation(invalidDto, _taskController);
            var result = await _taskController.Insert(invalidDto);
            var objectResult = result.Result as ObjectResult;

            // Assert
            Assert.AreEqual(400, objectResult.StatusCode);
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task Update_Success()
        {
            // Arrange
            var requestDto = _fixture.Create<TaskRequestDto>();

            _taskServiceMock.Setup(c => c.UpdateTaskAsync(It.IsAny<TaskRequestDto>(), It.IsAny<string>())).ReturnsAsync(1);

            // Action
            var result = await _taskController.Update(requestDto, It.IsAny<string>());
            var objectResult = (ObjectResult)result.Result;
            int dtoResult = (int)objectResult.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsTrue(dtoResult > 0);
        }

        [TestMethod]
        public async Task Update_Fail_EmptyTitle()
        {
            // Arrange
            var requestDto = _fixture.Create<TaskRequestDto>();
            requestDto.Title = "";

            _taskServiceMock.Setup(c => c.UpdateTaskAsync(It.IsAny<TaskRequestDto>(), It.IsAny<string>())).ReturnsAsync(1);

            // Action
            SimulateValidation(requestDto, _taskController);
            var result = await _taskController.Update(requestDto, It.IsAny<string>());
            var objectResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Update_Fail_InvalidPriority()
        {
            // Arrange
            var requestDto = _fixture.Create<TaskRequestDto>();
            requestDto.Priority = "Invalid Priority";

            _taskServiceMock.Setup(c => c.UpdateTaskAsync(It.IsAny<TaskRequestDto>(), It.IsAny<string>())).ReturnsAsync(1);

            // Action
            SimulateValidation(requestDto, _taskController);
            var result = await _taskController.Update(requestDto, It.IsAny<string>());
            var objectResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(400, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task Update_Fail_InvalidDueDate()
        {
            // Arrange
            var requestDto = _fixture.Create<TaskRequestDto>();
            requestDto.DueDate = DateTime.Now.AddDays(-1);

            _taskServiceMock.Setup(c => c.UpdateTaskAsync(It.IsAny<TaskRequestDto>(), It.IsAny<string>())).ReturnsAsync(1);

            // Action
            SimulateValidation(requestDto, _taskController);
            var result = await _taskController.Update(requestDto, It.IsAny<string>());
            var objectResult = (ObjectResult)result.Result;

            // Assert
            Assert.AreEqual(400, objectResult.StatusCode);
        }
        #endregion

        #region Delete
        [TestMethod]
        public async Task Delete_Success()
        {
            var dto = _fixture.Create<TaskDto>();

            // Arrange
            _taskServiceMock.Setup(c => c.GetTaskByIdAsync(It.IsAny<string>())).ReturnsAsync(dto);
            _taskServiceMock.Setup(c => c.DeleteTaskAsync(It.IsAny<string>())).ReturnsAsync(1);

            // Action
            var result = await _taskController.Delete(It.IsAny<string>());
            var objectResult = (ObjectResult)result.Result;
            int dtoResult = (int)objectResult.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.IsTrue(dtoResult > 0);
        }
        #endregion

        #region Get&GetList
        [TestMethod]
        public async Task Get_Success()
        {
            var dto = _fixture.Create<TaskDto>();

            // Arrange
            _taskServiceMock.Setup(c => c.GetTaskByIdAsync(It.IsAny<string>())).ReturnsAsync(dto);

            // Action
            var result = await _taskController.GetById(It.IsAny<string>());
            var objectResult = (ObjectResult)result.Result;
            TaskDto dtoResult = (TaskDto)objectResult.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.AreEqual(dto, dtoResult);
        }

        [TestMethod]
        public async Task GetList_Success()
        {
            var dtos = _fixture.CreateMany<TaskDto>().ToList();
            var expectedResult = new PagedDto<TaskDto>(5, dtos);

            // Arrange
            _taskServiceMock.Setup(c => c.GetTaskListAsync(It.IsAny<TaskFilterDto>())).ReturnsAsync(expectedResult);

            // Action
            var result = await _taskController.GetList(It.IsAny<TaskFilterDto>());
            var objectResult = (ObjectResult)result.Result;
            PagedDto<TaskDto> pagedDto = (PagedDto<TaskDto>)objectResult.Value;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.AreEqual(expectedResult.Data.Count(), pagedDto.Data.Count());
        }
        #endregion


    }
}
