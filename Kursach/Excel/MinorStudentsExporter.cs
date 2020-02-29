using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Manager;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Excel
{
    /// <summary>
    /// Экспорт несовершеннолетних.
    /// </summary>
    class MinorStudentsExporter : BaseExporter, IAsyncExporter<IEnumerable<IGrouping<Group, Student>>>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public MinorStudentsExporter(IDialogManager dialogManager) : base(dialogManager)
        {

        }

        /// <summary>
        /// Экспорт.
        /// </summary>
        /// <param name="students"></param>
        public Task<bool> Export(IEnumerable<IGrouping<Group, Student>> students)
        {
            if (students.Count() == 0)
                return Task.FromResult(false);

            if (!SelectFile("контингент несовершеннолетних"))
                return Task.FromResult(false);

            using (var excel = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Несовершеннолетние");

                //Глобальный стиль.
                worksheet.Cells.SetFontName("Arial").SetFontSize(12);

                var now = DateTime.Now;
                int year = now.Month >= 9 ? now.Year + 1 : now.Year - 1;
                int nowYear = now.Month >= 9 ? now.Year : now.Year;

                bool isFirst = now.Month >= 9;
                string years = $"{(isFirst ? nowYear : year)}-{(isFirst ? year : nowYear)}";

                worksheet.Cells["A1:F1"]
                    .SetValueWithBold($"Контингент несовершеннолетних, обучающихся ГБПОУ БГК {years} уч.год")
                    .SetMerge()
                    .SetWrapText()
                    .SetVerticalHorizontalAligment();

                worksheet.Cells["B2"]
                    .SetValueWithBold("Группа");

                worksheet.Cells["C2"]
                    .SetValueWithBold(DateTime.Now.ToShortDateString());

                int total = 0;
                int row = 0;
                foreach (var item in students)
                {
                    worksheet.Cells[3 + row, 2].SetValueWithBold(item.Key.Name);
                    int count = item.Count(x => IsMinor(x));
                    total += count;
                    worksheet.Cells[3 + row, 3].SetValue(count).SetHorizontalAligment();
                    row++;
                }

                worksheet.Cells[3, 1, 2 + row, 1].SetMerge()
                                                 .SetValueWithBold("Дневное отделение")
                                                 .SetVerticalHorizontalAligment()
                                                 .SetTextRotation(90);

                var corresCount = students.Where(x => !x.Key.IsIntramural).Sum(x => x.Count(s => IsMinor(s)));
                total += corresCount;

                worksheet.Cells[3 + row, 2].SetValueWithBold("заоч. отд.");
                worksheet.Cells[3 + row, 3].SetValue(corresCount).SetHorizontalAligment();

                var sabbaticalCount = students.SelectMany(x => x.Where(s => s.OnSabbatical && IsMinor(s))).Count();
                total += sabbaticalCount;

                worksheet.Cells[4 + row, 1].SetValueWithBold("Академ.");
                worksheet.Cells[4 + row, 3].SetValue(sabbaticalCount).SetHorizontalAligment();

                worksheet.Cells[5 + row, 1, 5 + row, 2].SetMerge().SetValueWithBold("Итого до 18").SetHorizontalAligment();
                worksheet.Cells[5 + row, 3].SetValueWithBold(total).SetHorizontalAligment();

                worksheet.Cells[2, 1, 5 + row, 3].SetTable();

                worksheet.Cells.AutoFitColumns(10);

                excel.SaveAs(new System.IO.FileInfo(FileName));
            }

            return Task.FromResult(true);
        }

        bool IsMinor(Student student)
        {
            var age = DateTime.Now.Year - student.Birthdate.Value.Year;
            if (DateTime.Now.DayOfYear > student.Birthdate.Value.DayOfYear)
                age--;

            return age < 18;
        }
    }
}
