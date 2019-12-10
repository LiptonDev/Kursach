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

            modelBuilder.Entity<Group>().HasMany(x => x.Students).WithRequired(x => x.Group).HasForeignKey(x => x.GroupId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Group>().HasRequired(x => x.Curator).WithMany(x => x.Groups).HasForeignKey(x => x.CuratorId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Student>().HasRequired(x => x.Group).WithMany(x => x.Students).HasForeignKey(x => x.GroupId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Staff>().HasMany(x => x.Groups).WithRequired(x => x.Curator).HasForeignKey(x => x.CuratorId).WillCascadeOnDelete(true);
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
