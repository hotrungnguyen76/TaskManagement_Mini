using AutoFixture;
using AutoMapper;
using Business.MapperProfiles;
using Business.Services;
using Common.Objects;
using Domain.Filters;
using Domain.Models;
using Domain.Repositories;
using Dtos.Task;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTest.Business
{
    [TestClass]
    public class TaskServiceTest
    {
        private readonly Fixture _fixture;
        private readonly IMapper _mapper;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly ITaskService _taskService;

        public TaskServiceTest()
        {
            _fixture = new Fixture();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile<AutoMapperProfile>();
            });

            _mapper = mappingConfig.CreateMapper();

            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_mapper, _taskRepositoryMock.Object);
        }

        [TestMethod]
        public async Task InsertTaskAsync_Success()
        {
            // Arrange
            var expectedResult = _fixture.Create<TaskModel>();
            var requestDto = _fixture.Create<TaskRequestDto>();

            _taskRepositoryMock.Setup(c => c.InsertAsync(It.IsAny<TaskModel>())).ReturnsAsync(expectedResult);

            // Action
            var result = await _taskService.InsertTaskAsync(requestDto);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UpdateTaskAsync_Success()
        {
            // Arrange
            var expectedResult = 1;
            var task = _fixture.Create<TaskModel>();
            var requestDto = _fixture.Create<TaskRequestDto>();

            _taskRepositoryMock.Setup(c => c.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(task);
            _taskRepositoryMock.Setup(c => c.UpdateAsync(It.IsAny<TaskModel>())).ReturnsAsync(expectedResult);

            // Action
            var result = await _taskService.UpdateTaskAsync(requestDto, It.IsAny<string>());

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public async Task DeleteTaskAsync_Success()
        {
            // Arrange
            var expectedResult = 1;
            var requestDto = _fixture.Create<TaskRequestDto>();

            _taskRepositoryMock.Setup(c => c.DeleteAsync(It.IsAny<string>())).ReturnsAsync(expectedResult);

            // Action
            var result = await _taskService.DeleteTaskAsync(It.IsAny<string>());

            // Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public async Task GetTaskByIdAsync_Success()
        {
            // Arrange
            var expectedResult = _fixture.Create<TaskModel>();

            _taskRepositoryMock.Setup(c => c.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(expectedResult);

            // Action
            var result = await _taskService.GetTaskByIdAsync(It.IsAny<string>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetTaskListAsync_Success()
        {
            // Arrange
            var list = _fixture.CreateMany<TaskModel>().ToList();
            var expectedResult = new PagedDto<TaskModel>(list.Count(), list);

            _taskRepositoryMock.Setup(c => c.GetListAsync(It.IsAny<TaskFilter>())).ReturnsAsync(expectedResult);

            // Action
            var result = await _taskService.GetTaskListAsync(It.IsAny<TaskFilterDto>());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.TotalRecords, result.TotalRecords);
        }

        [TestMethod]
        public async Task GetTaskByIdAsync_Fail_NotFound()
        {
            // Arrange
            string taskId = Guid.NewGuid().ToString();

            // Act
            var result = await _taskService.GetTaskByIdAsync(taskId);

            // Assert
            Assert.IsNull(result);
        }
    }
}
