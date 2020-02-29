using DevExpress.Mvvm;
using DryIoc;
using ISTraining_Part.Client.Design;
using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Core.Models.Enums;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Excel.Interfaces;
using ISTraining_Part.Providers;
using ISTraining_Part.ViewModels.Classes;
using ISTraining_Part.ViewModels.Interfaces;
using MaterialDesignThemes.Wpf;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ISTraining_Part.ViewModels
{
    /// <summary>
    /// Staf view model.
    /// </summary>
    class StaffViewModel : BaseViewModel<Staff>, IDetail
    {
        /// <summary>
        /// Экспорт данных.
        /// </summary>
        readonly IExporter<IEnumerable<Staff>> exporter;

        /// <summary>
        /// Конструктор для DesignTime
        /// </summary>
        public StaffViewModel()
        {
            Items = new ObservableCollection<Staff>();
            var res = new DesignStaff().GetStaffsAsync().Result;
            Items.AddRange(res.Response);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StaffViewModel(IExporter<IEnumerable<Staff>> exporter,
                              IDialogManager dialogManager,
                              ISnackbarMessageQueue snackbarMessageQueue,
                              IClient client,
                              IDataProvider dataProvider,
                              IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, dataProvider, container)
        {
            this.exporter = exporter;

            Items = dataProvider.Staff;

            ExportToExcelCommand = new DelegateCommand(ExportToExcel);

            ShowDetailInfoCommand = new DelegateCommand<People>(ShowDetailInfo, s => s != null);
            ShowDetailInfoEditorCommand = new DelegateCommand<People>(ShowDetailInfoEditor, s => s != null);
        }

        /// <summary>
        /// Команда экспорта данных в Excel.
        /// </summary>
        public ICommand ExportToExcelCommand { get; }

        /// <summary>
        /// Команда открытия детальной информации.
        /// </summary>
        public ICommand<People> ShowDetailInfoCommand { get; }

        /// <summary>
        /// Команда открытия редактирования детальной информации.
        /// </summary>
        public ICommand<People> ShowDetailInfoEditorCommand { get; }

        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        public override async void Add()
        {
            var editor = await dialogManager.StaffEditor(null, false);
            if (editor == null)
                return;

            var res = await client.Staff.AddStaffAsync(editor);
            var msg = res ? "Сотрудник добавлен" : res;

            Log(msg, editor);
        }

        /// <summary>
        /// Редактирование сотрудника.
        /// </summary>
        public override async Task Edit(Staff staff)
        {
            var editor = await dialogManager.StaffEditor(staff, true);
            if (editor == null)
                return;

            var res = await client.Staff.SaveStaffAsync(editor);
            var msg = res ? "Сотрудник сохранен" : res;

            Log(msg, staff);
        }

        /// <summary>
        /// Удаление сотрудника.
        /// </summary>
        public override async Task Delete(Staff staff)
        {
            var answ = await dialogIdentifier.ShowMessageBoxAsync($"Удалить '{staff.FullName}'?", MaterialMessageBoxButtons.YesNo);
            if (answ != MaterialMessageBoxButtons.Yes)
                return;

            var res = await client.Staff.RemoveStaffAsync(staff);
            var msg = res ? "Сотрудник удален" : res;

            Log(msg, staff);
        }


        /// <summary>
        /// Открыть окно редактирования детальной информации.
        /// </summary>
        private async void ShowDetailInfoEditor(People staff)
        {
            var editor = await dialogManager.ShowDetailInfoEditor(staff.Id, DetailInfoType.Staff);
            if (editor == null)
                return;

            var res = await client.DetailInfo.AddOrUpdateAsync(editor, DetailInfoType.Staff);
            var msg = res ? "Детальная информация сохранена" : res;

            snackbarMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Открыть окно детальной информации.
        /// </summary>
        private void ShowDetailInfo(People staff)
        {
            dialogManager.ShowDetailInfo(staff.Id, DetailInfoType.Staff);
        }

        /// <summary>
        /// Экспорт данных в Excel.
        /// </summary>
        public void ExportToExcel()
        {
            var res = exporter.Export(Items);
            var msg = res ? "Сотрудники экспортированы" : "Сотрудники не экспортированы";

            snackbarMessageQueue.Enqueue(msg);
        }

        /// <summary>
        /// Лог.
        /// </summary>
        void Log(string msg, Staff staff)
        {
            Logger.Log.Info($"{msg}: {{id: {staff.Id}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
