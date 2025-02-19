using GetAnimeList.DTO;

namespace GetAnimeList
{
    internal sealed class NormalizeCsv
    {
        /// <summary>
        /// AnisonCSVを正規化する
        /// </summary>
        /// <param name="anisonCsv">AnisonCSV</param>
        internal static void NomalizeAnisonCsv(ref HashSet<AnisonCsvDTO> anisonCsv)
        {
            foreach (var anison in anisonCsv)
            {
                anison.ProgramGenre = NomalizeAnisonCsvProgramGenre(anison.ProgramGenre);
            }
        }

        /// <summary>
        /// ProgramCSVを正規化する
        /// </summary>
        /// <param name="programCsv">ProgramCSV</param>
        internal static void NomalizeProgramCsv(ref HashSet<ProgramCsvDTO> programCsv)
        {
            foreach (var program in programCsv)
            {
                program.ProgramGenre = NomalizeProgramCsvProgramGenre(program.ProgramGenre);
                program.ProgramStartDateString = NormalizeDateTimeString(program.ProgramStartDateString);
            }
        }

        /// <summary>
        /// AnisonCSVの番組分類を正規化する
        /// </summary>
        /// <param name="musicGenre">番組分類</param>
        /// <returns>正規化した番組分類</returns>
        private static string NomalizeAnisonCsvProgramGenre(string musicGenre)
        {
            return musicGenre switch
            {
                "MV" => "MVA",
                "TS" or "TV" => "TVA",
                "VD" => "OVA",
                "WA" => "WBA",
                _ => "",
            };
        }

        /// <summary>
        /// ProgramCSVの番組分類を正規化する
        /// </summary>
        /// <param name="musicGenre">番組分類</param>
        /// <returns>正規化した番組分類</returns>
        private static string NomalizeProgramCsvProgramGenre(string musicGenre)
        {
            return musicGenre switch
            {
                "劇場用アニメーション" => "MVA",
                "テレビスペシャル" or "テレビアニメーション" => "TVA",
                "オリジナルビデオアニメーション" => "OVA",
                "Webアニメーション" => "WBA",
                _ => "",
            };
        }

        /// <summary>
        /// 放送開始日を正規化する
        /// </summary>
        /// <param name="dateTimeString">放送開始日</param>
        /// <returns>正規化した放送開始日</returns>
        private static string NormalizeDateTimeString(string dateTimeString)
        {
            return dateTimeString.Replace("\\N", "1900-01-01")
                                 .Replace("0000", "1900")
                                 .Replace("-00", "-01")
                                 .Replace("-", "/");
        }
    }
}
