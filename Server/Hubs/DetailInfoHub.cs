using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using ISTraining_Part.Core.ServerMethods;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server.DataBase.Interfaces;
using System.Threading.Tasks;

namespace Server.Hubs
{
    /// <summary>
    /// Хаб управления детальной информацией.
    /// </summary>
    [AuthorizeUser]
    [HubName(HubNames.DetailInfoHub)]
    public class DetailInfoHub : Hub, IDetailInfoHub
    {
        /// <summary>
        /// Репозиторий детальной информации.
        /// </summary>
        readonly IDetailInfoRepository repository;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DetailInfoHub(IDetailInfoRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Получить детальную информацию.
        /// </summary>
        /// <param name="id">Id человека.</param>
        /// <param name="type">Тип человека.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<DetailInfo>> GetDetailInfoAsync(int id, DetailInfoType type)
        {
            return repository.GetDetailInfoAsync(id, type);
        }

        /// <summary>
        /// Добавить или сохранить детальную информацию.
        /// </summary>
        /// <param name="id">Id человека.</param>
        /// <param name="detailInfo">Детальная информация.</param>
        /// <param name="type">Тип человека.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> AddOrUpdateAsync(DetailInfo detailInfo, DetailInfoType type)
        {
            return repository.AddOrUpdateAsync(detailInfo, type);
        }
    }
}
