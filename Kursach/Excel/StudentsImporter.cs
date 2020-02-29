using ISTraining_Part.Core.Models;
using ISTraining_Part.Dialogs.Manager;
using ISTraining_Part.Excel.Classes;
using ISTraining_Part.Excel.Interfaces;
using ISTraining_Part.Providers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ISTraining_Part.Excel
{
    /// <summary>
    /// Импорт данных.
    /// </summary>
    class StudentsImporter : BaseImporter, IImporter<IEnumerable<Student>>
    {
        /// <summary>
        /// Поставщик данных.
        /// </summary>
        readonly IDataProvider dataProvider;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public StudentsImporter(IDialogManager dialogManager, IDataProvider dataProvider) : base(dialogManager)
        {
            this.dataProvider = dataProvider;
        }

        /// <summary>
        /// Импорт данных о студентах.
        /// </summary>
        public IEnumerable<Student> Import()
        {
            if (!SelectFile())
                return null;

            try
            {
                using (var excel = new ExcelPackage(new FileInfo(FileName)))
                {
                    //список студентов
                    var students = new List<Student>();

                    for (int i = 1; i < excel.Workbook.Worksheets.Count + 1; i++)
                    {
                        var worksheet = excel.Workbook.Worksheets[i];
                        var groupName = worksheet.Name.ToLower();

                        var group = dataProvider.Groups.FirstOrDefault(x => x.Name.ToLower() == groupName);
                        if (group == null)
                        {
                            continue;
                        }

                        var row = 0;
                        while (true)
                        {
                            var fio = worksheet.Cells[8 + row, 2]; //ФИО
                            var strFio = fio.GetValue<string>();
                            if (strFio.IsEmpty()) //конец
                                break;

                            var fioArr = strFio.Split(' ');
                            if (fioArr.Length != 3)
                                break;

                            var student = new Student
                            {
                                LastName = fioArr[0],
                                FirstName = fioArr[1],
                                MiddleName = fioArr[2],
                                PoPkNumber = worksheet.Cells[8 + row, 3].GetValue<string>(),
                                Birthdate = DateTime.Parse(worksheet.Cells[8 + row, 4].GetValue<string>()),
                                DecreeOfEnrollment = worksheet.Cells[8 + row, 5].GetValue<string>(),
                                Notice = worksheet.Cells[8 + row, 6].GetValue<string>(),
                                Expelled = fio.Style.Font.Strike,
                                GroupId = group.Id
                            };

                            students.Add(student);

                            row++;
                        }
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
