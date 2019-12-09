using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Groups view model.
    /// </summary>
    class GroupsViewModel : INavigationAware
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

        /// <summary>
        /// Команда открытия окна редактирования группы.
        /// </summary>
        public ICommand<Group> GroupEditorCommand { get; }

        /// <summary>
        /// Команда удаления группы.
        /// </summary>
        public ICommand<Group> DeleteGroupCommand { get; }

        /// <summary>
        /// Открытие окна редактирования группы.
        /// </summary>
        private async void GroupEditor(Group group)
        {
            var editor = await dialogManager.GroupEditor(group);

            if (editor != null)
            {
                group.CuratorId = editor.CuratorId;
                group.Name = editor.Name;
                var res = await dataBase.SaveGroupAsync(group);
                var msg = res ? "Группа сохранена" : "Группа не сохранена";

                Logger.Log.Info($"{msg}: {{oldName: {group.Name}, newName: {editor.Name}, oldCurator: {group.CuratorId}, newCurator: {editor.CuratorId}}}");

                await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
            }
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

            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);

            Logger.Log.Info($"{msg}: {{group: {group.Name}}}");

            if (res)
                Groups.Remove(group);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            User = navigationContext.Parameters["user"] as User;

            Load();
        }
    }
}
