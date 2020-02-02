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
    class DesignStudents : IStudents
    {
        public DesignStudents(IHubConfigurator hubConfigurator)
        {

        }
        Random rn = new Random();
        public event OnChanged<Student> OnChanged;
        public event StudentsImported Imported;

        public Task<KursachResponse<bool>> AddStudentAsync(Student student)
        {
            OnChanged?.Invoke(DbChangeStatus.Add, student);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }

        public Task<KursachResponse<bool>> AddStudentsAsync(IEnumerable<Student> students, int groupId)
        {
            Imported?.Invoke(groupId);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }

        public Task<KursachResponse<IEnumerable<Student>>> GetStudentsAsync(int groupId)
        {
            var students = Enumerable.Range(0, 15).Select(x => new Student
            {
                LastName = "Фамилия",
                FirstName = "Имя",
                MiddleName = "Отчество",
                Birthdate = DateTime.Now,
                DecreeOfEnrollment = "ДА!",
                Expelled = x % 2 == 0,
                GroupId = x,
                Notice = "NOTICE?!",
                OnSabbatical = x % 5 == 0,
                PoPkNumber = 1337,
                Id = x
            });

            return Task.FromResult(new KursachResponse<IEnumerable<Student>>(KursachResponseCode.Ok, students));
        }

        public Task<KursachResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds)
        {
            var res = new Dictionary<int, StudentsCount>();

            for (int i = 0; i < 15; i++)
            {
                res.Add(i, new StudentsCount(rn.Next(15, 25), rn.Next(0, 5)));
            }

            return Task.FromResult(new KursachResponse<Dictionary<int, StudentsCount>>(KursachResponseCode.Ok, res));
        }

        public Task<KursachResponse<bool>> RemoveStudentAsync(Student student)
        {
            OnChanged?.Invoke(DbChangeStatus.Remove, student);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }

        public Task<KursachResponse<bool>> SaveStudentAsync(Student student)
        {
            OnChanged?.Invoke(DbChangeStatus.Update, student);
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }
    }
}
