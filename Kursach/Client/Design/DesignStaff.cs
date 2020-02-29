using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Design
{
    class DesignStaff : IStaff
    {
        public DesignStaff()
        {

        }

        public event OnChanged<Staff> OnChanged;

        public Task<ISTrainingPartResponse<bool>> AddStaffAsync(Staff staff)
        {
            OnChanged?.Invoke(DbChangeStatus.Add, staff);
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }

        public Task<ISTrainingPartResponse<Staff, bool>> GetOrCreateFirstStaffAsync()
        {
            return Task.FromResult(new ISTrainingPartResponse<Staff, bool>(ISTrainingPartResponseCode.Ok, true, GetStaffsAsync().Result.Response.First()));
        }

        public Task<ISTrainingPartResponse<IEnumerable<Staff>>> GetStaffsAsync()
        {
            var staff = Enumerable.Range(0, 10).Select(x => new Staff
            {
                LastName = "Фамилия",
                FirstName = "Имя",
                MiddleName = "Отчество",
                Position = "Должность",
                Id = x
            });

            return Task.FromResult(new ISTrainingPartResponse<IEnumerable<Staff>>(ISTrainingPartResponseCode.Ok, staff));
        }

        public Task<ISTrainingPartResponse<bool>> RemoveStaffAsync(Staff staff)
        {
            OnChanged?.Invoke(DbChangeStatus.Remove, staff);
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }

        public Task<ISTrainingPartResponse<bool>> SaveStaffAsync(Staff staff)
        {
            OnChanged?.Invoke(DbChangeStatus.Update, staff);
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }
    }
}
