using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Excel.Classes;
using ISTraining_Part.Excel.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace ISTraining_Part.Excel
{
    /// <summary>
    /// Экспорт данных о сотрудниках.
    /// </summary>
    class StaffExporter : BaseExporter, IExporter<IEnumerable<Staff>>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public StaffExporter(IDialogManager dialogManager) : base(dialogManager)
        {
        }

        /// <summary>
        /// Экспорт данных о сотрудниках.
        /// </summary>
        /// <param name="staff">Сотрудники.</param>
        public bool Export(IEnumerable<Staff> staff)
        {
            if (!SelectFile("сотрудники"))
                return false;

            using (var excel = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Сотрудники");

                //Глобальный стиль.
                worksheet.Cells.SetFontName("Arial").SetFontSize(12);

                //ФИО
                worksheet.Cells["A1"]
                    .SetValueWithBold("ФИО")
                    .SetVerticalHorizontalAligment();

                //Должнось
                worksheet.Cells["B1"]
                    .SetValueWithBold("Должность")
                    .SetVerticalHorizontalAligment();

                int i = 0;
                foreach (var item in staff)
                {
                    worksheet.Cells[2 + i, 1].SetValue(item.FullName);
                    worksheet.Cells[2 + i, 2].SetValue(item.Position);

                    i++;
                }

                //Таблица.
                worksheet.Cells[1, 1, 1 + i, 2].SetTable();

                worksheet.Cells.AutoFitColumns(1);

                try
                {
                    excel.SaveAs(new FileInfo(FileName));
                    Logger.Log.Info($"Экспорт информации о сотрудниках: {{fileName: {FileName}}}");
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Экспорт информации о сотрудниках: {{fileName: {FileName}}}", ex);
                    return false;
                }
            }
        }
    }
}
