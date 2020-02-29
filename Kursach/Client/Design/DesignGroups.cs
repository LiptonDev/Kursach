using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Design
{
    class DesignGroups : IGroups
    {
        public DesignGroups()
        {

        }

        public event OnChanged<Group> OnChanged;
        public event Action Imported;

        public Task<ISTrainingPartResponse<bool>> AddGroupAsync(Group group)
        {
            OnChanged?.Invoke(DbChangeStatus.Add, group);
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }

        public Task<ISTrainingPartResponse<bool>> AddGroupsAsync(IEnumerable<Group> groups)
        {
            Imported?.Invoke();
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }

        public Task<ISTrainingPartResponse<IEnumerable<Group>>> GetGroupsAsync(int divisionId = -1)
        {
            var groups = Enumerable.Range(0, 15).Select(x => new Group
            {
                CuratorId = 0,
                Division = x % 3,
                End = DateTime.Now,
                Start = DateTime.Now,
                IsBudget = x % 2 == 0,
                IsIntramural = x % 3 == 0,
                Name = $"ГР-{x + 1}",
                Specialty = "Специальность",
                SpoNpo = x % 2,
                Id = x,
            });
            return Task.FromResult(new ISTrainingPartResponse<IEnumerable<Group>>(ISTrainingPartResponseCode.Ok, groups));
        }

        public Task<ISTrainingPartResponse<bool>> RemoveGroupAsync(Group group)
        {
            OnChanged?.Invoke(DbChangeStatus.Remove, group);
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }

        public Task<ISTrainingPartResponse<bool>> SaveGroupAsync(Group group)
        {
            OnChanged?.Invoke(DbChangeStatus.Update, group);
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }
    }
}
