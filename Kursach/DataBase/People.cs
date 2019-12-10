using Kursach.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Kursach.DataBase
{
    /// <summary>
    /// Человек.
    /// </summary>
    class People : ValidateViewModel
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [Display(Name = "Имя")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [RegularExpression("^[а-яА-Я]+$", ErrorMessage = "{0} должно состоять из кириллицы")]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [RegularExpression("^[а-яА-Я]+$", ErrorMessage = "{0} должно состоять из кириллицы")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        [RegularExpression("^[а-яА-Я]+$", ErrorMessage = "{0} должна состоять из кириллицы")]
        public string LastName { get; set; }

        public override bool Equals(object obj)
        {
            return (obj is People people) && people.Id == Id;
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {MiddleName}";
        }
    }
}
