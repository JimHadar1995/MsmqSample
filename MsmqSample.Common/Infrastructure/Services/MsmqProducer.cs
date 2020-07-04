using Experimental.System.Messaging;

namespace MsmqSample.Common.Infrastructure.Services
{
    /// <summary>
    /// Класс-helper для отправки данных в Msmq
    /// </summary>
    public sealed class MsmqProducer
    {
        private readonly string _queueName;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName"></param>
        public MsmqProducer(string queueName)
        {
            _queueName = queueName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void Produce<T>(T obj)
        {
            if (!MessageQueue.Exists(_queueName))
            {
                using var queueToCreate = MessageQueue.Create(_queueName);
                queueToCreate.Label = _queueName;
            }

            using var queue = new MessageQueue(_queueName);

            using var message = new Message(obj)
            {
                Recoverable = true
            };
            queue.Send(message);
        }
    }
}
