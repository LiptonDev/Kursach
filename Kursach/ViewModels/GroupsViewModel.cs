using DevExpress.Mvvm;
using DryIoc;
using Kursach.Client.Interfaces;
using Kursach.Core.Models;
using Kursach.Dialogs;
using Kursach.Excel;
using Kursach.Providers;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Groups view model.
    /// </summary>
    class GroupsViewModel : BaseViewModel<Group>, IExcelExporterViewModel, IExcelImporterViewModel
    {
        /// <summary>
        /// Группы.
        /// </summary>
        public ListCollectionView Groups { get; }

        int selectedDivision = -1;
        /// <summary>
        /// Выбранное подразделение.
        /// </summary>
        public int SelectedDivision
        {
            get => selectedDivision;
            set
            {
                selectedDivision = value;
                Groups.Refresh();
            }
        }

        /// <summary>
        /// Экспорт данных.
        /// </summary>
        readonly IAsyncExporter<IEnumerable<Group>> exporter;

        /// <summary>
        /// Импорт данных.
        /// </summary>
        readonly IAsyncImporter<IEnumerable<Group>> importer;

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public GroupsViewModel()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupsViewModel(IDialogManager dialogManager,
                               IAsyncExporter<IEnumerable<Group>> exporter,
                               IAsyncImporter<IEnumerable<Group>> importer,
                               ISnackbarMessageQueue snackbarMessageQueue,
                               IClient client,
                               IDataProvider dataProvider,
                               IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, dataProvider, container)
        {
            this.exporter = exporter;
            this.importer = importer;

            Groups = new ListCollectionView(dataProvider.Groups);
            Groups.Filter += FilerGroup;

            ExportToExcelCommand = new DelegateCommand(ExportToExcel);
            ImportFromExcelCommand = new AsyncCommand(ImportFromExcel);
        }

        /// <summary>
        /// Фильтрация группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        private bool FilerGroup(object group)
        {
            var gr = (Group)group;

            return gr.Division == selectedDivision;
        }

        /// <summary>
        /// Команда экспорта данных в Excel.
        /// </summary>
        public ICommand ExportToExcelCommand { get; }

        /// <summary>
        /// Команда импорта данных из Excel.
        /// </summary>
        public ICommand ImportFromExcelCommand { get; }

        /// <summary>
        /// Добавление группы.
        /// </summary>
        public override async void Add()
        {
            var editor = await dialogManager.GroupEditor(null, false, SelectedDivision == -1 ? 0 : SelectedDivision);
            if (editor == null)
                return;

            var res = await client.Groups.AddGroupAsync(editor);
            var msg = res ? "Группа добавлена" : res;

            Log(msg, editor);
        }

        /// <summary>
        /// Открытие окна редактирования группы.
        /// </summary>
        public override async Task Edit(Group group)
        {
            var editor = await dialogManager.GroupEditor(group, true, group.Division);
            if (editor == null)
                return;

            var res = await client.Groups.SaveGroupAsync(editor);
            var msg = res ? "Группа сохранена" : res;

            Log(msg, group);
        }

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <returns></returns>
        public override async Task Delete(Group group)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить группу '{group.Name}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await client.Groups.RemoveGroupAsync(group);
            var msg = res ? "Группа удалена" : res;

            Log(msg, group);
        }

        /// <summary>
        /// Экспорт данных.
        /// </summary>
        public async void ExportToExcel()
        {
            var groups = await client.Groups.GetGroupsAsync();
            if (!groups)
            {
                snackbarMessageQueue.Enqueue(groups);
                return;
            }

            var res = await exporter.Export(groups.Response);
            var msg = res ? "Группы экспортированы" : "Группы не экспортированы";

            snackbarMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Импорт данных.
        /// </summary>
        public async Task ImportFromExcel()
        {
            var groups = await importer.Import();

            if (groups == null)
                return;

            var res = await client.Groups.AddGroupsAsync(groups);

            if (res)
                snackbarMessageQueue.Enqueue($"Добавлено групп: {groups.Count()}");
        }

        void Log(string msg, Group group)
        {
            Logger.Log.Info($"{msg}: {{name: {group.Name}, curator: {group.CuratorId}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
