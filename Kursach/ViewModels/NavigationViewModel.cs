﻿using DevExpress.Mvvm;
using Kursach.Models;
using Prism.Regions;

namespace Kursach.ViewModels
{
    abstract class NavigationViewModel : ViewModelBase, INavigationAware
    {
        /// <summary>
        /// Текущий пользователь.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        protected virtual void Load()
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("user"))
            {
                var robj = navigationContext.Parameters["user"] as User;
                User = robj;
                User.Mode = robj.Mode;
            }

            Load();
        }
    }
}
