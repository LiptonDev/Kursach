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
    class DesignStudents : IStudents
    {
        public DesignStudents()
        {

        }
        Random rn = new Random();
        public event OnChanged<Student> OnChanged;
        public event StudentsImported Imported;

        public Task RaiseStudentsImported()
        {
            Imported?.Invoke();
            return Task.CompletedTask;
        }

        public Task<ISTrainingPartResponse<bool>> AddStudentAsync(Student student)
        {
            OnChanged?.Invoke(DbChangeStatus.Add, student);
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }

        public Task<ISTrainingPartResponse<IEnumerable<Student>>> GetStudentsAsync(int groupId)
        {
            var students = Enumerable.Range(0, 15).Select(x => new Student
            {
                LastName = $"Фамилия {x}",
                FirstName = $"Имя {x}",
                MiddleName = $"Отчество {x}",
                Birthdate = DateTime.Now,
                DecreeOfEnrollment = "ДА!",
                Expelled = x % 2 == 0,
                GroupId = groupId,
                Notice = "NOTICE?!",
                OnSabbatical = x % 5 == 0,
                PoPkNumber = "1337",
                Id = x
            });

            return Task.FromResult(new ISTrainingPartResponse<IEnumerable<Student>>(ISTrainingPartResponseCode.Ok, students));
        }

        public Task<ISTrainingPartResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds)
        {
            var res = new Dictionary<int, StudentsCount>();

            for (int i = 0; i < 15; i++)
            {
                res.Add(i, new StudentsCount(rn.Next(15, 25), rn.Next(0, 5)));
            }

            return Task.FromResult(new ISTrainingPartResponse<Dictionary<int, StudentsCount>>(ISTrainingPartResponseCode.Ok, res));
        }

        public Task<ISTrainingPartResponse<bool>> RemoveStudentAsync(Student student)
        {
            OnChanged?.Invoke(DbChangeStatus.Remove, student);
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }

        public Task<ISTrainingPartResponse<bool>> SaveStudentAsync(Student student)
        {
            OnChanged?.Invoke(DbChangeStatus.Update, student);
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }

        public Task<ISTrainingPartResponse<bool>> ImportStudentsAsync(IEnumerable<Student> students)
        {
            Imported?.Invoke();
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }
    }
}
