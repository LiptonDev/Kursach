using Kursach.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursach.Core.ServerMethods
{
    /// <summary>
    /// Список методов хаба сотрудников.
    /// </summary>
    public interface StaffMethods
    {
        #region Get region
        /// <summary>
        /// Получение всех работников.
        /// </summary>
        /// <returns></returns>
        Task<KursachResponse<IEnumerable<Staff>>> GetStaffsAsync();

        /// <summary>
        /// Получить первого (создать если нет) сотрудника.
        /// </summary>
        /// <returns></returns>
        Task<KursachResponse<int>> GetOrCreateFirstStaffIdAsync();
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> AddStaffAsync(Staff staff);

        /// <summary>
        /// Сохранить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> SaveStaffAsync(Staff staff);

        /// <summary>
        /// Удалить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> RemoveStaffAsync(Staff staff);
        #endregion
    }
}
