using System.Windows;

namespace Kursach.Models
{
    /// <summary>
    /// Настройки программы.
    /// </summary>
    interface IProgramSettings
    {
        /// <summary>
        /// Состояние окна.
        /// </summary>
        WindowState State { get; set; }

        /// <summary>
        /// Позиция 'X'.
        /// </summary>
        int Left { get; set; }

        /// <summary>
        /// Позиция 'Y'.
        /// </summary>
        int Top { get; set; }

        /// <summary>
        /// Высота окна.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Ширина окна.
        /// </summary>
        int Width { get; set; }
    }
}
