using CsvHelper.Configuration.Attributes;

namespace GetAnimeList.DTO
{
    public class AnisonCsvDTO
    {
        /// <summary>
        /// 番組分類
        /// </summary>
        [Name("番組分類")]
        public string ProgramGenre { get; set; }

        /// <summary>
        /// 番組名
        /// </summary>
        [Name("番組名")]
        public string ProgramName { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [Name("摘要")]
        public string MusicGenre { get; set; }

        /// <summary>
        /// 放映順
        /// </summary>
        [Name("放映順")]
        public string MusicGenreOrder { get; set; }

        /// <summary>
        /// 楽曲名
        /// </summary>
        [Name("楽曲名")]
        public string MusicName { get; set; }

        /// <summary>
        /// 歌手名
        /// </summary>
        [Name("歌手名")]
        public string ArtistName { get; set; }
    }
}
