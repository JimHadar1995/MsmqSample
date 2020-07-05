using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MsmqSample.Common.Application.Repositories;
using MsmqSample.Common.Entities;
using MsmqSample.Common.Infrastructure.Contexts;

namespace MsmqSample.Common.Infrastructure.Repositories
{
    /// <inheritdoc />
    public sealed class TaskRepository : ITaskRepository
    {
        private readonly MsmqSampleDbContext _dbContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public TaskRepository(MsmqSampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<SampleTask> CreateAsync(SampleTask task, CancellationToken token)
        {
            _dbContext.Set<SampleTask>().Add(task);
            await _dbContext.SaveChangesAsync(token);
            return task;
        }

        /// <inheritdoc />
        public Task<List<SampleTask>> GetAllAsync(CancellationToken token)
            => _dbContext.Set<SampleTask>().ToListAsync();
    }
}
