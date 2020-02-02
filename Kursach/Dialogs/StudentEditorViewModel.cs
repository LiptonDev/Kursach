using DryIoc;
using Kursach.Client.Interfaces;
using Kursach.Core.Models;
using Kursach.Providers;
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
        /// Клиент.
        /// </summary>
        readonly IClient client;

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
                                      IClient client,
                                      IDataProvider dataProvider,
                                      IDialogManager dialogManager,
                                      IContainer container)
            : base(student, isEditMode, container)
        {
            this.dialogManager = dialogManager;
            this.client = client;

            Groups = new ListCollectionView(dataProvider.Groups);
            Groups.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Group.Division)));

            SelectedGroup = dataProvider.Groups.FirstOrDefault(x => x.Id == groupId);
        }
    }
}
