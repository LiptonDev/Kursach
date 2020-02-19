using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ISTraining_Part.Excel
{
    static class BudgetHelper
    {
        public static bool GetBoolBudget(string str)
        {
            switch (str)
            {
                case "Б": return true;
                case "К": return false;
                default: return true;
            }
        }

        public static string GetStrBudget(bool isBudget)
        {
            return isBudget ? "Б" : "К";
        }
    }

    static class SPOHelper
    {
        public static int GetIntSpo(string spo)
        {
            switch (spo)
            {
                case "СПО": return 0;
                case "НПО": return 1;
                case "ОВЗ": return 2;

                default:
                    return 0;
            }
        }

        public static string GetStrSpo(int value)
        {
            switch (value)
            {
                case 0: return "СПО";
                case 1: return "НПО";
                case 2: return "ОВЗ";

                default:
                    return "???";
            }
        }
    }

    static class ExcelHelper
    {
        public static ExcelRange SetValueWithBold(this ExcelRange excelRange, object value, bool bold = true)
        {
            return excelRange.SetValue(value).SetBold(bold);
        }

        public static ExcelRange SetValueWithBold(this ExcelRange excelRange, object value, float fontSize, bool bold = true)
        {
            return excelRange.SetValue(value, fontSize).SetBold(bold);
        }

        public static ExcelRange SetTextRotation(this ExcelRange excelRange, int rotation)
        {
            excelRange.Style.TextRotation = rotation;

            return excelRange;
        }

        public static ExcelRange SetTable(this ExcelRange excelRange)
        {
            excelRange.Style.Border.Top.Style = ExcelBorderStyle.Medium;
            excelRange.Style.Border.Right.Style = ExcelBorderStyle.Medium;
            excelRange.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            excelRange.Style.Border.Left.Style = ExcelBorderStyle.Medium;

            return excelRange;
        }

        public static ExcelRange SetWrapText(this ExcelRange excelRange, bool wrapText = true)
        {
            excelRange.Style.WrapText = wrapText;

            return excelRange;
        }

        public static ExcelRange SetFontName(this ExcelRange excelRange, string name)
        {
            excelRange.Style.Font.Name = name;

            return excelRange;
        }

        public static ExcelRange SetFontSize(this ExcelRange excelRange, float size)
        {
            excelRange.Style.Font.Size = size;

            return excelRange;
        }

        public static ExcelRange SetValue(this ExcelRange excelRange, object value)
        {
            excelRange.Value = value;

            return excelRange;
        }

        public static ExcelRange SetValue(this ExcelRange excelRange, object value, float fontSize)
        {
            return excelRange.SetValue(value).SetFontSize(fontSize);
        }

        public static ExcelRange SetVerticalHorizontalAligment(this ExcelRange excelRange, ExcelVerticalAlignment verticalAlignment = ExcelVerticalAlignment.Center, ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center)
        {
            return excelRange.SetHorizontalAligment(horizontalAlignment).SetVerticalAligment(verticalAlignment);
        }

        public static ExcelRange SetVerticalAligment(this ExcelRange excelRange, ExcelVerticalAlignment verticalAlignment = ExcelVerticalAlignment.Center)
        {
            excelRange.Style.VerticalAlignment = verticalAlignment;

            return excelRange;
        }

        public static ExcelRange SetHorizontalAligment(this ExcelRange excelRange, ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center)
        {
            excelRange.Style.HorizontalAlignment = horizontalAlignment;

            return excelRange;
        }

        public static ExcelRange SetBold(this ExcelRange excelRange, bool bold = true)
        {
            excelRange.Style.Font.Bold = bold;

            return excelRange;
        }

        public static ExcelRange SetMerge(this ExcelRange excelRange, bool merge = true)
        {
            excelRange.Merge = merge;

            return excelRange;
        }
    }
}
