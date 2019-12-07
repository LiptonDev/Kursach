using DevExpress.Mvvm;
using Kursach.Models;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// SignUp view model.
    /// </summary>
    class SignUpViewModel : ViewModelBase, IDialogIdentifier
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public string Identifier => "SignUpViewModel";

        /// <summary>
        /// Пользователь для регистрации.
        /// </summary>
        public LoginUser User { get; private set; }

        /// <summary>
        /// Права пользователя.
        /// </summary>
        public UserMode Mode { get; set; }

        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Ctor.
        /// </summary>
        public SignUpViewModel(IDataBase dataBase)
        {
            this.dataBase = dataBase;

            User = new LoginUser();
            Mode = UserMode.Read;

            SignUpCommand = new DelegateCommand(SignUp);
        }

        /// <summary>
        /// Команда регистрации.
        /// </summary>
        public ICommand SignUpCommand { get; }

        /// <summary>
        /// Регистрация.
        /// </summary>
        private async void SignUp()
        {
            if (!User.IsValid)
            {
                await this.ShowMessageBoxAsync(User.Error, MaterialMessageBoxButtons.Ok);
                return;
            }

            var user = await dataBase.GetUserAsync(User.Login, null, false);
            if (user != null)
            {
                await this.ShowMessageBoxAsync("Такой пользователь уже есть", MaterialMessageBoxButtons.Ok);
                Logger.Log.Info($"Ошибка регистрации нового пользователя: {{login: {User.Login}, mode: {Mode}}}");
                return;
            }

            Logger.Log.Info($"Регистрация нового пользователя: {{login: {User.Login}, mode: {Mode}}}");
            await dataBase.SignUpAsync(User.Login, User.Password, Mode);
            await this.ShowMessageBoxAsync("Пользователь успешно зарегистрирован", MaterialMessageBoxButtons.Ok);
            User.Login = "";
            User.Password = "";
            Mode = UserMode.Read;
        }
    }
}
