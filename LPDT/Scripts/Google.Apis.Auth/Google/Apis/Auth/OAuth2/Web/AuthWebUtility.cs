using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;

namespace Google.Apis.Auth.OAuth2.Web
{
	public class AuthWebUtility
	{
		public static async Task<string> ExtracRedirectFromState(IDataStore dataStore, string userId, string state)
		{
			string oauthState = state;
			if (dataStore != null)
			{
				string userKey = "oauth_" + userId;
				if (!object.Equals(oauthState, await dataStore.GetAsync<string>(userKey).ConfigureAwait(continueOnCapturedContext: false)))
				{
					throw new TokenResponseException(new TokenErrorResponse
					{
						Error = "State is invalid"
					});
				}
				await dataStore.DeleteAsync<string>(userKey).ConfigureAwait(continueOnCapturedContext: false);
				oauthState = oauthState.Substring(0, oauthState.Length - 8);
			}
			return oauthState;
		}
	}
}
