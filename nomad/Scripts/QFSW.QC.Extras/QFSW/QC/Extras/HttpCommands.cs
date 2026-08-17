using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QFSW.QC.Extras
{
	[CommandPrefix("http.")]
	public static class HttpCommands
	{
		private static readonly HttpClient _client = new HttpClient();

		[Command("get", "Sends a GET request to the specified URL.", Platform.AllPlatforms, MonoTargetType.Single)]
		private static async Task<string> Get(string url)
		{
			return await (await _client.GetAsync(url)).Content.ReadAsStringAsync();
		}

		[Command("delete", "Sends a DELETE request to the specified URL.", Platform.AllPlatforms, MonoTargetType.Single)]
		private static async Task<string> Delete(string url)
		{
			return await (await _client.DeleteAsync(url)).Content.ReadAsStringAsync();
		}

		[Command("post", "Sends a POST request to the specified URL. A body may be sent with the request, with a default mediaType of text/plain.", Platform.AllPlatforms, MonoTargetType.Single)]
		private static async Task<string> Post(string url, string content = "", string mediaType = "text/plain")
		{
			HttpContent content2 = new StringContent(content, Encoding.Default, mediaType);
			return await (await _client.PostAsync(url, content2)).Content.ReadAsStringAsync();
		}

		[Command("put", "Sends a PUT request to the specified URL. A body may be sent with the request, with a default mediaType of text/plain.", Platform.AllPlatforms, MonoTargetType.Single)]
		private static async Task<string> Put(string url, string content = "", string mediaType = "text/plain")
		{
			HttpContent content2 = new StringContent(content, Encoding.Default, mediaType);
			return await (await _client.PutAsync(url, content2)).Content.ReadAsStringAsync();
		}
	}
}
