using Kursach.DataBase;
using Kursach.Dialogs;
using Kursach.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kursach.Excel
{
    /// <summary>
    /// Импорт данных.
    /// </summary>
    class GroupsImporter : BaseImporter, IAsyncImporter<IEnumerable<Group>>
    {
        /// <summary>
        /// База данных.
        /// </summary>
        readonly IDataBase dataBase;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupsImporter(IDataBase dataBase, IDialogManager dialogManager) : base(dialogManager)
        {
            this.dataBase = dataBase;
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

                    var groups = new List<Group>();

                    int curator = await dataBase.GetOrCreateFirstStaffIdAsync();

                    for (int i = 0; i < 3; i++)
                    {
                        int i6 = i * 6;

                        int row = 0;
                        while (true)
                        {
                            int currentRow = 3 + row;
                            string name = worksheet.Cells[currentRow, 2 + i6].GetValue<string>();

                            if (!System.Text.RegularExpressions.Regex.IsMatch(name ?? "", "^[а-яА-Я]{1,3}-[0-9]{2,2}$"))
                                break;

                            var group = new Group
                            {
                                Name = name,
                                SpoNpo = SPOHelper.GetIntSpo(worksheet.Cells[currentRow, 3 + i6].GetValue<string>()),
                                IsBudget = BudgetHelper.GetBoolBudget(worksheet.Cells[currentRow, 4 + i6].GetValue<string>()),
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

                    Logger.Log.Info($"Импорт данных о группах: {{count: {groups.Count}}}");

                    return groups;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error($"Импорт данных о группах", ex);

                return null;
            }
        }
    }
}
