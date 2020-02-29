using ISTraining_Part.Client.Delegates;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{
    /// <summary>
    /// Управление студентами.
    /// </summary>
    class Students : Invoker, IStudents
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Students(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.StudentsHub)
        {
            Proxy.On<DbChangeStatus, Student>(nameof(IStudentsHubEvents.OnChanged),
                (status, student) => OnChanged?.Invoke(status, student));

            Proxy.On(nameof(IStudentsHubEvents.StudentsImported),
                () => Imported?.Invoke());
        }

        /// <summary>
        /// Студент изменен.
        /// </summary>
        public event OnChanged<Student> OnChanged;

        /// <summary>
        /// Студенты импортированы.
        /// </summary>
        public event StudentsImported Imported;

        #region Other region
        /// <summary>
        /// Вызвать событие, что студенты импортированы.
        /// </summary>
        /// <returns></returns>
        public Task RaiseStudentsImported()
        {
            Logger.Log.Info("Вызов события импортирования студентов");

            return TryInvokeAsync();
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
            Logger.Log.Info($"Получение списка студентов: {{group: {groupId}}}");

            return TryInvokeAsync(args: groupId, defaultValue: Enumerable.Empty<Student>());
        }

        /// <summary>
        /// Получение количества студентов в группах.
        /// Ключ - ИД группы.
        /// </summary>
        /// <param name="groupIds">ИДы групп.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds)
        {
            Logger.Log.Info($"Получение количества студентов в группах: {{group ids: {string.Join(",", groupIds)}}}");

            return TryInvokeAsync(args: groupIds, defaultValue: new Dictionary<int, StudentsCount>());
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> AddStudentAsync(Student student)
        {
            Logger.Log.Info($"Добавление студента: {{id: {student.Id}}}");

            return TryInvokeAsync<bool>(args: student);
        }

        /// <summary>
        /// Добавить студентов.
        /// </summary>
        /// <param name="students">Студенты.</param>
        /// <param name="groupId">ИД группы.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> ImportStudentsAsync(IEnumerable<Student> students)
        {
            Logger.Log.Info($"Добавление студентов: {{count: {students.Count()}}}");

            return TryInvokeAsync<bool>(args: students);
        }

        /// <summary>
        /// Сохранить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> SaveStudentAsync(Student student)
        {
            Logger.Log.Info($"Сохранение студента: {{id: {student.Id}}}");

            return TryInvokeAsync<bool>(args: student);
        }

        /// <summary>
        /// Удалить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> RemoveStudentAsync(Student student)
        {
            Logger.Log.Info($"Удаление студента: {{id: {student.Id}}}");

            return TryInvokeAsync<bool>(args: student);
        }
        #endregion
    }
}
