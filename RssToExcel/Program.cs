using OfficeOpenXml;

public class Program
{
    public static async Task Main(string[] args)
    {
        // if you are using epplus for noncommercial purposes, see https://polyformproject.org/licenses/noncommercial/1.0.0/
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


        var rssLoader = new RssLoader();
        var rssParser = new RssParser();
        // Укажите URL RSS ленты
        string url = "https://automobili.ru/media/rss/";
        string filePath = "RSSFeedItems.xlsx"; // Путь к файлу Excel для сохранения

        try
        {
            string rssContent = await rssLoader.LoadRssFeedAsync(url);
            Console.WriteLine("RSS лента загружена успешно:");
            Console.WriteLine(rssContent);

            List<RssFeedItem> feedItems = await rssParser.LoadAndParseRssFeedAsync(url);
            rssParser.SaveToExcel(feedItems, filePath);
            Console.WriteLine($"Данные успешно сохранены в {filePath}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить RSS ленту: {ex.Message}");
        }
        Console.ReadLine();
    }
}