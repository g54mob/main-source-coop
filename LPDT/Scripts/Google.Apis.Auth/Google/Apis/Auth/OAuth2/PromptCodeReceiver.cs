using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Logging;

namespace Google.Apis.Auth.OAuth2
{
	[Obsolete("The OAuth out-of-band flow will be deprecated and this class will be removed on October 3rd 2022. You can read more about deprecation here: https://developers.googleblog.com/2022/02/making-oauth-flows-safer.html#disallowed-oob.")]
	public class PromptCodeReceiver : ICodeReceiver
	{
		private static readonly ILogger Logger = ApplicationContext.Logger.ForType<PromptCodeReceiver>();

		public string RedirectUri => "urn:ietf:wg:oauth:2.0:oob";

		public Task<AuthorizationCodeResponseUrl> ReceiveCodeAsync(AuthorizationCodeRequestUrl url, CancellationToken taskCancellationToken)
		{
			string absoluteUri = url.Build().AbsoluteUri;
			Logger.Debug("Requested user open a browser with \"{0}\" URL", absoluteUri);
			Console.WriteLine("Please visit the following URL in a web browser, then enter the code shown after authorization:");
			Console.WriteLine(absoluteUri);
			Console.WriteLine();
			string text = string.Empty;
			while (string.IsNullOrEmpty(text))
			{
				Console.WriteLine("Please enter code: ");
				text = Console.ReadLine();
			}
			Logger.Debug("Code is: \"{0}\"", text);
			return Task.FromResult(new AuthorizationCodeResponseUrl
			{
				Code = text
			});
		}
	}
}
