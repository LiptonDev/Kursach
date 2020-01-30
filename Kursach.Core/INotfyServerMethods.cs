namespace Kursach.Core
{
    /// <summary>
    /// Методы на сервере уведомлений.
    /// </summary>
    public interface INotfyServerMethods
    {
        /// <summary>
        /// Установить статус пользователя (вошел/вышел в программе).
        /// </summary>
        /// <param name="status">Статус.</param>
        /// <returns></returns>
        void SetStatus(bool status);

        /// <summary>
        /// Уведомить об изменении студента.
        /// </summary>
        /// <param name="oldGroupId">ИД группы, в которой находился студент.</param>
        /// <param name="newGroupId">ИД группы, в котором находится студент.</param>
        void ChangeStudent(int oldGroupId, int newGroupId);

        /// <summary>
        /// Уведомить об изменении группы.
        /// </summary>
        /// <param name="oldDivision">Подразделение, в котором находилась группа.</param>
        /// <param name="newDivision">Подразделение, в котором находится группа.</param>
        void ChangeGroup(int oldDivision, int newDivision);

        /// <summary>
        /// Уведомить об изменении сотрудника.
        /// </summary>
        void ChangeStaff();

        /// <summary>
        /// Уведомить об изменении пользователя.
        /// </summary>
        void ChangeUser();
    }
}
