using System.Threading.Tasks;

namespace Google.Apis.Http
{
	public interface IHttpExceptionHandler
	{
		Task<bool> HandleExceptionAsync(HandleExceptionArgs args);
	}
}
