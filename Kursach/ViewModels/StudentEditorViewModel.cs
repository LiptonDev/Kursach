using DevExpress.Mvvm;
using DryIoc;
using Kursach.Models;
using Kursach.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using Kursach.DataBase;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Student editor view model.
    /// </summary>
    [DialogName(nameof(Views.StudentEditorView))]
    class StudentEditorViewModel : BaseEditModeViewModel<Student>
    {
        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Группы.
        /// </summary>
        public ObservableCollection<Group> Groups { get; }

        Group selectedGroup;
        /// <summary>
        /// Выбранная группа.
        /// </summary>
        public Group SelectedGroup
        {
            get
            {
                return selectedGroup;
            }
            set
            {
                selectedGroup = value;
                EditableObject.GroupId = value?.Id ?? -1;
            }
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        public StudentEditorViewModel(Student student, bool isEditMode, IContainer container, IDataBase dataBase, IDialogManager dialogManager)
            : base(student, isEditMode, container)
        {
            this.dialogManager = dialogManager;
            this.dataBase = dataBase;

            Groups = new ObservableCollection<Group>();

            Load();
        }

        /// <summary>
        /// Загрузка групп.
        /// </summary>
        private async void Load()
        {
            var res = await dataBase.GetGroupsAsync();
            Groups.AddRange(res);

            SelectedGroup = Groups.FirstOrDefault(x => x.Id == EditableObject.GroupId);
        }
    }
}
