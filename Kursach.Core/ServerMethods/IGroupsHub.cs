using Kursach.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursach.Core.ServerMethods
{
    /// <summary>
    /// Список методов хаба групп.
    /// </summary>
    public interface IGroupsHub
    {
        #region Get region
        /// <summary>
        /// Получение всех групп.
        /// </summary>
        /// <param name="divisionId">Подразделение (от 0 до 2). -1 - все группы.</param>
        /// <returns></returns>
        Task<KursachResponse<IEnumerable<Group>>> GetGroupsAsync(int divisionId = -1);
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить группы.
        /// </summary>
        /// <param name="groups">Группы.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> AddGroupsAsync(IEnumerable<Group> groups);

        /// <summary>
        /// Добавить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> AddGroupAsync(Group group);

        /// <summary>
        /// Сохранить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> SaveGroupAsync(Group group);

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> RemoveGroupAsync(Group group);
        #endregion
    }
}
