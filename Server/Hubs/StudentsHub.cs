using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    /// <summary>C:\Users\Alexandr\source\repos\Kursach\Server\Hubs\StudentsHub.cs
    /// Хаб студентов.
    /// </summary>
    [AuthorizeUser]
    [HubName(HubNames.StudentsHub)]
    public class StudentsHub : Hub<IStudentsHubEvents>, IStudentsHub
    {
        /// <summary>
        /// Репозиторий студентов.
        /// </summary>
        readonly IStudentsRepository repository;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentsHub(IStudentsRepository repository)
        {
            this.repository = repository;
        }

        #region Other region
        /// <summary>
        /// Вызвать событие, что студенты импортированы.
        /// </summary>
        /// <returns></returns>
        public Task RaiseStudentsImported()
        {
            Clients.Group(Consts.AuthorizedGroup).StudentsImported();
            return Task.CompletedTask;
        }
        #endregion

        #region Get region
        /// <summary>
        /// Получение студентов определенной группы.
        /// </summary>
        /// <param name="groupId">ИД группы.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<IEnumerable<Student>>> GetStudentsAsync(int groupId)
        {
            return repository.GetStudentsAsync(groupId);
        }

        /// <summary>
        /// Получение количества студентов в группах.
        /// Ключ - ИД группы.
        /// </summary>
        /// <param name="groupIds">ИДы групп.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds)
        {
            return repository.GetStudentsCountAsync(groupIds);
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<ISTrainingPartResponse<bool>> AddStudentAsync(Student student)
        {
            Logger.Log.Info($"Add student: {student.FullName}");

            var res = await repository.AddStudentAsync(student);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Add, student);

            return res;
        }

        /// <summary>
        /// Добавить студентов.
        /// </summary>
        /// <param name="students">Студенты.</param>
        /// <param name="groupId">ИД группы.</param>
        /// <returns></returns>
        public async Task<ISTrainingPartResponse<bool>> ImportStudentsAsync(IEnumerable<Student> students)
        {
            Logger.Log.Info($"Import students: {students.Count()}");

            var res = await repository.ImportStudentsAsync(students);

            return res;
        }

        /// <summary>
        /// Сохранить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<ISTrainingPartResponse<bool>> SaveStudentAsync(Student student)
        {
            Logger.Log.Info($"Save student: {student.FullName}");

            var res = await repository.SaveStudentAsync(student);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Update, student);

            return res;
        }

        /// <summary>
        /// Удалить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<ISTrainingPartResponse<bool>> RemoveStudentAsync(Student student)
        {
            Logger.Log.Info($"Remove student: {student.FullName}");

            var res = await repository.RemoveStudentAsync(student);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Remove, student);

            return res;
        }
        #endregion
    }
}
