using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace Kursach.Models
{
    /// <summary>
    /// Настройки программы.
    /// </summary>
    class ProgramSettings : IProgramSettings
    {
        #region static region
        static string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        /// <summary>
        /// Загрузка настроек.
        /// </summary>
        /// <returns></returns>
        public static ProgramSettings Load()
        {
            ProgramSettings settings = null;

            if (File.Exists(settingsPath))
            {
                try
                {
                    var json = File.ReadAllText(settingsPath);
                    settings = JsonConvert.DeserializeObject<ProgramSettings>(json);
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Ошибка чтения настроек: {ex.Message}");
                }
            }

            if (settings?.State == WindowState.Minimized)
                settings.State = WindowState.Normal;

            return settings ?? new ProgramSettings();
        }

        /// <summary>
        /// Сохранение настроек.
        /// </summary>
        /// <param name="settings">Настройки.</param>
        public static void Save(IProgramSettings settings)
        {
            try
            {
                var json = JsonConvert.SerializeObject(settings);
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                Logger.Log.Error($"Ошибка сохранения настроек: {ex.Message}");
            }
        }
        #endregion

        /// <summary>
        /// Состояние окна.
        /// </summary>
        public WindowState State { get; set; }

        /// <summary>
        /// Позиция 'X'.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Позиция 'Y'.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Высота окна.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Ширина окна.
        /// </summary>
        public int Width { get; set; }
    }
}
