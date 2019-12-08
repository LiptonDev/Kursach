using PropertyChanged;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach.DataBase
{
    /// <summary>
    /// Группа.
    /// </summary>
    [Table("groups")]
    class Group
    {
        /// <summary>
        /// Id.
        /// </summary>
        [DoNotNotify]
        public int Id { get; set; }

        /// <summary>
        /// Название группы.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Куратор.
        /// </summary>
        public int CuratorId { get; set; }

        /// <summary>
        /// Куратор.
        /// </summary>
        public virtual Staff Curator { get; set; }

        /// <summary>
        /// Студенты группы.
        /// </summary>
        public virtual ICollection<Student> Students { get; set; }
    }
}
