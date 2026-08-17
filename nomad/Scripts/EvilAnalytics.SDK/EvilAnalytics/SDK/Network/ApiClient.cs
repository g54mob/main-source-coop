using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EvilAnalytics.SDK.Core;
using EvilAnalytics.Shared.BugReports;
using EvilAnalytics.Shared.Common;
using EvilAnalytics.Shared.Crashlytics;
using EvilAnalytics.Shared.Events;
using EvilAnalytics.Shared.Gdpr;
using EvilAnalytics.Shared.Leaderboards;
using EvilAnalytics.Shared.RemoteConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace EvilAnalytics.SDK.Network
{
	public class ApiClient
	{
		private readonly EvilAnalyticsConfig _config;

		private readonly HttpClient _httpClient;

		private readonly Action<string> _logger;

		private readonly JsonSerializerSettings _jsonSettings;

		public ApiClient(EvilAnalyticsConfig config, Action<string> logger = null)
		{
			_config = config;
			_logger = logger;
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri(config.ServerUrl.TrimEnd('/')),
				Timeout = TimeSpan.FromSeconds(config.RequestTimeout)
			};
			_httpClient.DefaultRequestHeaders.Add("X-API-Key", config.ApiKey);
			_httpClient.DefaultRequestHeaders.Add("X-SDK-Version", "1.0.0");
			_jsonSettings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				NullValueHandling = NullValueHandling.Ignore,
				Converters = { (JsonConverter)new StringEnumConverter() }
			};
		}

		public async Task<SessionStartResponse> StartSessionAsync(SessionStartRequest request)
		{
			return await PostAsync<SessionStartRequest, SessionStartResponse>("/api/v1/session/start", request);
		}

		public async Task EndSessionAsync(SessionEndRequest request)
		{
			await PostAsync<SessionEndRequest, object>("/api/v1/session/end", request);
		}

		public async Task<HeartbeatResponse> SendHeartbeatAsync(HeartbeatRequest request)
		{
			return await PostAsync<HeartbeatRequest, HeartbeatResponse>("/api/v1/session/heartbeat", request);
		}

		public async Task<EventBatchResponse> SendEventBatchAsync(EventBatch batch)
		{
			return await PostAsync<EventBatch, EventBatchResponse>("/api/v1/events/batch", batch, _config.EnableCompression);
		}

		public async Task SendHardwareInfoAsync(HardwareInfo info)
		{
			await PostAsync<HardwareInfo, object>("/api/v1/hardware", info);
		}

		public async Task<PerformanceSnapshotResponse> SendPerformanceSnapshotAsync(PerformanceSnapshotRequest request)
		{
			return await PostAsync<PerformanceSnapshotRequest, PerformanceSnapshotResponse>("/api/v1/performance", request);
		}

		public async Task<ConsentStatus> GetConsentAsync(string deviceId)
		{
			return await GetAsync<ConsentStatus>("/api/v1/gdpr/consent/" + deviceId);
		}

		public async Task<ConsentStatus> UpdateConsentAsync(ConsentUpdateRequest request)
		{
			return await PostAsync<ConsentUpdateRequest, ConsentStatus>("/api/v1/gdpr/consent", request);
		}

		public async Task<DataExportResponse> ExportDataAsync(DataExportRequest request)
		{
			return await PostAsync<DataExportRequest, DataExportResponse>("/api/v1/gdpr/export", request);
		}

		public async Task<AnonymizeResponse> AnonymizeAsync(AnonymizeRequest request)
		{
			return await PostAsync<AnonymizeRequest, AnonymizeResponse>("/api/v1/gdpr/anonymize", request);
		}

		public async Task<BugReportResponse> SubmitBugReportAsync(BugReportRequest request, byte[] screenshot = null)
		{
			if (_logger != null)
			{
				_logger($"[EvilAnalytics] SubmitBugReportAsync - Screenshot: {screenshot != null}, Size: {((screenshot != null) ? screenshot.Length : 0)} bytes");
			}
			if (screenshot != null && screenshot.Length != 0)
			{
				return await PostMultipartAsync<BugReportResponse>("/api/v1/bugreport/submit", request, screenshot);
			}
			return await PostAsync<BugReportRequest, BugReportResponse>("/api/v1/bugreport/submit", request);
		}

		public async Task<LogBatchResponse> SendLogBatchAsync(LogBatchRequest request)
		{
			return await PostAsync<LogBatchRequest, LogBatchResponse>("/api/v1/crashlytics/logs", request, _config.EnableCompression);
		}

		public async Task<SubmitScoreResponse> SubmitLeaderboardScoreAsync(Guid leaderboardId, SubmitScoreRequest request)
		{
			return await PostAsync<SubmitScoreRequest, SubmitScoreResponse>($"/api/v1/leaderboards/{leaderboardId}/scores", request);
		}

		public async Task<RankingsResponse> GetLeaderboardRankingsAsync(Guid leaderboardId, string timeWindow = "AllTime", int limit = 10)
		{
			return await GetAsync<RankingsResponse>($"/api/v1/leaderboards/{leaderboardId}/rankings?timeWindow={timeWindow}&limit={limit}");
		}

		public async Task<PlayerRankResponse> GetMyLeaderboardRankAsync(Guid leaderboardId, string deviceId, string timeWindow = "AllTime", int nearbyCount = 3)
		{
			return await GetAsync<PlayerRankResponse>($"/api/v1/leaderboards/{leaderboardId}/rankings/me?deviceId={Uri.EscapeDataString(deviceId)}&timeWindow={timeWindow}&nearby={nearbyCount}");
		}

		public async Task<LiveConfigResponse> GetRemoteConfigAsync()
		{
			_ = 1;
			try
			{
				HttpResponseMessage response = await ExecuteWithRetryAsync(() => _httpClient.GetAsync("/api/v1/config"));
				response.EnsureSuccessStatusCode();
				string text = await response.Content.ReadAsStringAsync();
				if (_logger != null)
				{
					_logger($"[EvilAnalytics] RemoteConfig fetched - Status: {response.StatusCode}, Size: {text.Length} bytes");
				}
				ApiResponse<LiveConfigResponse> apiResponse = JsonConvert.DeserializeObject<ApiResponse<LiveConfigResponse>>(text, _jsonSettings);
				if (_logger != null)
				{
					_logger($"[EvilAnalytics] RemoteConfig parsed - Configs count: {(apiResponse?.Data?.Configs?.Count).GetValueOrDefault()}");
				}
				return apiResponse?.Data;
			}
			catch (Exception ex)
			{
				if (_logger != null)
				{
					_logger("[EvilAnalytics] GetRemoteConfigAsync failed: " + ex.Message);
				}
				throw;
			}
		}

		private async Task<TResponse> PostMultipartAsync<TResponse>(string endpoint, object request, byte[] fileData)
		{
			try
			{
				string content = JsonConvert.SerializeObject(request, _jsonSettings);
				if (_logger != null)
				{
					_logger($"[EvilAnalytics] PostMultipartAsync - FileData: {((fileData != null) ? fileData.Length : 0)} bytes");
				}
				MultipartFormDataContent content2 = new MultipartFormDataContent();
				try
				{
					content2.Add(new StringContent(content, Encoding.UTF8, "application/json"), "request");
					if (fileData != null && fileData.Length != 0)
					{
						ByteArrayContent byteArrayContent = new ByteArrayContent(fileData);
						byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
						content2.Add(byteArrayContent, "screenshot", "screenshot.png");
						if (_logger != null)
						{
							_logger($"[EvilAnalytics] Added screenshot to multipart: {fileData.Length} bytes, ContentType: image/png");
						}
					}
					if (_logger != null)
					{
						_logger(string.Format("[EvilAnalytics] Sending multipart request to {0}, ContentType: {1}", endpoint, content2.Headers.ContentType?.ToString() ?? "null"));
					}
					HttpResponseMessage obj = await ExecuteWithRetryAsync(() => _httpClient.PostAsync(endpoint, content2));
					obj.EnsureSuccessStatusCode();
					string value = await obj.Content.ReadAsStringAsync();
					if (string.IsNullOrEmpty(value))
					{
						return default(TResponse);
					}
					ApiResponse<TResponse> apiResponse = JsonConvert.DeserializeObject<ApiResponse<TResponse>>(value, _jsonSettings);
					return (TResponse)((apiResponse != null) ? ((object)apiResponse.Data) : ((object)default(TResponse)));
				}
				finally
				{
					if (content2 != null)
					{
						((IDisposable)content2).Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				if (_logger != null)
				{
					_logger($"[EvilAnalytics] POST {endpoint} failed: {ex.Message}");
				}
				throw;
			}
		}

		private async Task<TResponse> GetAsync<TResponse>(string endpoint)
		{
			try
			{
				HttpResponseMessage obj = await ExecuteWithRetryAsync(() => _httpClient.GetAsync(endpoint));
				obj.EnsureSuccessStatusCode();
				ApiResponse<TResponse> apiResponse = JsonConvert.DeserializeObject<ApiResponse<TResponse>>(await obj.Content.ReadAsStringAsync(), _jsonSettings);
				return (TResponse)((apiResponse != null) ? ((object)apiResponse.Data) : ((object)default(TResponse)));
			}
			catch (Exception ex)
			{
				if (_logger != null)
				{
					_logger("[EvilAnalytics] GET " + endpoint + " failed: " + ex.Message);
				}
				throw;
			}
		}

		private async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, bool useCompression = false)
		{
			try
			{
				string text = JsonConvert.SerializeObject(request, _jsonSettings);
				HttpContent content2;
				if (useCompression)
				{
					byte[] content = CompressString(text);
					content2 = new ByteArrayContent(content);
					content2.Headers.ContentType = new MediaTypeHeaderValue("application/json");
					content2.Headers.ContentEncoding.Add("gzip");
				}
				else
				{
					content2 = new StringContent(text, Encoding.UTF8, "application/json");
				}
				HttpResponseMessage obj = await ExecuteWithRetryAsync(() => _httpClient.PostAsync(endpoint, content2));
				obj.EnsureSuccessStatusCode();
				string value = await obj.Content.ReadAsStringAsync();
				if (string.IsNullOrEmpty(value))
				{
					return default(TResponse);
				}
				ApiResponse<TResponse> apiResponse = JsonConvert.DeserializeObject<ApiResponse<TResponse>>(value, _jsonSettings);
				return (TResponse)((apiResponse != null) ? ((object)apiResponse.Data) : ((object)default(TResponse)));
			}
			catch (Exception ex)
			{
				if (_logger != null)
				{
					_logger("[EvilAnalytics] POST " + endpoint + " failed: " + ex.Message);
				}
				throw;
			}
		}

		private async Task<HttpResponseMessage> ExecuteWithRetryAsync(Func<Task<HttpResponseMessage>> action)
		{
			Exception lastException = null;
			for (int attempt = 0; attempt <= _config.MaxRetryAttempts; attempt++)
			{
				try
				{
					HttpResponseMessage httpResponseMessage = await action();
					if (httpResponseMessage.IsSuccessStatusCode || !IsRetryableStatusCode(httpResponseMessage.StatusCode))
					{
						return httpResponseMessage;
					}
					lastException = new HttpRequestException($"HTTP {(int)httpResponseMessage.StatusCode}");
				}
				catch (TaskCanceledException ex)
				{
					lastException = ex;
				}
				catch (HttpRequestException ex2)
				{
					lastException = ex2;
				}
				if (attempt < _config.MaxRetryAttempts)
				{
					TimeSpan delay = TimeSpan.FromSeconds((double)_config.RetryBaseDelay * Math.Pow(2.0, attempt));
					if (_logger != null)
					{
						_logger($"[EvilAnalytics] Retry attempt {attempt + 1} after {delay.TotalSeconds}s");
					}
					await Task.Delay(delay);
				}
			}
			throw lastException;
		}

		private static bool IsRetryableStatusCode(HttpStatusCode statusCode)
		{
			if (statusCode != HttpStatusCode.RequestTimeout && statusCode != HttpStatusCode.TooManyRequests)
			{
				return statusCode >= HttpStatusCode.InternalServerError;
			}
			return true;
		}

		private static byte[] CompressString(string text)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			using MemoryStream memoryStream = new MemoryStream();
			using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionLevel.Fastest))
			{
				gZipStream.Write(bytes, 0, bytes.Length);
			}
			return memoryStream.ToArray();
		}
	}
}
