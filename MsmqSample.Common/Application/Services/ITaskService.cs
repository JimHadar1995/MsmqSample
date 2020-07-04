using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MsmqSample.Common.Application.Dto;
using MsmqSample.Common.Entities;

namespace MsmqSample.Common.Application.Services
{
    /// <summary>
    /// Сервис для работы с задачами
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Получение всех задач.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<SampleTaskDto>> GetAllTaskAsync(CancellationToken token);

        /// <summary>
        /// Постановка задачи в очередь.
        /// </summary>
        /// <param name="model">Модель задачи для постановки в очередь.</param>
        /// <param name="token"></param>
        /// <returns>Асинхронная операция.</returns>
        ValueTask AddTaskToQueue(CreateSampleTaskDto model, CancellationToken token);
    }
}
