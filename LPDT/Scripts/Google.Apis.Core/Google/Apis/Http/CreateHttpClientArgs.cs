using System.Collections.Generic;

namespace Google.Apis.Http
{
	public class CreateHttpClientArgs
	{
		public bool GZipEnabled { get; set; }

		public string ApplicationName { get; set; }

		public IList<IConfigurableHttpClientInitializer> Initializers { get; private set; }

		public string GoogleApiClientHeader { get; set; }

		public CreateHttpClientArgs()
		{
			Initializers = new List<IConfigurableHttpClientInitializer>();
		}
	}
}
