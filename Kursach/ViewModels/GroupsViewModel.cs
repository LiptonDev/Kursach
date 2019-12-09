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
        /// Ctor.
        /// </summary>
        public GroupsViewModel(IDataBase dataBase, IContainer container, IDialogManager dialogManager)
        {
            this.dataBase = dataBase;
            dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.dialogManager = dialogManager;

            Groups = new ObservableCollection<Group>();

            DeleteGroupCommand = new AsyncCommand<Group>(DeleteGroup);
            GroupEditorCommand = new DelegateCommand<Group>(GroupEditor);
            AddGroupCommand = new DelegateCommand(AddGroup);
        }

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

            Logger.Log.Info($"{msg}: {{name: {editor.Name}, curatorId: {editor.CuratorId}}}");

            if (res)
                Groups.Add(editor);

            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
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
            var res = await dataBase.SaveGroupAsync(group);
            var msg = res ? "Группа сохранена" : "Группа не сохранена";

            Logger.Log.Info($"{msg}: {{name: {group.Name}, Curator: {group.CuratorId}}}");

            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
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

            Logger.Log.Info($"{msg}: {{name: {group.Name}, curatorId: {group.CuratorId}}}");

            if (res)
                Groups.Remove(group);

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
