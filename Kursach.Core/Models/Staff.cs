using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISTraining_Part.Core.Models
{
    /// <summary>
    /// Сотрудник.
    /// </summary>
    [Table("staff")]
    public class Staff : People, ICloneable
    {
        /// <summary>
        /// Должность.
        /// </summary>
        [Display(Name = "Должность")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        [JsonProperty("p")]
        public string Position { get; set; }

        /// <summary>
        /// Копия сотрудника.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}