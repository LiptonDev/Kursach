using Kursach.Client.Delegates;
using Kursach.Client.Interfaces;
using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Client.Design
{
    class DesignGroups : IGroups
    {
        public DesignGroups(IHubConfigurator hubConfigurator)
        {

        }

        public event OnChanged<Group> OnChanged;
        public event GroupsImported Imported;

        public Task<KursachResponse<bool>> AddGroupAsync(Group group)
        {
            OnChanged?.Invoke(DbChangeStatus.Add, group);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }

        public Task<KursachResponse<bool>> AddGroupsAsync(IEnumerable<Group> groups)
        {
            Imported?.Invoke();
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }

        public Task<KursachResponse<IEnumerable<Group>>> GetGroupsAsync(int divisionId = -1)
        {
            var groups = Enumerable.Range(0, 5).Select(x => new Group
            {
                CuratorId = 0,
                Division = divisionId,
                End = DateTime.Now,
                Start = DateTime.Now,
                IsBudget = x % 2 == 0,
                IsIntramural = x % 3 == 0,
                Name = $"ГР-{x + 1}",
                Specialty = "Специальность",
                SpoNpo = x % 2,
                Id = x,
            });
            return Task.FromResult(new KursachResponse<IEnumerable<Group>>(KursachResponseCode.Ok, groups));
        }

        public Task<KursachResponse<bool>> RemoveGroupAsync(Group group)
        {
            OnChanged?.Invoke(DbChangeStatus.Remove, group);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }

        public Task<KursachResponse<bool>> SaveGroupAsync(Group group)
        {
            OnChanged?.Invoke(DbChangeStatus.Update, group);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }
    }
}
