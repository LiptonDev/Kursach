using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Students view model.
    /// </summary>
    class StudentsViewModel : NavigationViewModel
    {
        /// <summary>
        /// Пользователь.
        /// </summary>
        public User User { get; private set; }

        /// <summary>
        /// Группы.
        /// </summary>
        public ObservableCollection<Group> Groups { get; }

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// Идентификатор диалогов.
        /// </summary>
        readonly IDialogIdentifier dialogIdentifier;

        /// <summary>
        /// Ctor.
        /// </summary>
        public StudentsViewModel(IDataBase dataBase, IDialogManager dialogManager, IContainer container)
        {
            this.dataBase = dataBase;
            this.dialogManager = dialogManager;
            this.dialogIdentifier = container.ResolveRootDialogIdentifier();

            Groups = new ObservableCollection<Group>();

            EditStudentCommand = new DelegateCommand<Student>(EditStudent);
            DeleteStudentCommand = new AsyncCommand<Student>(DeleteStudent);
            AddStudentCommand = new DelegateCommand(AddStudent);
        }

        /// <summary>
        /// Команда добавления студента.
        /// </summary>
        public ICommand AddStudentCommand { get; }

        /// <summary>
        /// Команда редактирования студента.
        /// </summary>
        public ICommand<Student> EditStudentCommand { get; }

        /// <summary>
        /// Команда удаления студента.
        /// </summary>
        public ICommand<Student> DeleteStudentCommand { get; }

        /// <summary>
        /// Добавление студента.
        /// </summary>
        private async void AddStudent()
        {
            var editor = await dialogManager.StudentEditor(null, false);

            if (editor == null)
                return;

            var res = await dataBase.AddStudentAsync(editor);
            var msg = res ? "Студент добавлен" : "Студент не добавлен";

            Log(msg, editor);
        }

        /// <summary>
        /// Удаление студента.
        /// </summary>
        /// <returns></returns>
        private async Task DeleteStudent(Student student)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить студента '{student}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await dataBase.RemoveStudentAsync(student);
            var msg = res ? "Студент удален" : "Студент не удален";

            Log(msg, student);
        }

        /// <summary>
        /// Редактирование студента.
        /// </summary>
        private async void EditStudent(Student student)
        {
            var editor = await dialogManager.StudentEditor(student, true);

            if (editor == null)
                return;

            student.FirstName = editor.FirstName;
            student.LastName = editor.LastName;
            student.MiddleName = editor.MiddleName;
            student.GroupId = editor.GroupId;
            var res = await dataBase.SaveStudentAsync(student);
            var msg = res ? "Студент сохранен" : "Студент не сохранен";

            Log(msg, student);
        }

        async void Log(string msg, Student student)
        {
            Logger.Log.Info($"{msg}: {{student: {student}, group: {student.GroupId}}}");
            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
        }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        private async void Load()
        {
            Groups.Clear();
            await dataBase.LoadStudentsAsync();
            var res = await dataBase.GetGroupsAsync();
            Groups.AddRange(res);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            User = navigationContext.Parameters["user"] as User;

            Load();
        }
    }
}
