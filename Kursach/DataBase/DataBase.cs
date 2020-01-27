using Dapper;
using Dapper.Contrib.Extensions;
using Kursach.Models;
using Kursach.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kursach.DataBase
{
    /// <summary>
    /// База данных.
    /// </summary>
    class DataBase : IDataBase
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public DataBase()
        {
        }

        /// <summary>
        /// Асинхронный запрос к базе.
        /// </summary>
        /// <returns></returns>
        async Task<T> QueryAsync<T>(Func<MySqlConnection, Task<T>> func, T defaultValue = default, [CallerMemberName]string callerName = null)
        {
            using (var connection = new MySqlConnection($"server={Settings.Default.mysqlHost};port={Settings.Default.mysqlPort};userid={Settings.Default.mysqlUser};pwd={Settings.Default.mysqlPwd};database={Settings.Default.mysqlDb};Convert Zero Datetime=True"))
            {
                try
                {
                    await connection.OpenAsync();
                    var res = await func(connection);

                    return res;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Ошибка запроса к базе {{{Logger.GetParamsNamesValues(() => callerName)}}}", ex);
                    return defaultValue;
                }
            }
        }

        #region User region
        /// <summary>
        /// Получить пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string login, string password, bool usePassword)
        {
            return await QueryAsync(async con =>
            {
                if (usePassword)
                    return await con.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE login = @login AND password = @password", new { login, password });
                return await con.QueryFirstOrDefaultAsync<User>("SELECT * FROM users WHERE login = @login", new { login });
            });
        }

        /// <summary>
        /// Добавить лог входа в программу.
        /// </summary>
        /// <param name="user">Вошедший пользователь.</param>
        /// <returns></returns>
        public async Task<bool> AddSignInLogAsync(User user)
        {
            return await QueryAsync(async con =>
            {
                await con.InsertAsync(new SignInLog { UserId = user.Id });

                return true;
            });
        }

        /// <summary>
        /// Получить логи входов пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async Task<IEnumerable<SignInLog>> GetSignInLogsAsync(User user)
        {
            return await QueryAsync(async con =>
            {
                return await con.QueryAsync<SignInLog>("SELECT * FROM signinlogs WHERE userId = @Id", user);
            }, Enumerable.Empty<SignInLog>());
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async Task<bool> SignUpAsync(User user)
        {
            if (await GetUserAsync(user.Login, null, false) != null)
                return false;

            return await QueryAsync(async con =>
            {
                await con.InsertAsync(user);

                return true;
            });
        }

        /// <summary>
        /// Получение списка всех пользователей.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await QueryAsync(async con =>
            {
                return await con.GetAllAsync<User>();
            }, Enumerable.Empty<User>());
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async Task<bool> RemoveUserAsync(User user)
        {
            return await QueryAsync(async con =>
            {
                return await con.DeleteAsync(user);
            });
        }

        /// <summary>
        /// Сохранить пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async Task<bool> SaveUserAsync(User user)
        {
            return await QueryAsync(async con =>
            {
                return await con.UpdateAsync(user);
            });
        }
        #endregion

        #region Group region
        /// <summary>
        /// Получение всех групп.
        /// </summary>
        /// <param name="divisionId">Подразделение (от 0 до 2). -1 - все группы.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Group>> GetGroupsAsync(int divisionId = -1)
        {
            return await QueryAsync(async con =>
            {
                if (divisionId == -1)
                    return await con.GetAllAsync<Group>();
                else return await con.QueryAsync<Group>("SELECT * FROM groups WHERE division = @division", new { division = divisionId });
            }, Enumerable.Empty<Group>());
        }

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public async Task<bool> RemoveGroupAsync(Group group)
        {
            return await QueryAsync(async con =>
            {
                return await con.DeleteAsync(group);
            });
        }

        /// <summary>
        /// Сохранить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public async Task<bool> SaveGroupAsync(Group group)
        {
            return await QueryAsync(async con =>
            {
                return await con.UpdateAsync(group);
            });
        }

        /// <summary>
        /// Проверка наличия группы с таким именем.
        /// </summary>
        /// <returns></returns>
        async Task<Group> checkGroupName(Group group)
        {
            return await QueryAsync(async con =>
            {
                return await con.QueryFirstOrDefaultAsync<Group>("SELECT * FROM groups WHERE name = @Name", group);
            });
        }

        /// <summary>
        /// Добавить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public async Task<bool> AddGroupAsync(Group group)
        {
            if (await checkGroupName(group) != null)
                return false;

            return await QueryAsync(async con =>
            {
                await con.InsertAsync(group);
                return true;
            });
        }

        /// <summary>
        /// Добавить группы.
        /// </summary>
        /// <param name="groups">Группы.</param>
        /// <returns></returns>
        public async Task<bool> AddGroupsAsync(IEnumerable<Group> groups)
        {
            return await QueryAsync(async con =>
            {
                await con.InsertAsync(groups);

                return true;
            });
        }
        #endregion

        #region Staff region
        /// <summary>
        /// Получить первого (создать если нет) сотрудника.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetOrCreateFirstStaffIdAsync()
        {
            return await QueryAsync(async con =>
            {
                var staff = await con.QueryFirstOrDefaultAsync<Staff>("SELECT id FROM staff");
                if (staff == null)
                {
                    staff = new Staff
                    {
                        LastName = "Иванов",
                        FirstName = "Иван",
                        MiddleName = "Иванович",
                        Position = "Должность"
                    };

                    var insert = await AddStaffAsync(staff);

                    return insert ? staff.Id : -1;
                }
                else return staff.Id;
            });
        }

        /// <summary>
        /// Получение всех работников.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Staff>> GetStaffsAsync()
        {
            return await QueryAsync(async con =>
            {
                return await con.GetAllAsync<Staff>();
            }, Enumerable.Empty<Staff>());
        }

        /// <summary>
        /// Удалить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public async Task<bool> RemoveStaffAsync(Staff staff)
        {
            return await QueryAsync(async con =>
            {
                return await con.DeleteAsync(staff);
            });
        }

        /// <summary>
        /// Сохранить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public async Task<bool> SaveStaffAsync(Staff staff)
        {
            return await QueryAsync(async (con) =>
            {
                return await con.UpdateAsync(staff);
            });
        }

        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public async Task<bool> AddStaffAsync(Staff staff)
        {
            return await QueryAsync(async con =>
            {
                await con.InsertAsync(staff);

                return true;
            });
        }
        #endregion

        #region Student region
        /// <summary>
        /// Сохранить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<bool> SaveStudentAsync(Student student)
        {
            return await QueryAsync(async con =>
            {
                return await con.UpdateAsync(student);
            });
        }

        /// <summary>
        /// Удалить студент.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<bool> RemoveStudentAsync(Student student)
        {
            return await QueryAsync(async con =>
            {
                return await con.DeleteAsync(student);
            });
        }

        /// <summary>
        /// Добавить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<bool> AddStudentAsync(Student student)
        {
            return await QueryAsync(async con =>
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
        public async Task<bool> AddStudentsAsync(IEnumerable<Student> students)
        {
            return await QueryAsync(async con =>
            {
                await con.InsertAsync(students);
                return true;
            });
        }

        /// <summary>
        /// Получение студентов определенной группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Student>> GetStudentsAsync(Group group)
        {
            return await QueryAsync(async con =>
            {
                return await con.QueryAsync<Student>("SELECT * FROM students WHERE groupId = @Id", group);
            }, Enumerable.Empty<Student>());
        }

        /// <summary>
        /// Получение количества студентов в группах.
        /// </summary>
        /// <param name="groups">Группы.</param>
        /// <returns></returns>
        public async Task<Dictionary<Group, int>> GetStudentsCountAsync(IEnumerable<Group> groups)
        {
            return await QueryAsync(async con =>
            {
                var students = new Dictionary<Group, int>();

                foreach (var item in groups)
                {
                    var res = await con.QueryFirstOrDefaultAsync<int>("SELECT COUNT(*) FROM students WHERE groupId = @Id", item);
                    students[item] = res;
                }

                return students;
            }, new Dictionary<Group, int>());
        }
        #endregion
    }
}
