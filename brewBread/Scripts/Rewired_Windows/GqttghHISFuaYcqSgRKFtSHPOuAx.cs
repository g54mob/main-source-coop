using System;
using System.Runtime.InteropServices;
using System.Security;
using Rewired.Utils.Attributes;
using Rewired.Utils.Classes.Utility;

internal class GqttghHISFuaYcqSgRKFtSHPOuAx : IDisposable
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate IntPtr oyRVEcgWKZSKUYOzHXwZOoYScKbK(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct OBmCRdCcnPKiAfXMPOqobwWFaaYgB
	{
		public uint AlyokBHHslwVZDibFClBLZfQuISW;

		public IntPtr xWwVIJzfqtudskQAaVUYiEotrLRe;

		public int XzTdpcjOZhoOoeErImcgHligVDhDC;

		public int AuqyQYtXFJFbbxEFZiLbcWNRjuuNA;

		public IntPtr gZnVUOlBbJCHGbdjjSlmFqRzmcrIA;

		public IntPtr smwCyVWGQSGFHVfbQAOsMZDHrJZC;

		public IntPtr jivFWMCJMEXoFexiGxAwoTMtMwEUA;

		public IntPtr aYLbSiDZlvLcCUceCsQEYKIaaxbP;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string MqRzQMKZgUUsqEnPEMYMKkcJQgrv;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string TcnGuyhImVJYpAdlcRsgviuZEVeIA;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct RtArnBfkyYWSDOxLBnVbZahIFDMs
	{
		public IntPtr DlMwOhdRqDmQcNHytuxlpNzzSZnG;

		public IntPtr gZnVUOlBbJCHGbdjjSlmFqRzmcrIA;

		public IntPtr pEsmgfwjrgodYkoqweakEajxRIYfA;

		public IntPtr DmgrDBwbjumKPbEyAtUuVadfVcAw;

		public int UqEqGiMdRqvpaBTubGUULfSKAnks;

		public int vSdByAfIUKyUveKOfbaXCNbvlSzR;

		public int nWpIIQRjiCIFaiiGnHzTvHDxytFG;

		public int BqmjxAfwgseNVDKGAzjtxWWBOPHv;

		public int AlyokBHHslwVZDibFClBLZfQuISW;

		public IntPtr EPvgXcBjAfWEMvrtnnhFdQltdECd;

		public IntPtr sdpjIzMHksbrXOIzEwKaSTejzXXh;

		public uint VecxcNDRBnXQoJmHARflwegpBweS;
	}

	private const int RbooMrUswbddnaKwiETAHzbYfEKIA = 20;

	private const int eeAhYsvdcDYwXYtzudOrmoooMusD = 1410;

	private readonly ushort eZHROvbfXwPSJfVsarxCKqhQMpEe;

	private readonly string RXgGuhHrRtwPhmjnCnQfFoHExVcG;

	private bool nXMYebxBEVLkAcQekUgEahQwHNmM;

	private IntPtr VDdkaWwGPVtoVwfHFdnpjOokkqkz;

	private int exYGPOMzyQsKNADtCjSSHPBTrQhv;

	private uint gSXxwrOZBevfedkvWwvRhYkVILhr;

	private oyRVEcgWKZSKUYOzHXwZOoYScKbK iamHOLKbiHtHWkqLXIBOPuZpmwrDb;

	private oyRVEcgWKZSKUYOzHXwZOoYScKbK xIrIQUmPlsSeaOTUDXCBoasnEyYl;

	public IntPtr oepVrmXtIGzRyuQJAVrHRwaiVpGj => VDdkaWwGPVtoVwfHFdnpjOokkqkz;

	public uint PkavxEgFmrYaBKnAnNhIHJmPRneY => gSXxwrOZBevfedkvWwvRhYkVILhr;

	public bool DgFfRazlpGlPRvXiNDREVkfKncsI
	{
		get
		{
			if (!(VDdkaWwGPVtoVwfHFdnpjOokkqkz != IntPtr.Zero))
			{
				return false;
			}
			return OiaxAhOcCrpxoTRQQDMjFSbySsem(VDdkaWwGPVtoVwfHFdnpjOokkqkz);
		}
	}

	[DllImport("user32.dll", EntryPoint = "RegisterClassW", SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	private static extern ushort aHtuwvyBgUWNxCIrHqsZvNWvEFkbA([In] ref OBmCRdCcnPKiAfXMPOqobwWFaaYgB P_0);

	[DllImport("user32.dll", EntryPoint = "UnregisterClassW", SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	private static extern bool cbUJefaYxMlvczKNskuqkAJckYRb([MarshalAs(UnmanagedType.LPWStr)] string P_0, IntPtr P_1);

	[DllImport("user32.dll", EntryPoint = "CreateWindowExW", SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	private static extern IntPtr FASwgYJUAcoFMnOGnGRAHeCiSySOA(uint P_0, [MarshalAs(UnmanagedType.LPWStr)] string P_1, [MarshalAs(UnmanagedType.LPWStr)] string P_2, uint P_3, int P_4, int P_5, int P_6, int P_7, IntPtr P_8, IntPtr P_9, IntPtr P_10, IntPtr P_11);

	[DllImport("user32.dll", EntryPoint = "DefWindowProcW", SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	private static extern IntPtr XCEeOpdNZXUgBsKznZZbtbwNBVsSA(IntPtr P_0, uint P_1, IntPtr P_2, IntPtr P_3);

	[DllImport("user32.dll", EntryPoint = "DestroyWindow", SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	private static extern bool bvNSnpvMuRkgdgaTRmyHkYdWBeNb(IntPtr P_0);

	[DllImport("user32.dll", EntryPoint = "IsWindow", SetLastError = true)]
	[SuppressUnmanagedCodeSecurity]
	private static extern bool OiaxAhOcCrpxoTRQQDMjFSbySsem(IntPtr P_0);

	public void Dispose()
	{
		lDxnsjCDTQrmresvWgbliNUVruIc(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void zNJVymYugIbeeZuNgMrKxyYWbziV()
	{
		try
		{
			lDxnsjCDTQrmresvWgbliNUVruIc(false);
		}
		finally
		{
			base.Finalize();
		}
	}

	private void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0)
	{
		if (!nXMYebxBEVLkAcQekUgEahQwHNmM)
		{
			if (P_0)
			{
				ObjectInstanceTracker.Default.Unregister(gSXxwrOZBevfedkvWwvRhYkVILhr);
			}
			if (VDdkaWwGPVtoVwfHFdnpjOokkqkz != IntPtr.Zero)
			{
				bvNSnpvMuRkgdgaTRmyHkYdWBeNb(VDdkaWwGPVtoVwfHFdnpjOokkqkz);
				VDdkaWwGPVtoVwfHFdnpjOokkqkz = IntPtr.Zero;
			}
			if (eZHROvbfXwPSJfVsarxCKqhQMpEe != 0 && !string.IsNullOrEmpty(RXgGuhHrRtwPhmjnCnQfFoHExVcG))
			{
				cbUJefaYxMlvczKNskuqkAJckYRb(RXgGuhHrRtwPhmjnCnQfFoHExVcG, IntPtr.Zero);
			}
			nXMYebxBEVLkAcQekUgEahQwHNmM = true;
		}
	}

	public GqttghHISFuaYcqSgRKFtSHPOuAx(string P_0, bool P_1, oyRVEcgWKZSKUYOzHXwZOoYScKbK P_2)
	{
		if (string.IsNullOrEmpty(P_0))
		{
			throw new ArgumentNullException("className");
		}
		if (P_2 == null)
		{
			throw new ArgumentNullException("staticCustomWndProcDelegate");
		}
		gSXxwrOZBevfedkvWwvRhYkVILhr = ObjectInstanceTracker.Default.Register(this);
		RXgGuhHrRtwPhmjnCnQfFoHExVcG = P_0;
		iamHOLKbiHtHWkqLXIBOPuZpmwrDb = NDDbFgiywgOWxormmNwrXsDVgexPA;
		xIrIQUmPlsSeaOTUDXCBoasnEyYl = P_2;
		exYGPOMzyQsKNADtCjSSHPBTrQhv = 0;
		OBmCRdCcnPKiAfXMPOqobwWFaaYgB oBmCRdCcnPKiAfXMPOqobwWFaaYgB = new OBmCRdCcnPKiAfXMPOqobwWFaaYgB
		{
			xWwVIJzfqtudskQAaVUYiEotrLRe = Marshal.GetFunctionPointerForDelegate((Delegate)iamHOLKbiHtHWkqLXIBOPuZpmwrDb)
		};
		while (eZHROvbfXwPSJfVsarxCKqhQMpEe == 0 && exYGPOMzyQsKNADtCjSSHPBTrQhv < 20)
		{
			oBmCRdCcnPKiAfXMPOqobwWFaaYgB.TcnGuyhImVJYpAdlcRsgviuZEVeIA = P_0;
			eZHROvbfXwPSJfVsarxCKqhQMpEe = aHtuwvyBgUWNxCIrHqsZvNWvEFkbA(ref oBmCRdCcnPKiAfXMPOqobwWFaaYgB);
			if (eZHROvbfXwPSJfVsarxCKqhQMpEe != 0)
			{
				break;
			}
			exYGPOMzyQsKNADtCjSSHPBTrQhv++;
			P_0 = RXgGuhHrRtwPhmjnCnQfFoHExVcG + exYGPOMzyQsKNADtCjSSHPBTrQhv;
		}
		if (eZHROvbfXwPSJfVsarxCKqhQMpEe == 0)
		{
			throw new Exception("Could not register window class!");
		}
		if (RXgGuhHrRtwPhmjnCnQfFoHExVcG != P_0)
		{
			RXgGuhHrRtwPhmjnCnQfFoHExVcG = P_0;
		}
		if (P_1)
		{
			VDdkaWwGPVtoVwfHFdnpjOokkqkz = PSINCYDdZqbEFctUiiqcDkEcInaoA(P_0, new IntPtr((int)gSXxwrOZBevfedkvWwvRhYkVILhr));
		}
		else
		{
			VDdkaWwGPVtoVwfHFdnpjOokkqkz = XFoQrQrYAObySJlKtFZOtyjcVnRC(P_0, new IntPtr((int)gSXxwrOZBevfedkvWwvRhYkVILhr));
		}
	}

	private IntPtr XFoQrQrYAObySJlKtFZOtyjcVnRC(string P_0, IntPtr P_1)
	{
		return FASwgYJUAcoFMnOGnGRAHeCiSySOA(0u, P_0, string.Empty, 0u, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, P_1);
	}

	private IntPtr PSINCYDdZqbEFctUiiqcDkEcInaoA(string P_0, IntPtr P_1)
	{
		return FASwgYJUAcoFMnOGnGRAHeCiSySOA(0u, P_0, string.Empty, 0u, 0, 0, 0, 0, vQxfanyYJnehFMFEjGpxIpuLKpqg.sgdXNClpUAXELvLGYaHfoAQeaqxG, IntPtr.Zero, IntPtr.Zero, P_1);
	}

	[MonoPInvokeCallback(typeof(oyRVEcgWKZSKUYOzHXwZOoYScKbK))]
	private unsafe static IntPtr NDDbFgiywgOWxormmNwrXsDVgexPA(IntPtr P_0, uint P_1, IntPtr P_2, IntPtr P_3)
	{
		if (P_0 == IntPtr.Zero)
		{
			return XCEeOpdNZXUgBsKznZZbtbwNBVsSA(P_0, P_1, P_2, P_3);
		}
		bool flag = false;
		uint instanceId = 0u;
		if (P_1 == 1)
		{
			RtArnBfkyYWSDOxLBnVbZahIFDMs* ptr = (RtArnBfkyYWSDOxLBnVbZahIFDMs*)(void*)P_3;
			if (ptr->DlMwOhdRqDmQcNHytuxlpNzzSZnG != IntPtr.Zero)
			{
				hUfdZejvJYlOtHWdillPtEZZfcOAA.sXPoaVBoARfqsHrUgPkAMXtOQWThA(P_0, -21, ptr->DlMwOhdRqDmQcNHytuxlpNzzSZnG);
			}
		}
		else
		{
			instanceId = (uint)hUfdZejvJYlOtHWdillPtEZZfcOAA.LvjxsRoEpaQBxmxMmNxbVBlGLwBq(P_0, -21).ToInt32();
			flag = true;
		}
		if (flag && ObjectInstanceTracker.Default.TryGetInstance<GqttghHISFuaYcqSgRKFtSHPOuAx>(instanceId, out var instance))
		{
			instance.xIrIQUmPlsSeaOTUDXCBoasnEyYl(P_0, P_1, P_2, P_3);
		}
		return XCEeOpdNZXUgBsKznZZbtbwNBVsSA(P_0, P_1, P_2, P_3);
	}

	public void rmbOpUmccBJUqzdTIwkwVXsZokvv(oyRVEcgWKZSKUYOzHXwZOoYScKbK P_0)
	{
		xIrIQUmPlsSeaOTUDXCBoasnEyYl = P_0;
	}
}
