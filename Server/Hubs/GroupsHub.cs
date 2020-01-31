using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Kursach.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    /// <summary>
    /// Хаб групп.
    /// </summary>
    [AuthorizeUser]
    [HubName(HubNames.GroupsHub)]
    public class GroupsHub : Hub<GroupsEvents>, GroupsMethods
    {
        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupsHub(IDataBase dataBase)
        {
            this.dataBase = dataBase;
        }

        #region Get region
        /// <summary>
        /// Получение всех групп.
        /// </summary>
        /// <param name="divisionId">Подразделение (от 0 до 2). -1 - все группы.</param>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<Group>>> GetGroupsAsync(int divisionId = -1)
        {
            return dataBase.GetGroupsAsync(divisionId);
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить группы.
        /// </summary>
        /// <param name="groups">Группы.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> AddGroupsAsync(IEnumerable<Group> groups)
        {
            Logger.Log.Info($"Import groups: {groups.Count()}");

            var res = await dataBase.AddGroupsAsync(groups);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).GroupsImport();

            return res;
        }

        /// <summary>
        /// Добавить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> AddGroupAsync(Group group)
        {
            Logger.Log.Info($"Add group: {group.Name}");

            var res = await dataBase.AddGroupAsync(group);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).GroupChanged(DbChangeStatus.Add, group);

            return res;
        }

        /// <summary>
        /// Сохранить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> SaveGroupAsync(Group group)
        {
            Logger.Log.Info($"Save group: {group.Name}");

            var res = await dataBase.SaveGroupAsync(group);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).GroupChanged(DbChangeStatus.Update, group);

            return res;
        }

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> RemoveGroupAsync(Group group)
        {
            Logger.Log.Info($"Remove group: {group.Name}");

            var res = await dataBase.RemoveGroupAsync(group);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).GroupChanged(DbChangeStatus.Remove, group);

            return res;
        }
        #endregion
    }
}
