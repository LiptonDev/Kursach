using Kursach.Client.Delegates;
using Kursach.Client.Interfaces;
using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursach.Client.Classes
{
    /// <summary>
    /// Управление сотрудниками.
    /// </summary>
    class Staffs : Invoker, IStaff
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Staffs(IHubConfigurator hubConfigurator, TaskFactory sync) : base(hubConfigurator, HubNames.StaffHub)
        {
            Proxy.On<DbChangeStatus, Staff>(nameof(StaffEvents.StaffChange),
                (status, staff) => sync.StartNew(() => OnChanged?.Invoke(status, staff)));
        }

        /// <summary>
        /// Сотрудник изменен.
        /// </summary>
        public event OnChanged<Staff> OnChanged;

        #region Get region
        /// <summary>
        /// Получение всех работников.
        /// </summary>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<Staff>>> GetStaffsAsync()
        {
            return TryInvokeAsync<IEnumerable<Staff>>();
        }

        /// <summary>
        /// Получить первого (создать если нет) сотрудника.
        /// </summary>
        /// <returns></returns>
        public Task<KursachResponse<int>> GetOrCreateFirstStaffIdAsync()
        {
            return TryInvokeAsync<int>();
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> AddStaffAsync(Staff staff)
        {
            return TryInvokeAsync<bool>(args: staff);
        }

        /// <summary>
        /// Сохранить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> SaveStaffAsync(Staff staff)
        {
            return TryInvokeAsync<bool>(args: staff);
        }

        /// <summary>
        /// Удалить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> RemoveStaffAsync(Staff staff)
        {
            return TryInvokeAsync<bool>(args: staff);
        }
        #endregion
    }
}
