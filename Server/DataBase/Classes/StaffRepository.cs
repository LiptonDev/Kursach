using Dapper;
using Dapper.Contrib.Extensions;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using Server.DataBase.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DataBase.Classes
{
    /// <summary>
    /// Репозиторий сотрудников.
    /// </summary>
    class StaffRepository : IStaffRepository
    {
        /// <summary>
        /// Репозиторий.
        /// </summary>
        readonly IRepository repository;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StaffRepository(IRepository repository)
        {
            this.repository = repository;
        }

        #region Get region
        /// <summary>
        /// Получение всех работников.
        /// </summary>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<IEnumerable<Staff>>> GetStaffsAsync()
        {
            return repository.QueryAsync(con =>
            {
                return con.GetAllAsync<Staff>();
            }, Enumerable.Empty<Staff>());
        }

        /// <summary>
        /// Получить первого (создать если нет) сотрудника.
        /// </summary>
        /// <returns></returns>
        public async Task<ISTrainingPartResponse<Staff, bool>> GetOrCreateFirstStaffAsync()
        {
            ISTrainingPartResponse<Staff, bool> response = null;
            bool added = false;

            var query = await repository.QueryAsync(async con =>
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

            response = new ISTrainingPartResponse<Staff, bool>(query.Code, added, query.Response);

            return response;
        }
        #endregion

        #region CUD region
        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> AddStaffAsync(Staff staff)
        {
            return repository.QueryAsync(async con =>
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
        public Task<ISTrainingPartResponse<bool>> SaveStaffAsync(Staff staff)
        {
            return repository.QueryAsync(con =>
            {
                return con.UpdateAsync(staff);
            });
        }

        /// <summary>
        /// Удалить сотрудника.
        /// </summary>
        /// <param name="staff">Сотрудник.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> RemoveStaffAsync(Staff staff)
        {
            return repository.QueryAsync(con =>
            {
                return con.DeleteAsync(staff);
            });
        }
        #endregion
    }
}
