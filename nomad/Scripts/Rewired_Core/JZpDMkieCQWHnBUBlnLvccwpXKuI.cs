using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

internal sealed class JZpDMkieCQWHnBUBlnLvccwpXKuI<_0001> where _0001 : class
{
	private const float jgkSzZZrtumTTbdChsLrwBVppLJM = 60f;

	private readonly IndexedDictionary<Bytes20, List<WeakReference>> XwFnMIidXFldTUcVMwdQbstiRboW;

	private float ZdyfLibzRYiJonyjHywjFpohUuZPA;

	private double VBjeLaJplEXWPSTbpnCVYsgJrLgS;

	private Func<_0001, _0001, bool> fbKNrNKyXHWPHCFQQHUKKNuRoQFP;

	public float wHBdZQXofuGjVellgIFNAzbzbogGb
	{
		get
		{
			return ZdyfLibzRYiJonyjHywjFpohUuZPA;
		}
		set
		{
			if (num < 0f)
			{
				num = 0f;
			}
			ZdyfLibzRYiJonyjHywjFpohUuZPA = num;
		}
	}

	public JZpDMkieCQWHnBUBlnLvccwpXKuI(Func<_0001, _0001, bool> P_0)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException();
		}
		fbKNrNKyXHWPHCFQQHUKKNuRoQFP = P_0;
		ZdyfLibzRYiJonyjHywjFpohUuZPA = 60f;
		XwFnMIidXFldTUcVMwdQbstiRboW = new IndexedDictionary<Bytes20, List<WeakReference>>();
		XwFnMIidXFldTUcVMwdQbstiRboW.KeyComparer = EqualityComparerNoAlloc<Bytes20>.Default;
	}

	public _0001 gOGhAlpaDWHngyOvcwqBdSVIJQot(Bytes20 P_0, _0001 P_1)
	{
		if (UCVjRYAbAROoXGysMPODoXVTfPnm(P_0, P_1, out var result))
		{
			return result;
		}
		jwZfZMJFGsXepvnbcdwNeIsRDvtv(P_0, P_1);
		return P_1;
	}

	public bool UCVjRYAbAROoXGysMPODoXVTfPnm(Bytes20 P_0, _0001 P_1, out _0001 P_2)
	{
		if (P_1 == null)
		{
			P_2 = null;
			return false;
		}
		jYshRXJlEfFhQmFjLYgOmKQtmfVx();
		if (!XwFnMIidXFldTUcVMwdQbstiRboW.TryGetValue(P_0, out var value))
		{
			P_2 = null;
			return false;
		}
		for (int num = value.Count - 1; num >= 0; num--)
		{
			if (!(value[num].Target is _0001 val))
			{
				value.RemoveAt(num);
			}
			else if (fbKNrNKyXHWPHCFQQHUKKNuRoQFP(P_1, val))
			{
				P_2 = val;
				return true;
			}
		}
		P_2 = null;
		return false;
	}

	public void jwZfZMJFGsXepvnbcdwNeIsRDvtv(Bytes20 P_0, _0001 P_1)
	{
		if (P_1 != null)
		{
			jYshRXJlEfFhQmFjLYgOmKQtmfVx();
			if (!XwFnMIidXFldTUcVMwdQbstiRboW.TryGetValue(P_0, out var value))
			{
				value = new List<WeakReference>();
				XwFnMIidXFldTUcVMwdQbstiRboW.Add(P_0, value);
			}
			value.Add(new WeakReference(P_1, trackResurrection: false));
		}
	}

	public void XjHFWzHrmxetKYnoxeOWJmnDDuCE()
	{
		for (int num = XwFnMIidXFldTUcVMwdQbstiRboW.Count - 1; num >= 0; num--)
		{
			List<WeakReference> list = XwFnMIidXFldTUcVMwdQbstiRboW[num];
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				if (!list[num2].IsAlive)
				{
					list.RemoveAt(num2);
				}
			}
			if (list.Count == 0)
			{
				XwFnMIidXFldTUcVMwdQbstiRboW.RemoveAt(num);
			}
		}
		VBjeLaJplEXWPSTbpnCVYsgJrLgS = ReInput.unscaledTime + (double)wHBdZQXofuGjVellgIFNAzbzbogGb;
	}

	private void jYshRXJlEfFhQmFjLYgOmKQtmfVx()
	{
		if (ZdyfLibzRYiJonyjHywjFpohUuZPA != 0f && !(ReInput.unscaledTime < VBjeLaJplEXWPSTbpnCVYsgJrLgS))
		{
			XjHFWzHrmxetKYnoxeOWJmnDDuCE();
		}
	}
}
