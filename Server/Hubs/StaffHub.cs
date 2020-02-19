using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Hubs
{
    /// <summary>
    /// Хаб сотрудников.
    /// </summary>
    [AuthorizeUser]
    [HubName(HubNames.StaffHub)]
    public class StaffHub : Hub<IStaffHubEvents>, IStaffHub
    {
        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StaffHub(IDataBase dataBase)
        {
            this.dataBase = dataBase;
        }

        #region Get region
        /// <summary>
        /// Получение всех работников.
        /// </summary>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<Staff>>> GetStaffsAsync()
        {
            return dataBase.GetStaffsAsync();
        }

        /// <summary>
        /// Получить первого (создать если нет) сотрудника.
        /// </summary>
        /// <returns></returns>
        public async Task<KursachResponse<Staff, bool>> GetOrCreateFirstStaffAsync()
        {
            var res = await dataBase.GetOrCreateFirstStaffAsync();

            if (res && res.Arg) //Сотрудник был добавлен
            {
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Add, res.Response);
            }

            return res;
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> AddStaffAsync(Staff staff)
        {
            Logger.Log.Info($"Add staff: {staff.FullName}");

            var res = await dataBase.AddStaffAsync(staff);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Add, staff);

            return res;
        }

        /// <summary>
        /// Сохранить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> SaveStaffAsync(Staff staff)
        {
            Logger.Log.Info($"Save staff: {staff.FullName}");

            var res = await dataBase.SaveStaffAsync(staff);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Update, staff);

            return res;
        }

        /// <summary>
        /// Удалить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> RemoveStaffAsync(Staff staff)
        {
            Logger.Log.Info($"Remove staff: {staff.FullName}");

            var res = await dataBase.RemoveStaffAsync(staff);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Remove, staff);

            return res;
        }
        #endregion
    }
}
