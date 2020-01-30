using Kursach.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.DataBase
{
    class DesignDataBase : IDataBase
    {
        Random rn = new Random();

        public Task<bool> SaveStudentAsync(Student student)
        {
            return Task.FromResult(true);
        }

        public Task<bool> RemoveStudentAsync(Student student)
        {
            return Task.FromResult(true);
        }

        public Task<bool> AddStudentAsync(Student student)
        {
            return Task.FromResult(true);
        }

        public Task<bool> SaveStaffAsync(Staff staff)
        {
            return Task.FromResult(true);
        }

        public Task<bool> AddStaffAsync(Staff staff)
        {
            return Task.FromResult(true);
        }

        public Task<bool> RemoveStaffAsync(Staff staff)
        {
            return Task.FromResult(true);
        }

        public Task<bool> SaveGroupAsync(Group group)
        {
            return Task.FromResult(true);
        }

        public Task<bool> AddGroupAsync(Group group)
        {
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Staff>> GetStaffsAsync()
        {
            List<Staff> staff = new List<Staff>();

            for (int i = 0; i < 50; i++)
            {
                staff.Add(new Staff
                {
                    FirstName = "First Name",
                    LastName = "Last Name",
                    MiddleName = "Middle Name",
                    Position = "Position / BGK",
                    Id = i
                });
            }

            return Task.FromResult(staff.AsEnumerable());
        }

        public Task<bool> RemoveGroupAsync(Group group)
        {
            return Task.FromResult(true);
        }

        public Task<bool> AddSignInLogAsync(User user)
        {
            return Task.FromResult(true);
        }

        public Task<IEnumerable<SignInLog>> GetSignInLogsAsync(User user)
        {
            List<SignInLog> logs = new List<SignInLog>();
            for (int i = 0; i < 50; i++)
            {
                logs.Add(new SignInLog { UserId = i });
            }
            return Task.FromResult(logs.AsEnumerable());
        }

        public Task<User> GetUserAsync(string login, string password, bool usePassword)
        {
            return Task.FromResult(GetUser(login, password, 0));
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            List<User> users = new List<User>();
            for (int i = 0; i < 50; i++)
            {
                users.Add(GetUser(i.ToString(), "password", i, UserMode.ReadWrite));
            }

            return Task.FromResult(users.AsEnumerable());
        }

        public Task<bool> RemoveUserAsync(User user)
        {
            return Task.FromResult(true);
        }

        public Task<bool> SaveUserAsync(User user)
        {
            return Task.FromResult(true);
        }

        public Task<bool> SignUpAsync(User user)
        {
            return Task.FromResult(true);
        }

        private User GetUser(string login, string password, int id, UserMode mode = UserMode.Admin) => 
            new User { Id = id, Login = login, Password = password, Mode = mode };

        public Task<bool> AddStudentsAsync(IEnumerable<Student> students)
        {
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Student>> GetStudentsAsync(Group group)
        {
            List<Student> students = new List<Student>();

            for (int i = 0; i < 20; i++)
            {
                students.Add(new Student
                {
                    Birthdate = DateTime.Now,
                    DecreeOfEnrollment = "Приказ",
                    Expelled = rn.Next(0, 2) == 1,
                    FirstName = "Имя" + i,
                    LastName = "Фамилия" + i,
                    MiddleName = "Отчество" + i,
                    GroupId = group.Id,
                    Notice = "Notice",
                    PoPkNumber = i,
                    Id = i
                });
            }

            return Task.FromResult(students.AsEnumerable());
        }

        public Task<IEnumerable<Group>> GetGroupsAsync(int divisionId = -1)
        {
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
                    Division = divisionId,
                    IsIntramural = true,
                };

                groups.Add(group);
            }

            return Task.FromResult(groups.AsEnumerable());
        }

        public Task<bool> AddGroupsAsync(IEnumerable<Group> groups)
        {
            return Task.FromResult(true);
        }

        public Task<int> GetOrCreateFirstStaffIdAsync()
        {
            return Task.FromResult(1);
        }

        public Task<Dictionary<Group, StudentsCount>> GetStudentsCountAsync(IEnumerable<Group> groups)
        {
            return Task.FromResult(new Dictionary<Group, StudentsCount>());
        }
    }
}