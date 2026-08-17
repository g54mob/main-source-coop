using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Edgegap.Editor.Api.Models.Results;

namespace Edgegap.Editor.Api
{
	public class EdgegapIpApi : EdgegapApiBase
	{
		public EdgegapIpApi(ApiEnvironment apiEnvironment, string apiToken, EdgegapWindowMetadata.LogLevel logLevel = EdgegapWindowMetadata.LogLevel.Error)
			: base(apiEnvironment, apiToken, logLevel)
		{
		}

		public async Task<EdgegapHttpResult<GetYourPublicIpResult>> GetYourPublicIp()
		{
			HttpResponseMessage obj = await GetAsync("v1/ip");
			EdgegapHttpResult<GetYourPublicIpResult> result = new EdgegapHttpResult<GetYourPublicIpResult>(obj);
			_ = obj.StatusCode == HttpStatusCode.OK;
			return result;
		}
	}
}
