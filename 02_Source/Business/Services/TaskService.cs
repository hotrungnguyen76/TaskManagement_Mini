using AutoMapper;
using Common.Constants;
using Common.Objects;
using Domain.Filters;
using Domain.Models;
using Domain.Repositories;
using Dtos.Task;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class TaskService : ITaskService
    {

        #region Fields
        private readonly IMapper _mapper;
        protected readonly ITaskRepository _taskRepository;
        #endregion

        #region Constructors
        public TaskService(
            IMapper mapper,
            ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
        }
        #endregion

        #region Get By ID
        public async Task<TaskDto?> GetTaskByIdAsync(string id)
        {
            TaskModel? task = await _taskRepository.GetByIdAsync(id);
            if (task != null)
            {
                return _mapper.Map<TaskModel, TaskDto>(task);
            }

            return null;
        }
        #endregion

        #region Get Task List
        public async Task<PagedDto<TaskDto>> GetTaskListAsync(TaskFilterDto filterDto)
        {
            PagedDto<TaskModel> dt = await _taskRepository.GetListAsync(_mapper.Map<TaskFilterDto, TaskFilter>(filterDto));

            List<TaskDto> dtos = new List<TaskDto>();
            foreach (TaskModel item in dt.Data)
            {
                dtos.Add(_mapper.Map<TaskModel, TaskDto>(item));
            }

            return new PagedDto<TaskDto>(dt.TotalRecords, dtos);
        }
        #endregion

        public async Task<TaskDto?> InsertTaskAsync(TaskRequestDto dto)
        {
            TaskModel task = new TaskModel();
            task.Id = Guid.NewGuid().ToString();
            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;
            // dto always can be parsed to enum because having ModelState validation check before
            task.Priority = (PriorityEnum)Enum.Parse(typeof(PriorityEnum), dto.Priority, true);

            TaskModel? newTask = await _taskRepository.InsertAsync(task);

            if (newTask != null)
            {
                return _mapper.Map<TaskModel, TaskDto> (newTask);
            }

            return null;
        }

        public async Task<int> UpdateTaskAsync(TaskRequestDto requestDto, string id)
        {
            TaskModel? task = await _taskRepository.GetByIdAsync(id);
            if (task != null)
            {
                TaskDto taskDto = new TaskDto()
                {
                    Id = id,
                    Title = requestDto.Title,
                    Description = requestDto.Description,
                    Priority = (PriorityEnum)Enum.Parse(typeof(PriorityEnum), requestDto.Priority, true),
                    DueDate = requestDto.DueDate,
                };
                return await _taskRepository.UpdateAsync(_mapper.Map<TaskDto, TaskModel>(taskDto));
            }
            return 0;
        }

        public async Task<int> DeleteTaskAsync(string id)
        {
            return await _taskRepository.DeleteAsync(id);
        }
     
    }
}
