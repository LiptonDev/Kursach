using Kursach.Core.Models;
using Kursach.Core.ServerMethods;
using System.Threading.Tasks;

namespace Server.DataBase
{
    /// <summary>
    /// База данных.
    /// </summary>
    public interface IDataBase : IUsersHub, IStudentsHub, IStaffHub, IGroupsHub
    {
        /// <summary>
        /// Добавить лог входа в программу.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        void AddSignInLogAsync(User user);
    }
}
