using Kursach.DataBase;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Groups view model.
    /// </summary>
    class GroupsViewModel : INavigationAware
    {
        /// <summary>
        /// Группы.
        /// </summary>
        public ObservableCollection<Group> Groups { get; }

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Ctor.
        /// </summary>
        public GroupsViewModel(IDataBase dataBase)
        {
            this.dataBase = dataBase;

            Groups = new ObservableCollection<Group>();
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

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Load();
        }
    }
}
