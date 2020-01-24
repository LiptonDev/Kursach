using Kursach.DataBase;
using Kursach.Dialogs;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Drawing;
using System.IO;

namespace Kursach.Excel
{
    /// <summary>
    /// Экспорт данных о группе.
    /// </summary>
    class GroupExporter : BaseExporter, IExporter<Group>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupExporter(IDialogManager dialogManager) : base(dialogManager)
        {

        }

        /// <summary>
        /// Экспорт данных о группе.
        /// </summary>
        /// <param name="group">Группа.</param>
        public void Export(Group group)
        {
            if (!SelectFile(group.Name))
                return;

            using (var excel = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("ИТ-41");

                //Глобальный стиль.
                worksheet.Cells.SetFontName("Arial").SetFontSize(12);

                //Название группы.
                worksheet.Cells["B1"]
                    .SetValue($"Группа {group.Name} ({(group.IsBudget ? "бюдж." : "ком.")})")
                    .SetBold();

                //Дата начала обучения.
                worksheet.Cells["A3:B3"]
                    .SetValue($"Начало обучения: {group.Start?.ToString("dd.MM.yyyy")}г.")
                    .SetMerge()
                    .SetHorizontalAligment(ExcelHorizontalAlignment.Center)
                    .SetVerticalAligment(ExcelVerticalAlignment.Center);

                //Дата окончания обучения.
                worksheet.Cells["E3:F3"]
                    .SetValue($"Окончание обучения: {group.End?.ToString("dd.MM.yyyy")}г.")
                    .SetMerge()
                    .SetHorizontalAligment(ExcelHorizontalAlignment.Center)
                    .SetVerticalAligment(ExcelVerticalAlignment.Center);

                //Название специальности.
                worksheet.Cells["A5:F5"]
                    .SetValue($"Специальность {group.Specialty}")
                    .SetMerge();

                worksheet.Cells["A7:F7"]
                    .SetBold()
                    .SetFontSize(11)
                    .SetHorizontalAligment(ExcelHorizontalAlignment.Center)
                    .SetVerticalAligment(ExcelVerticalAlignment.Center);

                worksheet.Cells["A7"].SetValue("№");
                worksheet.Cells["B7"].SetValue("ФИО студента");
                worksheet.Cells["C7"].SetValue("№ по п/к").SetFontSize(9).SetWrapText();
                worksheet.Cells["D7"].SetValue("Дата рождения").SetWrapText();
                worksheet.Cells["E7"].SetValue("Приказ о зачислении");
                worksheet.Cells["F7"].SetValue("Примечание");

                int count = group.Students.Count;

                //Список студентов.
                for (int i = 0; i < count; i++)
                {
                    var student = group.Students[i];
                    worksheet.Cells[8 + i, 1].SetValue(i + 1);
                    worksheet.Cells[8 + i, 2].SetValue(student);
                    worksheet.Cells[8 + i, 3].SetValue(student.PoPkNumber);
                    worksheet.Cells[8 + i, 4].SetValue(student.Birthdate?.ToString("dd.MM.yyyy"));
                    worksheet.Cells[8 + i, 5].SetValue(student.DecreeOfEnrollment);
                    worksheet.Cells[8 + i, 6].SetValue(student.Notice);

                    if (student.Expelled)
                    {
                        worksheet.Cells[8 + i, 1, 8 + i, 6].Style.Fill.PatternType = ExcelFillStyle.DarkGray;
                        worksheet.Cells[8 + i, 1, 8 + i, 6].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        worksheet.Cells[8 + i, 1, 8 + i, 5].Style.Font.Strike = true;
                    }
                }

                //Выравнивание по середине.
                worksheet.Cells[8, 1, 8 + count - 1, 6]
                    .SetHorizontalAligment(ExcelHorizontalAlignment.Center)
                    .SetVerticalAligment(ExcelVerticalAlignment.Center);

                //ФИО и Примечание выравнивание слева.
                worksheet.Cells[8, 2, 8 + count - 1, 2].SetHorizontalAligment(ExcelHorizontalAlignment.Left);
                worksheet.Cells[8, 6, 8 + count - 1, 6].SetHorizontalAligment(ExcelHorizontalAlignment.Left);

                //Размер "№".
                worksheet.Cells[8, 1, 8 + count - 1, 1].SetFontSize(11);
                //Размер "№ по п\к".
                worksheet.Cells[8, 3, 8 + count - 1, 3].SetFontSize(11);
                //Размер "Дата рождения".
                worksheet.Cells[8, 4, 8 + count - 1, 4].SetFontSize(11);
                //Размер "Приказ о зачислении".
                worksheet.Cells[8, 5, 8 + count - 1, 5].SetFontSize(10);
                //Размер "Примечание".
                worksheet.Cells[8, 6, 8 + count - 1, 6].SetFontSize(8).SetWrapText();

                //Таблица.
                worksheet.Cells[7, 1, 7 + count, 6].SetTable();

                worksheet.Cells.AutoFitColumns(1);

                try
                {
                    excel.SaveAs(new FileInfo(FileName));
                    Logger.Log.Info($"Экспорт информации о группе {{{group.Name}}}");
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Ошибка экспорта информации о группе {{{group.Name}}}: {{{ex.Message}}}");
                }
            }
        }
    }
}
