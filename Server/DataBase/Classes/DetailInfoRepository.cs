using Dapper;
using Dapper.Contrib.Extensions;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using Server.DataBase.Interfaces;
using System.Threading.Tasks;

namespace Server.DataBase.Classes
{
    /// <summary>
    /// Репозиторий детальной информации.
    /// </summary>
    class DetailInfoRepository : IDetailInfoRepository
    {
        /// <summary>
        /// Репозиторий.
        /// </summary>
        readonly IRepository repository;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DetailInfoRepository(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Получить детальную информацию.
        /// </summary>
        /// <param name="id">ИД человека.</param>
        /// <param name="type">Тип человека.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<DetailInfo>> GetDetailInfoAsync(int id, DetailInfoType type)
        {
            return repository.QueryAsync(con =>
            {
                return con.QueryFirstOrDefaultAsync<DetailInfo>($"SELECT * FROM detailInfo WHERE {type}Id = @peopleId", new { peopleId = id });
            });
        }

        /// <summary>
        /// Добавить или сохранить детальную информацию.
        /// </summary>
        /// <param name="detailInfo">Детальная информация.</param>
        /// <param name="type">Тип человека.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> AddOrUpdateAsync(DetailInfo detailInfo, DetailInfoType type)
        {
            return repository.QueryAsync(async con =>
            {
                if (detailInfo.Id > 0)
                    return await con.UpdateAsync(detailInfo);
                else return await con.InsertAsync(detailInfo) > 0;
            });
        }
    }
}
