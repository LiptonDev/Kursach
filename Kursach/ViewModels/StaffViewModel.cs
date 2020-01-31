﻿using DevExpress.Mvvm;
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
using System.Windows.Input;

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
        public StaffViewModel(IExporter<IEnumerable<Staff>> exporter,
                              IDialogManager dialogManager,
                              ISnackbarMessageQueue snackbarMessageQueue,
                              IClient client,
                              IContainer container)
            : base(dialogManager, snackbarMessageQueue, client, container)
        {
            this.exporter = exporter;

            Staff = new ObservableCollection<Staff>();

            ExportToExcelCommand = new DelegateCommand(ExportToExcel);

            client.Staff.OnChanged += Staff_OnChanged;
        }

        /// <summary>
        /// Изменения в сотрудниках.
        /// </summary>
        private void Staff_OnChanged(DbChangeStatus status, Staff staff)
        {
            switch (status)
            {
                case DbChangeStatus.Add:
                    Staff.Add(staff);
                    break;

                case DbChangeStatus.Update:
                    var current = Staff.FirstOrDefault(x => x.Id == staff.Id);
                    current?.SetAllFields(staff);
                    break;

                case DbChangeStatus.Remove:
                    Staff.Remove(staff);
                    break;
            }
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
        /// Загрузка данных.
        /// </summary>
        protected override async void Load()
        {
            Staff.Clear();
            var res = await client.Staff.GetStaffsAsync();
            if (res)
                Staff.AddRange(res.Response);
        }

        void Log(string msg, Staff staff)
        {
            Logger.Log.Info($"{msg}: {{fullName: {staff.FullName}, position: {staff.Position}}}");
            snackbarMessageQueue.Enqueue(msg);
        }
    }
}
