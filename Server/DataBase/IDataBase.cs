using Kursach.Core.ServerMethods;

namespace Server.DataBase
{
    /// <summary>
    /// База данных.
    /// </summary>
    public interface IDataBase : UsersMethods, StudentsMethods, StaffMethods, GroupsMethods
    {
        //#region User region
        //#region Get region
        ///// <summary>
        ///// Получение списка всех пользователей.
        ///// </summary>
        ///// <returns></returns>
        //Task<IEnumerable<User>> GetUsersAsync();

        ///// <summary>
        ///// Получить пользователя.
        ///// </summary>
        ///// <param name="login">Логин.</param>
        ///// <param name="password">Пароль.</param>
        ///// <param name="usePassword">Проверять пароль.</param>
        ///// <returns></returns>
        //Task<User> GetUserAsync(string login, string password, bool usePassword);

        ///// <summary>
        ///// Получить пользователя.
        ///// </summary>
        ///// <param name="id">ИД.</param>
        ///// <returns></returns>
        //Task<User> GetUserByIdAsync(int id);
        //#endregion

        //#region Log region
        ///// <summary>
        ///// Получить логи входов пользователя.
        ///// </summary>
        ///// <param name="userId">ИД пользователя.</param>
        ///// <returns></returns>
        //Task<IEnumerable<SignInLog>> GetSignInLogsAsync(int userId);
        //#endregion

        //#region CUD region
        ///// <summary>
        ///// Регистрация нового пользователя.
        ///// </summary>
        ///// <param name="user">Пользователь.</param>
        ///// <returns></returns>
        //Task<bool> AddUserAsync(User user);

        ///// <summary>
        ///// Сохранить пользователя.
        ///// </summary>
        ///// <param name="user">Пользователь.</param>
        ///// <returns></returns>
        //Task<bool> SaveUserAsync(User user);

        ///// <summary>
        ///// Удаление пользователя.
        ///// </summary>
        ///// <param name="user">Пользователь.</param>
        ///// <returns></returns>
        //Task<bool> RemoveUserAsync(User user);
        //#endregion
        //#endregion

        //#region Group region
        //#region Get region
        ///// <summary>
        ///// Получение всех групп.
        ///// </summary>
        ///// <param name="divisionId">Подразделение (от 0 до 2). -1 - все группы.</param>
        ///// <returns></returns>
        //Task<IEnumerable<Group>> GetGroupsAsync(int divisionId = -1);

        ///// <summary>
        ///// Получить группу по Id.
        ///// </summary>
        ///// <param name="id">ИД.</param>
        ///// <returns></returns>
        //Task<Group> GetGroupByIdAsync(int id);
        //#endregion

        //#region CUD region
        ///// <summary>
        ///// Добавить группы.
        ///// </summary>
        ///// <param name="groups">Группы.</param>
        ///// <returns></returns>
        //Task<bool> AddGroupsAsync(IEnumerable<Group> groups);

        ///// <summary>
        ///// Добавить группу.
        ///// </summary>
        ///// <param name="group">Группа.</param>
        ///// <returns></returns>
        //Task<bool> AddGroupAsync(Group group);

        ///// <summary>
        ///// Сохранить группу.
        ///// </summary>
        ///// <param name="group">Группа.</param>
        ///// <returns></returns>
        //Task<bool> SaveGroupAsync(Group group);

        ///// <summary>
        ///// Удаление группы.
        ///// </summary>
        ///// <param name="group">Группа.</param>
        ///// <returns></returns>
        //Task<bool> RemoveGroupAsync(Group group);
        //#endregion
        //#endregion

        //#region Staff region
        //#region Get region
        ///// <summary>
        ///// Получение всех работников.
        ///// </summary>
        ///// <returns></returns>
        //Task<IEnumerable<Staff>> GetStaffsAsync();

        ///// <summary>
        ///// Получить первого (создать если нет) сотрудника.
        ///// </summary>
        ///// <returns></returns>
        //Task<int> GetOrCreateFirstStaffIdAsync();

        ///// <summary>
        ///// Получить сотрудника по Id.
        ///// </summary>
        ///// <param name="id">ИД.</param>
        ///// <returns></returns>
        //Task<Staff> GetStaffByIdAsync(int id);
        //#endregion

        //#region CUD region
        ///// <summary>
        ///// Добавить сотрудника.
        ///// </summary>
        ///// <param name="staff">Сотрудник.</param>
        ///// <returns></returns>
        //Task<bool> AddStaffAsync(Staff staff);

        ///// <summary>
        ///// Сохранить сотрудника.
        ///// </summary>
        ///// <param name="staff">Сотрудник.</param>
        ///// <returns></returns>
        //Task<bool> SaveStaffAsync(Staff staff);

        ///// <summary>
        ///// Удалить сотрудника.
        ///// </summary>
        ///// <param name="staff">Сотрудник.</param>
        ///// <returns></returns>
        //Task<bool> RemoveStaffAsync(Staff staff);
        //#endregion
        //#endregion

        //#region Student region
        //#region Get region
        ///// <summary>
        ///// Получение студентов определенной группы.
        ///// </summary>
        ///// <param name="groupId">ИД группы.</param>
        ///// <returns></returns>
        //Task<IEnumerable<Student>> GetStudentsAsync(int groupId);

        ///// <summary>
        ///// Получение количества студентов в группах.
        ///// Ключ - ИД группы.
        ///// </summary>
        ///// <param name="groupIds">ИДы групп.</param>
        ///// <returns></returns>
        //Task<Dictionary<int, StudentsCount>> GetStudentsCountAsync(IEnumerable<int> groupIds);

        ///// <summary>
        ///// Получить студента по ИД.
        ///// </summary>
        ///// <param name="id">ИД.</param>
        ///// <returns></returns>
        //Task<Student> GetStudentById(int id);
        //#endregion

        //#region CUD region
        ///// <summary>
        ///// Добавить студента.
        ///// </summary>
        ///// <param name="student">Студент.</param>
        ///// <returns></returns>
        //Task<bool> AddStudentAsync(Student student);

        ///// <summary>
        ///// Добавить студентов.
        ///// </summary>
        ///// <param name="students">Студенты.</param>
        ///// <returns></returns>
        //Task<bool> AddStudentsAsync(IEnumerable<Student> students);

        ///// <summary>
        ///// Сохранить студента.
        ///// </summary>
        ///// <param name="student">Студент.</param>
        ///// <returns></returns>
        //Task<bool> SaveStudentAsync(Student student);

        ///// <summary>
        ///// Удалить студента.
        ///// </summary>
        ///// <param name="student">Студент.</param>
        ///// <returns></returns>
        //Task<bool> RemoveStudentAsync(Student student);
        //#endregion
        //#endregion
    }
}
