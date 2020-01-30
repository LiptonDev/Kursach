using Dapper.Contrib.Extensions;
using Kursach.ViewModels;
using PropertyChanged;
using System.ComponentModel.DataAnnotations;

namespace Kursach.Models
{
    /// <summary>
    /// Человек.
    /// </summary>
    class People : ValidateViewModel
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
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [RegularExpression("^[а-яА-Я]+$", ErrorMessage = "{0} должно состоять из кириллицы")]
        [AlsoNotifyFor(nameof(FullName))]
        public string MiddleName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        [RegularExpression("^[а-яА-Я]+$", ErrorMessage = "{0} должна состоять из кириллицы")]
        [AlsoNotifyFor(nameof(FullName))]
        public string LastName { get; set; }

        /// <summary>
        /// ФИО.
        /// </summary>
        [Write(false)]
        public string FullName => $"{LastName} {FirstName} {MiddleName}";

        public override bool Equals(object obj)
        {
            return (obj is People people) && people.Id == Id;
        }
    }
}
