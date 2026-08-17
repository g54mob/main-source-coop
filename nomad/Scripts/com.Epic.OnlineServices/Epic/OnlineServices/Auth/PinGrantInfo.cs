using System.Runtime.CompilerServices;

namespace Epic.OnlineServices.Auth
{
	public struct PinGrantInfo
	{
		[CompilerGenerated]
		private Utf8String _003CUserCode_003Ek__BackingField;

		[CompilerGenerated]
		private Utf8String _003CVerificationURI_003Ek__BackingField;

		[CompilerGenerated]
		private int _003CExpiresIn_003Ek__BackingField;

		[CompilerGenerated]
		private Utf8String _003CVerificationURIComplete_003Ek__BackingField;

		public Utf8String UserCode
		{
			[CompilerGenerated]
			set
			{
				_003CUserCode_003Ek__BackingField = value;
			}
		}

		public Utf8String VerificationURI
		{
			[CompilerGenerated]
			set
			{
				_003CVerificationURI_003Ek__BackingField = value;
			}
		}

		public int ExpiresIn
		{
			[CompilerGenerated]
			set
			{
				_003CExpiresIn_003Ek__BackingField = value;
			}
		}

		public Utf8String VerificationURIComplete
		{
			[CompilerGenerated]
			set
			{
				_003CVerificationURIComplete_003Ek__BackingField = value;
			}
		}
	}
}
