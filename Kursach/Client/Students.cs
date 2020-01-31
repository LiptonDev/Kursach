using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursach.Client
{
    /// <summary>
    /// Управление студентами.
    /// </summary>
    class Students : IStudents
    {
        /// <summary>
        /// Прокси.
        /// </summary>
        IHubProxy proxy;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Students(IHubConfigurator hubConfigurator, TaskFactory sync)
        {
            proxy = hubConfigurator.Hub.CreateHubProxy(HubNames.StudentsHub);

            proxy.On<DbChangeStatus, Student>(nameof(StudentsEvents.StudentChanged),
                (status, student) => sync.StartNew(() => OnChanged?.Invoke(status, student)));

            proxy.On<int>(nameof(StudentsEvents.StudentsImportTo),
                (groupId) => sync.StartNew(() => Imported?.Invoke(groupId)));
        }

        /// <summary>
        /// Студент изменен.
        /// </summary>
        public event OnChanged<Student> OnChanged;

        /// <summary>
        /// Студенты импортированы.
        /// </summary>
        public event StudentsImported Imported;

        #region Get region
        /// <summary>
        /// Получение студентов определенной группы.
        /// </summary>
        /// <param name="groupId">ИД группы.</param>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<Student>>> GetStudentsAsync(int groupId)
        {
            return proxy.TryInvokeAsync<IEnumerable<Student>>(args: groupId);
        }

        /// <summary>
        /// Получение количества студентов в группах.
        /// Ключ - ИД группы.
        /// </summary>
        /// <param name="groupIds">ИДы групп.</param>
        /// <returns></returns>
        public Task<KursachResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds)
        {
            return proxy.TryInvokeAsync<Dictionary<int, StudentsCount>>(args: groupIds);
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> AddStudentAsync(Student student)
        {
            return proxy.TryInvokeAsync<bool>(args: student);
        }

        /// <summary>
        /// Добавить студентов.
        /// </summary>
        /// <param name="students">Студенты.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> AddStudentsAsync(IEnumerable<Student> students, int groupId)
        {
            return proxy.TryInvokeAsync<bool>(args: new object[] { students, groupId });
        }

        /// <summary>
        /// Сохранить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> SaveStudentAsync(Student student)
        {
            return proxy.TryInvokeAsync<bool>(args: student);
        }

        /// <summary>
        /// Удалить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> RemoveStudentAsync(Student student)
        {
            return proxy.TryInvokeAsync<bool>(args: student);
        }
        #endregion
    }
}
