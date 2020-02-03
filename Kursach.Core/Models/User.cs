using Dapper.Contrib.Extensions;
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
    public class User : LoginUser, ICloneable
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DoNotNotify]
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [RegularExpression("^[а-яА-Я]+$", ErrorMessage = "{0} должно состоять из кириллицы")]
        [JsonProperty("n")]
        public string Name { get; set; }

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
