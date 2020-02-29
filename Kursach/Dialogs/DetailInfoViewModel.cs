using DevExpress.Mvvm;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using ISTraining_Part.Dialogs.Attributes;

namespace ISTraining_Part.Dialogs
{
    /// <summary>
    /// Detail info view model.
    /// </summary>
    [DialogName(nameof(DetailInfoView))]
    class DetailInfoViewModel : ViewModelBase
    {
        /// <summary>
        /// Детальная информация.
        /// </summary>
        public DetailInfo DetailInfo { get; private set; }

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public DetailInfoViewModel()
        {
            DetailInfo = new DetailInfo
            {
                Phone = "88005553535",
                Address = "г.о.г. Бор",
                EMail = "allaha@net.net",
            };
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DetailInfoViewModel(int id, DetailInfoType type, IClient client)
        {
            Load(client, id, type);
        }

        /// <summary>
        /// Загрузка информации.
        /// </summary>
        private async void Load(IClient client, int id, DetailInfoType type)
        {
            var res = await client.DetailInfo.GetDetailInfoAsync(id, type);

            DetailInfo = res.Response;
        }
    }
}
