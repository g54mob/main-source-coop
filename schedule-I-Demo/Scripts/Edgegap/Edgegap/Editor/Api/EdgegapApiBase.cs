using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Edgegap.Codice.Utils;
using UnityEngine;

namespace Edgegap.Editor.Api
{
	public abstract class EdgegapApiBase
	{
		private readonly HttpClient _httpClient = new HttpClient();

		protected ApiEnvironment SelectedApiEnvironment { get; }

		protected EdgegapWindowMetadata.LogLevel LogLevel { get; set; }

		protected bool IsLogLevelDebug => LogLevel == EdgegapWindowMetadata.LogLevel.Debug;

		private string GetBaseUrl()
		{
			if (SelectedApiEnvironment != ApiEnvironment.Staging)
			{
				return ApiEnvironment.Console.GetApiUrl();
			}
			return ApiEnvironment.Staging.GetApiUrl();
		}

		protected EdgegapApiBase(ApiEnvironment apiEnvironment, string apiToken, EdgegapWindowMetadata.LogLevel logLevel = EdgegapWindowMetadata.LogLevel.Error)
		{
			SelectedApiEnvironment = apiEnvironment;
			_httpClient.BaseAddress = new Uri(GetBaseUrl() + "/");
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			string parameter = apiToken.Replace("token ", "");
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", parameter);
			LogLevel = logLevel;
		}

		protected async Task<HttpResponseMessage> PostAsync(string relativePath = "", string json = "{}")
		{
			StringContent stringContent = CreateStringContent(json);
			Uri uri = new Uri(_httpClient.BaseAddress, relativePath);
			if (IsLogLevelDebug)
			{
				Debug.Log($"PostAsync to: `{uri}` with json: `{json}`");
			}
			try
			{
				return await ExecuteRequestAsync(() => _httpClient.PostAsync(uri, stringContent));
			}
			catch (Exception arg)
			{
				Debug.LogError($"Error: {arg}");
				throw;
			}
		}

		protected async Task<HttpResponseMessage> PatchAsync(string relativePath = "", string json = "{}")
		{
			StringContent content = CreateStringContent(json);
			Uri uri = new Uri(_httpClient.BaseAddress, relativePath);
			if (IsLogLevelDebug)
			{
				Debug.Log($"PatchAsync to: `{uri}` with json: `{json}`");
			}
			HttpRequestMessage patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), uri)
			{
				Content = content
			};
			try
			{
				return await ExecuteRequestAsync(() => _httpClient.SendAsync(patchRequest));
			}
			catch (Exception arg)
			{
				Debug.LogError($"Error: {arg}");
				throw;
			}
		}

		protected async Task<HttpResponseMessage> GetAsync(string relativePath = "", string customQuery = "")
		{
			string completeRelativeUri = prepareEdgegapUriWithQuery(relativePath, customQuery);
			if (IsLogLevelDebug)
			{
				Debug.Log("GetAsync to: `" + completeRelativeUri + " with customQuery: `" + customQuery + "`");
			}
			try
			{
				return await ExecuteRequestAsync(() => _httpClient.GetAsync(completeRelativeUri));
			}
			catch (Exception arg)
			{
				Debug.LogError($"Error: {arg}");
				throw;
			}
		}

		protected async Task<HttpResponseMessage> DeleteAsync(string relativePath = "", string customQuery = "")
		{
			string completeRelativeUri = prepareEdgegapUriWithQuery(relativePath, customQuery);
			if (IsLogLevelDebug)
			{
				Debug.Log("DeleteAsync to: `" + completeRelativeUri + " with customQuery: `" + customQuery + "`");
			}
			try
			{
				return await ExecuteRequestAsync(() => _httpClient.DeleteAsync(completeRelativeUri));
			}
			catch (Exception arg)
			{
				Debug.LogError($"Error: {arg}");
				throw;
			}
		}

		private static async Task<HttpResponseMessage> ExecuteRequestAsync(Func<Task<HttpResponseMessage>> requestFunc, CancellationToken cancellationToken = default(CancellationToken))
		{
			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await requestFunc();
			}
			catch (HttpRequestException ex)
			{
				Debug.LogError("HttpRequestException: " + ex.Message);
				return null;
			}
			catch (TaskCanceledException ex2)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					Debug.LogError("Task was cancelled by caller.");
				}
				else
				{
					Debug.LogError("TaskCanceledException: Timeout - " + ex2.Message);
				}
				return null;
			}
			catch (Exception ex3)
			{
				Debug.LogError("Unexpected error occurred: " + ex3.Message);
				return null;
			}
			if (httpResponseMessage == null)
			{
				Debug.Log("!Success (null response) - returning 500");
				return CreateUnknown500Err();
			}
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				HttpMethod method = httpResponseMessage.RequestMessage.Method;
				Debug.Log($"!Success: {(short)httpResponseMessage.StatusCode} {httpResponseMessage.ReasonPhrase} - " + $"{method} | {httpResponseMessage.RequestMessage.RequestUri}` - " + httpResponseMessage.Content?.ReadAsStringAsync().Result);
			}
			return httpResponseMessage;
		}

		private StringContent CreateStringContent(string json = "{}")
		{
			return new StringContent(json, Encoding.UTF8, "application/json");
		}

		private static HttpResponseMessage CreateUnknown500Err()
		{
			return new HttpResponseMessage(HttpStatusCode.InternalServerError);
		}

		private string prepareEdgegapUriWithQuery(string relativePath, string customQuery)
		{
			UriBuilder uriBuilder = new UriBuilder(_httpClient.BaseAddress);
			uriBuilder.Path += relativePath;
			NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(uriBuilder.Query);
			nameValueCollection["source"] = "unity";
			NameValueCollection nameValueCollection2 = HttpUtility.ParseQueryString(customQuery);
			foreach (string item in nameValueCollection2)
			{
				nameValueCollection[item] = nameValueCollection2[item];
			}
			uriBuilder.Query = nameValueCollection.ToString();
			return uriBuilder.Uri.PathAndQuery;
		}
	}
}
