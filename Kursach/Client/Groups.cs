using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursach.Client
{
    class Groups : IGroups
    {
        /// <summary>
        /// Прокси.
        /// </summary>
        IHubProxy proxy;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Groups(IHubConfigurator hubConfigurator, TaskFactory sync)
        {
            proxy = hubConfigurator.Hub.CreateHubProxy(HubNames.GroupsHub);

            proxy.On<DbChangeStatus, Group>(nameof(GroupsEvents.GroupChanged),
                (status, group) => sync.StartNew(() => OnChanged?.Invoke(status, group)));

            proxy.On<IEnumerable<Group>>(nameof(GroupsEvents.GroupsImport),
                (groups) => sync.StartNew(() => Imported?.Invoke(groups)));
        }

        /// <summary>
        /// Группа изменена.
        /// </summary>
        public event OnChanged<Group> OnChanged;

        /// <summary>
        /// Группы импортированы.
        /// </summary>
        public event GroupsImported Imported;

        #region Get region
        /// <summary>
        /// Получение всех групп.
        /// </summary>
        /// <param name="divisionId">Подразделение (от 0 до 2). -1 - все группы.</param>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<Group>>> GetGroupsAsync(int divisionId = -1)
        {
            return proxy.TryInvokeAsync<IEnumerable<Group>>(args: divisionId);
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить группы.
        /// </summary>
        /// <param name="groups">Группы.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> AddGroupsAsync(IEnumerable<Group> groups)
        {
            return proxy.TryInvokeAsync<bool>(args: groups);
        }

        /// <summary>
        /// Добавить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> AddGroupAsync(Group group)
        {
            return proxy.TryInvokeAsync<bool>(args: group);
        }

        /// <summary>
        /// Сохранить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> SaveGroupAsync(Group group)
        {
            return proxy.TryInvokeAsync<bool>(args: group);
        }

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> RemoveGroupAsync(Group group)
        {
            return proxy.TryInvokeAsync<bool>(args: group);
        }
        #endregion
    }
}
