namespace GetAnimeList
{
    internal sealed class UpdateDate
    {
        /// <summary>
        /// アップデートログファイルパス
        /// </summary>
        private readonly string _updateLogPath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="updateLogPath">アップデートログファイルパス</param>
        internal UpdateDate(string updateLogPath)
        {
            _updateLogPath = updateLogPath;
        }

        /// <summary>
        /// 現在のリストが最新版か確認する
        /// </summary>
        /// <returns>最新版ならtrue、最新版でないならばfalse</returns>
        internal bool IsLatest()
        {
            if (!File.Exists(_updateLogPath))
            {
                File.Create(_updateLogPath).Close();
                return false;
            }
            if (!File.ReadLines(_updateLogPath).Any())
            {
                return false;
            }

            DateTime lastUpdateDate = DateTime.Parse(File.ReadLines(_updateLogPath).First());
            DateTime validityPeriod = CalculateValidityPeriod(lastUpdateDate);
            return validityPeriod >= DateTime.Now;
        }

        /// <summary>
        /// 現在のリストの有効期限を計算する
        /// </summary>
        /// <param name="lastUpdateDate">リスト最終更新日</param>
        /// <returns>リスト有効期限</returns>
        private static DateTime CalculateValidityPeriod(DateTime lastUpdateDate)
        {
            return lastUpdateDate.Day <= 10
                ? DateTime.Parse($"{lastUpdateDate.Year}/{lastUpdateDate.Month}/10")
                : DateTime.Parse($"{lastUpdateDate.Year}/{lastUpdateDate.Month}/10").AddMonths(1);
        }

        /// <summary>
        /// 最終更新日を更新する
        /// </summary>
        internal void Update()
        {
            File.WriteAllText(_updateLogPath, DateTime.Today.ToString("yyyy/MM/dd"));
        }
    }
}
