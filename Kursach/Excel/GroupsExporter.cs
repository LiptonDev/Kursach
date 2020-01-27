using Kursach.DataBase;
using Kursach.Dialogs;
using Kursach.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Excel
{
    /// <summary>
    /// Экспорт данных о всех группах колледжа.
    /// </summary>
    class GroupsExporter : BaseExporter, IAsyncExporter<IEnumerable<Group>>
    {
        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupsExporter(IDialogManager dialogManager, IDataBase dataBase) : base(dialogManager)
        {
            this.dataBase = dataBase;
        }

        /// <summary>
        /// Экспорт данных о всех группах колледжа.
        /// </summary>
        /// <param name="groups">Группы.</param>
        public async Task<bool> Export(IEnumerable<Group> groups)
        {
            if (!SelectFile("контингент по подразделениям"))
                return false;

            using (var excel = new ExcelPackage())
            {
                var worksheet = excel.Workbook.Worksheets.Add(DateTime.Now.ToString("MMMM"));

                //Глобальный стиль.
                worksheet.Cells.SetFontName("Arial").SetFontSize(10);

                var now = DateTime.Now;
                var year = now.Month >= 9 ? now.Year + 1 : now.Year - 1;
                var nowYear = now.Month >= 9 ? now.Year : now.Year;

                var isFirst = now.Month >= 9;
                var years = $"{(isFirst ? nowYear : year)}-{(isFirst ? year : nowYear)}";

                worksheet.Cells["B1:P1"]
                    .SetMerge()
                    .SetValueWithBold($"Контингент обучающихся ГБПОУ БГК {years} уч.год", 14)
                    .SetVerticalHorizontalAligment();

                var division0 = groups.Where(x => x.Division == 0);
                var division1 = groups.Where(x => x.Division == 1);
                var division2 = groups.Where(x => x.Division == 2);

                var divisions = new[] { division0, division1, division2 };

                var max = Enumerable.Max(new[] { division0.Count(), division1.Count(), division2.Count() });

                max += 2; //ВСЕГО, Очная, заочная

                var intramuralCount = 0;
                var correspondenceCount = 0;

                //Заголовки
                worksheet.Cells[2, 1, 2, 18].SetBold();

                for (int i = 0; i < 3; i++)
                {
                    var i6 = i * 6;

                    //Заголовки таблицы
                    SetTableHeader(worksheet, i6, max);

                    var studentsCount = await dataBase.GetStudentsCountAsync(divisions[i]);
                    var row = 0;
                    foreach (var item in studentsCount)
                    {
                        //Название группы, Б/К, пр.
                        SetGroupInfo(worksheet, row, i6, item);
                        row++;
                    }

                    var intra = studentsCount.Where(x => x.Key.IsIntramural).Sum(x => x.Value);
                    var corres = studentsCount.Where(x => !x.Key.IsIntramural).Sum(x => x.Value);
                    //Кол-во студентов в группах, в таблице
                    SetTableCount(worksheet, max, i6, intra, corres);
                    intramuralCount += intra;
                    correspondenceCount += corres;
                }

                //Кол-во студентов после таблицы.
                SetCountAfterTable(worksheet, max, intramuralCount, correspondenceCount);

                worksheet.Cells.SetVerticalHorizontalAligment();
                worksheet.Cells[2, 1, 2 + max + 1, 18].SetTable();
                worksheet.Cells.AutoFitColumns(5);

                try
                {
                    excel.SaveAs(new FileInfo(FileName));
                    Logger.Log.Info($"Экспорт информации о группах: {{count: {groups.Count()}}}");
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Экспорт информации о группах", ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Установить заголовки таблицы.
        /// </summary>
        void SetTableHeader(ExcelWorksheet worksheet, int i6, int max)
        {
            worksheet.Cells[2, 1 + i6, 2, 2 + i6].SetMerge().SetValue("Группа");
            worksheet.Cells[2, 3 + i6].SetValue("СПО/НПО").SetWrapText();
            worksheet.Cells[2, 4 + i6].SetValue("Б/К");
            worksheet.Cells[2, 5 + i6].SetValue($"На {DateTime.Now.ToString("dd.MM.yyyy")}").SetWrapText();
            worksheet.Cells[2, 6 + i6].SetValue("ак. отп").SetWrapText();

            worksheet.Cells[2, 5 + i6, 2, 6 + i6].SetFontSize(8);

            worksheet.Cells[3, 1 + i6, 3 + max, 1 + i6]
                .SetMerge()
                .SetValue($"{i6 / 6 + 1} подразделение")
                .SetTextRotation(90)
                .SetVerticalHorizontalAligment();
        }

        /// <summary>
        /// Информация о группе в таблице.
        /// </summary>
        void SetGroupInfo(ExcelWorksheet worksheet, int row, int i6, KeyValuePair<Group, int> keyValuePair)
        {
            worksheet.Cells[3 + row, 2 + i6].SetValueWithBold(keyValuePair.Key.Name); //Название
            worksheet.Cells[3 + row, 3 + i6].SetValue(SPOHelper.GetStrSpo(keyValuePair.Key.SpoNpo)); //СПО/НПО
            worksheet.Cells[3 + row, 4 + i6].SetValue(BudgetHelper.GetStrBudget(keyValuePair.Key.IsBudget)); //Б/К
            worksheet.Cells[3 + row, 5 + i6].SetValue(keyValuePair.Value); //Кол-во
        }

        /// <summary>
        /// Кол-во студентов после таблицы.
        /// </summary>
        void SetCountAfterTable(ExcelWorksheet worksheet, int max, int intra, int corres)
        {
            //ВСЕГО после таблицы
            worksheet.Cells[6 + max, 2, 6 + max, 4].SetMerge().SetValueWithBold($"Всего на {DateTime.Now.ToString("dd.MM.yyyy")}:");
            worksheet.Cells[6 + max, 5].SetValueWithBold(intra + corres);

            //ДНЕВНОЕ после таблицы
            worksheet.Cells[7 + max, 2].SetValueWithBold("Дневное:");
            worksheet.Cells[7 + max, 5].SetValueWithBold(intra);

            //ЗАОЧНОЕ после таблицы
            worksheet.Cells[8 + max, 2].SetValueWithBold("Заочное:");
            worksheet.Cells[8 + max, 5].SetValueWithBold(corres);
        }

        /// <summary>
        /// Кол-во студентов в таблице.
        /// </summary>
        void SetTableCount(ExcelWorksheet worksheet, int max, int i6, int intra, int corres)
        {
            //ВСЕГО в таблице
            worksheet.Cells[3 + max - 2, 2 + i6, 3 + max - 2, 4 + i6].SetMerge().SetValueWithBold("ВСЕГО:", 12);
            worksheet.Cells[3 + max - 2, 5 + i6].SetValueWithBold(intra + corres, 9.5f);

            //ОЧНАЯ в таблице
            worksheet.Cells[4 + max - 2, 2 + i6, 4 + max - 2, 4 + i6].SetMerge().SetValueWithBold("очная:", 11);
            worksheet.Cells[4 + max - 2, 5 + i6].SetValueWithBold(intra, 9.5f);

            //ЗАОЧНАЯ в таблице
            worksheet.Cells[5 + max - 2, 2 + i6, 5 + max - 2, 4 + i6].SetMerge().SetValueWithBold("заочная:", 11);
            worksheet.Cells[5 + max - 2, 5 + i6].SetValueWithBold(corres, 9.5f);
        }
    }
}
