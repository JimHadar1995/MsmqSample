using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MsmqSample.Api.Models;
using MsmqSample.Common.Application.Dto;
using MsmqSample.Common.Application.Services;
using MsmqSample.Common.Entities;

namespace MsmqSample.Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с задачами
    /// </summary>
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskService"></param>
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Получение списка всех задач из БД
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<SampleTaskDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<List<SampleTaskDto>> GetAllTasks(CancellationToken token)
            => await _taskService.GetAllTaskAsync(token);

        /// <summary>
        /// Постановка задачи <paramref name="model"/> в очередь
        /// </summary>
        /// <param name="model">Задача для постановки в очередь</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> AddTaskToQueue([FromBody] CreateSampleTaskDto model, CancellationToken token)
        {
            await _taskService.AddTaskToQueue(model, token);
            return Ok("Задача поставлена в очередь");
        }
    }
}
