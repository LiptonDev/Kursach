using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kursach.Models
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
        /// № по п/к.
        /// </summary>
        [Display(Name = "№ по п/к")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} должен находиться в диапазоне положительных чисел")]
        public int PoPkNumber { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        [Display(Name = "Дата рождения")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// Приказ о зачислении.
        /// </summary>
        [Display(Name = "Приказ о зачислении")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        public string DecreeOfEnrollment { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public string Notice { get; set; }

        /// <summary>
        /// Отчислен.
        /// </summary>
        public bool Expelled { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}