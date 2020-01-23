using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Kursach.Excel
{
    static class ExcelHelper
    {
        public static ExcelRange SetWrapText(this ExcelRange excelRange, bool wrapText = true)
        {
            excelRange.Style.WrapText = wrapText;

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

        public static ExcelRange SetVerticalAligment(this ExcelRange excelRange, ExcelVerticalAlignment verticalAlignment)
        {
            excelRange.Style.VerticalAlignment = verticalAlignment;

            return excelRange;
        }

        public static ExcelRange SetHorizontalAligment(this ExcelRange excelRange, ExcelHorizontalAlignment horizontalAlignment)
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
