using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using ISTraining_Part.Dialogs.Attributes;
using ISTraining_Part.Dialogs.Classes;

namespace ISTraining_Part.Dialogs
{
    /// <summary>
    /// Detail info editor view model.
    /// </summary>
    [DialogName(nameof(DetailInfoEditorView))]
    class DetailInfoEditorViewModel : ViewModelBase
    {
        /// <summary>
        /// Редактор.
        /// </summary>
        public BaseEditModeViewModel<DetailInfo> Editor { get; private set; }

        /// <summary>
        /// Контейнер.
        /// </summary>
        readonly IContainer container;

        /// <summary>
        /// Конструктор для Design time.
        /// </summary>
        public DetailInfoEditorViewModel()
        {
            Editor = new BaseEditModeViewModel<DetailInfo>();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DetailInfoEditorViewModel(int id, DetailInfoType type, IClient client, IContainer container)
        {
            this.container = container;

            Load(client, id, type);
        }

        /// <summary>
        /// Загрузка информации.
        /// </summary>
        private async void Load(IClient client, int id, DetailInfoType type)
        {
            var res = await client.DetailInfo.GetDetailInfoAsync(id, type);

            var item = res.Response;
            if (item == null)
                item = new DetailInfo();

            Editor = new BaseEditModeViewModel<DetailInfo>(item, true, container);

            switch (type)
            {
                case DetailInfoType.Staff:
                    Editor.EditableObject.Staff = id;
                    break;
                case DetailInfoType.Student:
                    Editor.EditableObject.Student = id;
                    break;
            }
        }
    }
}
