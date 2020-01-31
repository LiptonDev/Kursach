using DevExpress.Mvvm;
using DryIoc;
using Kursach.Client.Interfaces;
using Kursach.Core.Models;
using Kursach.Core.ServerEvents;
using Kursach.Dialogs;
using Kursach.Excel;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
        public ObservableCollection<Group> Groups { get; }

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
                Load();
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
                               IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, container)
        {
            this.exporter = exporter;
            this.importer = importer;

            Groups = new ObservableCollection<Group>();

            ExportToExcelCommand = new DelegateCommand(ExportToExcel);
            ImportFromExcelCommand = new DelegateCommand(ImportFromExcel);

            client.Groups.OnChanged += Groups_OnChanged;
            client.Groups.Imported += Groups_Imported;
        }

        /// <summary>
        /// Группы были импортированы.
        /// </summary>
        private void Groups_Imported()
        {
            Load();
        }

        /// <summary>
        /// Изменения групп.
        /// </summary>
        private void Groups_OnChanged(DbChangeStatus status, Group group)
        {
            switch (status)
            {
                case DbChangeStatus.Add:
                    if (group.Division == selectedDivision)
                        Groups.Add(group);
                    break;

                case DbChangeStatus.Update:
                    bool contains = Groups.Contains(group);
                    if (group.Division != selectedDivision && contains)
                    {
                        Groups.Remove(group);
                    }
                    else if (group.Division == selectedDivision && !contains)
                    {
                        Groups.Add(group);
                    }
                    else
                    {
                        var current = Groups.FirstOrDefault(x => x.Division == selectedDivision);
                        current?.SetAllFields(group);
                    }
                    break;

                case DbChangeStatus.Remove:
                    if (group.Division == selectedDivision)
                        Groups.Remove(group);
                    break;
            }
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
        public async void ImportFromExcel()
        {
            var groups = await importer.Import();

            if (groups == null)
                return;

            var res = await client.Groups.AddGroupsAsync(groups);

            if (res)
                snackbarMessageQueue.Enqueue($"Добавлено групп: {groups.Count()}");
        }

        /// <summary>
        /// Загрузка групп.
        /// </summary>
        protected override async void Load()
        {
            if (SelectedDivision == -1)
                return;

            Groups.Clear();
            var res = await client.Groups.GetGroupsAsync(SelectedDivision);
            if (res)
                Groups.AddRange(res.Response);
        }

        void Log(string msg, Group group)
        {
            Logger.Log.Info($"{msg}: {{{Logger.GetParamsNamesValues(() => group.Name, () => group.CuratorId)}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
