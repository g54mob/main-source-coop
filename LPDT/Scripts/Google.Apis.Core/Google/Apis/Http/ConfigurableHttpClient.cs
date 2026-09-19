using System.Net.Http;

namespace Google.Apis.Http
{
	public class ConfigurableHttpClient : HttpClient
	{
		public ConfigurableMessageHandler MessageHandler { get; private set; }

		public ConfigurableHttpClient(ConfigurableMessageHandler handler)
			: this(handler, disposeHandler: true)
		{
		}

		public ConfigurableHttpClient(ConfigurableMessageHandler handler, bool disposeHandler)
			: base(handler, disposeHandler)
		{
			MessageHandler = handler;
			base.DefaultRequestHeaders.ExpectContinue = false;
		}
	}
}
