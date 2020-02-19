using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase;
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
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentsHub(IDataBase dataBase)
        {
            this.dataBase = dataBase;
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
        public Task<KursachResponse<IEnumerable<Student>>> GetStudentsAsync()
        {
            return dataBase.GetStudentsAsync();
        }

        /// <summary>
        /// Получение количества студентов в группах.
        /// Ключ - ИД группы.
        /// </summary>
        /// <param name="groupIds">ИДы групп.</param>
        /// <returns></returns>
        public Task<KursachResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds)
        {
            return dataBase.GetStudentsCountAsync(groupIds);
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> AddStudentAsync(Student student)
        {
            Logger.Log.Info($"Add student: {student.FullName}");

            var res = await dataBase.AddStudentAsync(student);

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
        public async Task<KursachResponse<bool>> ImportStudentsAsync(IEnumerable<Student> students)
        {
            Logger.Log.Info($"Import students: {students.Count()}");

            var res = await dataBase.ImportStudentsAsync(students);

            return res;
        }

        /// <summary>
        /// Сохранить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> SaveStudentAsync(Student student)
        {
            Logger.Log.Info($"Save student: {student.FullName}");

            var res = await dataBase.SaveStudentAsync(student);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Update, student);

            return res;
        }

        /// <summary>
        /// Удалить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> RemoveStudentAsync(Student student)
        {
            Logger.Log.Info($"Remove student: {student.FullName}");

            var res = await dataBase.RemoveStudentAsync(student);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).OnChanged(DbChangeStatus.Remove, student);

            return res;
        }
        #endregion
    }
}
