using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Google.Apis.Services;

namespace Google.Apis.Requests
{
	internal static class HttpRequestMessageExtenstions
	{
		internal static void SetRequestSerailizedContent(this HttpRequestMessage request, IClientService service, object body, bool gzipEnabled)
		{
			if (body != null)
			{
				HttpContent httpContent = null;
				string mediaType = "application/" + service.Serializer.Format;
				string content = service.SerializeObject(body);
				if (gzipEnabled)
				{
					httpContent = CreateZipContent(content);
					httpContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType)
					{
						CharSet = Encoding.UTF8.WebName
					};
				}
				else
				{
					httpContent = new StringContent(content, Encoding.UTF8, mediaType);
				}
				request.Content = httpContent;
			}
		}

		internal static HttpContent CreateZipContent(string content)
		{
			return new StreamContent(CreateGZipStream(content))
			{
				Headers = 
				{
					ContentEncoding = { "gzip" }
				}
			};
		}

		private static Stream CreateGZipStream(string serializedObject)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(serializedObject);
			using MemoryStream memoryStream = new MemoryStream();
			using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
			{
				gZipStream.Write(bytes, 0, bytes.Length);
			}
			memoryStream.Position = 0L;
			byte[] array = new byte[memoryStream.Length];
			memoryStream.Read(array, 0, array.Length);
			return new MemoryStream(array);
		}
	}
}
