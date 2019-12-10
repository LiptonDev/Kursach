using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach.DataBase
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

        /// <summary>
        /// Кураторство в группах.
        /// </summary>
        public virtual ICollection<Group> Groups { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}