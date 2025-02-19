using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace GetAnimeList
{
    internal sealed class LoadCsv
    {
        /// <summary>
        /// CSVファイルを読み込む
        /// </summary>
        /// <typeparam name="T">CSVファイルカラムDTO</typeparam>
        /// <param name="programCSVPath">CSVファイルパス</param>
        /// <returns>読み込んだCSVファイルのリスト</returns>
        internal static HashSet<T> Execute<T>(string programCSVPath) where T : class
        {
            {
                CsvConfiguration config = new(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true,
                    Delimiter = ",",
                    ShouldQuote = (context) => true
                };

                using StreamReader reader = new(programCSVPath, Encoding.UTF8);
                using StringReader modifiedReader = new(reader.ReadToEnd().Replace("\\\"", "\"\""));
                using CsvReader csv = new(modifiedReader, config);
                return csv.GetRecords<T>().ToHashSet();
            }
        }
    }
}
