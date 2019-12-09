using System.Data.Entity;

namespace Kursach.DataBase
{
    /// <summary>
    /// База данных.
    /// </summary>
    class Context : DbContext
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public Context() : base("DefaultConnection")
        {
        }

        /// <summary>
        /// Пользователи.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Логи входов.
        /// </summary>
        public DbSet<SignInLog> SignInLogs { get; set; }

        /// <summary>
        /// Сотрудники.
        /// </summary>
        public DbSet<Staff> Staff { get; set; }

        /// <summary>
        /// Группы.
        /// </summary>
        public DbSet<Group> Groups { get; set; }

        /// <summary>
        /// Студенты.
        /// </summary>
        public DbSet<Student> Students { get; set; }
    }
}
