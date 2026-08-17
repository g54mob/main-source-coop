using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Edgegap.Editor.Api.Models.Requests;
using Edgegap.Editor.Api.Models.Results;

namespace Edgegap.Editor.Api
{
	public class EdgegapDeploymentsApi : EdgegapApiBase
	{
		public EdgegapDeploymentsApi(ApiEnvironment apiEnvironment, string apiToken, EdgegapWindowMetadata.LogLevel logLevel = EdgegapWindowMetadata.LogLevel.Error)
			: base(apiEnvironment, apiToken, logLevel)
		{
		}

		public async Task<EdgegapHttpResult<CreateDeploymentResult>> CreateDeploymentAsync(CreateDeploymentRequest request)
		{
			HttpResponseMessage obj = await PostAsync("v1/deploy", request.ToString());
			EdgegapHttpResult<CreateDeploymentResult> result = new EdgegapHttpResult<CreateDeploymentResult>(obj);
			_ = obj.StatusCode == HttpStatusCode.OK;
			return result;
		}

		public async Task<EdgegapHttpResult<GetDeploymentStatusResult>> GetDeploymentStatusAsync(string requestId)
		{
			HttpResponseMessage obj = await GetAsync("v1/status/" + requestId);
			EdgegapHttpResult<GetDeploymentStatusResult> result = new EdgegapHttpResult<GetDeploymentStatusResult>(obj);
			_ = obj.StatusCode == HttpStatusCode.OK;
			return result;
		}

		public async Task<EdgegapHttpResult<StopActiveDeploymentResult>> StopActiveDeploymentAsync(string requestId)
		{
			HttpResponseMessage obj = await DeleteAsync("v1/stop/" + requestId);
			EdgegapHttpResult<StopActiveDeploymentResult> result = new EdgegapHttpResult<StopActiveDeploymentResult>(obj);
			_ = obj.StatusCode == HttpStatusCode.OK;
			return result;
		}

		public async Task<EdgegapHttpResult<CreateDeploymentResult>> CreateDeploymentAwaitReadyStatusAsync(CreateDeploymentRequest request, TimeSpan pollInterval)
		{
			EdgegapHttpResult<CreateDeploymentResult> createResponse = await CreateDeploymentAsync(request);
			if (createResponse.StatusCode != HttpStatusCode.OK)
			{
				return createResponse;
			}
			string requestId = createResponse.Data.RequestId;
			await AwaitReadyStatusAsync(requestId, pollInterval);
			return createResponse;
		}

		public async Task<EdgegapHttpResult<GetDeploymentStatusResult>> AwaitReadyStatusAsync(string requestId, TimeSpan pollInterval)
		{
			EdgegapHttpResult<GetDeploymentStatusResult> edgegapHttpResult = null;
			CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(1.0));
			bool flag = false;
			while (!flag && !cts.Token.IsCancellationRequested)
			{
				await Task.Delay(pollInterval, cts.Token);
				edgegapHttpResult = await GetDeploymentStatusAsync(requestId);
				flag = edgegapHttpResult.Data.CurrentStatus == "Status.READY";
			}
			return edgegapHttpResult;
		}

		public async Task<EdgegapHttpResult<StopActiveDeploymentResult>> AwaitTerminatedDeleteStatusAsync(string requestId, TimeSpan pollInterval)
		{
			EdgegapHttpResult<StopActiveDeploymentResult> edgegapHttpResult = null;
			CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(1.0));
			bool flag = false;
			while (!flag && !cts.Token.IsCancellationRequested)
			{
				await Task.Delay(pollInterval, cts.Token);
				edgegapHttpResult = await StopActiveDeploymentAsync(requestId);
				flag = edgegapHttpResult.StatusCode == HttpStatusCode.Gone;
			}
			return edgegapHttpResult;
		}
	}
}
