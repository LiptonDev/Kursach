using DevExpress.Mvvm;
using DryIoc;
using Kursach.Models;
using Kursach.Dialogs;
using Kursach.Excel;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Kursach.DataBase;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Students view model.
    /// </summary>
    class StudentsViewModel : BaseViewModel<Student>, IExcelExporterViewModel
    {
        /// <summary>
        /// Группы.
        /// </summary>
        public ObservableCollection<Group> Groups { get; }

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
        readonly IImporter<IEnumerable<Student>, Group> importer;

        /// <summary>
        /// Ctor.
        /// </summary>
        public StudentsViewModel(IDataBase dataBase, IDialogManager dialogManager, IExporter<Group, IEnumerable<Student>> exporter, IImporter<IEnumerable<Student>, Group> importer, IContainer container)
            : base(dataBase, dialogManager, container)
        {
            this.exporter = exporter;
            this.importer = importer;

            Groups = new ObservableCollection<Group>();
            Students = new ObservableCollection<Student>();

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
            var editor = await dialogManager.StudentEditor(null, false);

            if (editor == null)
                return;

            var res = await dataBase.AddStudentAsync(editor);
            var msg = res ? "Студент добавлен" : "Студент не добавлен";

            if (editor.GroupId == selectedGroup.Id)
            {
                LoadStudents(selectedGroup);
            }

            Log(msg, editor);
        }

        /// <summary>
        /// Редактирование студента.
        /// </summary>
        public override async void Edit(Student student)
        {
            var editor = await dialogManager.StudentEditor(student, true);

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
                return;

            exporter.Export(SelectedGroup, Students);
        }

        /// <summary>
        /// Импорт данных о группе.
        /// </summary>
        private async void ImportFromExcel()
        {
            if (SelectedGroup == null)
                return;

            var students = importer.Import(SelectedGroup);

            if (students == null)
                return;

            var res = await dataBase.AddStudentsAsync(students);

            if (res)
            {
                Logger.Log.Info($"Импорт данных в группу: {{{SelectedGroup.Name}}}, кол-во студентов добавлено: {{{students.Count()}}}");
                LoadStudents(SelectedGroup);
            }
            else
            {
                Logger.Log.Error($"Импорт данных в группу: {{{SelectedGroup.Name}}}");
            }
        }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        protected override async void Load()
        {
            Groups.Clear();
            var res = await dataBase.GetGroupsAsync();
            Groups.AddRange(res);
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

        async void Log(string msg, Student student)
        {
            Logger.Log.Info($"{msg}: {{student: {student}, group: {student.GroupId}}}");
            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
        }
    }
}
