using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using Kursach.Excel;
using Kursach.Models;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        /// Группы.
        /// </summary>
        ObservableCollection<Group> groups { get; }

        /// <summary>
        /// Студенты выбранной группы.
        /// </summary>
        public ObservableCollection<Student> Students { get; }

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
                LoadStudents(value);
            }
        }

        /// <summary>
        /// Экспорт данных.
        /// </summary>
        readonly IExporter<Group, IEnumerable<Student>> exporter;

        /// <summary>
        /// Импорт данных.
        /// </summary>
        readonly IAsyncImporter<IEnumerable<Student>, Group> importer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentsViewModel(IDataBase dataBase,
                                 IDialogManager dialogManager,
                                 IExporter<Group, IEnumerable<Student>> exporter,
                                 IAsyncImporter<IEnumerable<Student>, Group> importer,
                                 ISnackbarMessageQueue snackbarMessageQueue,
                                 IContainer container)
            : base(dataBase, dialogManager, snackbarMessageQueue, container)
        {
            this.exporter = exporter;
            this.importer = importer;

            groups = new ObservableCollection<Group>();
            Students = new ObservableCollection<Student>();

            Groups = new ListCollectionView(groups);
            Groups.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Group.Division)));

            ExportToExcelCommand = new DelegateCommand(ExportToExcel);
            ImportFromExcelCommand = new DelegateCommand(ImportFromExcel);
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
            var editor = await dialogManager.StudentEditor(null, false, SelectedGroup?.Id ?? 0);
            if (editor == null)
                return;

            var res = await dataBase.AddStudentAsync(editor);
            var msg = res ? "Студент добавлен" : "Студент не добавлен";

            if (editor.GroupId == selectedGroup.Id)
                LoadStudents(selectedGroup);

            Log(msg, editor);
        }

        /// <summary>
        /// Редактирование студента.
        /// </summary>
        public override async void Edit(Student student)
        {
            var editor = await dialogManager.StudentEditor(student, true, student.GroupId);
            if (editor == null)
                return;

            var res = await dataBase.SaveStudentAsync(editor);
            var msg = res ? "Студент сохранен" : "Студент не сохранен";

            if (res)
            {
                student.FirstName = editor.FirstName;
                student.LastName = editor.LastName;
                student.MiddleName = editor.MiddleName;
                student.GroupId = editor.GroupId;
                student.Birthdate = editor.Birthdate;
                student.Expelled = editor.Expelled;
                student.DecreeOfEnrollment = editor.DecreeOfEnrollment;
                student.Notice = editor.Notice;
                student.PoPkNumber = editor.PoPkNumber;

                if (student.GroupId != selectedGroup.Id)
                    Students.Remove(student);
            }

            Log(msg, student);
        }

        /// <summary>
        /// Удаление студента.
        /// </summary>
        /// <returns></returns>
        public override async void Delete(Student student)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить студента '{student}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await dataBase.RemoveStudentAsync(student);
            var msg = res ? "Студент удален" : "Студент не удален";

            if (res)
                Students.Remove(student);

            Log(msg, student);
        }

        /// <summary>
        /// Экспорт информации о группе.
        /// </summary>
        public void ExportToExcel()
        {
            if (SelectedGroup == null)
            {
                snackbarMessageQueue.Enqueue("Вы не выбрали группу");
                return;
            }

            var res = exporter.Export(SelectedGroup, Students);
            var msg = res ? "Студенты экспортированы" : "Студенты не экспортированы";

            snackbarMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Импорт данных о группе.
        /// </summary>
        public async void ImportFromExcel()
        {
            if (SelectedGroup == null)
            {
                snackbarMessageQueue.Enqueue("Вы не выбрали группу");
                return;
            }

            var students = await importer.Import(SelectedGroup);

            if (students == null)
                return;

            var res = await dataBase.AddStudentsAsync(students);

            if (res)
            {
                snackbarMessageQueue.Enqueue($"Добавлено студентов: {students.Count()}");
                LoadStudents(SelectedGroup);
            }
        }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        protected override async void Load()
        {
            groups.Clear();
            var res = await dataBase.GetGroupsAsync();
            groups.AddRange(res);
        }

        /// <summary>
        /// Загрузка студентов группы.
        /// </summary>
        private async void LoadStudents(Group group)
        {
            Students.Clear();
            if (group == null)
                return;

            var res = await dataBase.GetStudentsAsync(group);
            Students.AddRange(res);
        }

        void Log(string msg, Student student)
        {
            Logger.Log.Info($"{msg}: {{student: {student}, group: {student.GroupId}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
