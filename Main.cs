using GetAnimeList.DTO;

namespace GetAnimeList
{
    internal sealed class Main
    {
        /// <summary>
        /// カレントディレクトリ
        /// </summary>
        private readonly string _currentDirectory = AppContext.BaseDirectory;

        /// <summary>
        /// 生成Excelファイル名
        /// </summary>
        private readonly string _animeListFileName = "AnimeLlst.xlsx";

        /// <summary>
        /// 更新ログファイル名
        /// </summary>
        private readonly string _updateLogFileName = "LastUpdate.log";

        /// <summary>
        /// 一時フォルダ名
        /// </summary>
        private readonly string _tempDirectoryName = "temp";


        /// <summary>
        /// プログラム実行
        /// </summary>
        internal void Start()
        {
            // バージョン確認確認
            Console.WriteLine("1. バージョン確認 開始");
            string updateLogPath = Path.Combine(_currentDirectory, _updateLogFileName);
            UpdateDate updateDate = new(updateLogPath);
            if (updateDate.IsLatest())
            {
                Console.WriteLine("既に最新版です");
                Console.WriteLine("プログラムを終了するには何かキーを押してください...");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("バージョン確認完了");
            Console.WriteLine();


            // Excelファイルの削除
            Console.WriteLine("2. ファイル生成準備 開始");
            File.Delete(_animeListFileName);

            // tempフォルダ作成
            string tempDirectory = Path.Combine(_currentDirectory, _tempDirectoryName);
            Directory.CreateDirectory(tempDirectory);
            foreach (string tempFile in Directory.GetFiles(tempDirectory))
            {
                File.Delete(tempFile);
            }
            Console.WriteLine("ファイル生成準備 終了");
            Console.WriteLine();


            // CSVファイルダウンロード
            Console.WriteLine("3. csvファイルダウンロード 開始");
            DownloadFile downloadFile = new();
            downloadFile.DownloadCSVs(tempDirectory);
            Console.WriteLine("csvファイルダウンロード 完了");
            Console.WriteLine();


            // CSVファイル読み込み
            Console.WriteLine("4. csvファイル読み込み 開始");
            string anisonCSVPath = Path.Combine(tempDirectory, "anison.csv");
            string programCSVPath = Path.Combine(tempDirectory, "program.csv");
            var anisonCSV = LoadCsv.Execute<AnisonCsvDTO>(anisonCSVPath);
            var programCSV = LoadCsv.Execute<ProgramCsvDTO>(programCSVPath);

            // リスト正規化
            NormalizeCsv.NomalizeAnisonCsv(ref anisonCSV);
            NormalizeCsv.NomalizeProgramCsv(ref programCSV);

            // リスト生成
            HashSet<AnisonDbDTO> anisonDb = [];
            foreach (var anison in anisonCSV)
            {
                // 放送開始日を検索(一致するものがない場合には1900/01/01に設定)
                string matchProgramStartDate = programCSV.Where(program => program.ProgramGenre == anison.ProgramGenre && program.ProgramName == anison.ProgramName)
                                                         .Select(program => program.ProgramStartDateString)
                                                         .FirstOrDefault() ?? "1900/01/01";
                AnisonDbDTO anisonDbDTO = new()
                {
                    ProgramGenre = anison.ProgramGenre,
                    ProgramName = anison.ProgramName,
                    MusicGenre = anison.MusicGenre,
                    MusicGenreOrder = anison.MusicGenreOrder,
                    MusicName = anison.MusicName,
                    ArtistName = anison.ArtistName,
                    RenameString = $"{anison.ProgramGenre}_{anison.MusicName} - {anison.ArtistName} (『{anison.ProgramName}』 {anison.MusicGenre})",
                    ProgramStartDate = DateTime.Parse(matchProgramStartDate)
                };
                anisonDb.Add(anisonDbDTO);
            }
            Console.WriteLine("csvファイル読み込み 完了");
            Console.WriteLine();


            // Excelファイル作成
            Console.WriteLine("5. Excelファイル生成 開始");
            ExcelControl excelControl = new();
            excelControl.CreateAnimeList(anisonDb, Path.Combine(_currentDirectory, _animeListFileName));
            Console.WriteLine("Excelファイル生成 完了");
            Console.WriteLine();

            // アップデート日記録
            updateDate.Update();

            // tempフォルダの削除
            Directory.Delete(tempDirectory, true);

            Console.WriteLine("プログラムを終了するには何かキーを押してください...");
            Console.ReadLine();
        }
    }
}
