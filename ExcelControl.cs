using GetAnimeList.DTO;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace GetAnimeList
{
    internal sealed class ExcelControl
    {
        /// <summary>
        /// ヘッダー情報
        /// </summary>
        private readonly string[] _header =
        [
            "種類",
            "アニメ名",
            "種類2",
            "備考",
            "曲名",
            "アーティスト名",
            "リネーム用文字列",
            "放送開始日"
        ];

        /// <summary>
        /// AnimeListを作成する
        /// </summary>
        /// <param name="anisonDb">AnisonDbデータ</param>
        /// <param name="filePath">Excelファイルパス</param>
        internal void CreateAnimeList(HashSet<AnisonDbDTO> anisonDb, string filePath)
        {
            XSSFWorkbook wb = new();
            var sheet = wb.CreateSheet("リネーム用");

            // ヘッダー作成
            SetHeader(wb, sheet);

            // データ読み込み
            int rowCnt = 1;
            foreach (var anison in anisonDb)
            {
                SetAnisonData(wb, sheet, rowCnt, anison);
                rowCnt++;
            }

            // スタイル整形
            sheet.SetColumnWidth(0, 1500);
            sheet.SetColumnWidth(1, 12000);
            sheet.SetColumnWidth(2, 1500);
            sheet.SetColumnWidth(3, 2000);
            sheet.SetColumnWidth(4, 10000);
            sheet.SetColumnWidth(5, 7000);
            sheet.SetColumnWidth(6, 20000);
            sheet.SetColumnWidth(7, 4000);

            // ファイル保存
            using FileStream fs = new(filePath, FileMode.Create);
            wb.Write(fs);
        }

        /// <summary>
        /// ヘッダー情報をセルに書き込む
        /// </summary>
        /// <param name="sheet">ワークシート</param>
        private void SetHeader(XSSFWorkbook wb, ISheet sheet)
        {
            IRow row = sheet.CreateRow(0);

            for (int cnt = 0; cnt < _header.Length; cnt++)
            {
                SetCellValue(wb, sheet, row, 0, cnt, _header[cnt]);
                SetHeaderStyle(wb, sheet, cnt);
            }

            // オートフィルター設定
            sheet.SetAutoFilter(new CellRangeAddress(0, 0, 0, 7));
        }

        /// <summary>
        /// ヘッダーのスタイルを整形する
        /// </summary>
        /// <param name="wb">ワークブック</param>
        /// <param name="sheet">ワークシート</param>
        /// <param name="columnCnt">列番号</param>
        private static void SetHeaderStyle(XSSFWorkbook wb, ISheet sheet, int columnCnt)
        {
            var style = wb.CreateCellStyle();

            // フォント設定
            var font = wb.CreateFont();
            font.FontName = "游ゴシック";
            font.IsBold = true;
            font.Color = HSSFColor.White.Index;
            style.SetFont(font);

            // セル色設定
            XSSFColor color = new([0x00, 0xB0, 0x50]);
            ((XSSFCellStyle)style).SetFillForegroundColor(color);
            style.FillPattern = FillPattern.SolidForeground;

            sheet.GetRow(0).GetCell(columnCnt).CellStyle = style;
        }

        /// <summary>
        /// AnisonDbデータをセルに書き込む
        /// </summary>
        /// <param name="sheet">ワークシート</param>
        /// <param name="rowCnt">行番号</param>
        /// <param name="anison">AnisonDb行データ</param>
        private static void SetAnisonData(XSSFWorkbook wb, ISheet sheet, int rowCnt, AnisonDbDTO anison)
        {
            IRow row = sheet.CreateRow(rowCnt);
            SetCellValue(wb, sheet, row, rowCnt, 0, anison.ProgramGenre);
            SetCellValue(wb, sheet, row, rowCnt, 1, anison.ProgramName);
            SetCellValue(wb, sheet, row, rowCnt, 2, anison.MusicGenre);
            SetCellValue(wb, sheet, row, rowCnt, 3, anison.MusicGenreOrder);
            SetCellValue(wb, sheet, row, rowCnt, 4, anison.MusicName);
            SetCellValue(wb, sheet, row, rowCnt, 5, anison.ArtistName);
            SetCellValue(wb, sheet, row, rowCnt, 6, anison.RenameString);
            SetCellValue(wb, sheet, row, rowCnt, 7, anison.ProgramStartDate);
        }

        /// <summary>
        /// セルに値を書き込む
        /// </summary>
        /// <param name="sheet">ワークシート</param>
        /// <param name="row">列</param>
        /// <param name="rowCnt">行番号</param>
        /// <param name="columnCnt">列番号</param>
        /// <param name="value">書き込み値</param>
        private static void SetCellValue(XSSFWorkbook wb, ISheet sheet, IRow row, int rowCnt, int columnCnt, object value)
        {
            row.CreateCell(columnCnt);
            var cell = sheet.GetRow(rowCnt).GetCell(columnCnt);

            // 引数の型を基にセルに値を入力する
            switch (value)
            {
                case string:
                    cell.SetCellValue((string)value);
                    break;
                case int:
                case long:
                    cell.SetCellValue((int)value);
                    break;
                case double:
                case float:
                    cell.SetCellValue((double)value);
                    break;
                case bool:
                    cell.SetCellValue((bool)value);
                    break;
                case DateTime:
                    cell.SetCellValue((DateTime)value);
                    var dateCellStyle = wb.CreateCellStyle();
                    var dataFormat = wb.CreateDataFormat();
                    dateCellStyle.DataFormat = dataFormat.GetFormat("yyyy/MM/dd");
                    cell.CellStyle = dateCellStyle;
                    break;
                default:
                    break;
            }
        }
    }
}
