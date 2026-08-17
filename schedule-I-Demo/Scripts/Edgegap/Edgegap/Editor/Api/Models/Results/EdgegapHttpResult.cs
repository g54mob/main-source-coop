using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Edgegap.Editor.Api.Models.Results
{
	public class EdgegapHttpResult
	{
		public HttpMethod HttpMethod;

		public HttpStatusCode StatusCode { get; }

		public string Json { get; }

		public string ReasonPhrase { get; }

		public bool HasErr => Error != null;

		public EdgegapErrorResult Error { get; set; }

		public bool IsResultCode200 => StatusCode == HttpStatusCode.OK;

		public bool IsResultCode204 => StatusCode == HttpStatusCode.NoContent;

		public bool IsResultCode403 => StatusCode == HttpStatusCode.Forbidden;

		public bool IsResultCode409 => StatusCode == HttpStatusCode.Conflict;

		public bool IsResultCode400 => StatusCode == HttpStatusCode.BadRequest;

		public bool IsResultCode410 => StatusCode == HttpStatusCode.Gone;

		public EdgegapHttpResult(HttpResponseMessage httpResponse)
		{
			ReasonPhrase = httpResponse.ReasonPhrase;
			StatusCode = httpResponse.StatusCode;
			try
			{
				Json = httpResponse.Content.ReadAsStringAsync().Result;
				Error = JsonConvert.DeserializeObject<EdgegapErrorResult>(Json);
				if (Error != null && string.IsNullOrEmpty(Error.ErrorMessage))
				{
					Error = null;
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Error (reading httpResponse.Content): Client expected json, " + $"but server returned !json: {arg} - ");
			}
		}
	}
	public class EdgegapHttpResult<TResult> : EdgegapHttpResult
	{
		public TResult Data { get; set; }

		public EdgegapHttpResult(HttpResponseMessage httpResponse, bool isLogLevelDebug = false)
			: base(httpResponse)
		{
			HttpMethod = httpResponse.RequestMessage.Method;
			if (httpResponse.Content != null && httpResponse.Content.Headers.ContentType.MediaType == "application/json")
			{
				try
				{
					Data = JsonConvert.DeserializeObject<TResult>(base.Json);
				}
				catch (Exception arg)
				{
					Debug.LogError($"Error (deserializing EdgegapHttpResult.Data): {arg} - json: {base.Json}");
					throw;
				}
			}
			if (isLogLevelDebug)
			{
				Debug.Log($"{typeof(TResult).Name} result: {JObject.Parse(base.Json)}");
			}
		}
	}
}
