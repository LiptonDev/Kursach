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
    }
}
