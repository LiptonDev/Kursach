using Dapper.Contrib.Extensions;
using Kursach.Models;
using Kursach.ViewModels;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kursach.Models
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    [Table("users")]
    class User : ValidateViewModel, ICloneable
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DoNotNotify]
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        /// <summary>
        /// User login.
        /// </summary>
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} не должен быть больше {1} и не меньше {2} символов")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "{0} должен состоять из латинских букв и цифр")]
        public string Login { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} не должен быть больше {1} и не меньше {2} символов")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "{0} должен состоять из латинских букв и цифр")]
        public string Password { get; set; }

        /// <summary>
        /// Права пользователя.
        /// </summary>
        public UserMode Mode { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            return (obj is User user) && user.Id == Id;
        }
    }
}
