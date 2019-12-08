using DevExpress.Mvvm;
using Kursach.ViewModels;
using PropertyChanged;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach.DataBase
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    [Table("users")]
    class User : ValidateViewModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DoNotNotify]
        public int Id { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "{0} не может быть пустым", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} не должен быть больше {1} и не меньше {2} символов")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "{0} должен состоять из латинских букв и цифр")]
        public string Password { get; set; }

        /// <summary>
        /// Права пользователя.
        /// </summary>
        public UserMode Mode { get; set; }
    }
}
