using PropertyChanged;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach.Models
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    [Table("users")]
    class User
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DoNotNotify]
        public int Id { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        [DoNotNotify]
        public string Login { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [DoNotNotify]
        public string Password { get; set; }

        /// <summary>
        /// Права пользователя.
        /// </summary>
        [DoNotNotify]
        public UserMode Mode { get; set; }
    }
}
