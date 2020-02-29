using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using System.Threading.Tasks;

namespace ISTraining_Part.Core.ServerMethods
{
    /// <summary>
    /// Список методов хаба детальной информации.
    /// </summary>
    public interface IDetailInfoHub
    {
        /// <summary>
        /// Получить детальную информацию.
        /// </summary>
        /// <param name="id">Id человека.</param>
        /// <param name="type">Тип человека.</param>
        /// <returns></returns>
        Task<ISTrainingPartResponse<DetailInfo>> GetDetailInfoAsync(int id, DetailInfoType type);

        /// <summary>
        /// Добавить или сохранить детальную информацию.
        /// </summary>
        /// <param name="id">Id человека.</param>
        /// <param name="detailInfo">Детальная информация.</param>
        /// <param name="type">Тип человека.</param>
        /// <returns></returns>
        Task<ISTrainingPartResponse<bool>> AddOrUpdateAsync(DetailInfo detailInfo, DetailInfoType type);
    }
}
