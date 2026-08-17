using System;

namespace Rewired.Utils.Classes.Data
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class IntPtrWrapper
	{
		private IntPtr ljzfrArFMuiyYRPqewhwgAyklHtP;

		public bool IsValid => ljzfrArFMuiyYRPqewhwgAyklHtP != IntPtr.Zero;

		public IntPtrWrapper(IntPtr P_0)
		{
			ljzfrArFMuiyYRPqewhwgAyklHtP = P_0;
		}

		public void Clear()
		{
			ljzfrArFMuiyYRPqewhwgAyklHtP = IntPtr.Zero;
		}

		public static implicit operator IntPtr(IntPtrWrapper obj)
		{
			return obj?.ljzfrArFMuiyYRPqewhwgAyklHtP ?? IntPtr.Zero;
		}
	}
}
