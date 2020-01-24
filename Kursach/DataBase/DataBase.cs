using Kursach.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        /// База данных.
        /// </summary>
        readonly Context context;

        /// <summary>
        /// Ctor.
        /// </summary>
        public DataBase(Context context)
        {
            this.context = context;
        }

        /// <summary>
        /// Запрос в базу с логированием.
        /// </summary>
        /// <returns></returns>
        private async Task<T> query<T>(Func<Task<T>> action, [CallerMemberName]string name = null)
        {
            try
            {
                return await action?.Invoke();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                InnerEx(ex, sb);
                Logger.Log.Error($"Ошибка запроса к базе: {{ex: {sb.ToString()}, member: {name}}}");
                return default;
            }
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
        /// Получить пользователя.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string login, string password, bool usePassword)
        {
            return await query(async () =>
            {
                if (usePassword)
                    return await context.Users.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
                return await context.Users.FirstOrDefaultAsync(x => x.Login == login);
            });
        }

        /// <summary>
        /// Добавить лог входа в программу.
        /// </summary>
        /// <param name="user">Вошедший пользователь.</param>
        /// <returns></returns>
        public async Task<bool> AddSignInLogAsync(User user)
        {
            return await query(async () =>
            {
                context.SignInLogs.Add(new SignInLog { User = user });
                await context.SaveChangesAsync();

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
            return await query(async () => await context.SignInLogs.Where(x => x.UserId == user.Id).ToListAsync());
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="mode">Права.</param>
        /// <returns></returns>
        public async Task<bool> SignUpAsync(LoginUser user, UserMode mode)
        {
            if (await GetUserAsync(user.Login, null, false) != null)
                return false;

            return await query(async () =>
            {
                context.Users.Add(user.ToUser(mode));
                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Получение списка всех пользователей.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await query(async () => await context.Users.ToListAsync());
        }

        /// <summary>
        /// Удаление пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async Task<bool> RemoveUserAsync(User user)
        {
            return await query(async () =>
            {
                context.Users.Remove(user);
                context.Entry(user).State = EntityState.Deleted;

                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Сохранить пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns></returns>
        public async Task<bool> SaveUserAsync(User user)
        {
            return await query(async () =>
            {
                context.Entry(user).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Получение всех групп.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            return await query(async () =>
            {
                return await context.Groups.Include(x => x.Curator).ToListAsync();
            });
        }

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public async Task<bool> RemoveGroupAsync(Group group)
        {
            return await query(async () =>
            {
                context.Groups.Remove(group);
                context.Entry(group).State = EntityState.Deleted;

                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Получение всех работников.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Staff>> GetStaffsAsync()
        {
            return await query(async () => await context.Staff.ToListAsync());
        }

        /// <summary>
        /// Сохранить группу.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        public async Task<bool> SaveGroupAsync(Group group)
        {
            return await query(async () =>
            {
                context.Entry(group).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Проверка наличия группы с таким именем.
        /// </summary>
        /// <returns></returns>
        async Task<Group> checkGroupName(Group group)
        {
            return await query(async () =>
            {
                return await context.Groups.FirstOrDefaultAsync(x => x.Name == group.Name);
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

            return await query(async () =>
            {
                context.Groups.Add(group);
                await context.SaveChangesAsync();

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
            return await query(async () =>
            {
                context.Staff.Remove(staff);
                context.Entry(staff).State = EntityState.Deleted;

                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Сохранить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public async Task<bool> SaveStaffAsync(Staff staff)
        {
            return await query(async () =>
            {
                context.Entry(staff).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public async Task<bool> AddStaffAsync(Staff staff)
        {
            return await query(async () =>
            {
                context.Staff.Add(staff);
                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Загрузка всех студентов.
        /// </summary>
        /// <returns></returns>
        public async Task LoadStudentsAsync()
        {
            await context.Students.ToListAsync();
        }

        /// <summary>
        /// Сохранить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<bool> SaveStudentAsync(Student student)
        {
            return await query(async () =>
            {
                context.Entry(student).State = EntityState.Modified;

                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Удалить студент.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<bool> RemoveStudentAsync(Student student)
        {
            return await query(async () =>
            {
                context.Students.Remove(student);
                context.Entry(student).State = EntityState.Deleted;

                await context.SaveChangesAsync();

                return true;
            });
        }

        /// <summary>
        /// Добавить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<bool> AddStudentAsync(Student student)
        {
            return await query(async () =>
            {
                context.Students.Add(student);
                await context.SaveChangesAsync();
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
            return await query(async () =>
            {
                context.Students.AddRange(students);
                await context.SaveChangesAsync();
                return true;
            });
        }
    }
}
