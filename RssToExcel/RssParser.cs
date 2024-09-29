using OfficeOpenXml;
using System.Xml.Linq;

public class RssParser
{
    private static readonly HttpClient httpClient = new HttpClient();

    public async Task<List<RssFeedItem>> LoadAndParseRssFeedAsync(string url)
    {
        string rssContent = await LoadRssFeedAsync(url);
        return ParseRssFeed(rssContent);
    }

    private async Task<string> LoadRssFeedAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL не может быть пустым.", nameof(url));
        }

        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Ошибка загрузки RSS ленты: {e.Message}");
            throw;
        }
    }

    private List<RssFeedItem> ParseRssFeed(string rssContent)
    {
        var items = new List<RssFeedItem>();
        XDocument rssXml = XDocument.Parse(rssContent);

        foreach (var item in rssXml.Descendants("item"))
        {
            var feedItem = new RssFeedItem
            {
                Title = item.Element("title")?.Value,
                Link = item.Element("link")?.Value,
                PublicationDate = DateTime.Parse(item.Element("pubDate")?.Value),
                Author = item.Element("author")?.Value,
                Description = item.Element("description")?.Value
            };

            items.Add(feedItem);
        }

        return items;
    }

    

    public void SaveToExcel(List<RssFeedItem> feedItems, string filePath)
    {
        using (ExcelPackage package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("RSS Feed Items");
            worksheet.Cells[1, 1].Value = "Заголовок";
            worksheet.Cells[1, 2].Value = "Ссылка";
            worksheet.Cells[1, 3].Value = "Дата публикации";
            worksheet.Cells[1, 4].Value = "Автор";
            worksheet.Cells[1, 5].Value = "Описание";

            for (int i = 0; i < feedItems.Count; i++)
            {
                var item = feedItems[i];
                worksheet.Cells[i + 2, 1].Value = item.Title;
                worksheet.Cells[i + 2, 2].Value = item.Link;
                worksheet.Cells[i + 2, 3].Value = item.PublicationDate;
                worksheet.Cells[i + 2, 4].Value = item.Author;
                worksheet.Cells[i + 2, 5].Value = item.Description;
            }

            FileInfo fi = new FileInfo(filePath);
            package.SaveAs(fi);
        }
    }
}