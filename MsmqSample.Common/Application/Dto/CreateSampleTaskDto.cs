using System;
using System.Collections.Generic;
using System.Text;

namespace MsmqSample.Common.Application.Dto
{
    /// <summary>
    /// Dto создания задачи для постановки в очередь
    /// </summary>
    public class CreateSampleTaskDto
    {
        /// <summary>
        /// Описание задачи.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
