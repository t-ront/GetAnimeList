using CsvHelper.Configuration.Attributes;

namespace GetAnimeList.DTO
{
    public sealed class ProgramCsvDTO
    {
        /// <summary>
        /// 番組分類
        /// </summary>
        [Name("番組分類")]
        public string ProgramGenre { get; set; }

        /// <summary>
        ///番組名
        /// </summary>
        [Name("番組名")]
        public string ProgramName { get; set; }

        /// <summary>
        /// 放映開始日
        /// </summary>
        [Name("放映開始日")]
        public string ProgramStartDateString { get; set; }
    }
}
