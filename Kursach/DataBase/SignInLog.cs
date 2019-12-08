using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach.DataBase
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

        /// <summary>
        /// Пользователь.
        /// </summary>
        [DoNotNotify]
        public virtual User User { get; set; }
    }
}
