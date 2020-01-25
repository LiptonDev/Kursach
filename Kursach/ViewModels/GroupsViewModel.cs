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

namespace Kursach.ViewModels
{
    /// <summary>
    /// Groups view model.
    /// </summary>
    class GroupsViewModel : BaseViewModel<Group>, IExcelExporterViewModel
    {
        /// <summary>
        /// Группы.
        /// </summary>
        public ObservableCollection<Group> Groups { get; }

        /// <summary>
        /// Экспорт данных.
        /// </summary>
        readonly IExporter<IEnumerable<Group>> exporter;

        /// <summary>
        /// Ctor.
        /// </summary>
        public GroupsViewModel(IDataBase dataBase, IContainer container, IDialogManager dialogManager, IExporter<IEnumerable<Group>> exporter)
            : base(dataBase, dialogManager, container)
        {
            this.exporter = exporter;

            Groups = new ObservableCollection<Group>();

            ExportToExcelCommand = new DelegateCommand(ExportToExcel);
        }

        /// <summary>
        /// Команда экспорта данных в Excel.
        /// </summary>
        public ICommand ExportToExcelCommand { get; }

        /// <summary>
        /// Добавление группы.
        /// </summary>
        public override async void Add()
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
        public override async void Edit(Group group)
        {
            var editor = await dialogManager.GroupEditor(group, true);

            if (editor == null)
                return;

            var res = await dataBase.SaveGroupAsync(editor);
            var msg = res ? "Группа сохранена" : "Группа не сохранена";

            if (res)
            {
                group.CuratorId = editor.CuratorId;
                group.Name = editor.Name;
                group.Start = editor.Start;
                group.End = editor.End;
                group.Specialty = editor.Specialty;
                group.IsBudget = editor.IsBudget;
            }

            Log(msg, group.Name, group.CuratorId);
        }

        /// <summary>
        /// Удаление группы.
        /// </summary>
        /// <returns></returns>
        public override async void Delete(Group group)
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
        public void ExportToExcel()
        {
            exporter.Export(Groups);
        }

        /// <summary>
        /// Загрузка групп.
        /// </summary>
        protected override async void Load()
        {
            Groups.Clear();
            var res = await dataBase.GetGroupsAsync();
            Groups.AddRange(res);
        }

        async void Log(string msg, string name, int id)
        {
            Logger.Log.Info($"{msg}: {{name: {name}, curatorId: {id}}}");
            await dialogIdentifier.ShowMessageBoxAsync(msg, MaterialMessageBoxButtons.Ok);
        }
    }
}
