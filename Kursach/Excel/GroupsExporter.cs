using Kursach.Client.Interfaces;
using Kursach.Core.Models;
using Kursach.Dialogs;
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
        /// Клиент.
        /// </summary>
        readonly IClient client;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupsExporter(IDialogManager dialogManager, IClient client) : base(dialogManager)
        {
            this.client = client;
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

                //Глобальный стиль
                worksheet.Cells.SetFontName("Arial").SetFontSize(10);

                var now = DateTime.Now;
                int year = now.Month >= 9 ? now.Year + 1 : now.Year - 1;
                int nowYear = now.Month >= 9 ? now.Year : now.Year;

                bool isFirst = now.Month >= 9;
                string years = $"{(isFirst ? nowYear : year)}-{(isFirst ? year : nowYear)}";

                worksheet.Cells["B1:P1"]
                    .SetMerge()
                    .SetValueWithBold($"Контингент обучающихся ГБПОУ БГК {years} уч.год", 14)
                    .SetVerticalHorizontalAligment();

                var division0 = groups.Where(x => x.Division == 0); //1-е подразделение
                var division1 = groups.Where(x => x.Division == 1); //2-е подразделение
                var division2 = groups.Where(x => x.Division == 2); //3-е подразделение

                var divisions = new[] { division0, division1, division2 }; //все подразделения

                int max = divisions.Max(x => x.Count()); //максимальное кол-во групп в любом подразделении
                max += 2; //ВСЕГО, Очная, заочная

                int intramuralCount = 0; //общее кол-во студентов на очном
                int correspondenceCount = 0; //общее кол-во студентов на заочном

                //Заголовки
                worksheet.Cells[2, 1, 2, 18].SetBold();

                for (int i = 0; i < 3; i++)
                {
                    var i6 = i * 6;

                    //Заголовки таблицы
                    SetTableHeader(worksheet, i6, max);

                    var count = await client.Students.GetStudentsCountAsync(divisions[i].Select(x => x.Division)); //кол-во студентов в группах определенного подразделения
                    var studentsCount = divisions[i].ToDictionary(x => x, x => count.Response[x.Id]);
                    int row = 0; //текущая строка
                    int intraSabbatical = 0; //очное в академ. отпуске
                    int corresSabbatical = 0; //заочное в академ. отпуске
                    int intra = 0; //кол-во студентов текущего подразделения (очное)
                    int corres = 0; //кол-во студентов текущего подразделения (заочное)
                    foreach (var item in studentsCount)
                    {
                        //Название группы, Б/ К, пр.
                        SetGroupInfo(worksheet, row, i6, item);
                        if (item.Key.IsIntramural)
                        {
                            intra += item.Value.Total;
                            intraSabbatical += item.Value.OnSabbatical;
                        }
                        else
                        {
                            corres += item.Value.Total;
                            corresSabbatical += item.Value.OnSabbatical;
                        }
                        row++;
                    }

                    SetTableCount(worksheet, max, i6, intra, corres, intraSabbatical, corresSabbatical);
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
                    Logger.Log.Info($"Экспорт информации о группах: {{fileName: {FileName}}}");
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error($"Экспорт информации о группах: {{fileName: {FileName}}}", ex);
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
        void SetGroupInfo(ExcelWorksheet worksheet, int row, int i6, KeyValuePair<Group, StudentsCount> keyValuePair)
        {
            worksheet.Cells[3 + row, 2 + i6].SetValueWithBold(keyValuePair.Key.Name); //Название
            worksheet.Cells[3 + row, 3 + i6].SetValue(SPOHelper.GetStrSpo(keyValuePair.Key.SpoNpo)); //СПО/НПО
            worksheet.Cells[3 + row, 4 + i6].SetValue(BudgetHelper.GetStrBudget(keyValuePair.Key.IsBudget)); //Б/К
            worksheet.Cells[3 + row, 5 + i6].SetValue(keyValuePair.Value.Total); //Кол-во
            worksheet.Cells[3 + row, 6 + i6].SetValue(keyValuePair.Value.OnSabbatical); //Ак. отп.
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
        void SetTableCount(ExcelWorksheet worksheet, int max, int i6, int intra, int corres, int intraSabbatical, int corresSabbatical)
        {
            //ВСЕГО в таблице
            worksheet.Cells[3 + max - 2, 2 + i6, 3 + max - 2, 4 + i6].SetMerge().SetValueWithBold("ВСЕГО:", 12);
            worksheet.Cells[3 + max - 2, 5 + i6].SetValueWithBold(intra + corres, 9.5f);
            worksheet.Cells[3 + max - 2, 6 + i6].SetValueWithBold(intraSabbatical + corresSabbatical, 9.5f);

            //ОЧНАЯ в таблице
            worksheet.Cells[4 + max - 2, 2 + i6, 4 + max - 2, 4 + i6].SetMerge().SetValueWithBold("очная:", 11);
            worksheet.Cells[4 + max - 2, 5 + i6].SetValueWithBold(intra, 9.5f);
            worksheet.Cells[4 + max - 2, 6 + i6].SetValueWithBold(intraSabbatical, 9.5f);

            //ЗАОЧНАЯ в таблице
            worksheet.Cells[5 + max - 2, 2 + i6, 5 + max - 2, 4 + i6].SetMerge().SetValueWithBold("заочная:", 11);
            worksheet.Cells[5 + max - 2, 5 + i6].SetValueWithBold(corres, 9.5f);
            worksheet.Cells[5 + max - 2, 6 + i6].SetValueWithBold(corresSabbatical, 9.5f);
        }
    }
}
