using System;
using System.Runtime.InteropServices;

internal class MunXNfRHcjueFTUWStyiJduemnZw
{
	internal enum jVAoMBoFzyNugseuvwVmUJPpFxfA
	{
		WndProc = -4,
		HInstance = -6,
		HwndParent = -8,
		Style = -16,
		ExtendedStyle = -20,
		UserData = -21,
		Id = -12
	}

	private static IntPtr AadwHCNURkEDWNpZkQDGlVFPypHi = IntPtr.Zero;

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "CallWindowProc")]
	public static extern IntPtr kSAUGLoOXKJJrPuYquMrmlEdGZyI(IntPtr P_0, IntPtr P_1, int P_2, IntPtr P_3, IntPtr P_4);

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetModuleHandle")]
	public static extern IntPtr TzeMdTIYVDfgseMkEtqUuDXRYPzQ(string P_0);

	public static IntPtr bmNIBpJNlzojQttATVAxfcuQofDx(IntPtr P_0, jVAoMBoFzyNugseuvwVmUJPpFxfA P_1)
	{
		if (IntPtr.Size == 4)
		{
			return eJOCbVkgaiuFLekpucrFcNkkrJQG(P_0, P_1);
		}
		return haqAeVCVJvUBeAtbApBpElXvNynq(P_0, P_1);
	}

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLong")]
	private static extern IntPtr eJOCbVkgaiuFLekpucrFcNkkrJQG(IntPtr P_0, jVAoMBoFzyNugseuvwVmUJPpFxfA P_1);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetWindowLongPtr")]
	private static extern IntPtr haqAeVCVJvUBeAtbApBpElXvNynq(IntPtr P_0, jVAoMBoFzyNugseuvwVmUJPpFxfA P_1);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "IsWindow")]
	public static extern bool rJbsNbcZQghyvLNTnPNMWGGPhCMGA(IntPtr P_0);
}
