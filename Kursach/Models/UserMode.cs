using System.ComponentModel;

namespace Kursach.Models
{
    /// <summary>
    /// Права пользователя.
    /// </summary>
    enum UserMode
    {
        [Description("Администратор")]
        Admin = 0,

        [Description("Только просмотр")]
        Read = 1,

        [Description("Просмотр и редактирование")]
        ReadWrite = 2
    }
}
