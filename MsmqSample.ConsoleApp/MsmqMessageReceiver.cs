using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
                catch (Exception ex)
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
                var receivedTasks = new List<SampleTask>(100);
                dbContext.Database.BeginTransaction();
                await _consumer.Consume<CreateSampleTaskDto>(async (taskDto) =>
                {
                    var task = new SampleTask
                    {
                        Description = taskDto.Description
                    };
                    var savedTask = await taskRepo.CreateAsync(task, token);
                    receivedTasks.Add(savedTask);
                });
                dbContext.Database.CommitTransaction();
                if (receivedTasks.Count > 0)
                {
                    Console.WriteLine("----------------");
                    Console.WriteLine("Из очереди прочитаны и сохранены задачи:");
                    receivedTasks.ForEach(t =>
                    {
                        Console.WriteLine($"TaskID: {t.TaskId}, " +
                            $"Description: {t.Description}, " +
                            $"CreateDate: {t.CreateTime.ToLocalTime()}");
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dbContext.Database.RollbackTransaction();
            }
        }
    }
}
