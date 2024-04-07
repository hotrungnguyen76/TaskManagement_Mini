using Common.Objects;
using Dtos.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface ITaskService
    {
        Task<TaskDto?> InsertTaskAsync(TaskRequestDto requestDto);
        Task<int> UpdateTaskAsync(TaskRequestDto requestDto, string id);
        Task<int> DeleteTaskAsync(string id);
        Task<TaskDto?> GetTaskByIdAsync(string id);
        Task<PagedDto<TaskDto>> GetTaskListAsync(TaskFilterDto filterDto);
    }
}
