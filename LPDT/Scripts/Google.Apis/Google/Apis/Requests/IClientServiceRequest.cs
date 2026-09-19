using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Discovery;
using Google.Apis.Services;

namespace Google.Apis.Requests
{
	public interface IClientServiceRequest
	{
		string MethodName { get; }

		string RestPath { get; }

		string HttpMethod { get; }

		IDictionary<string, IParameter> RequestParameters { get; }

		IClientService Service { get; }

		HttpRequestMessage CreateRequest(bool? overrideGZipEnabled = null);

		Task<Stream> ExecuteAsStreamAsync();

		Task<Stream> ExecuteAsStreamAsync(CancellationToken cancellationToken);

		Stream ExecuteAsStream();
	}
	public interface IClientServiceRequest<TResponse> : IClientServiceRequest
	{
		Task<TResponse> ExecuteAsync();

		Task<TResponse> ExecuteAsync(CancellationToken cancellationToken);

		TResponse Execute();
	}
}
