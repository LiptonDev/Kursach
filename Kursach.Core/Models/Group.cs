using Dapper.Contrib.Extensions;
using ISTraining_Part.Core.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISTraining_Part.Core.Models
{
    /// <summary>
    /// Группа.
    /// </summary>
    [Table("groups")]
    public class Group : ValidateViewModel, ICloneable
    {
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
        [RegularExpression("^[а-яА-Я]{1,3}-[0-9]{1,3}$", ErrorMessage = "{0} должно быть записано в виде 'АА-11'")]
        [JsonProperty("n")]
        public string Name { get; set; }

        /// <summary>
        /// Куратор.
        /// </summary>
        [Display(Name = "ID куратора")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} должен находиться в диапазоне положительных чисел")]
        [JsonProperty("c")]
        public int CuratorId { get; set; } = -1;

        /// <summary>
        /// Специальность.
        /// </summary>
        [Display(Name = "Специальность")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        [JsonProperty("s")]
        public string Specialty { get; set; }

        /// <summary>
        /// Дата начала обучения.
        /// </summary>
        [Display(Name = "Дата начала обучения")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        [JsonProperty("st")]
        public DateTime? Start { get; set; }

        /// <summary>
        /// Дата конца обучения.
        /// </summary>
        [Display(Name = "Дата конца обучения")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        [JsonProperty("en")]
        public DateTime? End { get; set; }

        /// <summary>
        /// Группа является бюджетной.
        /// </summary>
        [JsonProperty("b")]
        public bool IsBudget { get; set; } = true;

        /// <summary>
        /// Подразделение.
        /// </summary>
        [Display(Name = "Подразделение")]
        [Range(0, 2, ErrorMessage = "{0} должно находится в диапазоне от 0 до 2")]
        [JsonProperty("d")]
        public int Division { get; set; }

        /// <summary>
        /// СПО/НПО/ОВЗ.
        /// </summary>
        [Display(Name = "Образование")]
        [Range(0, 2, ErrorMessage = "{0} должно находиться в диапазоне от 0 до 2")]
        [JsonProperty("sp")]
        public int SpoNpo { get; set; }

        /// <summary>
        /// Группа является очной группой.
        /// </summary>
        [JsonProperty("i")]
        public bool IsIntramural { get; set; } = true;

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
