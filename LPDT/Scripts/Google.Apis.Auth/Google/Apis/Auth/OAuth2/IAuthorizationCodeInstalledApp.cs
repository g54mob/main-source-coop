using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Flows;

namespace Google.Apis.Auth.OAuth2
{
	public interface IAuthorizationCodeInstalledApp
	{
		IAuthorizationCodeFlow Flow { get; }

		ICodeReceiver CodeReceiver { get; }

		Task<UserCredential> AuthorizeAsync(string userId, CancellationToken taskCancellationToken);
	}
}
