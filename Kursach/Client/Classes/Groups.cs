using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{
    /// <summary>
    /// Управление группами.
    /// </summary>
    class Groups : Invoker, IGroups
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Groups(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.GroupsHub)
        {
            Proxy.On<DbChangeStatus, Group>(nameof(IGroupsHubEvents.OnChanged),
                (status, group) => OnChanged?.Invoke(status, group));

            Proxy.On(nameof(IGroupsHubEvents.GroupsImport), () => Imported?.Invoke());
        }

        /// <summary>
        /// Группа изменена.
        /// </summary>
        public event OnChanged<Group> OnChanged;

        /// <summary>
        /// Группы импортированы.
        /// </summary>
        public event Action Imported;

        #region Get region
        /// <summary>
        /// Получение всех групп.
        /// </summary>
        /// <param name="divisionId">Подразделение (от 0 до 2). -1 - все группы.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<IEnumerable<Group>>> GetGroupsAsync(int divisionId = -1)
        {
            Logger.Log.Info($"Получение списка групп: {{division: {divisionId}}}");

            return TryInvokeAsync(args: divisionId, defaultValue: Enumerable.Empty<Group>());
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить группы.
        /// </summary>
        /// <param name="groups">Группы.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> AddGroupsAsync(IEnumerable<Group> groups)
        {
            Logger.Log.Info($"Добавление групп: {{name: {string.Join(",", groups)}}}");

            return TryInvokeAsync<bool>(args: groups);
        }

        /// <summary>
        /// Добавить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> AddGroupAsync(Group group)
        {
            Logger.Log.Info($"Добавление группы: {{id: {group.Id}}}");

            return TryInvokeAsync<bool>(args: group);
        }

        /// <summary>
        /// Сохранить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> SaveGroupAsync(Group group)
        {
            Logger.Log.Info($"Сохранение группы: {{id: {group.Id}}}");

            return TryInvokeAsync<bool>(args: group);
        }

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> RemoveGroupAsync(Group group)
        {
            Logger.Log.Info($"Удаление группы: {{id: {group.Id}}}");

            return TryInvokeAsync<bool>(args: group);
        }
        #endregion
    }
}
