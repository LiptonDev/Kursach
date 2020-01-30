namespace Kursach.Core
{
    /// <summary>
    /// События, которые происходят на сервере уведомлений.
    /// </summary>
    public interface INotifyServerEvents
    {
        /// <summary>
        /// Уведомить об изменении студента.
        /// </summary>
        /// <param name="oldGroupId">ИД группы, в которой находился студент.</param>
        /// <param name="newGroupId">ИД группы, в котором находится студент.</param>
        void StudentChanged(int oldGroupId, int newGroupId);

        /// <summary>
        /// Уведомить об изменении группы.
        /// </summary>
        /// <param name="oldDivision">Подразделение, в котором находилась группа.</param>
        /// <param name="newDivision">Подразделение, в котором находится группа.</param>
        void GroupChanged(int oldDivision, int newDivision);

        /// <summary>
        /// Уведомить об изменении сотрудника.
        /// </summary>
        void StaffChanged();

        /// <summary>
        /// Уведомить об изменении пользователя.
        /// </summary>
        void UserChanged();
    }
}
