﻿using DevExpress.Mvvm;
using DryIoc;
using Kursach.DataBase;
using Kursach.Models;
using Kursach.NotifyClient;
using Kursach.Properties;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using Prism.Regions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Login view model.
    /// </summary>
    class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// Пользователь для авторизации.
        /// </summary>
        public User User { get; }

        /// <summary>
        /// Менеджер регионов.
        /// </summary>
        readonly IRegionManager regionManager;

        /// <summary>
        /// Идентификатор диалоговых окон.
        /// </summary>
        readonly IDialogIdentifier dialogIdentifier;

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Клиент сервера уведомлений.
        /// </summary>
        readonly INotifyClient notifyClient;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public LoginViewModel(IRegionManager regionManager,
                              IDataBase dataBase,
                              INotifyClient notifyClient,
                              IContainer container)
        {
            this.regionManager = regionManager;
            this.dialogIdentifier = container.ResolveRootDialogIdentifier();
            this.dataBase = dataBase;
            this.notifyClient = notifyClient;

            TryLoginCommand = new AsyncCommand(TryLogin);

            User = new User
            {
                Login = Settings.Default.lastLogin,
                Password = Settings.Default.lastPassword
            };
        }

        /// <summary>
        /// Команда попытки входа в систему.
        /// </summary>
        public ICommand TryLoginCommand { get; }

        /// <summary>
        /// Попытка входа в систему.
        /// </summary>
        private async Task TryLogin()
        {
            if (!User.IsValid)
            {
                await dialogIdentifier.ShowMessageBoxAsync(User.Error, MaterialMessageBoxButtons.Ok);
                return;
            }

            Settings.Default.lastLogin = User.Login;
            Settings.Default.lastPassword = User.Password;

            var user = await dataBase.GetUserAsync(User.Login, User.Password, true);

            if (user == null)
            {
                await dialogIdentifier.ShowMessageBoxAsync("Неверный логин или пароль", MaterialMessageBoxButtons.Ok);
                Logger.Log.Info($"Неудачная попытка входа в систему: {{{Logger.GetParamsNamesValues(() => User.Login)}}}");
                return;
            }
            else
            {
                notifyClient.SetStatus(true);
                Logger.Log.Info($"Успешный вход в систему: {{{Logger.GetParamsNamesValues(() => User.Login)}}}");
                await dataBase.AddSignInLogAsync(user);

                NavigationParameters parameters = NavigationParametersFluent.GetNavigationParameters().SetUser(user).SetValue("fromLogin", true);
                regionManager.RequestNavigateInRootRegion(RegionViews.MainView, parameters);
                regionManager.ReqeustNavigateInMainRegion(RegionViews.WelcomeView);
            }
        }
    }
}
