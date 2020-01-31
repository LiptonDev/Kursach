using Dapper.Contrib.Extensions;
using Kursach.Core.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kursach.Core.Models
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    [Table("users")]
    public class User : ValidateViewModel, ICloneable
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
        [JsonProperty("l")]
        public string Login { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} не должен быть больше {1} и не меньше {2} символов")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "{0} должен состоять из латинских букв и цифр")]
        [JsonProperty("p")]
        public string Password { get; set; }

        /// <summary>
        /// Права пользователя.
        /// </summary>
        [JsonProperty("m")]
        public UserMode Mode { get; set; }

        /// <summary>
        /// Копия пользователя.
        /// </summary>
        /// <returns></returns>
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
