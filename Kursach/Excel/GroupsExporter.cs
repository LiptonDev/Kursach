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
    /// Экспорт данных о всех группах колледжа.
    /// </summary>
    class GroupsExporter : BaseExporter, IExporter<IEnumerable<Group>>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupsExporter(IDialogManager dialogManager) : base(dialogManager)
        {

        }

        /// <summary>
        /// Экспорт данных о всех группах колледжа.
        /// </summary>
        /// <param name="groups">Группы.</param>
        public void Export(IEnumerable<Group> groups)
        {
            if (!SelectFile("контингент по подразделениям"))
                return;

            using (var excel = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(DateTime.Now.ToString("MMMM"));

                //Глобальный стиль.
                worksheet.Cells.SetFontName("Arial").SetFontSize(12);

                worksheet.Cells["B1"]
                    .SetValue("Контингент обучающихся ГБПОУ БГК 2019-2020 уч.год")
                    .SetFontSize(14)
                    .SetBold();

                worksheet.Cells["A2:F2"]
                    .SetFontSize(10)
                    .SetHorizontalAligment(ExcelHorizontalAlignment.Center)
                    .SetVerticalAligment(ExcelVerticalAlignment.Center);

                worksheet.Cells["E2:F2"].SetFontSize(8);

                worksheet.Cells["A2:B2"].SetMerge().SetValue("Группа");
                worksheet.Cells["C2"].SetValue("СПО/НПО").SetWrapText();
                worksheet.Cells["D2"].SetValue("Б/К");
                worksheet.Cells["E2"].SetValue($"На {DateTime.Now.ToString("dd.MM.yyyy")}").SetWrapText();
                worksheet.Cells["F2"].SetValue("ак. отп").SetWrapText();

                int i = 0;
                foreach (var item in groups)
                {

                }

                worksheet.Cells.AutoFitColumns(1);

                try
                {
                    excel.SaveAs(new FileInfo(FileName));
                    Logger.Log.Info($"Экспорт информации о группах");
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Ошибка экспорта информации о группах: {{{ex.Message}}}");
                }
            }
        }
    }
}
