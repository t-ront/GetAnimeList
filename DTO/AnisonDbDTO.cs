namespace GetAnimeList.DTO
{
    internal sealed class AnisonDbDTO
    {
        /// <summary>
        /// 番組分類
        /// </summary>
        internal string ProgramGenre { get; set; }

        /// <summary>
        /// 番組名
        /// </summary>
        internal string ProgramName { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        internal string MusicGenre { get; set; }

        /// <summary>
        /// 放映順
        /// </summary>
        internal string MusicGenreOrder { get; set; }

        /// <summary>
        /// 楽曲名
        /// </summary>
        internal string MusicName { get; set; }

        /// <summary>
        /// 歌手名
        /// </summary>
        internal string ArtistName { get; set; }

        /// <summary>
        /// リネーム用文字列
        /// </summary>
        internal string RenameString { get; set; }

        /// <summary>
        /// 放送開始日
        /// </summary>
        internal DateTime ProgramStartDate { get; set; }
    }
}
