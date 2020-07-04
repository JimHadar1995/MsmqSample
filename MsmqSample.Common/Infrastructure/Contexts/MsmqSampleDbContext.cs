using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MsmqSample.Common.Entities;

namespace MsmqSample.Common.Infrastructure.Contexts
{
    /// <summary>
    /// Db context
    /// </summary>
    public sealed class MsmqSampleDbContext : DbContext
    {
        /// <inheritdoc />
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public MsmqSampleDbContext([NotNull] DbContextOptions options)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
            : base(options)
        {
            //Не использую миграции, так как проект простой.
            Database.EnsureCreated();
        }

        /// <summary>
        /// DbSet для работы с задачами.
        /// </summary>
        public DbSet<SampleTask> Tasks { get; set; }
    }
}
