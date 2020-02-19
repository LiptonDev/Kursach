namespace ISTraining_Part.Client.Interfaces
{
    /// <summary>
    /// Клиент для сервера.
    /// </summary>
    interface IClient
    {
        /// <summary>
        /// Конфигуратор хаба.
        /// </summary>
        IHubConfigurator HubConfigurator { get; }

        /// <summary>
        /// Управление пользователями.
        /// </summary>
        IUsers Users { get; }

        /// <summary>
        /// Управление сотрудниками.
        /// </summary>
        IStaff Staff { get; }

        /// <summary>
        /// Управление студентами.
        /// </summary>
        IStudents Students { get; }

        /// <summary>
        /// Управление группами.
        /// </summary>
        IGroups Groups { get; }

        /// <summary>
        /// Управление авторизацией.
        /// </summary>
        ILogin Login { get; }

        /// <summary>
        /// Управление чатом.
        /// </summary>
        IChat Chat { get; }
    }
}
