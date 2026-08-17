using System;

namespace Epic.OnlineServices.Auth
{
	public struct Credentials
	{
		public Utf8String Id { get; set; }

		public Utf8String Token { get; set; }

		public LoginCredentialType Type { get; set; }

		public IntPtr SystemAuthCredentialsOptions { get; }

		public ExternalCredentialType ExternalType { get; set; }
	}
}
