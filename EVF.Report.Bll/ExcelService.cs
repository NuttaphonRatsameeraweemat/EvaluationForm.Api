using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace EVF.Report.Bll
{
    public static class ExcelService
    {

        #region [Feilds]

        /// <summary>
        /// Specific font style using.
        /// </summary>
        public const string FontStyle = "TH Sarabun New";

        #endregion

        #region [Methods]

        /// <summary>
        /// Set cell value and set header style format.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet">The npoi sheet interface.</param>
        /// <param name="row">The npoi row interface.</param>
        /// <param name="cellIndex">The target cell index.</param>
        /// <param name="value">The cell string value.</param>
        public static void CreateHeaderCell(IWorkbook workbook, ISheet sheet, IRow row, int cellIndex, string value)
        {
            var style = SetHeaderCellStyle(workbook);
            row.GetCell(cellIndex).SetCellValue(value);
            sheet.SetColumnWidth(cellIndex, value.Length * 2 * 200);
        }

        /// <summary>
        /// Create cell value and set content style format.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet">The npoi sheet interface.</param>
        /// <param name="row">The npoi row interface.</param>
        /// <param name="cellIndex">The target cell index.</param>
        /// <param name="value">The cell string value.</param>
        /// <param name="verticalAlignment">The vertical alignment property setting.</param>
        /// <param name="horizontalAlignment">The horizontal alignment property setting.</param>
        public static void CreateContentCell(IWorkbook workbook, ISheet sheet, IRow row, int cellIndex, string value,
                                             VerticalAlignment verticalAlignment = VerticalAlignment.Center,
                                             HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center)
        {
            var style = SetContentCellStyle(workbook, verticalAlignment, horizontalAlignment);
            row.CreateCell(cellIndex).SetCellValue(value);
            row.GetCell(cellIndex).CellStyle = style;
        }

        /// <summary>
        /// Create topic cell and set topic style format.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="sheet">The npoi sheet interface.</param>
        /// <param name="row">The npoi row interface.</param>
        /// <param name="cellIndex">The target cell index.</param>
        /// <param name="value">The cell string value.</param>
        public static void CreateTopicCell(IWorkbook workbook, ISheet sheet, IRow row, int cellIndex, string value)
        {
            var style = SetTopicCellStyle(workbook);
            row.CreateCell(cellIndex).SetCellValue(value);
            row.GetCell(cellIndex).CellStyle = style;
            sheet.AutoSizeColumn(cellIndex);
        }

        /// <summary>
        /// Set header style format.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <returns></returns>
        private static ICellStyle SetHeaderCellStyle(IWorkbook workbook)
        {
            var font = workbook.CreateFont();
            font.FontName = FontStyle;
            font.FontHeight = 13;
            font.IsBold = true;
            var style = workbook.CreateCellStyle();
            BorderStyle borderStyle = BorderStyle.Thin;
            style.FillForegroundColor = HSSFColor.LightCornflowerBlue.Index;
            style.FillPattern = FillPattern.SolidForeground;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.Alignment = HorizontalAlignment.Center;
            style.BorderBottom = borderStyle;
            style.BorderLeft = borderStyle;
            style.BorderRight = borderStyle;
            style.BorderTop = borderStyle;
            style.SetFont(font);
            return style;
        }

        /// <summary>
        /// Set content style format.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="verticalAlignment">The vertical alignment property setting.</param>
        /// <param name="horizontalAlignment">The horizontal alignment property setting.</param>
        /// <returns></returns>
        private static ICellStyle SetContentCellStyle(IWorkbook workbook,
                                                      VerticalAlignment verticalAlignment = VerticalAlignment.Center,
                                                      HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center)
        {
            var font = workbook.CreateFont();
            font.FontName = FontStyle;
            font.FontHeight = 12;
            BorderStyle borderStyle = BorderStyle.Thin;
            var style = workbook.CreateCellStyle();
            style.VerticalAlignment = verticalAlignment;
            style.Alignment = horizontalAlignment;
            style.BorderBottom = borderStyle;
            style.BorderLeft = borderStyle;
            style.BorderRight = borderStyle;
            style.BorderTop = borderStyle;
            style.SetFont(font);
            return style;
        }

        /// <summary>
        /// Set topic style format.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <returns></returns>
        private static ICellStyle SetTopicCellStyle(IWorkbook workbook)
        {
            var font = workbook.CreateFont();
            font.FontName = FontStyle;
            font.FontHeight = 20;
            font.IsBold = true;
            var style = workbook.CreateCellStyle();
            style.VerticalAlignment = VerticalAlignment.Center;
            style.Alignment = HorizontalAlignment.Left;
            style.SetFont(font);
            return style;
        }

        /// <summary>
        /// Create cell and set cell header style single cell.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="row">The npoi row interface.</param>
        /// <param name="cellStart">The cell target index.</param>
        public static void SetCellHeaderStyle(IWorkbook workbook, IRow row, int cellStart)
        {
            var style = SetHeaderCellStyle(workbook);
            row.CreateCell(cellStart);
            row.GetCell(cellStart).CellStyle = style;
        }

        /// <summary>
        /// Create cell and set cell header style.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="row">The npoi row interface.</param>
        /// <param name="cellStart">The cell index start.</param>
        /// <param name="cellEnd">The cell index end.</param>
        public static void SetCellHeaderStyle(IWorkbook workbook, IRow row, int cellStart, int cellEnd)
        {
            var style = SetHeaderCellStyle(workbook);
            for (int i = cellStart; i <= cellEnd; i++)
            {
                row.CreateCell(i);
                row.GetCell(i).CellStyle = style;
            }
        }

        /// <summary>
        /// Create cell and set cell content style.
        /// </summary>
        /// <param name="workbook">The npoi workbook interface.</param>
        /// <param name="row">The npoi row interface.</param>
        /// <param name="cellStart">The cell index start.</param>
        /// <param name="cellEnd">The cell index end.</param>
        public static void SetCellContentStyle(IWorkbook workbook, IRow row, int cellStart, int cellEnd)
        {
            var style = SetContentCellStyle(workbook);
            for (int i = cellStart; i <= cellEnd; i++)
            {
                row.CreateCell(i);
                row.GetCell(i).CellStyle = style;
            }
        }

        #endregion

    }
}
