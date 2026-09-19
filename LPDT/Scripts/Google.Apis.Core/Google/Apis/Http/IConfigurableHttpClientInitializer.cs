namespace Google.Apis.Http
{
	public interface IConfigurableHttpClientInitializer
	{
		void Initialize(ConfigurableHttpClient httpClient);
	}
}
