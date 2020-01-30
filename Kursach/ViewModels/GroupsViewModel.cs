using DevExpress.Mvvm;
using DryIoc;
using Kursach.Models;
using Kursach.Dialogs;
using Kursach.Excel;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Kursach.DataBase;
using MaterialDesignThemes.Wpf;
using System.Linq;
using Kursach.NotifyClient;

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
        public GroupsViewModel(IDataBase dataBase,
                               IDialogManager dialogManager,
                               IAsyncExporter<IEnumerable<Group>> exporter,
                               IAsyncImporter<IEnumerable<Group>> importer,
                               ISnackbarMessageQueue snackbarMessageQueue,
                               INotifyClient notifyClient,
                               IContainer container)
            : base(dataBase, dialogManager, snackbarMessageQueue, notifyClient, container)
        {
            this.exporter = exporter;
            this.importer = importer;

            Groups = new ObservableCollection<Group>();

            ExportToExcelCommand = new DelegateCommand(ExportToExcel);
            ImportFromExcelCommand = new DelegateCommand(ImportFromExcel);

            notifyClient.GroupChanged += NotifyClient_GroupChanged;
        }

        /// <summary>
        /// Группа в подразделении обновлена.
        /// </summary>
        private void NotifyClient_GroupChanged(int oldId, int newId)
        {
            if (selectedDivision == oldId || selectedDivision == newId)
                Load();

            if (oldId == -1 && newId == -1) //import
                Load();
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

            var res = await dataBase.AddGroupAsync(editor);
            var msg = res ? "Группа добавлена" : "Группа не добавлена";

            if (res && editor.Division == selectedDivision)
                Groups.Add(editor);

            if (res)
                notifyClient.ChangeGroup(editor.Division, editor.Division);

            Log(msg, editor);
        }

        /// <summary>
        /// Открытие окна редактирования группы.
        /// </summary>
        public override async void Edit(Group group)
        {
            var editor = await dialogManager.GroupEditor(group, true, group.Division);
            if (editor == null)
                return;

            var res = await dataBase.SaveGroupAsync(editor);
            var msg = res ? "Группа сохранена" : "Группа не сохранена";

            if (res)
            {
                int oldId = group.Division;
                int newId = editor.Division;

                group.CuratorId = editor.CuratorId;
                group.Name = editor.Name;
                group.Start = editor.Start;
                group.End = editor.End;
                group.Specialty = editor.Specialty;
                group.IsBudget = editor.IsBudget;
                group.Division = editor.Division;
                group.SpoNpo = editor.SpoNpo;
                group.IsIntramural = editor.IsIntramural;

                if (group.Division != selectedDivision)
                    Groups.Remove(group);

                notifyClient.ChangeGroup(oldId, newId);
            }

            Log(msg, group);
        }

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <returns></returns>
        public override async void Delete(Group group)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить группу '{group.Name}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await dataBase.RemoveGroupAsync(group);
            var msg = res ? "Группа удалена" : "Группа не удалена";

            if (res)
            {
                Groups.Remove(group);
                notifyClient.ChangeGroup(selectedDivision, selectedDivision);
            }

            Log(msg, group);
        }

        /// <summary>
        /// Экспорт данных.
        /// </summary>
        public async void ExportToExcel()
        {
            var groups = await dataBase.GetGroupsAsync();
            var res = await exporter.Export(groups);
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

            var res = await dataBase.AddGroupsAsync(groups);

            if (res)
            {
                snackbarMessageQueue.Enqueue($"Добавлено групп: {groups.Count()}");
                Load();
                notifyClient.ChangeGroup(-1, -1);
            }
        }

        /// <summary>
        /// Загрузка групп.
        /// </summary>
        protected override async void Load()
        {
            if (SelectedDivision == -1)
                return;

            Groups.Clear();
            var res = await dataBase.GetGroupsAsync(SelectedDivision);
            Groups.AddRange(res);
        }

        void Log(string msg, Group group)
        {
            Logger.Log.Info($"{msg}: {{{Logger.GetParamsNamesValues(() => group.Name, () => group.CuratorId)}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
