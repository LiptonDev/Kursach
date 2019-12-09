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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Students => Group
            modelBuilder.Entity<Student>().HasRequired(x => x.Group).WithMany(x => x.Students).HasForeignKey(x => x.GroupId).WillCascadeOnDelete(true);

            //Group => Curator
            modelBuilder.Entity<Group>().HasRequired(x => x.Curator).WithMany(x => x.Groups).HasForeignKey(x => x.CuratorId).WillCascadeOnDelete(true);
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
