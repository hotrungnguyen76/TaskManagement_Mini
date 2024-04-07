using Common.Constants;
using Common.Objects;
using Domain.Filters;
using Domain.Models;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public partial class TaskRepository : ITaskRepository
    {
        #region Fields
        private readonly List<TaskModel> tasks;
        #endregion

        public TaskRepository() 
        {
            tasks = FakeDb.tasks;
        }

        public TaskRepository(List<TaskModel> fakeData)
        {
            tasks = fakeData;
        }

        #region Get Filtered Task List
        public Task<PagedDto<TaskModel>> GetListAsync(TaskFilter filter)
        {
            int total = 0;

            List<TaskModel> query = new List<TaskModel>(tasks);

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(task =>
                    task.Title.ToLower().Contains(filter.Keyword.ToLower()) ||
                    (!string.IsNullOrEmpty(task.Description) && task.Description.ToLower().Contains(filter.Keyword.ToLower()))
                ).ToList();
            }

            if (filter.IsOutputTotal) 
            {
                var queryCount = tasks.Select(o => o.Id);
                total = queryCount.Count();
            }

            IOrderedEnumerable<TaskModel> orderedResult;
            switch (filter.OrderBy?.ToLower())
            {
                case "id":
                    orderedResult = filter.IsDescending ? query.OrderByDescending(o => o.Id) : query.OrderBy(o => o.Id);
                    break;
                case "title":
                    orderedResult = filter.IsDescending ? query.OrderByDescending(o => o.Title) : query.OrderBy(o => o.Title);
                    break;
                case "priority":
                    orderedResult = filter.IsDescending ? query.OrderByDescending(o => o.Priority) : query.OrderBy(o => o.Priority);
                    break;
                case "duedate":
                    orderedResult = filter.IsDescending ? query.OrderByDescending(o => o.DueDate) : query.OrderBy(o => o.DueDate);
                    break;

                default:
                    orderedResult = filter.IsDescending ? query.OrderByDescending(o => o.DueDate) : query.OrderBy(o => o.DueDate);
                    break;
            }

            query = orderedResult.ToList();

            query = query.Skip(filter.GetSkip()).Take(filter.GetTake()).ToList();

            return Task.FromResult(new PagedDto<TaskModel>(total, query));
        }
        #endregion

        #region Delete 
        public Task<int> DeleteAsync(object ids)
        {
            if (ids is string id)
            {
                TaskModel? taskToDelete = tasks.SingleOrDefault(e => e.Id == id);
                if (taskToDelete == null)
                {
                    return Task.FromResult(0);
                }
                var removedEntity = tasks.Remove(taskToDelete);
                return Task.FromResult(1);
            }
            else
            {
                return Task.FromResult(0);
            }
        }
        #endregion

        #region Get All Tasks
        public Task<List<TaskModel>> GetAllAsync()
        {
            return Task.FromResult<List<TaskModel>>(tasks);
        }

        public Task<TaskModel?> GetByIdAsync(object ids)
        {
            if (ids is string id)
            {
                var entity = tasks.SingleOrDefault(e => e.Id == id);
                return Task.FromResult(entity);
            }
            else
            {
                return Task.FromResult<TaskModel?>(null);
            }
        }
        #endregion

        #region Insert Task
        public Task<TaskModel?> InsertAsync(TaskModel obj)
        {
            tasks.Add(obj);
            return Task.FromResult<TaskModel?>(obj);
        }
        #endregion

        #region Update Task
        public Task<int> UpdateAsync(TaskModel obj)
        {
            var existingEntity = tasks.FirstOrDefault(e => e.Id == obj.Id);
            if (existingEntity != null)
            {
                existingEntity.Title = obj.Title;
                existingEntity.Description = obj.Description;
                existingEntity.Priority = obj.Priority;
                existingEntity.DueDate = obj.DueDate;

                return Task.FromResult(1);
            }
            else
            {
                return Task.FromResult(0);
            }
        }
        #endregion

    }
}
