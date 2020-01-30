using Kursach.Dialogs;
using System;

namespace Kursach.Excel
{
    class BaseExporter
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
        public BaseExporter(IDialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        /// <summary>
        /// Выбор файла для экспорта.
        /// </summary>
        /// <param name="defName">Начальное название файла.</param>
        /// <returns></returns>
        public bool SelectFile(string defName)
        {
            string fileName = dialogManager.SelectExportFileName(defName);

            FileName = fileName;

            return !fileName.IsEmpty();
        }
    }
}
