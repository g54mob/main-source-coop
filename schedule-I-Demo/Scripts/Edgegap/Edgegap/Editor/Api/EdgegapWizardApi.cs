using System.Threading.Tasks;
using Edgegap.Editor.Api.Models.Results;
using Newtonsoft.Json.Linq;

namespace Edgegap.Editor.Api
{
	public class EdgegapWizardApi : EdgegapApiBase
	{
		public EdgegapWizardApi(ApiEnvironment apiEnvironment, string apiToken, EdgegapWindowMetadata.LogLevel logLevel = EdgegapWindowMetadata.LogLevel.Error)
			: base(apiEnvironment, apiToken, logLevel)
		{
		}

		public async Task<EdgegapHttpResult> InitQuickStart()
		{
			string json = new JObject { ["source"] = "unity" }.ToString();
			return new EdgegapHttpResult(await PostAsync("v1/wizard/init-quick-start", json));
		}

		public async Task<EdgegapHttpResult<GetRegistryCredentialsResult>> GetRegistryCredentials()
		{
			return new EdgegapHttpResult<GetRegistryCredentialsResult>(await GetAsync("v1/wizard/registry-credentials"));
		}
	}
}
