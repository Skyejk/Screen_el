using System;
using System.Net.Http;
using System.Threading.Tasks;

public class RssLoader
{
	private static readonly HttpClient httpClient = new HttpClient();

	public async Task<string> LoadRssFeedAsync(string url)
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			throw new ArgumentException("URL не может быть пустым.", nameof(url));
		}

		try
		{
			// Выполнение GET-запроса
			HttpResponseMessage response = await httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode(); // Проверка успешного ответа

			// Чтение содержимого ответа
			string content = await response.Content.ReadAsStringAsync();
			return content;
		}
		catch (HttpRequestException e)
		{
			// Обработка исключений сети
			Console.WriteLine($"Ошибка загрузки RSS ленты: {e.Message}");
			throw;
		}
	}
}