using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach.DataBase
{
    /// <summary>
    /// Студент.
    /// </summary>
    [Table("students")]
    class Student
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Id группы.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Группа.
        /// </summary>
        public virtual Group Group { get; set; }
    }
}