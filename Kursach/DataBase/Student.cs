using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach.DataBase
{
    /// <summary>
    /// Студент.
    /// </summary>
    [Table("students")]
    class Student : People, ICloneable
    {
        /// <summary>
        /// Id группы.
        /// </summary>
        [Display(Name = "ID группы")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} должен находиться в диапазоне положительных чисел")]
        public int GroupId { get; set; } = -1;

        /// <summary>
        /// Группа.
        /// </summary>
        public virtual Group Group { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}