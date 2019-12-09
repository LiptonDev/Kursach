using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Staf view model.
    /// </summary>
    class StaffViewModel : NavigationViewModel
    {
        /// <summary>
        /// Сотрудники.
        /// </summary>
        public ObservableCollection<Staff> Staff { get; }

        /// <summary>
        /// Пользователь.
        /// </summary>
        public User User { get; private set; }

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
        public StaffViewModel(IDataBase dataBase, IContainer container, IDialogManager dialogManager)
        {
            this.dataBase = dataBase;
            this.dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.dialogManager = dialogManager;

            Staff = new ObservableCollection<Staff>();

            EditStaffCommand = new DelegateCommand<Staff>(EditStaff);
            DeleteStaffCommand = new DelegateCommand<Staff>(DeleteStaff);
            AddStaffCommand = new DelegateCommand(AddStaff);
        }

        /// <summary>
        /// Команда добавления сотрудника.
        /// </summary>
        public ICommand AddStaffCommand { get; }

        /// <summary>
        /// Команда редактирования сотрудника.
        /// </summary>
        public ICommand<Staff> EditStaffCommand { get; }

        /// <summary>
        /// Команда удаления сотрудника.
        /// </summary>
        public ICommand<Staff> DeleteStaffCommand { get; }

        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        private async void AddStaff()
        {
            var editor = await dialogManager.StaffEditor(null, false);

            if (editor == null)
                return;

            var res = await dataBase.AddStaffAsync(editor);
            var msg = res ? "Сотрудник добавлен" : "Сотрудник не добавлен";

            Logger.Log.Info($"{msg}: {{{editor}}}");

            if (res)
                Staff.Add(editor);

            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
        }

        /// <summary>
        /// Удаление сотрудника.
        /// </summary>
        private async void DeleteStaff(Staff staff)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить '{staff}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await dataBase.RemoveStaffAsync(staff);
            var msg = res ? "Сотрудник удален" : "Сотрудник не удален";

            Logger.Log.Info($"{msg}: {{{staff}}}");

            if (res)
                Staff.Remove(staff);

            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
        }

        /// <summary>
        /// Редактирование сотрудника.
        /// </summary>
        private async void EditStaff(Staff staff)
        {
            var editor = await dialogManager.StaffEditor(staff, true);

            if (editor == null)
                return;

            staff.FirstName = editor.FirstName;
            staff.LastName = editor.LastName;
            staff.MiddleName = editor.MiddleName;
            staff.Position = editor.Position;
            var res = await dataBase.SaveStaffAsync(staff);
            var msg = res ? "Сотрудник сохранен" : "Сотрудник не сохранен";

            Logger.Log.Info($"{msg}: {{{staff}}}");

            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
        }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        private async void Load()
        {
            Staff.Clear();
            var res = await dataBase.GetStaffsAsync();
            Staff.AddRange(res);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            User = navigationContext.Parameters["user"] as User;

            Load();
        }
    }
}
