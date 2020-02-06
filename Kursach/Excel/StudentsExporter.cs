﻿using Kursach.Core.Models;
using Kursach.Dialogs;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Kursach.Excel
{
    /// <summary>
    /// Экспорт данных о группе и студентах.
    /// </summary>
    class StudentsExporter : BaseExporter, IExporter<IEnumerable<IGrouping<Group, Student>>>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentsExporter(IDialogManager dialogManager) : base(dialogManager)
        {

        }

        /// <summary>
        /// Экспорт данных о группе.
        /// </summary>
        /// <param name="group">Группа.</param>
        public bool Export(IEnumerable<IGrouping<Group, Student>> students)
        {
            if (students.Count() == 0)
                return false;

            if (!SelectFile("Список групп"))
                return false;

            using (var excel = new ExcelPackage())
            {
                foreach (var item in students)
                {
                    int count = item.Count();
                    if (count == 0)
                        continue;

                    ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(item.Key.Name);

                    //Глобальный стиль.
                    worksheet.Cells.SetFontName("Arial").SetFontSize(12);

                    //Название группы.
                    worksheet.Cells["B1"]
                        .SetValueWithBold($"Группа {item.Key.Name} ({(item.Key.IsBudget ? "бюдж." : "ком.")})");

                    //Дата начала обучения.
                    worksheet.Cells["A3:B3"]
                        .SetValue($"Начало обучения: {item.Key.Start?.ToString("dd.MM.yyyy")}г.")
                        .SetMerge()
                        .SetVerticalHorizontalAligment();

                    //Дата окончания обучения.
                    worksheet.Cells["E3:F3"]
                        .SetValue($"Окончание обучения: {item.Key.End?.ToString("dd.MM.yyyy")}г.")
                        .SetMerge()
                        .SetVerticalHorizontalAligment();

                    //Название специальности.
                    worksheet.Cells["A5:F5"]
                        .SetValue($"Специальность {item.Key.Specialty}")
                        .SetMerge();

                    worksheet.Cells["A7:F7"]
                        .SetBold()
                        .SetFontSize(11)
                        .SetVerticalHorizontalAligment();

                    //Заголовки таблицы.
                    SetTableHeader(worksheet);

                    int row = 0;
                    foreach (var student in item)
                    {
                        SetStudentInfo(worksheet, student, row);
                        row++;
                    }

                    //Выравнивание по середине.
                    worksheet.Cells[8, 1, 8 + count - 1, 6].SetVerticalHorizontalAligment();

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
                }

                try
                {
                    excel.SaveAs(new FileInfo(FileName));
                    Logger.Log.Info($"Экспорт информации о группе: {{fileName: {FileName}}}");
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Экспорт информации о группе: {{fileName: {FileName}}}", ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Информация о студенте.
        /// </summary>
        private void SetStudentInfo(ExcelWorksheet worksheet, Student item, int i)
        {
            worksheet.Cells[8 + i, 1].SetValue(i + 1);
            worksheet.Cells[8 + i, 2].SetValue(item.FullName);
            worksheet.Cells[8 + i, 3].SetValue(item.PoPkNumber);
            worksheet.Cells[8 + i, 4].SetValue(item.Birthdate?.ToString("dd.MM.yyyy"));
            worksheet.Cells[8 + i, 5].SetValue(item.DecreeOfEnrollment);
            worksheet.Cells[8 + i, 6].SetValue(item.Notice);

            if (item.Expelled)
            {
                worksheet.Cells[8 + i, 1, 8 + i, 6].Style.Fill.PatternType = ExcelFillStyle.DarkGray;
                worksheet.Cells[8 + i, 1, 8 + i, 6].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                worksheet.Cells[8 + i, 1, 8 + i, 5].Style.Font.Strike = true;
            }
        }

        /// <summary>
        /// Заголовки таблицы.
        /// </summary>
        private void SetTableHeader(ExcelWorksheet worksheet)
        {
            worksheet.Cells["A7"].SetValue("№");
            worksheet.Cells["B7"].SetValue("ФИО студента");
            worksheet.Cells["C7"].SetValue("№ по п/к").SetFontSize(9).SetWrapText();
            worksheet.Cells["D7"].SetValue("Дата рождения").SetWrapText();
            worksheet.Cells["E7"].SetValue("Приказ о зачислении");
            worksheet.Cells["F7"].SetValue("Примечание");
        }
    }
}
