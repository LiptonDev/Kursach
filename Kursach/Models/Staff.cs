using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kursach.Models
{
    /// <summary>
    /// Сотрудник.
    /// </summary>
    [Table("staff")]
    class Staff : People, ICloneable
    {
        /// <summary>
        /// Должность.
        /// </summary>
        [Display(Name = "Должность")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        public string Position { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}