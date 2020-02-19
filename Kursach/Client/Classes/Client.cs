using ISTraining_Part.Client.Interfaces;

namespace ISTraining_Part.Client.Classes
{
    /// <summary>
    /// Клиент сервера.
    /// </summary>
    class Client : IClient
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Client(IUsers users, IStaff staff, IStudents students, IGroups groups, ILogin login, IChat chat, IHubConfigurator hubConfigurator)
        {
            Users = users;
            Staff = staff;
            Students = students;
            Groups = groups;
            Login = login;
            Chat = chat;
            HubConfigurator = hubConfigurator;
        }

        /// <summary>
        /// Управление пользователями.
        /// </summary>
        public IUsers Users { get; }

        /// <summary>
        /// Управление сотрудниками.
        /// </summary>
        public IStaff Staff { get; }

        /// <summary>
        /// Управление студентами.
        /// </summary>
        public IStudents Students { get; }

        /// <summary>
        /// Управление группами.
        /// </summary>
        public IGroups Groups { get; }

        /// <summary>
        /// Управление авторизацией.
        /// </summary>
        public ILogin Login { get; }

        /// <summary>
        /// Управление чатом.
        /// </summary>
        public IChat Chat { get; }

        /// <summary>
        /// Конфигуратор хаба.
        /// </summary>
        public IHubConfigurator HubConfigurator { get; }
    }
}
