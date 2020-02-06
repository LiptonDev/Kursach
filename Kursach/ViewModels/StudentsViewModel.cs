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
    /// Students view model.
    /// </summary>
    class StudentsViewModel : BaseViewModel<Student>, IExcelExporterViewModel, IExcelImporterViewModel
    {
        /// <summary>
        /// Группы.
        /// </summary>
        public ListCollectionView Groups { get; }

        /// <summary>
        /// Студенты выбранной группы.
        /// </summary>
        public ListCollectionView Students { get; }

        Group selectedGroup;
        /// <summary>
        /// Выбранная группа.
        /// </summary>
        public Group SelectedGroup
        {
            get => selectedGroup;
            set
            {
                selectedGroup = value;
                Students.Refresh();
            }
        }

        /// <summary>
        /// Экспорт данных.
        /// </summary>
        readonly IExporter<IEnumerable<IGrouping<Group, Student>>> exporter;

        /// <summary>
        /// Импорт данных.
        /// </summary>
        readonly IAsyncImporter<IEnumerable<Student>> importer;

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public StudentsViewModel()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentsViewModel(IDialogManager dialogManager,
                                 IExporter<IEnumerable<IGrouping<Group, Student>>> exporter,
                                 IAsyncImporter<IEnumerable<Student>> importer,
                                 ISnackbarMessageQueue snackbarMessageQueue,
                                 IClient client,
                                 IDataProvider dataProvider,
                                 IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, dataProvider, container)
        {
            this.exporter = exporter;
            this.importer = importer;

            Students = new ListCollectionView(dataProvider.Students);
            Students.Filter += StudentFilter;
            Groups = new ListCollectionView(dataProvider.Groups);
            Groups.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Group.Division)));

            ExportToExcelCommand = new DelegateCommand(ExportToExcel);
            ImportFromExcelCommand = new AsyncCommand(ImportFromExcel);
        }

        /// <summary>
        /// Фильтрация студентов.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns></returns>
        private bool StudentFilter(object student)
        {
            if (selectedGroup == null)
                return false;

            var st = (Student)student;

            return st.GroupId == selectedGroup.Id;
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
        /// Добавление студента.
        /// </summary>
        public override async void Add()
        {
            var editor = await dialogManager.StudentEditor(null, false, selectedGroup?.Id ?? 0);
            if (editor == null)
                return;

            var res = await client.Students.AddStudentAsync(editor);
            var msg = res ? "Студент добавлен" : res;

            Log(msg, editor);
        }

        /// <summary>
        /// Редактирование студента.
        /// </summary>
        public override async Task Edit(Student student)
        {
            var editor = await dialogManager.StudentEditor(student, true, student.GroupId);
            if (editor == null)
                return;

            var res = await client.Students.SaveStudentAsync(editor);
            var msg = res ? "Студент сохранен" : res;

            Log(msg, student);
        }

        /// <summary>
        /// Удаление студента.
        /// </summary>
        /// <returns></returns>
        public override async Task Delete(Student student)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить студента '{student.FullName}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await client.Students.RemoveStudentAsync(student);
            var msg = res ? "Студент удален" : res;

            Log(msg, student);
        }

        /// <summary>
        /// Экспорт информации о группе.
        /// </summary>
        public void ExportToExcel()
        {

            var res = exporter.Export(dataProvider.Students.GroupBy(x => dataProvider.Groups.FirstOrDefault(g => g.Id == x.GroupId)));
            var msg = res ? "Студенты экспортированы" : "Студенты не экспортированы";

            snackbarMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Импорт данных о студентах.
        /// </summary>
        public async Task ImportFromExcel()
        {
            var students = await importer.Import();

            if (students == null || students.Count() == 0)
                return;

            foreach (var item in students.Chunk(10)) //отправка данных по 10 студентов
            {
                var res = await client.Students.ImportStudentsAsync(item);

                if (!res)
                    return;
            }

            await client.Students.RaiseStudentsImported();

            snackbarMessageQueue.Enqueue($"Добавлено студентов: {students.Count()}");
        }

        void Log(string msg, Student student)
        {
            Logger.Log.Info($"{msg}: {{fullName: {student.FullName}, groupId: {student.GroupId}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
