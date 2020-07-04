using System;
using System.Collections.Generic;
using System.Text;
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
        public Task<int> CreateAsync(SampleTask task, CancellationToken token)
        {
            _dbContext.Set<SampleTask>().Add(task);
            return _dbContext.SaveChangesAsync(token);
        }

        /// <inheritdoc />
        public Task<List<SampleTask>> GetAllAsync(CancellationToken token)
            => _dbContext.Set<SampleTask>().ToListAsync();
    }
}
