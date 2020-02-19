using Dapper;
using Dapper.Contrib.Extensions;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using MySql.Data.MySqlClient;
using Server.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Server.DataBase
{
    /// <summary>
    /// База данных.
    /// </summary>
    class DataBase : IDataBase
    {
        /// <summary>
        /// Асинхронный запрос к базе.
        /// </summary>
        /// <returns></returns>
        async Task<KursachResponse<T>> QueryAsync<T>(Func<MySqlConnection, Task<T>> func,
                                    T defaultValue = default,
                                    [CallerMemberName]string callerName = null)
        {
            using (var connection = new MySqlConnection($"server={Settings.Default.mysqlHost};port={Settings.Default.mysqlPort};userid={Settings.Default.mysqlUser};pwd={Settings.Default.mysqlPwd};database={Settings.Default.mysqlDb};Convert Zero Datetime=True"))
            {
                try
                {
                    await connection.OpenAsync();
                    var res = await func(connection);

                    return new KursachResponse<T>(KursachResponseCode.Ok, res);
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Ошибка запроса к базе, caller: {callerName}", ex);
                    return new KursachResponse<T>(KursachResponseCode.DbError, defaultValue);
                }
            }
        }

        #region User region
        #region Get region
        /// <summary>
        /// Получение списка всех пользователей.
        /// </summary>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<User>>> GetUsersAsync()
        {
            return QueryAsync(con =>
            {
                return con.GetAllAsync<User>();
            }, Enumerable.Empty<User>());
        }

        /// <summary>
        /// Получить пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="usePassword">Проверять пароль.</param>
        /// <returns></returns>
        public Task<KursachResponse<User>> GetUserAsync(string login, string password, bool usePassword)
        {
            return QueryAsync(con =>
            {
                if (usePassword)
                    return con.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE login = @login AND password = @password", new { login, password });
                return con.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE login = @login", new { login });
            });
        }
        #endregion

        #region Log region
        /// <summary>
        /// Получить логи входов пользователя.
        /// </summary>
        /// <param name="userId">ИД пользователя.</param>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<SignInLog>>> GetSignInLogsAsync(int userId)
        {
            return QueryAsync(con =>
            {
                return con.QueryAsync<SignInLog>("SELECT * FROM signinlogs WHERE userId = @userId", new { userId });
            }, Enumerable.Empty<SignInLog>());
        }

        /// <summary>
        /// Добавить лог авторизации пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async void AddSignInLogAsync(User user)
        {
            await QueryAsync<long>(async con =>
            {
                return await con.InsertAsync(new SignInLog { UserId = user.Id });
            });
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> AddUserAsync(User user)
        {
            return QueryAsync(async con =>
            {
                await con.InsertAsync(user);
                return true;
            });
        }

        /// <summary>
        /// Сохранить пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> SaveUserAsync(User user)
        {
            return QueryAsync(con =>
            {
                return con.UpdateAsync(user);
            });
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> RemoveUserAsync(User user)
        {
            return QueryAsync(con =>
            {
                return con.DeleteAsync(user);
            });
        }
        #endregion
        #endregion

        #region Group region
        #region Get region
        /// <summary>
        /// Получение всех групп.
        /// </summary>
        /// <param name="divisionId">Подразделение (от 0 до 2). -1 - все группы.</param>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<Group>>> GetGroupsAsync(int divisionId = -1)
        {
            return QueryAsync(con =>
            {
                if (divisionId == -1)
                    return con.GetAllAsync<Group>();
                else return con.QueryAsync<Group>("SELECT * FROM groups WHERE division = @division", new { division = divisionId });
            }, Enumerable.Empty<Group>());
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить группы.
        /// </summary>
        /// <param name="groups">Группы.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> AddGroupsAsync(IEnumerable<Group> groups)
        {
            return QueryAsync(async con =>
            {
                await con.InsertAsync(groups);
                return true;
            });
        }

        /// <summary>
        /// Добавить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> AddGroupAsync(Group group)
        {
            return QueryAsync(async con =>
            {
                await con.InsertAsync(group);
                return true;
            });
        }

        /// <summary>
        /// Сохранить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> SaveGroupAsync(Group group)
        {
            return QueryAsync(con =>
            {
                return con.UpdateAsync(group);
            });
        }

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> RemoveGroupAsync(Group group)
        {
            return QueryAsync(con =>
            {
                return con.DeleteAsync(group);
            });
        }
        #endregion
        #endregion

        #region Staff region
        #region Get region
        /// <summary>
        /// Получение всех работников.
        /// </summary>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<Staff>>> GetStaffsAsync()
        {
            return QueryAsync(con =>
            {
                return con.GetAllAsync<Staff>();
            }, Enumerable.Empty<Staff>());
        }

        /// <summary>
        /// Получить первого (создать если нет) сотрудника.
        /// </summary>
        /// <returns></returns>
        public async Task<KursachResponse<Staff, bool>> GetOrCreateFirstStaffAsync()
        {
            KursachResponse<Staff, bool> response = null;
            bool added = false;

            var query = await QueryAsync(async con =>
            {
                var staff = await con.QueryFirstOrDefaultAsync<Staff>("SELECT id FROM staff LIMIT 1");
                if (staff == null)
                {
                    staff = new Staff
                    {
                        LastName = "Иванов",
                        FirstName = "Иван",
                        MiddleName = "Иванович",
                        Position = "Должность"
                    };

                    added = await AddStaffAsync(staff);

                    return added ? staff : null;
                }
                else return staff;
            });

            response = new KursachResponse<Staff, bool>(query.Code, added, query.Response);

            return response;
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> AddStaffAsync(Staff staff)
        {
            return QueryAsync(async con =>
            {
                await con.InsertAsync(staff);
                return true;
            });
        }

        /// <summary>
        /// Сохранить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> SaveStaffAsync(Staff staff)
        {
            return QueryAsync(con =>
            {
                return con.UpdateAsync(staff);
            });
        }

        /// <summary>
        /// Удалить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> RemoveStaffAsync(Staff staff)
        {
            return QueryAsync(con =>
            {
                return con.DeleteAsync(staff);
            });
        }
        #endregion
        #endregion

        #region Student region
        /// <summary>
        /// НЕ ИСПОЛЬЗУЕТСЯ.
        /// </summary>
        /// <returns></returns>
        public Task RaiseStudentsImported()
        {
            return Task.CompletedTask;
        }

        #region Get region
        /// <summary>
        /// Получение студентов определенной группы.
        /// </summary>
        /// <param name="groupId">ИД группы (-1 для получения всех студентов).</param>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<Student>>> GetStudentsAsync()
        {
            return QueryAsync(con =>
            {
                return con.QueryAsync<Student>("SELECT * FROM students");
            }, Enumerable.Empty<Student>());
        }

        /// <summary>
        /// Получение количества студентов в группах.
        /// </summary>
        /// <param name="groupIds">ИДы групп.</param>
        /// <returns></returns>
        public Task<KursachResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds)
        {
            return QueryAsync(async con =>
            {
                var students = new Dictionary<int, StudentsCount>();

                foreach (var item in groupIds)
                {
                    var total = await con.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM students WHERE groupId = @item", new { item });
                    var sabbatical = await con.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM students WHERE groupId = @item AND onSabbatical = 1", new { item });
                    students[item] = new StudentsCount(total, sabbatical);
                }

                return students;
            }, new Dictionary<int, StudentsCount>());
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
            return QueryAsync(async con =>
            {
                await con.InsertAsync(student);
                return true;
            });
        }

        /// <summary>
        /// Добавить студентов.
        /// </summary>
        /// <param name="students">Студенты.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> ImportStudentsAsync(IEnumerable<Student> students)
        {
            return QueryAsync(async con =>
            {
                await con.InsertAsync(students);
                return true;
            });
        }

        /// <summary>
        /// Сохранить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> SaveStudentAsync(Student student)
        {
            return QueryAsync(con =>
            {
                return con.UpdateAsync(student);
            });
        }

        /// <summary>
        /// Удалить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public Task<KursachResponse<bool>> RemoveStudentAsync(Student student)
        {
            return QueryAsync(con =>
            {
                return con.DeleteAsync(student);
            });
        }
        #endregion
        #endregion
    }
}
