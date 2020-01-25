using Dapper;
using Dapper.Contrib.Extensions;
using Kursach.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
        /// Ctor.
        /// </summary>
        public DataBase()
        {
        }

        private void InnerEx(Exception ex, StringBuilder sb)
        {
            if (ex == null)
                return;

            sb.Append(ex.Message);

            if (ex.InnerException != null)
                InnerEx(ex.InnerException, sb);
        }

        /// <summary>
        /// Асинхронный запрос к базе.
        /// </summary>
        /// <returns></returns>
        async Task<T> QueryAsync<T>(Func<MySqlConnection, Task<T>> func, [CallerMemberName]string name = null)
        {
            using (var connection = new MySqlConnection("server=localhost;UserId=root;database=kursach;Convert Zero Datetime=True"))
            {
                try
                {
                    await connection.OpenAsync();
                    var res = await func(connection);

                    return res;
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    InnerEx(ex, sb);
                    Logger.Log.Error($"Ошибка запроса к базе: {{ex: {sb.ToString()}, member: {name}}}");
                    return default;
                }
            }
        }

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
            return await QueryAsync(async con => await con.QueryAsync<SignInLog>("SELECT * FROM signinlogs WHERE userId = @Id", user));
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="mode">Права.</param>
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
            return await QueryAsync(async con => await con.GetAllAsync<User>());
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

        /// <summary>
        /// Получение всех групп.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            return await QueryAsync(async con =>
            {
                return await con.GetAllAsync<Group>();
            });
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
        /// Получение всех работников.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Staff>> GetStaffsAsync()
        {
            return await QueryAsync(async con =>
            {
                return await con.GetAllAsync<Staff>();
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
            });
        }
    }
}
