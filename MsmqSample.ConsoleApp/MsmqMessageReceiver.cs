using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MsmqSample.Common.Application.Dto;
using MsmqSample.Common.Entities;
using MsmqSample.Common.Infrastructure.Code;
using MsmqSample.Common.Infrastructure.Contexts;
using MsmqSample.Common.Infrastructure.Repositories;
using MsmqSample.Common.Infrastructure.Services;

namespace MsmqSample.ConsoleApp
{

    internal class MsmqMessageReceiver
    {
        private readonly MsmqConsumer _consumer;
        private readonly string _dbConnectionString;
        public MsmqMessageReceiver(string dbConnectionString)
        {
            _consumer = new MsmqConsumer(MsmqConsts.MsmqQueueName);
            _dbConnectionString = dbConnectionString;
        }

        internal async Task StartListen(CancellationToken token = default)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    //обрабатываем сообщения
                    await ReceiveTasks(token);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //засыпаем на 10 секунд
                await Task.Delay(TimeSpan.FromSeconds(10), token);
            }
        }

        private MsmqSampleDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MsmqSampleDbContext>();
            optionsBuilder.UseSqlServer(_dbConnectionString);

            return new MsmqSampleDbContext(optionsBuilder.Options);
        }

        private async Task ReceiveTasks(CancellationToken token)
        {
            using var dbContext = CreateDbContext();
            var taskRepo = new TaskRepository(dbContext);
            try
            {
                dbContext.Database.BeginTransaction();
                await _consumer.Consume<CreateSampleTaskDto>(async (taskDto) =>
                {
                    var task = new SampleTask
                    {
                        Description = taskDto.Description
                    };
                    await taskRepo.CreateAsync(task, token);
                });
                dbContext.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dbContext.Database.RollbackTransaction();
            }
        }
    }
}
