using Kursach.ViewModels;
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
    class Staff : ValidateViewModel, ICloneable
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

        public override bool Equals(object obj)
        {
            return (obj is Staff staff) && staff.Id == Id;
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {MiddleName}";
        }
    }
}