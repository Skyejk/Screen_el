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
			throw new ArgumentException("URL �� ����� ���� ������.", nameof(url));
		}

		try
		{
			// ���������� GET-�������
			HttpResponseMessage response = await httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode(); // �������� ��������� ������

			// ������ ����������� ������
			string content = await response.Content.ReadAsStringAsync();
			return content;
		}
		catch (HttpRequestException e)
		{
			// ��������� ���������� ����
			Console.WriteLine($"������ �������� RSS �����: {e.Message}");
			throw;
		}
	}
}