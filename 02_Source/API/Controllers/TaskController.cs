using Business.Services;
using Common.Objects;
using Dtos.Task;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        #region Fields
        private readonly IConfiguration _config;
        private readonly ITaskService _taskService;
        #endregion

        #region Constructors
        public TaskController(
            IConfiguration configuration,
            ITaskService taskService)
        {
            _config = configuration;
            _taskService = taskService;
        }
        #endregion

        #region Insert Task
        [HttpPost]
        public async Task<ActionResult<TaskDto?>> Insert([FromBody] TaskRequestDto taskRequestDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errors);
            }

            TaskDto? taskDto = await _taskService.InsertTaskAsync(taskRequestDto);

            if (taskDto != null)
            {
                return Ok(taskDto);
            }

            return StatusCode(500);
        }
        #endregion

        #region Get Task List
        [HttpGet]
        public async Task<ActionResult<PagedDto<TaskDto>>> GetList([FromQuery] TaskFilterDto filterDto)
        {
            return Ok(await _taskService.GetTaskListAsync(filterDto));
        }
        #endregion

        #region Get Task
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto?>> GetById(string id)
        {
            TaskDto? taskDto = await _taskService.GetTaskByIdAsync(id);

            if (taskDto == null)
            {
                return NotFound();
            }

            return Ok(taskDto);
        }
        #endregion

        #region Delete Task
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(string id)
        {
            TaskDto? taskDto = await _taskService.GetTaskByIdAsync(id);
            if (taskDto == null)
            {
                return NotFound();
            }

            int total = await _taskService.DeleteTaskAsync(id);
            if (total > 0)
            {
                return Ok(total);
            }

            return StatusCode(500);
        }
        #endregion

        #region Update Task
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update([FromBody] TaskRequestDto taskRequestDto, string id)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                return BadRequest(errors);
            }

            int total = await _taskService.UpdateTaskAsync(taskRequestDto, id);
            if (total > 0)
            {
                return Ok(total);
            }

            return StatusCode(500);
        }
        #endregion
    }
}
