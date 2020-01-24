using Kursach.Dialogs;

namespace Kursach.Excel
{
    class BaseImporter
    {
        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public BaseImporter(IDialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        /// <summary>
        /// Выбор файла.
        /// </summary>
        /// <returns></returns>
        public bool SelectFile()
        {
            string fileName = dialogManager.SelectImportFile();

            FileName = fileName;

            return fileName != null;
        }
    }
}
