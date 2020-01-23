using Kursach.DataBase;
using Kursach.Dialogs;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.IO;

namespace Kursach.Excel
{
    /// <summary>
    /// Экспорт данных о сотрудниках.
    /// </summary>
    class StaffExporter : IExporter<IEnumerable<Staff>>
    {
        /// <summary>
        /// Менеджер диалогов.
        /// </summary>
        readonly IDialogManager dialogManager;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StaffExporter(IDialogManager dialogManager)
        {
            this.dialogManager = dialogManager;
        }

        /// <summary>
        /// Экспорт данных о сотрудниках.
        /// </summary>
        /// <param name="staffs">Сотрудники.</param>
        public void Export(IEnumerable<Staff> staffs)
        {
            string fileName = dialogManager.SelectExportFileName("сотрудники");

            if (fileName == null)
                return;

            using (var excel = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Сотрудники");

                //Глобальный стиль.
                worksheet.Cells.Style.Font.Name = "Arial";
                worksheet.Cells.SetFontSize(12);

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
                var table = worksheet.Cells[1, 1, 1 + i, 2];
                table.Style.Border.Top.Style = ExcelBorderStyle.Medium;
                table.Style.Border.Right.Style = ExcelBorderStyle.Medium;
                table.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                table.Style.Border.Left.Style = ExcelBorderStyle.Medium;

                worksheet.Cells.AutoFitColumns(1);

                excel.SaveAs(new FileInfo(fileName));
            }
        }
    }
}
