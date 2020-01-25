using Dapper.Contrib.Extensions;
using Kursach.ViewModels;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kursach.Models
{
    /// <summary>
    /// Группа.
    /// </summary>
    [Table("groups")]
    class Group : ValidateViewModel, ICloneable
    {
        public Group()
        {
        }

        /// <summary>
        /// Id.
        /// </summary>
        [DoNotNotify]
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        /// <summary>
        /// Название группы.
        /// </summary>
        [Display(Name = "Название группы")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [RegularExpression("^[а-яА-Я]{2,2}-[0-9]{2,2}$", ErrorMessage = "{0} должно быть записано в виде 'АА-11'")]
        public string Name { get; set; }

        /// <summary>
        /// Куратор.
        /// </summary>
        [Display(Name = "ID куратора")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} должен находиться в диапазоне положительных чисел")]
        public int CuratorId { get; set; } = -1;

        /// <summary>
        /// Специальность.
        /// </summary>
        [Display(Name = "Специальность")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        public string Specialty { get; set; }

        /// <summary>
        /// Дата начала обучения.
        /// </summary>
        [Display(Name = "Дата начала обучения")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        public DateTime? Start { get; set; }

        /// <summary>
        /// Дата конца обучения.
        /// </summary>
        [Display(Name = "Дата конца обучения")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        public DateTime? End { get; set; }

        /// <summary>
        /// Группа является бюджетной.
        /// </summary>
        public bool IsBudget { get; set; } = true;

        /// <summary>
        /// Копия группы.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            return (obj is Group group) && group.Id == Id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
