using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models.Enums;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Classes
{
    /// <summary>
    /// Управление детальной информацией.
    /// </summary>
    class DetailInfo : Invoker, IDetailInfo
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public DetailInfo(IHubConfigurator hubConfigurator) : base(hubConfigurator, HubNames.DetailInfoHub)
        {

        }

        /// <summary>
        /// Получить детальную информацию.
        /// </summary>
        /// <param name="id">ИД человека.</param>
        /// <param name="type">Тип человека.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<Core.Models.DetailInfo>> GetDetailInfoAsync(int id, DetailInfoType type)
        {
            Logger.Log.Info($"Получение детальной информации: {{people id: {id}, type: {type}}}");

            return TryInvokeAsync(args: new object[] { id, type }, defaultValue: new Core.Models.DetailInfo());
        }

        /// <summary>
        /// Добавить или сохранить детальную информацию.
        /// </summary>
        /// <param name="detailInfo">Детальная информация.</param>
        /// <returns></returns>
        public Task<ISTrainingPartResponse<bool>> AddOrUpdateAsync(Core.Models.DetailInfo detailInfo, DetailInfoType type)
        {
            Logger.Log.Info($"Сохранение детальной информации: {{id: {detailInfo.Id}, {detailInfo}, type: {type}}}");

            return TryInvokeAsync<bool>(args: new object[] { detailInfo, type });
        }
    }
}
