using Dapper.Contrib.Extensions;
using Kursach.Core.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;
using System.ComponentModel.DataAnnotations;

namespace Kursach.Core.Models
{
    /// <summary>
    /// Человек.
    /// </summary>
    public class People : ValidateViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [RegularExpression("^[а-яА-Я]+$", ErrorMessage = "{0} должно состоять из кириллицы")]
        [AlsoNotifyFor(nameof(FullName))]
        [JsonProperty("fn")]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [RegularExpression("^[а-яА-Я]+$", ErrorMessage = "{0} должно состоять из кириллицы")]
        [AlsoNotifyFor(nameof(FullName))]
        [JsonProperty("mn")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        [RegularExpression("^[а-яА-Я]+$", ErrorMessage = "{0} должна состоять из кириллицы")]
        [AlsoNotifyFor(nameof(FullName))]
        [JsonProperty("ln")]
        public string LastName { get; set; }

        /// <summary>
        /// ФИО.
        /// </summary>
        [Write(false)]
        [JsonIgnore]
        [ChangeIgnore]
        public string FullName => $"{LastName} {FirstName} {MiddleName}";

        public override bool Equals(object obj)
        {
            return (obj is People people) && people.Id == Id;
        }
    }
}
