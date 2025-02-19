using System.IO.Compression;
using System.Net;
using File = System.IO.File;

namespace GetAnimeList
{
    internal sealed class DownloadFile
    {
        internal readonly HttpClient _httpClient = new();

        /// <summary>
        /// ダウンロード対象zipファイルリンク
        /// </summary>
        private readonly string[] _csvZipLinks =
        [
            "http://anison.info/data/download/program.zip",
            "http://anison.info/data/download/anison.zip"
        ];

        /// <summary>
        /// CSVファイルをダウンロードする
        /// </summary>
        /// <param name="targetDirectoryPath">ダウンロードディレクトリパス</param>
        internal void DownloadCSVs(string targetDirectoryPath)
        {
            foreach (string csvZipLink in _csvZipLinks)
            {
                if (!Download(csvZipLink, targetDirectoryPath))
                {
                    return;
                }

                string zipfileName = Path.GetFileName(csvZipLink);
                ZipFile.ExtractToDirectory(Path.Combine(targetDirectoryPath, zipfileName), targetDirectoryPath);

                foreach (string tempFile in Directory.GetFiles(targetDirectoryPath))
                {
                    if (Path.GetExtension(tempFile) != ".csv")
                    {
                        File.Delete(tempFile);
                    }
                }
            }
        }

        /// <summary>
        /// ファイルをダウンロードする
        /// </summary>
        /// <param name="downloadLink">ダウンロードリンク</param>
        /// <param name="targetDirectoryPath">ダウンロードディレクトリパス</param>
        /// <returns>ダウンロードに成功すればtrue、失敗したらfalse</returns>
        private bool Download(string downloadLink, string targetDirectoryPath)
        {
            string fileName = Path.GetFileName(downloadLink);
            string targetFilePath = Path.Combine(targetDirectoryPath, fileName);

            try
            {
                // HTTPステータスを確認する
                using HttpRequestMessage request = new(HttpMethod.Get, new Uri(downloadLink));
                using var response = _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }

                // ファイルをダウンロードする
                using var content = response.Content;
                using var stream = content.ReadAsStreamAsync().Result;
                using FileStream fileStream = new(targetFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                stream.CopyTo(fileStream);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
