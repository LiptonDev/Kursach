using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Design
{
    class DesignDetailInfo : IDetailInfo
    {
        public Task<ISTrainingPartResponse<bool>> AddOrUpdateAsync(DetailInfo detailInfo, DetailInfoType type)
        {
            return Task.FromResult(new ISTrainingPartResponse<bool>(ISTrainingPartResponseCode.Ok, true));
        }

        public Task<ISTrainingPartResponse<DetailInfo>> GetDetailInfoAsync(int id, DetailInfoType type)
        {
            return Task.FromResult(new ISTrainingPartResponse<DetailInfo>(ISTrainingPartResponseCode.Ok, new DetailInfo
            {
                Phone = "88005553535",
                Address = "г.о.г. Бор",
                EMail = "allaha@net.net",
            }));
        }
    }
}
