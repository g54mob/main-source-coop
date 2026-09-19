using System;

namespace Google.Apis.Http
{
	public class ExponentialBackOffInitializer : IConfigurableHttpClientInitializer
	{
		private ExponentialBackOffPolicy Policy { get; set; }

		private Func<BackOffHandler> CreateBackOff { get; set; }

		public ExponentialBackOffInitializer(ExponentialBackOffPolicy policy, Func<BackOffHandler> createBackOff)
		{
			Policy = policy;
			CreateBackOff = createBackOff;
		}

		public void Initialize(ConfigurableHttpClient httpClient)
		{
			BackOffHandler handler = CreateBackOff();
			if ((Policy & ExponentialBackOffPolicy.Exception) == ExponentialBackOffPolicy.Exception)
			{
				httpClient.MessageHandler.AddExceptionHandler(handler);
			}
			if ((Policy & ExponentialBackOffPolicy.UnsuccessfulResponse503) == ExponentialBackOffPolicy.UnsuccessfulResponse503)
			{
				httpClient.MessageHandler.AddUnsuccessfulResponseHandler(handler);
			}
		}
	}
}
