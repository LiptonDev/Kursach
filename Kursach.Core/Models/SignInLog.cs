using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using PropertyChanged;
using System;

namespace ISTraining_Part.Core.Models
{
    /// <summary>
    /// Лог входа в программу.
    /// </summary>
    [Table("signInLogs")]
    public class SignInLog
    {
        /// <summary>
        /// Id.
        /// </summary>
        [DoNotNotify]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// ИД пользователя.
        /// </summary>
        [DoNotNotify]
        [JsonProperty("u")]
        public int UserId { get; set; }

        /// <summary>
        /// Время и дата входа.
        /// </summary>
        [DoNotNotify]
        [JsonProperty("d")]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
