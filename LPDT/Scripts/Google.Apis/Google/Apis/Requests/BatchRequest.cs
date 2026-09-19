using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Http;
using Google.Apis.Services;
using Google.Apis.Testing;

namespace Google.Apis.Requests
{
	public sealed class BatchRequest
	{
		public delegate void OnResponse<in TResponse>(TResponse content, RequestError error, int index, HttpResponseMessage message) where TResponse : class;

		private class InnerRequest
		{
			public IClientServiceRequest ClientRequest { get; set; }

			public Type ResponseType { get; set; }

			public virtual void OnResponse(object content, RequestError error, int index, HttpResponseMessage message)
			{
				string text = ((message.Headers.ETag != null) ? message.Headers.ETag.Tag : null);
				if (content is IDirectResponseSchema { ETag: null } directResponseSchema && text != null)
				{
					directResponseSchema.ETag = text;
				}
			}
		}

		private class InnerRequest<TResponse> : InnerRequest where TResponse : class
		{
			public OnResponse<TResponse> OnResponseCallback { get; set; }

			public override void OnResponse(object content, RequestError error, int index, HttpResponseMessage message)
			{
				base.OnResponse(content, error, index, message);
				if (OnResponseCallback != null)
				{
					OnResponseCallback(content as TResponse, error, index, message);
				}
			}
		}

		private const string DefaultBatchUrl = "https://www.googleapis.com/batch";

		private const int QueueLimit = 1000;

		private readonly IList<InnerRequest> allRequests = new List<InnerRequest>();

		private readonly string batchUrl;

		private readonly IClientService service;

		internal string BatchUrl => batchUrl;

		public int Count => allRequests.Count;

		public BatchRequest(IClientService service)
			: this(service, (service as BaseClientService)?.BatchUri ?? "https://www.googleapis.com/batch")
		{
		}

		public BatchRequest(IClientService service, string batchUrl)
		{
			this.batchUrl = batchUrl;
			this.service = service;
		}

		public void Queue<TResponse>(IClientServiceRequest request, OnResponse<TResponse> callback) where TResponse : class
		{
			if (Count > 1000)
			{
				throw new InvalidOperationException("A batch request cannot contain more than 1000 single requests");
			}
			allRequests.Add(new InnerRequest<TResponse>
			{
				ClientRequest = request,
				ResponseType = typeof(TResponse),
				OnResponseCallback = callback
			});
		}

		public Task ExecuteAsync()
		{
			return ExecuteAsync(CancellationToken.None);
		}

		public async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			if (Count < 1)
			{
				return;
			}
			ConfigurableHttpClient httpClient = service.HttpClient;
			HttpContent content = await CreateOuterRequestContent(allRequests.Select((InnerRequest r) => r.ClientRequest)).ConfigureAwait(continueOnCapturedContext: false);
			HttpResponseMessage result = await httpClient.PostAsync(new Uri(batchUrl), content, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			await EnsureSuccessAsync(result).ConfigureAwait(continueOnCapturedContext: false);
			string fullContent = await result.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
			string text = result.Content.Headers.GetValues("Content-Type").First();
			string boundary = text.Substring(text.IndexOf("boundary=") + "boundary=".Length);
			int requestIndex = 0;
			while (true)
			{
				cancellationToken.ThrowIfCancellationRequested();
				int num = fullContent.IndexOf("--" + boundary);
				if (num == -1)
				{
					break;
				}
				fullContent = fullContent.Substring(num + boundary.Length + 2);
				int endIndex = fullContent.IndexOf("--" + boundary);
				if (endIndex == -1)
				{
					break;
				}
				HttpResponseMessage responseMessage = ParseAsHttpResponse(fullContent.Substring(0, endIndex));
				if (responseMessage.IsSuccessStatusCode)
				{
					string input = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
					object content2 = service.Serializer.Deserialize(input, allRequests[requestIndex].ResponseType);
					allRequests[requestIndex].OnResponse(content2, null, requestIndex, responseMessage);
				}
				else
				{
					RequestError error;
					try
					{
						error = await service.DeserializeError(responseMessage).ConfigureAwait(continueOnCapturedContext: false);
					}
					catch (GoogleApiException ex) when (ex.Error != null)
					{
						error = ex.Error;
					}
					allRequests[requestIndex].OnResponse(null, error, requestIndex, responseMessage);
				}
				requestIndex++;
				fullContent = fullContent.Substring(endIndex);
			}
		}

		private async Task EnsureSuccessAsync(HttpResponseMessage result)
		{
			if (!result.IsSuccessStatusCode)
			{
				Exception inner;
				try
				{
					RequestError error = await service.DeserializeError(result).ConfigureAwait(continueOnCapturedContext: false);
					throw new GoogleApiException(service.Name)
					{
						Error = error,
						HttpStatusCode = result.StatusCode
					};
				}
				catch (Exception ex)
				{
					inner = ex;
				}
				try
				{
					result.EnsureSuccessStatusCode();
				}
				catch (HttpRequestException ex2)
				{
					throw new HttpRequestException(ex2.Message, inner);
				}
			}
		}

		[VisibleForTestOnly]
		internal static HttpResponseMessage ParseAsHttpResponse(string content)
		{
			HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
			using StringReader stringReader = new StringReader(content);
			string value = stringReader.ReadLine();
			while (string.IsNullOrEmpty(value))
			{
				value = stringReader.ReadLine();
			}
			while (!string.IsNullOrEmpty(value))
			{
				value = stringReader.ReadLine();
			}
			value = stringReader.ReadLine();
			while (string.IsNullOrEmpty(value))
			{
				value = stringReader.ReadLine();
			}
			int statusCode = int.Parse(value.Split(new char[1] { ' ' })[1]);
			httpResponseMessage.StatusCode = (HttpStatusCode)statusCode;
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			while (!string.IsNullOrEmpty(value = stringReader.ReadLine()))
			{
				int num = value.IndexOf(':');
				string key = value.Substring(0, num).Trim();
				string text = value.Substring(num + 1).Trim();
				if (dictionary.ContainsKey(key))
				{
					dictionary[key] = dictionary[key] + ", " + text;
				}
				else
				{
					dictionary.Add(key, text);
				}
			}
			string mediaType = null;
			if (dictionary.ContainsKey("Content-Type"))
			{
				mediaType = dictionary["Content-Type"].Split(';', ' ')[0];
				dictionary.Remove("Content-Type");
			}
			httpResponseMessage.Content = new StringContent(stringReader.ReadToEnd(), Encoding.UTF8, mediaType);
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				HttpHeaders headers = httpResponseMessage.Headers;
				if (typeof(HttpContentHeaders).GetProperty(item.Key.Replace("-", "")) != null)
				{
					headers = httpResponseMessage.Content.Headers;
				}
				if (!headers.TryAddWithoutValidation(item.Key, item.Value))
				{
					throw new FormatException($"Could not parse header {item.Key} from batch reply");
				}
			}
			return httpResponseMessage;
		}

		[VisibleForTestOnly]
		internal static async Task<HttpContent> CreateOuterRequestContent(IEnumerable<IClientServiceRequest> requests)
		{
			MultipartContent mixedContent = new MultipartContent("mixed");
			foreach (IClientServiceRequest request in requests)
			{
				MultipartContent multipartContent = mixedContent;
				multipartContent.Add(await CreateIndividualRequest(request).ConfigureAwait(continueOnCapturedContext: false));
			}
			return mixedContent;
		}

		[VisibleForTestOnly]
		internal static async Task<HttpContent> CreateIndividualRequest(IClientServiceRequest request)
		{
			StringContent stringContent = new StringContent(await CreateRequestContentString(request.CreateRequest(false)).ConfigureAwait(continueOnCapturedContext: false));
			stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/http");
			return stringContent;
		}

		[VisibleForTestOnly]
		internal static async Task<string> CreateRequestContentString(HttpRequestMessage requestMessage)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0} {1}", requestMessage.Method, requestMessage.RequestUri.AbsoluteUri);
			foreach (KeyValuePair<string, IEnumerable<string>> header in requestMessage.Headers)
			{
				sb.Append(Environment.NewLine).AppendFormat("{0}: {1}", header.Key, string.Join(", ", header.Value.ToArray()));
			}
			if (requestMessage.Content != null)
			{
				foreach (KeyValuePair<string, IEnumerable<string>> header2 in requestMessage.Content.Headers)
				{
					sb.Append(Environment.NewLine).AppendFormat("{0}: {1}", header2.Key, string.Join(", ", header2.Value.ToArray()));
				}
			}
			if (requestMessage.Content != null)
			{
				sb.Append(Environment.NewLine);
				string text = await requestMessage.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
				sb.Append("Content-Length:  ").Append(text.Length);
				sb.Append(Environment.NewLine).Append(Environment.NewLine).Append(text);
			}
			return sb.Append(Environment.NewLine).ToString();
		}
	}
}
