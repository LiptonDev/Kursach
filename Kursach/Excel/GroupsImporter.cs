using ISTraining_Part.Client.Interfaces;
using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ISTraining_Part.Excel
{
    /// <summary>
    /// Импорт данных.
    /// </summary>
    class GroupsImporter : BaseImporter, IAsyncImporter<IEnumerable<Group>>
    {
        /// <summary>
        /// Клиент.
        /// </summary>
        readonly IClient client;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupsImporter(IClient client, IDialogManager dialogManager) : base(dialogManager)
        {
            this.client = client;
        }

        /// <summary>
        /// Импорт данных о групах.
        /// </summary>
        public async Task<IEnumerable<Group>> Import()
        {
            if (!SelectFile())
                return null;

            try
            {
                using (var excel = new ExcelPackage(new FileInfo(FileName)))
                {
                    var worksheet = excel.Workbook.Worksheets[1];

                    //список групп
                    var groups = new List<Group>();

                    //первый сотрудник из базы, если нет - создается новый "Иванов Иван Иванович"
                    var res = await client.Staff.GetOrCreateFirstStaffAsync();
                    if (!res)
                    {
                        return Enumerable.Empty<Group>();
                    }
                    int curator = res.Response.Id;

                    for (int i = 0; i < 3; i++)
                    {
                        int i6 = i * 6;

                        int row = 0; //строка
                        while (true)
                        {
                            int currentRow = 3 + row; //текущая строка
                            string name = worksheet.Cells[currentRow, 2 + i6].GetValue<string>() ?? ""; //название группы

                            //проверка регулярным выражением, формата: А-11, АА-11, ААА-11
                            if (!name.IsMatch("^[а-яА-Я]{1,3}-[0-9]{2}$"))
                                break;

                            var group = new Group
                            {
                                Name = name,
                                SpoNpo = SPOHelper.GetIntSpo(worksheet.Cells[currentRow, 3 + i6].GetValue<string>()), //СПО/НПО/ОВЗ -> int
                                IsBudget = BudgetHelper.GetBoolBudget(worksheet.Cells[currentRow, 4 + i6].GetValue<string>()), //Б/К -> int
                                Division = i,
                                End = DateTime.Now,
                                Start = DateTime.Now,
                                Specialty = "Специальность",
                                CuratorId = curator
                            };
                            groups.Add(group);
                            row++;
                        }
                    }

                    Logger.Log.Info($"Импорт данных о группах: {{fileName: {FileName}}}");

                    return groups;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error($"Импорт данных о группах: {{fileName: {FileName}}}", ex);

                return null;
            }
        }
    }
}
