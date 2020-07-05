using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MsmqSample.Common.Entities;

namespace MsmqSample.Common.Application.Repositories
{
    /// <summary>
    /// Репозиторий БД для работы с задачами.
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Получение всех списка задач.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Список задач.</returns>
        Task<List<SampleTask>> GetAllAsync(CancellationToken token);

        /// <summary>
        /// Создание задачи в БД
        /// </summary>
        /// <param name="task">Задача для создания</param>
        /// <param name="token"></param>
        /// <returns>Идентификатор созданной задачи.</returns>
        Task<SampleTask> CreateAsync(SampleTask task, CancellationToken token);
    }
}
