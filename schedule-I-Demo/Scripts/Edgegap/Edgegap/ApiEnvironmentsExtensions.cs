namespace Edgegap
{
	public static class ApiEnvironmentsExtensions
	{
		public static string GetApiUrl(this ApiEnvironment apiEnvironment)
		{
			return apiEnvironment switch
			{
				ApiEnvironment.Staging => "https://staging-api.edgegap.com", 
				ApiEnvironment.Console => "https://api.edgegap.com", 
				_ => null, 
			};
		}

		public static string GetDashboardUrl(this ApiEnvironment apiEnvironment)
		{
			return apiEnvironment switch
			{
				ApiEnvironment.Staging => "https://staging-console.edgegap.com", 
				ApiEnvironment.Console => "https://console.edgegap.com", 
				_ => null, 
			};
		}

		public static string GetDocumentationUrl(this ApiEnvironment apiEnvironment)
		{
			return apiEnvironment switch
			{
				ApiEnvironment.Staging => "https://staging-docs.edgegap.com/docs", 
				ApiEnvironment.Console => "https://docs.edgegap.com/docs", 
				_ => null, 
			};
		}
	}
}
