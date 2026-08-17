using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Edgegap.Editor.Api.Models.Requests;
using Edgegap.Editor.Api.Models.Results;

namespace Edgegap.Editor.Api
{
	public class EdgegapAppApi : EdgegapApiBase
	{
		public EdgegapAppApi(ApiEnvironment apiEnvironment, string apiToken, EdgegapWindowMetadata.LogLevel logLevel = EdgegapWindowMetadata.LogLevel.Error)
			: base(apiEnvironment, apiToken, logLevel)
		{
		}

		public async Task<EdgegapHttpResult<GetCreateAppResult>> CreateApp(CreateAppRequest request)
		{
			HttpResponseMessage obj = await PostAsync("v1/app", request.ToString());
			EdgegapHttpResult<GetCreateAppResult> result = new EdgegapHttpResult<GetCreateAppResult>(obj);
			_ = obj.StatusCode == HttpStatusCode.OK;
			return result;
		}

		public async Task<EdgegapHttpResult<GetCreateAppResult>> GetApp(string appName)
		{
			HttpResponseMessage obj = await GetAsync("v1/app/" + appName);
			EdgegapHttpResult<GetCreateAppResult> result = new EdgegapHttpResult<GetCreateAppResult>(obj);
			_ = obj.StatusCode == HttpStatusCode.OK;
			return result;
		}

		public async Task<EdgegapHttpResult<UpsertAppVersionResult>> UpdateAppVersion(UpdateAppVersionRequest request)
		{
			string relativePath = "v1/app/" + request.AppName + "/version/" + request.VersionName;
			HttpResponseMessage obj = await PatchAsync(relativePath, request.ToString());
			EdgegapHttpResult<UpsertAppVersionResult> result = new EdgegapHttpResult<UpsertAppVersionResult>(obj);
			_ = obj.StatusCode == HttpStatusCode.OK;
			return result;
		}

		public async Task<EdgegapHttpResult<UpsertAppVersionResult>> CreateAppVersion(CreateAppVersionRequest request)
		{
			string relativePath = "v1/app/" + request.AppName + "/version";
			HttpResponseMessage obj = await PostAsync(relativePath, request.ToString());
			EdgegapHttpResult<UpsertAppVersionResult> result = new EdgegapHttpResult<UpsertAppVersionResult>(obj);
			_ = obj.StatusCode == HttpStatusCode.OK;
			return result;
		}

		public async Task<EdgegapHttpResult<UpsertAppVersionResult>> UpsertAppVersion(UpdateAppVersionRequest request)
		{
			EdgegapHttpResult<UpsertAppVersionResult> edgegapHttpResult = await UpdateAppVersion(request);
			if (edgegapHttpResult.HasErr)
			{
				CreateAppVersionRequest request2 = CreateAppVersionRequest.FromUpdateRequest(request);
				edgegapHttpResult = await CreateAppVersion(request2);
			}
			_ = edgegapHttpResult.StatusCode == HttpStatusCode.OK;
			return edgegapHttpResult;
		}
	}
}
