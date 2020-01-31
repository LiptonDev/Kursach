using Kursach.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kursach.Core.ServerMethods
{
    /// <summary>
    /// Список методов хаба студентов.
    /// </summary>
    public interface StudentsMethods
    {
        #region Get region
        /// <summary>
        /// Получение студентов определенной группы.
        /// </summary>
        /// <param name="groupId">ИД группы.</param>
        /// <returns></returns>
        Task<KursachResponse<IEnumerable<Student>>> GetStudentsAsync(int groupId);

        /// <summary>
        /// Получение количества студентов в группах.
        /// Ключ - ИД группы.
        /// </summary>
        /// <param name="groupIds">ИДы групп.</param>
        /// <returns></returns>
        Task<KursachResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds);
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> AddStudentAsync(Student student);

        /// <summary>
        /// Добавить студентов.
        /// </summary>
        /// <param name="students">Студенты.</param>
        /// <param name="groupId">ИД группы.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> AddStudentsAsync(IEnumerable<Student> students, int groupId);

        /// <summary>
        /// Сохранить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> SaveStudentAsync(Student student);

        /// <summary>
        /// Удалить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        Task<KursachResponse<bool>> RemoveStudentAsync(Student student);
        #endregion
    }
}
