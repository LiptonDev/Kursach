using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kursach.Core.Models
{
    /// <summary>
    /// Студент.
    /// </summary>
    [Table("students")]
    public class Student : People, ICloneable
    {
        /// <summary>
        /// Id группы.
        /// </summary>
        [Display(Name = "ID группы")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} должен находиться в диапазоне положительных чисел")]
        [JsonProperty("g")]
        public int GroupId { get; set; } = -1;

        /// <summary>
        /// № по п/к.
        /// </summary>
        [Display(Name = "№ по п/к")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} должен находиться в диапазоне положительных чисел")]
        [JsonProperty("po")]
        public int PoPkNumber { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        [Display(Name = "Дата рождения")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        [JsonProperty("bd")]
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// Приказ о зачислении.
        /// </summary>
        [Display(Name = "Приказ о зачислении")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        [JsonProperty("decr")]
        public string DecreeOfEnrollment { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [JsonProperty("n")]
        public string Notice { get; set; }

        /// <summary>
        /// Отчислен.
        /// </summary>
        [JsonProperty("e")]
        public bool Expelled { get; set; }

        /// <summary>
        /// В академ. отпуске.
        /// </summary>
        [JsonProperty("sab")]
        public bool OnSabbatical { get; set; }

        /// <summary>
        /// Копия студента.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}