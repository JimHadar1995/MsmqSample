using System;
using System.Threading.Tasks;
using Experimental.System.Messaging;
using MsmqSample.Common.Infrastructure.Code;

namespace MsmqSample.Common.Infrastructure.Services
{
    /// <summary>
    /// Класс-helper для получения данных из Msmq
    /// </summary>
    public sealed class MsmqConsumer
    {
        private readonly string _queueName;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName"></param>
        public MsmqConsumer(string queueName)
        {
            _queueName = queueName;
        }
        /// <summary>
        /// Получение сообщений.
        /// В случае, если все сообщения успешно прочитаны и обработаны, то они удаляются из очереди.
        /// </summary>
        /// <typeparam name="T">Тип объекта из очереди.</typeparam>
        /// <param name="onMessageReceive">Action, вызываемый при обработке сообщения.</param>
        /// <returns></returns>
        public async Task Consume<T>(Func<T, Task> onMessageReceive)
        {
            if (!MessageQueue.Exists(_queueName))
            {
                using var queueToCreate = MessageQueue.Create(_queueName);
                queueToCreate.Label = _queueName;
            }

            using var queue = new MessageQueue(_queueName);
            queue.Formatter = new XmlMessageFormatter(
                new Type[]
                {
                    typeof(T)
                });

            var messages = queue.GetAllMessages();

            if (onMessageReceive != null)
            {
                //применяем необходимые действия для сообщений из очереди.
                foreach (var message in messages)
                {
                    if (message.Body is T task)
                    {
                        await onMessageReceive.Invoke(task);
                    }
                }
            }

            //сли все завершилось успешно, то удаляем сообщения из очереди
            foreach(var message in messages)
            {
                try
                {
                    queue.ReceiveById(message.Id);
                }
                catch
                {
                    //
                }
            }
        }
    }
}
