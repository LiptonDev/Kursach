using Dapper.Contrib.Extensions;
using PropertyChanged;
using System;

namespace Kursach.Models
{
    /// <summary>
    /// Лог входа в программу.
    /// </summary>
    [Table("signInLogs")]
    class SignInLog
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
        public int UserId { get; set; }

        /// <summary>
        /// Время и дата входа.
        /// </summary>
        [DoNotNotify]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
