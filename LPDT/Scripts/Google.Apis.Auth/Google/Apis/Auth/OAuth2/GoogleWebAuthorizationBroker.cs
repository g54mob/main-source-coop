using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;

namespace Google.Apis.Auth.OAuth2
{
	public class GoogleWebAuthorizationBroker
	{
		public static string Folder = "Google.Apis.Auth";

		public static async Task<UserCredential> AuthorizeAsync(ClientSecrets clientSecrets, IEnumerable<string> scopes, string user, CancellationToken taskCancellationToken, IDataStore dataStore = null, ICodeReceiver codeReceiver = null)
		{
			return await AuthorizeAsync(new GoogleAuthorizationCodeFlow.Initializer
			{
				ClientSecrets = clientSecrets
			}, scopes, user, taskCancellationToken, dataStore, codeReceiver).ConfigureAwait(continueOnCapturedContext: false);
		}

		public static async Task<UserCredential> AuthorizeAsync(Stream clientSecretsStream, IEnumerable<string> scopes, string user, CancellationToken taskCancellationToken, IDataStore dataStore = null, ICodeReceiver codeReceiver = null)
		{
			return await AuthorizeAsync(new GoogleAuthorizationCodeFlow.Initializer
			{
				ClientSecretsStream = clientSecretsStream
			}, scopes, user, taskCancellationToken, dataStore, codeReceiver).ConfigureAwait(continueOnCapturedContext: false);
		}

		public static async Task ReauthorizeAsync(UserCredential userCredential, CancellationToken taskCancellationToken, ICodeReceiver codeReceiver = null)
		{
			codeReceiver = codeReceiver ?? new LocalServerCodeReceiver();
			await userCredential.Flow.DeleteTokenAsync(userCredential.UserId, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			userCredential.Token = (await new AuthorizationCodeInstalledApp(userCredential.Flow, codeReceiver).AuthorizeAsync(userCredential.UserId, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false)).Token;
		}

		public static async Task<UserCredential> AuthorizeAsync(GoogleAuthorizationCodeFlow.Initializer initializer, IEnumerable<string> scopes, string user, CancellationToken taskCancellationToken, IDataStore dataStore = null, ICodeReceiver codeReceiver = null)
		{
			initializer.Scopes = scopes;
			initializer.DataStore = dataStore ?? new FileDataStore(Folder);
			GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(initializer);
			codeReceiver = codeReceiver ?? new LocalServerCodeReceiver();
			return await new AuthorizationCodeInstalledApp(flow, codeReceiver).AuthorizeAsync(user, taskCancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
