using DevExpress.Mvvm;
using DryIoc;
using Kursach.Models;
using Kursach.Dialogs;
using Kursach.Excel;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Kursach.DataBase;
using MaterialDesignThemes.Wpf;
using Kursach.NotifyClient;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Staf view model.
    /// </summary>
    class StaffViewModel : BaseViewModel<Staff>, IExcelExporterViewModel
    {
        /// <summary>
        /// Сотрудники.
        /// </summary>
        public ObservableCollection<Staff> Staff { get; }

        /// <summary>
        /// Экспорт данных.
        /// </summary>
        readonly IExporter<IEnumerable<Staff>> exporter;

        /// <summary>
        /// Конструктор для DesignTime
        /// </summary>
        public StaffViewModel()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StaffViewModel(IDataBase dataBase,
                              IExporter<IEnumerable<Staff>> exporter,
                              IDialogManager dialogManager,
                              ISnackbarMessageQueue snackbarMessageQueue,
                              INotifyClient notifyClient,
                              IContainer container)
            : base(dataBase, dialogManager, snackbarMessageQueue, notifyClient, container)
        {
            this.exporter = exporter;

            Staff = new ObservableCollection<Staff>();

            ExportToExcelCommand = new DelegateCommand(ExportToExcel);

            notifyClient.StaffChanged += Load;
        }

        /// <summary>
        /// Команда экспорта данных в Excel.
        /// </summary>
        public ICommand ExportToExcelCommand { get; }

        /// <summary>
        /// Экспорт данных в Excel.
        /// </summary>
        public void ExportToExcel()
        {
            var res = exporter.Export(Staff);
            var msg = res ? "Сотрудники экспортированы" : "Сотрудники не экспортированы";

            snackbarMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        public override async void Add()
        {
            var editor = await dialogManager.StaffEditor(null, false);
            if (editor == null)
                return;

            var res = await dataBase.AddStaffAsync(editor);
            var msg = res ? "Сотрудник добавлен" : "Сотрудник не добавлен";

            if (res)
            {
                Staff.Add(editor);
                notifyClient.ChangeStaff();
            }

            Log(msg, editor);
        }

        /// <summary>
        /// Редактирование сотрудника.
        /// </summary>
        public override async void Edit(Staff staff)
        {
            var editor = await dialogManager.StaffEditor(staff, true);
            if (editor == null)
                return;

            var res = await dataBase.SaveStaffAsync(editor);
            var msg = res ? "Сотрудник сохранен" : "Сотрудник не сохранен";

            if (res)
            {
                staff.FirstName = editor.FirstName;
                staff.LastName = editor.LastName;
                staff.MiddleName = editor.MiddleName;
                staff.Position = editor.Position;

                notifyClient.ChangeStaff();
            }

            Log(msg, staff);
        }

        /// <summary>
        /// Удаление сотрудника.
        /// </summary>
        public override async void Delete(Staff staff)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить '{staff}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await dataBase.RemoveStaffAsync(staff);
            var msg = res ? "Сотрудник удален" : "Сотрудник не удален";

            if (res)
            {
                Staff.Remove(staff);
                notifyClient.ChangeStaff();
            }

            Log(msg, staff);
        }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        protected override async void Load()
        {
            Staff.Clear();
            var res = await dataBase.GetStaffsAsync();
            Staff.AddRange(res);
        }

        void Log(string msg, Staff staff)
        {
            Logger.Log.Info($"{msg}: {{{Logger.GetParamsNamesValues(() => staff)}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
