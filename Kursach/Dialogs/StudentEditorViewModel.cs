using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace Kursach.Dialogs
{
    /// <summary>
    /// Student editor view model.
    /// </summary>
    [DialogName(nameof(StudentEditorView))]
    class StudentEditorViewModel : BaseEditModeViewModel<Student>
    {
        /// <summary>
        /// Группы.
        /// </summary>
        public ListCollectionView Groups { get; }

        /// <summary>
        /// Группы.
        /// </summary>
        ObservableCollection<Group> groups { get; }

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

                RaisePropertyChanged(nameof(SelectedGroup));
            }
        }

        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Конструктор для DesignTime.
        /// </summary>
        public StudentEditorViewModel()
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentEditorViewModel(Student student,
                                      bool isEditMode,
                                      int groupId,
                                      IDataBase dataBase,
                                      IDialogManager dialogManager,
                                      IContainer container)
            : base(student, isEditMode, container)
        {
            this.dialogManager = dialogManager;
            this.dataBase = dataBase;

            groups = new ObservableCollection<Group>();

            Groups = new ListCollectionView(groups);
            Groups.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Group.Division)));

            Load(groupId);
        }

        /// <summary>
        /// Загрузка групп.
        /// </summary>
        private async void Load(int groupId)
        {
            var res = await dataBase.GetGroupsAsync();
            groups.AddRange(res);

            SelectedGroup = groups.FirstOrDefault(x => x.Id == groupId);
        }
    }
}
