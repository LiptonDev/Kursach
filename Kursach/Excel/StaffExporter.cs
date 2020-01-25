using Kursach.Models;
using Kursach.Dialogs;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kursach.Excel
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
        /// <param name="staffs">Сотрудники.</param>
        public void Export(IEnumerable<Staff> staffs)
        {
            if (!SelectFile("сотрудники"))
                return;

            using (var excel = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Сотрудники");

                //Глобальный стиль.
                worksheet.Cells.SetFontName("Arial").SetFontSize(12);

                //ФИО
                worksheet.Cells["A1"]
                    .SetValue("ФИО")
                    .SetHorizontalAligment(ExcelHorizontalAlignment.Center)
                    .SetVerticalAligment(ExcelVerticalAlignment.Center)
                    .SetBold();

                //Должнось
                worksheet.Cells["B1"]
                    .SetValue("Должность")
                    .SetHorizontalAligment(ExcelHorizontalAlignment.Center)
                    .SetVerticalAligment(ExcelVerticalAlignment.Center)
                    .SetBold();

                int i = 0;
                foreach (var item in staffs)
                {
                    worksheet.Cells[2 + i, 1].SetValue(item);
                    worksheet.Cells[2 + i, 2].SetValue(item.Position);

                    i++;
                }

                //Таблица.
                worksheet.Cells[1, 1, 1 + i, 2].SetTable();

                worksheet.Cells.AutoFitColumns(1);

                try
                {
                    excel.SaveAs(new FileInfo(FileName));
                    Logger.Log.Info($"Экспорт информации о сотрудниках");
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Ошибка экспорта информации о сотрудниках: {{{ex.Message}}}");
                }
            }
        }
    }
}
