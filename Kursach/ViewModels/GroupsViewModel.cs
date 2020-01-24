using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using Kursach.Excel;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Groups view model.
    /// </summary>
    class GroupsViewModel : NavigationViewModel
    {
        /// <summary>
        /// Пользователь.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Группы.
        /// </summary>
        public ObservableCollection<Group> Groups { get; }

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Идентификатор диалогов.
        /// </summary>
        readonly IDialogIdentifier dialogIdentifier;

        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// Экспорт данных.
        /// </summary>
        readonly IExporter<IEnumerable<Group>> exporter;

        /// <summary>
        /// Ctor.
        /// </summary>
        public GroupsViewModel(IDataBase dataBase, IContainer container, IDialogManager dialogManager, IExporter<IEnumerable<Group>> exporter)
        {
            this.dataBase = dataBase;
            dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.dialogManager = dialogManager;
            this.exporter = exporter;

            Groups = new ObservableCollection<Group>();

            DeleteGroupCommand = new AsyncCommand<Group>(DeleteGroup);
            GroupEditorCommand = new DelegateCommand<Group>(GroupEditor);
            AddGroupCommand = new DelegateCommand(AddGroup);
            ExportToExcelCommand = new DelegateCommand(ExportToExcel);
        }

        /// <summary>
        /// Команда экспорта данных в Excel.
        /// </summary>
        public ICommand ExportToExcelCommand { get; }

        /// <summary>
        /// Команда добавления новой группы.
        /// </summary>
        public ICommand AddGroupCommand { get; }

        /// <summary>
        /// Команда открытия окна редактирования группы.
        /// </summary>
        public ICommand<Group> GroupEditorCommand { get; }

        /// <summary>
        /// Команда удаления группы.
        /// </summary>
        public ICommand<Group> DeleteGroupCommand { get; }

        /// <summary>
        /// Добавление группы.
        /// </summary>
        private async void AddGroup()
        {
            var editor = await dialogManager.GroupEditor(null, false);

            if (editor == null)
                return;

            var res = await dataBase.AddGroupAsync(editor);
            var msg = res ? "Группа добавлена" : "Группа не добавлена";

            if (res)
                Groups.Add(editor);

            Log(msg, editor.Name, editor.CuratorId);
        }

        /// <summary>
        /// Открытие окна редактирования группы.
        /// </summary>
        private async void GroupEditor(Group group)
        {
            var editor = await dialogManager.GroupEditor(group, true);

            if (editor == null)
                return;

            group.CuratorId = editor.CuratorId;
            group.Name = editor.Name;
            group.Start = editor.Start;
            group.End = editor.End;
            group.Specialty = editor.Specialty;
            group.IsBudget = editor.IsBudget;
            var res = await dataBase.SaveGroupAsync(group);
            var msg = res ? "Группа сохранена" : "Группа не сохранена";

            Log(msg, group.Name, group.CuratorId);
        }

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <returns></returns>
        private async Task DeleteGroup(Group group)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить группу '{group.Name}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await dataBase.RemoveGroupAsync(group);
            var msg = res ? "Группа удалена" : "Группа не удалена";

            if (res)
                Groups.Remove(group);

            Log(msg, group.Name, group.CuratorId);
        }

        /// <summary>
        /// Экспорт данных.
        /// </summary>
        private void ExportToExcel()
        {
            exporter.Export(Groups);
        }

        async void Log(string msg, string name, int id)
        {
            Logger.Log.Info($"{msg}: {{name: {name}, curatorId: {id}}}");
            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
        }

        /// <summary>
        /// Загрузка групп.
        /// </summary>
        private async void Load()
        {
            Groups.Clear();
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
