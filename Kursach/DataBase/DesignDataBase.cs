#if design
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kursach.Models;

namespace Kursach.DataBase
{
    class DesignDataBase : IDataBase
    {
        const int Delay = 100;


        public async Task LoadStudentsAsync()
        {
            await Task.Delay(Delay);
        }

        public async Task<bool> SaveStudentAsync(Student student)
        {
            return true;
        }

        public async Task<bool> RemoveStudentAsync(Student student)
        {
            return true;
        }

        public async Task<bool> AddStudentAsync(Student student)
        {
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
                groups.Add(new Group
                {
                    Name = $"ГР-{i}",
                    Id = i,
                    CuratorId = i,
                    End = DateTime.Now,
                    Start = DateTime.Now, 
                    Specialty = "Оооооочень длинное название специальности для теста",
                    IsBudget = true,
                    Curator = new Staff
                    {
                        FirstName = "Имя",
                        LastName = "Фамилия",
                        MiddleName = "Отчество",
                        Position = "Шестерка",
                        Id = i
                    },
                    Students = new ObservableCollection<Student>
                    {
                        new Student
                        {
                            FirstName = "Имя",
                            LastName = "Фамилия",
                            MiddleName = "Отчество",
                            Birthdate = DateTime.Now,
                            DecreeOfEnrollment = "№53-К от 23.08.2017",
                            Notice = "пер. из гр. ТЭО-21 (11) Пр№52-К от 02.07.2018",
                            PoPkNumber = 1337,
                            GroupId = i,
                            Id = i
                        }
                    }
                });
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
                logs.Add(new SignInLog { User = user });
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

        public async Task<bool> SignUpAsync(LoginUser user, UserMode mode)
        {
            await Task.Delay(Delay);
            return true;
        }

        private User GetUser(string login, string password, int id, UserMode mode = UserMode.Admin) => new User { Id = id, Login = login, Password = password, Mode = mode };
    }
}
#endif