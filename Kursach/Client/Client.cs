namespace Kursach.Client
{
    /// <summary>
    /// Клиент сервера.
    /// </summary>
    class Client : IClient
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Client(IUsers users, IStaff staff, IStudents students, IGroups groups, ILogin login, IHubConfigurator hubConfigurator)
        {
            Users = users;
            Staff = staff;
            Students = students;
            Groups = groups;
            Login = login;
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
        /// Конфигуратор хаба.
        /// </summary>
        public IHubConfigurator HubConfigurator { get; }
    }
}
