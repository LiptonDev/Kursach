using Kursach.DataBase;
using Kursach.Dialogs;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kursach.Excel
{
    /// <summary>
    /// Импорт данных.
    /// </summary>
    class GroupImporter : BaseImporter, IImporter<IEnumerable<Student>, Group>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupImporter(IDialogManager dialogManager) : base(dialogManager)
        {

        }

        /// <summary>
        /// Импорт данных из Excel файла.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Student> Import(Group group)
        {
            if (!SelectFile())
                return null;

            using (var excel = new ExcelPackage(new FileInfo(FileName)))
            {
                ExcelWorksheet worksheet = excel.Workbook.Worksheets[1];

                List<Student> students = new List<Student>();

                int i = 0;
                while (true)
                {
                    var fio = worksheet.Cells[8 + i, 2];
                    string strFio = fio.GetValue<string>();
                    if (strFio == null)
                        break;

                    string[] fioArr = strFio.Split(' ');
                    Student student = new Student
                    {
                        LastName = fioArr[0],
                        FirstName = fioArr[1],
                        MiddleName = fioArr[2],
                        PoPkNumber = worksheet.Cells[8 + i, 3].GetValue<int>(),
                        Birthdate = DateTime.Parse(worksheet.Cells[8 + i, 4].GetValue<string>()),
                        DecreeOfEnrollment = worksheet.Cells[8 + i, 5].GetValue<string>(),
                        Notice = worksheet.Cells[8 + i, 6].GetValue<string>(),
                        GroupId = group.Id,
                    };

                    if (fio.Style.Font.Strike) //Студент отчислен
                    {
                        student.Expelled = true;
                    }

                    students.Add(student);

                    i++;
                }

                return students;
            }
        }
    }
}
