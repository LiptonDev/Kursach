#if design
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Kursach.Models;

namespace Kursach.DataBase
{
    class DesignDataBase : IDataBase
    {
        const int Delay = 100;
        Random rn = new Random();

        public async Task LoadStudentsAsync()
        {
            await Task.Delay(Delay);
        }

        public async Task<bool> SaveStudentAsync(Student student)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<bool> RemoveStudentAsync(Student student)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<bool> AddStudentAsync(Student student)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<bool> SaveStaffAsync(Staff staff)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<bool> AddStaffAsync(Staff staff)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<bool> RemoveStaffAsync(Staff staff)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<bool> SaveGroupAsync(Group group)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<bool> AddGroupAsync(Group group)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<IEnumerable<Staff>> GetStaffsAsync()
        {
            await Task.Delay(Delay);

            List<Staff> staff = new List<Staff>();

            for (int i = 0; i < 50; i++)
            {
                staff.Add(new Staff { FirstName = "First Name", LastName = "Last Name", MiddleName = "Middle Name", Position = "Position / BGK", Id = i });
            }

            return staff;
        }

        public async Task<bool> RemoveGroupAsync(Group group)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<IEnumerable<Group>> GetGroupsAsync()
        {
            await Task.Delay(Delay);

            List<Group> groups = new List<Group>();

            for (int i = 0; i < 25; i++)
            {
                var group = new Group
                {
                    Name = $"ГР-{i}",
                    Id = i,
                    CuratorId = i,
                    End = DateTime.Now,
                    Start = DateTime.Now,
                    Specialty = "Оооооочень длинное название специальности для теста",
                    IsBudget = true,
                };

                groups.Add(group);
            }

            return groups;
        }

        public async Task<bool> AddSignInLogAsync(User user)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<IEnumerable<SignInLog>> GetSignInLogsAsync(User user)
        {
            await Task.Delay(Delay);
            List<SignInLog> logs = new List<SignInLog>();
            for (int i = 0; i < 50; i++)
            {
                logs.Add(new SignInLog { UserId = i });
            }
            return logs;
        }

        public async Task<User> GetUserAsync(string login, string password, bool usePassword)
        {
            await Task.Delay(Delay);
            return GetUser(login, password, 0);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            await Task.Delay(Delay);

            List<User> users = new List<User>();
            for (int i = 0; i < 50; i++)
            {
                users.Add(GetUser(i.ToString(), "password", i, UserMode.ReadWrite));
            }

            return users;
        }

        public async Task<bool> RemoveUserAsync(User user)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<bool> SaveUserAsync(User user)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<bool> SignUpAsync(User user)
        {
            await Task.Delay(Delay);
            return true;
        }

        private User GetUser(string login, string password, int id, UserMode mode = UserMode.Admin) => new User { Id = id, Login = login, Password = password, Mode = mode };

        public async Task<bool> AddStudentsAsync(IEnumerable<Student> students)
        {
            await Task.Delay(Delay);
            return true;
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync(Group group)
        {
            await Task.Delay(Delay);
            List<Student> students = new List<Student>();

            for (int i = 0; i < 20; i++)
            {
                students.Add(new Student
                {
                    Birthdate = DateTime.Now,
                    DecreeOfEnrollment = "Приказ",
                    Expelled = rn.Next(0, 2) == 1,
                    FirstName = "Имя"+i,
                    LastName = "Фамилия"+i,
                    MiddleName= "Отчество"+i,
                    GroupId = group.Id,
                    Notice = "Notice",
                    PoPkNumber = i,
                    Id = i
                });
            }

            return students;
        }
    }
}
#endif