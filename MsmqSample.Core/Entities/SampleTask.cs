using System;
using System.ComponentModel.DataAnnotations;

namespace MsmqSample.Core.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class SampleTask
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        [Key]
        public int TaskId { get; set; }

        /// <summary>
        /// Описание задачи
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Время создания записи о задаче в БД.
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    }
}
