using Dapper.Contrib.Extensions;
using ISTraining_Part.Core.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.ComponentModel.DataAnnotations;

namespace ISTraining_Part.Core.Models
{
    /// <summary>
    /// Детальная информация о человеке.
    /// </summary>
    [Table("detailInfo")]
    public class DetailInfo : ValidateViewModel, ICloneable
    {
        /// <summary>
        /// Id.
        /// </summary>
        [DoNotNotify]
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [StringLength(255, MinimumLength = 11, ErrorMessage = "{0} должен состоять из минимум 11 цифр")]
        [JsonProperty("p")]
        public string Phone { get; set; }

        /// <summary>
        /// Почта.
        /// </summary>
        [Display(Name = "Почта")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Укажите почту")]
        [JsonProperty("e")]
        public string EMail { get; set; }

        /// <summary>
        /// Адрес проживания.
        /// </summary>
        [Display(Name = "Адрес проживания")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "{0} должен состоять минимум из {2} букв")]
        [JsonProperty("a")]
        public string Address { get; set; }

        /// <summary>
        /// Id сотрудника.
        /// </summary>
        [JsonProperty("si")]
        public int? Staff { get; set; }

        /// <summary>
        /// Id студента.
        /// </summary>
        [JsonProperty("sti")]
        public int? Student { get; set; }

        /// <summary>
        /// Копия.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return $"student: {Student}, staff: {Staff}";
        }
    }
}
