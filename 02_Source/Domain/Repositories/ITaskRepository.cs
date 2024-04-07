using Common.Data;
using Common.Objects;
using Domain.Filters;
using Domain.Models;

namespace Domain.Repositories
{
    public interface ITaskRepository : IRepository<TaskModel>
    {
        public Task<PagedDto<TaskModel>> GetListAsync(TaskFilter filter);

    }
}
