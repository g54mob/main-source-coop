using System;

namespace Rewired.Utils.Classes.Data
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class IntPtrWrapper
	{
		private IntPtr yWCikRznZBWAdFzJsHvLSUsrIMow;

		public bool IsValid => yWCikRznZBWAdFzJsHvLSUsrIMow != IntPtr.Zero;

		public IntPtrWrapper(IntPtr P_0)
		{
			yWCikRznZBWAdFzJsHvLSUsrIMow = P_0;
		}

		public void Clear()
		{
			yWCikRznZBWAdFzJsHvLSUsrIMow = IntPtr.Zero;
		}

		public static implicit operator IntPtr(IntPtrWrapper obj)
		{
			return obj?.yWCikRznZBWAdFzJsHvLSUsrIMow ?? IntPtr.Zero;
		}
	}
}
