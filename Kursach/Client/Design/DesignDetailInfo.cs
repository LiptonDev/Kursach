using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using System.Threading.Tasks;

namespace ISTraining_Part.Client.Design
{
    class DesignDetailInfo : IDetailInfo
    {
        public Task<KursachResponse<bool>> AddOrUpdateAsync(DetailInfo detailInfo, DetailInfoType type)
        {
            return Task.FromResult(new KursachResponse<bool>(KursachResponseCode.Ok, true));
        }

        public Task<KursachResponse<DetailInfo>> GetDetailInfoAsync(int id, DetailInfoType type)
        {
            return Task.FromResult(new KursachResponse<DetailInfo>(KursachResponseCode.Ok, new DetailInfo
            {
                Phone = "88005553535",
                Address = "г.о.г. Бор",
                EMail = "allaha@net.net",
            }));
        }
    }
}
