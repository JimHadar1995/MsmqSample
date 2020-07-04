namespace MsmqSample.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalAllowAuthUser
    {
        /// <summary>
        /// Домен
        /// </summary>
        public string Domain { get; set; } = string.Empty;

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string User => $"{Domain}\\{UserName}";
    }
}
