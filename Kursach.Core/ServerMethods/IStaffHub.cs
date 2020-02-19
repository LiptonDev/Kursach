using ISTraining_Part.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISTraining_Part.Core.ServerMethods
{
    /// <summary>
    /// Список методов хаба сотрудников.
    /// </summary>
    public interface IStaffHub
    {
        #region Get region
        /// <summary>
        /// Получение всех работников.
        /// </summary>
        /// <returns></returns>
        Task<KursachResponse<IEnumerable<Staff>>> GetStaffsAsync();

        /// <summary>
        /// Получить первого (создать если нет) сотрудника.
        /// bool = true - сотрудник создан.
        /// </summary>
        /// <returns></returns>
        Task<KursachResponse<Staff, bool>> GetOrCreateFirstStaffAsync();
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
