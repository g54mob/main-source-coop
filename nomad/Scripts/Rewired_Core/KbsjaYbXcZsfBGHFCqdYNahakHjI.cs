using System;
using System.Collections.Generic;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

internal class KbsjaYbXcZsfBGHFCqdYNahakHjI
{
	[Flags]
	public enum uWXGllMjsNGbjJrhzIJzclgBdBsXB
	{
		None = 0,
		Self = 1,
		Children = 2
	}

	private class YEnuAbbuEUUHGrtYWSdAVUEjvVzS
	{
		public bool EQihlbPwqHIlRxeLqzOJCalxIJKiA;

		public bool tcsGPNIzTxQwcOoadjmtbrqeGSiO;

		public bool TivccjGRDtvONMSUgkeozpphLDZr;
	}

	private Dictionary<int, YEnuAbbuEUUHGrtYWSdAVUEjvVzS> YubUcKEFidAtArLzpuuAtcKrAaFiA;

	public uWXGllMjsNGbjJrhzIJzclgBdBsXB YzdAUlNavWZXmJtnIoZdkjSRrBGv;

	private bool HbaOjuKeGMialoJfNftOdANSUzDtA => UnityTools.supportsUnityUIGraphicRaycastTarget;

	public KbsjaYbXcZsfBGHFCqdYNahakHjI()
		: this(uWXGllMjsNGbjJrhzIJzclgBdBsXB.Self | uWXGllMjsNGbjJrhzIJzclgBdBsXB.Children)
	{
	}

	public KbsjaYbXcZsfBGHFCqdYNahakHjI(uWXGllMjsNGbjJrhzIJzclgBdBsXB P_0)
	{
		YzdAUlNavWZXmJtnIoZdkjSRrBGv = P_0;
		YubUcKEFidAtArLzpuuAtcKrAaFiA = new Dictionary<int, YEnuAbbuEUUHGrtYWSdAVUEjvVzS>();
	}

	public void xdyFfgAnFtvUqduRTeXdrPcIqmrJA(Transform P_0, bool P_1)
	{
		if (!HbaOjuKeGMialoJfNftOdANSUzDtA)
		{
			return;
		}
		if ((YzdAUlNavWZXmJtnIoZdkjSRrBGv & uWXGllMjsNGbjJrhzIJzclgBdBsXB.Self) != uWXGllMjsNGbjJrhzIJzclgBdBsXB.None)
		{
			if ((YzdAUlNavWZXmJtnIoZdkjSRrBGv & uWXGllMjsNGbjJrhzIJzclgBdBsXB.Children) != uWXGllMjsNGbjJrhzIJzclgBdBsXB.None)
			{
				VirpjLEtIwBPTPkRfaBAabcWBGdh(P_0, P_1, YubUcKEFidAtArLzpuuAtcKrAaFiA);
			}
			else
			{
				fsFbbLDvONcfmTFuyNMIjNpFgnhk(P_0, P_1, YubUcKEFidAtArLzpuuAtcKrAaFiA);
			}
		}
		else if ((YzdAUlNavWZXmJtnIoZdkjSRrBGv & uWXGllMjsNGbjJrhzIJzclgBdBsXB.Children) != uWXGllMjsNGbjJrhzIJzclgBdBsXB.None)
		{
			lDiqlhyNFValldmvPKvMPVSDkdqO(P_0, P_1, YubUcKEFidAtArLzpuuAtcKrAaFiA);
		}
	}

	public void WnkVoLeLcDIxHhUkiCBDfHgIYoxub()
	{
		if (HbaOjuKeGMialoJfNftOdANSUzDtA)
		{
			YubUcKEFidAtArLzpuuAtcKrAaFiA.Clear();
		}
	}

	private static void VirpjLEtIwBPTPkRfaBAabcWBGdh(Transform P_0, bool P_1, Dictionary<int, YEnuAbbuEUUHGrtYWSdAVUEjvVzS> P_2)
	{
		if (!(P_0 == null))
		{
			fsFbbLDvONcfmTFuyNMIjNpFgnhk(P_0, P_1, P_2);
			lDiqlhyNFValldmvPKvMPVSDkdqO(P_0, P_1, P_2);
		}
	}

	private static void lDiqlhyNFValldmvPKvMPVSDkdqO(Transform P_0, bool P_1, Dictionary<int, YEnuAbbuEUUHGrtYWSdAVUEjvVzS> P_2)
	{
		if (!(P_0 == null))
		{
			int childCount = P_0.childCount;
			for (int i = 0; i < childCount; i++)
			{
				VirpjLEtIwBPTPkRfaBAabcWBGdh(P_0.GetChild(i), P_1, P_2);
			}
		}
	}

	private static void fsFbbLDvONcfmTFuyNMIjNpFgnhk(Transform P_0, bool P_1, Dictionary<int, YEnuAbbuEUUHGrtYWSdAVUEjvVzS> P_2)
	{
		if (P_0 == null)
		{
			return;
		}
		Graphic component = P_0.GetComponent<Graphic>();
		if (component == null)
		{
			return;
		}
		bool flag = UnityTools.externalTools.UnityUI_Graphic_GetRaycastTarget(component);
		int instanceID = component.GetInstanceID();
		if (!P_2.TryGetValue(instanceID, out var value))
		{
			if (!flag)
			{
				return;
			}
			value = new YEnuAbbuEUUHGrtYWSdAVUEjvVzS();
			value.EQihlbPwqHIlRxeLqzOJCalxIJKiA = flag;
			P_2.Add(instanceID, value);
		}
		if ((value.tcsGPNIzTxQwcOoadjmtbrqeGSiO && flag == value.EQihlbPwqHIlRxeLqzOJCalxIJKiA) || (!value.tcsGPNIzTxQwcOoadjmtbrqeGSiO && flag != value.EQihlbPwqHIlRxeLqzOJCalxIJKiA))
		{
			value.tcsGPNIzTxQwcOoadjmtbrqeGSiO = false;
			value.TivccjGRDtvONMSUgkeozpphLDZr = false;
			value.EQihlbPwqHIlRxeLqzOJCalxIJKiA = flag;
			if (!flag)
			{
				P_2.Remove(instanceID);
				return;
			}
		}
		if (P_1 != flag && value.EQihlbPwqHIlRxeLqzOJCalxIJKiA)
		{
			if (value.EQihlbPwqHIlRxeLqzOJCalxIJKiA == P_1)
			{
				value.tcsGPNIzTxQwcOoadjmtbrqeGSiO = false;
				value.TivccjGRDtvONMSUgkeozpphLDZr = false;
			}
			else
			{
				value.tcsGPNIzTxQwcOoadjmtbrqeGSiO = true;
				value.TivccjGRDtvONMSUgkeozpphLDZr = P_1;
			}
			UnityTools.externalTools.UnityUI_Graphic_SetRaycastTarget(component, P_1);
		}
	}
}
