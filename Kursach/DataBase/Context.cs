using Kursach.Models;
using System.Data.Entity;

namespace Kursach.DataBase
{
    class Context : DbContext
    {
        public Context() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SignInLog> SignInLogs { get; set; }
    }
}
