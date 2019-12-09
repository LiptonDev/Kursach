using DevExpress.Mvvm;
using Kursach.DataBase;
using Kursach.Models;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using System.Windows.Input;

namespace Kursach.ViewModels
{
    /// <summary>
    /// Signup view model.
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
        public LoginUser User { get; }

        /// <summary>
        /// Права пользователя.
        /// </summary>
        public UserMode Mode { get; set; } = UserMode.Read;

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

            var res = await dataBase.SignUpAsync(User, Mode);

            if (res)
            {
                Logger.Log.Info($"Регистрация нового пользователя: {{login: {User.Login}, mode: {Mode}}}");
                await this.ShowMessageBoxAsync("Пользователь зарегистрирован", MaterialMessageBoxButtons.Ok);
            }
            else
            {
                Logger.Log.Info($"Не удачная регистрация нового пользователя, пользователь уже есть: {{login: {User.Login}, mode: {Mode}}}");
                await this.ShowMessageBoxAsync("Такой пользователь уже есть", MaterialMessageBoxButtons.Ok);
            }

            User.Login = "";
            User.Password = "";
            Mode = UserMode.Read;
        }
    }
}
