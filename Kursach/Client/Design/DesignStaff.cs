using Kursach.Client.Delegates;
using Kursach.Client.Interfaces;
using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Client.Design
{
    class DesignStaff : IStaff
    {
        public DesignStaff()
        {

        }

        public event OnChanged<Staff> OnChanged;

        public Task<KursachResponse<bool>> AddStaffAsync(Staff staff)
        {
            OnChanged?.Invoke(DbChangeStatus.Add, staff);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }

        public Task<KursachResponse<Staff, bool>> GetOrCreateFirstStaffAsync()
        {
            return Task.FromResult(new KursachResponse<Staff, bool>(KursachResponseCode.Ok, true, GetStaffsAsync().Result.Response.First()));
        }

        public Task<KursachResponse<IEnumerable<Staff>>> GetStaffsAsync()
        {
            var staff = Enumerable.Range(0, 10).Select(x => new Staff
            {
                LastName = "Фамилия",
                FirstName = "Имя",
                MiddleName = "Отчество",
                Position = "Должность",
                Id = x
            });

            return Task.FromResult(new KursachResponse<IEnumerable<Staff>>(KursachResponseCode.Ok, staff));
        }

        public Task<KursachResponse<bool>> RemoveStaffAsync(Staff staff)
        {
            OnChanged?.Invoke(DbChangeStatus.Remove, staff);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }

        public Task<KursachResponse<bool>> SaveStaffAsync(Staff staff)
        {
            OnChanged?.Invoke(DbChangeStatus.Update, staff);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }
    }
}
