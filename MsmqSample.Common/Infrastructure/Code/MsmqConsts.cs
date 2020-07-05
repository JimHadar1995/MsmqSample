using System;
using System.Collections.Generic;
using System.Text;

namespace MsmqSample.Common.Infrastructure.Code
{
    /// <summary>
    /// Константы для Msmq
    /// </summary>
    public class MsmqConsts
    {
        /// <summary>
        /// Название очереди в MSMQ
        /// </summary>
        public const string MsmqQueueName = @".\Private$\MsmqSample";
    }
}
