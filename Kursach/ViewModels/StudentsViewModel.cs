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
        ObservableCollection<Group> groups;

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
                LoadStudents();
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
        /// Конструктор для DesignTime.
        /// </summary>
        public StudentsViewModel()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentsViewModel(IDialogManager dialogManager,
                                 IExporter<Group, IEnumerable<Student>> exporter,
                                 IAsyncImporter<IEnumerable<Student>, Group> importer,
                                 ISnackbarMessageQueue snackbarMessageQueue,
                                 IClient client,
                                 IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, container)
        {
            this.exporter = exporter;
            this.importer = importer;

            groups = new ObservableCollection<Group>();
            Students = new ObservableCollection<Student>();

            Groups = new ListCollectionView(groups);
            Groups.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Group.Division)));

            ExportToExcelCommand = new DelegateCommand(ExportToExcel);
            ImportFromExcelCommand = new DelegateCommand(ImportFromExcel);

            client.Students.OnChanged += Students_OnChanged;
            client.Students.Imported += Students_Imported;
        }

        /// <summary>
        /// Импортированы студенты.
        /// </summary>
        /// <param name="groupId">ИД группы, в которую были импортированы студенты.</param>
        private void Students_Imported(int groupId)
        {
            if (SelectedGroup.Id == groupId)
                LoadStudents();
        }

        /// <summary>
        /// Изменения в студентах.
        /// </summary>
        private void Students_OnChanged(DbChangeStatus status, Student student)
        {
            switch (status)
            {
                case DbChangeStatus.Add:
                    if (selectedGroup != null && selectedGroup.Id == student.GroupId)
                        Students.Add(student);
                    break;

                case DbChangeStatus.Update:
                    if (selectedGroup == null)
                        return;

                    bool contains = Students.Contains(student);
                    if (student.GroupId != selectedGroup.Id && contains)
                    {
                        Students.Remove(student);
                    }
                    else if (student.GroupId == selectedGroup.Id && !contains)
                    {
                        Students.Add(student);
                    }
                    else
                    {
                        var current = Students.FirstOrDefault(x => x.Id == student.Id);
                        current?.SetAllFields(student);
                    }
                    break;

                case DbChangeStatus.Remove:
                    if (selectedGroup != null && selectedGroup.Id == student.GroupId && Students.Contains(student))
                    {
                        Students.Remove(student);
                    }
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
            if (selectedGroup == null)
            {
                snackbarMessageQueue.Enqueue("Вы не выбрали группу");
                return;
            }

            var res = exporter.Export(selectedGroup, Students);
            var msg = res ? "Студенты экспортированы" : "Студенты не экспортированы";

            snackbarMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Импорт данных о группе.
        /// </summary>
        public async void ImportFromExcel()
        {
            if (selectedGroup == null)
            {
                snackbarMessageQueue.Enqueue("Вы не выбрали группу");
                return;
            }

            var students = await importer.Import(selectedGroup);

            if (students == null)
                return;

            var res = await client.Students.AddStudentsAsync(students, selectedGroup.Id);
            var updateGroup = await client.Groups.SaveGroupAsync(selectedGroup);

            if (res && updateGroup)
            {
                snackbarMessageQueue.Enqueue($"Добавлено студентов: {students.Count()}");
            }
        }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        protected override async void Load()
        {
            groups.Clear();
            var res = await client.Groups.GetGroupsAsync();
            if (res)
                groups.AddRange(res.Response);
        }

        /// <summary>
        /// Загрузка студентов группы.
        /// </summary>
        private async void LoadStudents()
        {
            Students.Clear();
            if (selectedGroup == null)
                return;

            var res = await client.Students.GetStudentsAsync(selectedGroup.Id);
            if (res)
                Students.AddRange(res.Response);
        }

        void Log(string msg, Student student)
        {
            Logger.Log.Info($"{msg}: {{fullName: {student.FullName}, groupId: {student.GroupId}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
