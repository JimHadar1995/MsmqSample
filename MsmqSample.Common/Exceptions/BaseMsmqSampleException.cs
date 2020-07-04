using System;

namespace MsmqSample.Common.Exceptions
{
    /// <summary>
    /// Базовое исключение для приложения
    /// </summary>
    public class BaseMsmqSampleException : Exception
    {
        /// <inheritdoc/>
        public BaseMsmqSampleException()
        {
        }

        /// <inheritdoc/>
        public BaseMsmqSampleException(string? message) : base(message)
        {
        }

        /// <inheritdoc/>
        public BaseMsmqSampleException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
