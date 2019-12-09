using Kursach.ViewModels;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach.DataBase
{
    /// <summary>
    /// Группа.
    /// </summary>
    [Table("groups")]
    class Group : ValidateViewModel, ICloneable
    {
        /// <summary>
        /// Id.
        /// </summary>
        [DoNotNotify]
        public int Id { get; set; }

        /// <summary>
        /// Название группы.
        /// </summary>
        [Display(Name = "Название группы")]
        [Required(ErrorMessage = "{0} не может быть пустой", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} не должна быть больше {1} и не меньше {2} символов")]
        [RegularExpression("^[а-яА-Я]{2,2}-[0-9]{2,2}$", ErrorMessage = "{0} должно быть записано в виде 'ИТ-41'")]
        public string Name { get; set; }

        /// <summary>
        /// Куратор.
        /// </summary>
        [Display(Name = "ID куратора")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} должен находиться в диапазоне положительных чисел")]
        public int CuratorId { get; set; }

        /// <summary>
        /// Куратор.
        /// </summary>
        public virtual Staff Curator { get; set; }

        /// <summary>
        /// Студенты группы.
        /// </summary>
        public virtual ICollection<Student> Students { get; set; }

        /// <summary>
        /// Копия группы.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Group { Id = Id, Curator = Curator, CuratorId = CuratorId, Name = Name, Students = Students };
        }

        public override bool Equals(object obj)
        {
            return (obj is Group group) && group.Id == Id;
        }
    }
}
