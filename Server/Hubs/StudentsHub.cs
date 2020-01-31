﻿using Kursach.Core;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Kursach.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    /// <summary>
    /// Хаб студентов.
    /// </summary>
    [AuthorizeUser]
    [HubName(HubNames.StudentsHub)]
    public class StudentsHub : Hub<StudentsEvents>, StudentsMethods
    {
        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentsHub(IDataBase dataBase)
        {
            this.dataBase = dataBase;
        }

        #region Get region
        /// <summary>
        /// Получение студентов определенной группы.
        /// </summary>
        /// <param name="groupId">ИД группы.</param>
        /// <returns></returns>
        public Task<KursachResponse<IEnumerable<Student>>> GetStudentsAsync(int groupId)
        {
            return dataBase.GetStudentsAsync(groupId);
        }

        /// <summary>
        /// Получение количества студентов в группах.
        /// Ключ - ИД группы.
        /// </summary>
        /// <param name="groupIds">ИДы групп.</param>
        /// <returns></returns>
        public Task<KursachResponse<Dictionary<int, StudentsCount>>> GetStudentsCountAsync(IEnumerable<int> groupIds)
        {
            return dataBase.GetStudentsCountAsync(groupIds);
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> AddStudentAsync(Student student)
        {
            Logger.Log.Info($"Add student: {student.FullName}");

            var res = await dataBase.AddStudentAsync(student);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).StudentChanged(DbChangeStatus.Add, student);

            return res;
        }

        /// <summary>
        /// Добавить студентов.
        /// </summary>
        /// <param name="students">Студенты.</param>
        /// <param name="groupId">ИД группы.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> AddStudentsAsync(IEnumerable<Student> students, int groupId)
        {
            Logger.Log.Info($"Import students: {students.Count()}, group: {groupId}");

            var res = await dataBase.AddStudentsAsync(students, groupId);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).StudentsImportTo(groupId);

            return res;
        }

        /// <summary>
        /// Сохранить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> SaveStudentAsync(Student student)
        {
            Logger.Log.Info($"Save student: {student.FullName}");

            var res = await dataBase.SaveStudentAsync(student);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).StudentChanged(DbChangeStatus.Update, student);

            return res;
        }

        /// <summary>
        /// Удалить студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        public async Task<KursachResponse<bool>> RemoveStudentAsync(Student student)
        {
            Logger.Log.Info($"Remove student: {student.FullName}");

            var res = await dataBase.RemoveStudentAsync(student);

            if (res)
                Clients.Group(Consts.AuthorizedGroup).StudentChanged(DbChangeStatus.Remove, student);

            return res;
        }
        #endregion
    }
}
