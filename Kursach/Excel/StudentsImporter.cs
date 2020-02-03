using DryIoc;
using Kursach.Core.Models;
using Kursach.Dialogs;
using MaterialDesignXaml.DialogsHelper;
using MaterialDesignXaml.DialogsHelper.Enums;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Kursach.Excel
{
    /// <summary>
    /// Импорт данных.
    /// </summary>
    class StudentsImporter : BaseImporter, IAsyncImporter<IEnumerable<Student>, Group>
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        readonly IDialogIdentifier dialogIdentifier;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentsImporter(IContainer container, IDialogManager dialogManager) : base(dialogManager)
        {
            this.dialogIdentifier = container.ResolveRootDialogIdentifier();
        }

        /// <summary>
        /// Импорт данных о студентах.
        /// </summary>
        public async Task<IEnumerable<Student>> Import(Group group)
        {
            if (!SelectFile())
                return null;

            try
            {
                using (var excel = new ExcelPackage(new FileInfo(FileName)))
                {
                    var worksheet = excel.Workbook.Worksheets[1];

                    //проверка названия группы в файле и в программе
                    if (group.Name.ToLower() != worksheet.Name.ToLower())
                    {
                        var res = await dialogIdentifier.ShowMessageBoxAsync($"Выбранная группа: {group.Name}\nГруппа в файле: {worksheet.Name}\nПродолжить?", MaterialMessageBoxButtons.YesNo);
                        if (res == MaterialMessageBoxButtons.No)
                            return null;
                    }

                    //список студентов
                    var students = new List<Student>();

                    var i = 0;
                    while (true)
                    {
                        var fio = worksheet.Cells[8 + i, 2]; //ФИО
                        var strFio = fio.GetValue<string>();
                        if (strFio.IsEmpty()) //конец
                            break;

                        var fioArr = strFio.Split(' ');
                        var student = new Student
                        {
                            LastName = fioArr[0],
                            FirstName = fioArr[1],
                            MiddleName = fioArr[2],
                            PoPkNumber = worksheet.Cells[8 + i, 3].GetValue<int>(),
                            Birthdate = DateTime.Parse(worksheet.Cells[8 + i, 4].GetValue<string>()),
                            DecreeOfEnrollment = worksheet.Cells[8 + i, 5].GetValue<string>(),
                            Notice = worksheet.Cells[8 + i, 6].GetValue<string>(),
                            Expelled = fio.Style.Font.Strike,
                            GroupId = group.Id
                        };

                        students.Add(student);

                        i++;
                    }

                    Logger.Log.Info($"Импорт данных о студентах: {{fileName: {FileName}}}");

                    return students;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error($"Импорт данных о студентах: {{fileName: {FileName}}}", ex);

                return null;
            }
        }
    }
}
