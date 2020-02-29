using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using ISTraining_Part.Core.ServerEvents;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Providers;
using ISTraining_Part.ViewModels.Classes;
using ISTraining_Part.ViewModels.Interfaces;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
{
    /// <summary>
    /// Students view model.
    /// </summary>
    class StudentsViewModel : BaseViewModel<Student>, IDetail
    {
        /// <summary>
        /// Группа.
        /// </summary>
        Group selectedGroup;

        /// <summary>
        /// Журнал.
        /// </summary>
        IRegionNavigationJournal journal;

        /// <summary>
        /// Синхронизация.
        /// </summary>
        readonly TaskFactory sync;

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
                                 ISnackbarMessageQueue snackbarMessageQueue,
                                 IClient client,
                                 IDataProvider dataProvider,
                                 IContainer container,
                                 TaskFactory sync)
            : base(dialogManager, snackbarMessageQueue, client, dataProvider, container)
        {
            this.sync = sync;

            Items = new ObservableCollection<Student>();

            client.Students.OnChanged += Students_OnChanged;
            client.Students.Imported += Load;

            GoBackCommand = new DelegateCommand(GoBack);
            ShowDetailInfoCommand = new DelegateCommand<People>(ShowDetailInfo, s => s != null);
            ShowDetailInfoEditorCommand = new DelegateCommand<People>(ShowDetailInfoEditor, s => s != null);
        }

        /// <summary>
        /// Команда возвращения назад.
        /// </summary>
        public ICommand GoBackCommand { get; }

        /// <summary>
        /// Команда открытия детальной информации.
        /// </summary>
        public ICommand<People> ShowDetailInfoCommand { get; }

        /// <summary>
        /// Команда открытия редактирования детальной информации.
        /// </summary>
        public ICommand<People> ShowDetailInfoEditorCommand { get; }

        /// <summary>
        /// Открыть окно редактирования детальной информации.
        /// </summary>
        private async void ShowDetailInfoEditor(People student)
        {
            var editor = await dialogManager.ShowDetailInfoEditor(student.Id, DetailInfoType.Student);
            if (editor == null)
                return;

            var res = await client.DetailInfo.AddOrUpdateAsync(editor, DetailInfoType.Student);
            var msg = res ? "Детальная информация сохранена" : res;

            snackbarMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Открыть окно детальной информации.
        /// </summary>
        private void ShowDetailInfo(People student)
        {
            dialogManager.ShowDetailInfo(student.Id, DetailInfoType.Student);
        }

        /// <summary>
        /// Вернуться назад.
        /// </summary>
        private void GoBack()
        {
            journal.GoBack();
        }

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
        /// Переход на просмотр студентов.
        /// </summary>
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            journal = navigationContext.NavigationService.Journal;

            selectedGroup = navigationContext.Parameters["group"] as Group;

            Load();
        }

        /// <summary>
        /// Загрузка студентов в группе.
        /// </summary>
        private async void Load()
        {
            await sync.StartNew(() => Items.Clear());

            var res = await client.Students.GetStudentsAsync(selectedGroup.Id);

            if (res)
                await sync.StartNew(() => Items.AddRange(res.Response));
        }

        /// <summary>
        /// Изменения в студентах.
        /// </summary>
        private void Students_OnChanged(DbChangeStatus status, Student student)
        {
            if (student.GroupId != selectedGroup.Id)
                return;

            ProcessChangesHelper.ProcessChanges(status, student, Items, sync);
        }

        /// <summary>
        /// Log.
        /// </summary>
        void Log(string msg, Student student)
        {
            Logger.Log.Info($"{msg}: {{fullName: {student.FullName}, groupId: {student.GroupId}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
