namespace Google.Apis.Http
{
	public interface IHttpClientFactory
	{
		ConfigurableHttpClient CreateHttpClient(CreateHttpClientArgs args);
	}
}
