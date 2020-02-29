using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Excel;
using ISTraining_Part.Providers;
using ISTraining_Part.ViewModels.Classes;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
{
    /// <summary>
    /// Groups view model.
    /// </summary>
    class GroupsViewModel : BaseViewModel<Group>
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
        readonly IAsyncExporter<IEnumerable<Group>> divisionsContingentExporter;

        /// <summary>
        /// Импорт данных.
        /// </summary>
        readonly IAsyncImporter<IEnumerable<Group>> divisionsContingentImporter;

        /// <summary>
        /// Импорт данных "Список групп".
        /// </summary>
        readonly IAsyncImporter<IEnumerable<Student>> studentsImporter;

        /// <summary>
        /// Экспорт "Список групп".
        /// </summary>
        readonly IAsyncExporter<IEnumerable<IGrouping<Group, Student>>> studentsExporter;

        /// <summary>
        /// Экспорт несовершеннолетних.
        /// </summary>
        readonly IAsyncExporter<IEnumerable<IGrouping<Group, Student>>> minorStudentsExporter;

        /// <summary>
        /// Менеджер регионов.
        /// </summary>
        readonly IRegionManager regionManager;

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
                               IAsyncExporter<IEnumerable<Group>> divisionsContingentExporter,
                               IAsyncImporter<IEnumerable<Group>> divisionsContingentImporter,
                               IAsyncImporter<IEnumerable<Student>> studentsImporter,
                               ISnackbarMessageQueue snackbarMessageQueue,
                               IRegionManager regionManager,
                               IClient client,
                               IDataProvider dataProvider,
                               IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, dataProvider, container)
        {
            this.divisionsContingentExporter = divisionsContingentExporter;
            this.divisionsContingentImporter = divisionsContingentImporter;
            this.studentsImporter = studentsImporter;
            this.studentsExporter = container.Resolve<IAsyncExporter<IEnumerable<IGrouping<Group, Student>>>>();
            this.minorStudentsExporter = container.Resolve<IAsyncExporter<IEnumerable<IGrouping<Group, Student>>>>("minor");
            this.regionManager = regionManager;

            Items = dataProvider.Groups;
            Groups = new ListCollectionView(Items);
            Groups.Filter += FilterGroup;

            DivisionsContingentExportCommand = new DelegateCommand(DivisionsContingentExport);

            DivisionsContingentImportCommand = new AsyncCommand(DivisionsContingentImport);
            StudentsImportCommand = new AsyncCommand(StudentsImport);

            StudentsExportCommand = new AsyncCommand(StudentsExport);
            MinorStudentsExportCommand = new AsyncCommand(MinorStudentsExport);

            ShowStudentsCommand = new DelegateCommand<Group>(ShowStudents, group => group != null);
        }

        /// <summary>
        /// Фильтрация группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns></returns>
        private bool FilterGroup(object group)
        {
            var gr = (Group)group;

            return gr.Division == selectedDivision;
        }

        /// <summary>
        /// Команда экспорта данных в Excel.
        /// </summary>
        public ICommand DivisionsContingentExportCommand { get; }

        /// <summary>
        /// Команда импорта данных из Excel.
        /// </summary>
        public ICommand DivisionsContingentImportCommand { get; }

        /// <summary>
        /// Команда импорта "Список групп".
        /// </summary>
        public ICommand StudentsImportCommand { get; }

        /// <summary>
        /// Команда экспорта "Список групп".
        /// </summary>
        public ICommand StudentsExportCommand { get; }

        /// <summary>
        /// Команда экспорта несовершеннолетних.
        /// </summary>
        public ICommand MinorStudentsExportCommand { get; }

        /// <summary>
        /// Команда перехода на просмотр студентов.
        /// </summary>
        public ICommand<Group> ShowStudentsCommand { get; }

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
        /// Импорт "Список групп".
        /// </summary>
        /// <returns></returns>
        private async Task StudentsImport()
        {
            var students = await studentsImporter.Import();

            if (students == null || students.Count() == 0)
                return;

            foreach (var item in students.Chunk(10)) //отправка по 10 студентов.
            {
                var res = await client.Students.ImportStudentsAsync(item);

                if (!res)
                    return;
            }

            await client.Students.RaiseStudentsImported();

            snackbarMessageQueue.Enqueue($"Добавлено студентов: {students.Count()}");
        }

        /// <summary>
        /// Перейти к отображению студентов.
        /// </summary>
        /// <param name="group">Выбранная группа.</param>
        private void ShowStudents(Group group)
        {
            var @params = NavigationParametersFluent.GetNavigationParameters()
                                                    .SetValue("group", group)
                                                    .SetUser(User);

            regionManager.ReqeustNavigateInMainRegion(RegionViews.StudentsView, @params);
        }

        /// <summary>
        /// Экспорт данных.
        /// </summary>
        public async void DivisionsContingentExport()
        {
            var groups = await client.Groups.GetGroupsAsync();
            if (!groups)
            {
                snackbarMessageQueue.Enqueue(groups);
                return;
            }

            var res = await divisionsContingentExporter.Export(groups.Response);
            var msg = res ? "Группы экспортированы" : "Группы не экспортированы";

            snackbarMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Импорт данных.
        /// </summary>
        public async Task DivisionsContingentImport()
        {
            var groups = await divisionsContingentImporter.Import();

            if (groups == null)
                return;

            var res = await client.Groups.AddGroupsAsync(groups);

            if (res)
                snackbarMessageQueue.Enqueue($"Добавлено групп: {groups.Count()}");
        }

        /// <summary>
        /// Экспорт несовершеннолетних.
        /// </summary>
        private async Task MinorStudentsExport()
        {
            var students = await GetStudents();
            var res = await minorStudentsExporter.Export(students);
            var msg = res ? "Несовершеннолетние экспортированы" : "Несовершеннолетние не экспортированы";
            snackbarMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Экспорт "Список групп".
        /// </summary>
        private async Task StudentsExport()
        {
            var students = await GetStudents();
            var res = await studentsExporter.Export(students);
            var msg = res ? "Студенты экспортированы" : "Студенты не экспортированы";
            snackbarMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Получить и сгруппировать всех студентов.
        /// </summary>
        /// <returns></returns>
        async Task<IEnumerable<IGrouping<Group, Student>>> GetStudents()
        {
            List<Student> students = new List<Student>();
            foreach (var item in Items)
            {
                var res = await client.Students.GetStudentsAsync(item.Id);
                if (!res)
                    return Enumerable.Empty<IGrouping<Group, Student>>();

                students.AddRange(res.Response);
            }

            return students.GroupBy(x => Items.FirstOrDefault(g => g.Id == x.GroupId));
        }

        void Log(string msg, Group group)
        {
            Logger.Log.Info($"{msg}: {{name: {group.Name}, curator: {group.CuratorId}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
