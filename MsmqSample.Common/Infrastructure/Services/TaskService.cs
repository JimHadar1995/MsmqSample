using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MsmqSample.Common.Application.Dto;
using MsmqSample.Common.Application.Repositories;
using MsmqSample.Common.Application.Services;
using MsmqSample.Common.Entities;

namespace MsmqSample.Common.Infrastructure.Services
{
    /// <inheritdoc />
    public sealed class TaskService : ITaskService
    {
        private readonly MsmqProducer _producer;
        private readonly ITaskRepository _taskRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="producer"></param>
        /// <param name="taskRepository"></param>
        public TaskService(MsmqProducer producer, ITaskRepository taskRepository)
        {
            _producer = producer;
            _taskRepository = taskRepository;
        }
        /// <inheritdoc />
        public ValueTask AddTaskToQueue(CreateSampleTaskDto model, CancellationToken token)
        {
            _producer.Produce(model);
            return new ValueTask();
        }

        /// <inheritdoc />
        public async Task<List<SampleTaskDto>> GetAllTaskAsync(CancellationToken token)
        {
            var tasks = await _taskRepository.GetAllAsync(token);
            return tasks.Select(t => new SampleTaskDto
            {
                TaskId = t.TaskId,
                Description = t.Description,
                CreateTime = t.CreateTime.ToLocalTime()
            }).ToList();
        }
    }
}
