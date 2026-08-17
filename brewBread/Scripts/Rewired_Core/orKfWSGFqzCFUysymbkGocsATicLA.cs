using System;
using System.Collections.Generic;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

internal class orKfWSGFqzCFUysymbkGocsATicLA
{
	[Flags]
	public enum VoxcJRiMnCjlqEzftKtXfoudojNYA
	{
		None = 0,
		Self = 1,
		Children = 2
	}

	private class XHejpVnSbfUpuyDqoBlKSyRMokeN
	{
		public bool LlevBQcpSujocBrvshNwYerLCJUCA;

		public bool EfrikaWzhFnFburDTATKSVZaIsQl;

		public bool WarGBpLIhEHKhmAaiDPvZhpgiUVC;
	}

	private Dictionary<int, XHejpVnSbfUpuyDqoBlKSyRMokeN> DIbgSUVYdBBKHdAvDaAQrXWAEbkb;

	public VoxcJRiMnCjlqEzftKtXfoudojNYA gddVMmbdxmTUXKFepLAnbDhQjuUL;

	private bool HtaXGKSwgRsWaFcEpcKcAcuoUpZnA => UnityTools.supportsUnityUIGraphicRaycastTarget;

	public orKfWSGFqzCFUysymbkGocsATicLA()
		: this(VoxcJRiMnCjlqEzftKtXfoudojNYA.Self | VoxcJRiMnCjlqEzftKtXfoudojNYA.Children)
	{
	}

	public orKfWSGFqzCFUysymbkGocsATicLA(VoxcJRiMnCjlqEzftKtXfoudojNYA P_0)
	{
		gddVMmbdxmTUXKFepLAnbDhQjuUL = P_0;
		DIbgSUVYdBBKHdAvDaAQrXWAEbkb = new Dictionary<int, XHejpVnSbfUpuyDqoBlKSyRMokeN>();
	}

	public void mKNIhyiGRhrEXKxQDuaJFRiTMRVl(Transform P_0, bool P_1)
	{
		if (!HtaXGKSwgRsWaFcEpcKcAcuoUpZnA)
		{
			return;
		}
		if ((gddVMmbdxmTUXKFepLAnbDhQjuUL & VoxcJRiMnCjlqEzftKtXfoudojNYA.Self) != VoxcJRiMnCjlqEzftKtXfoudojNYA.None)
		{
			if ((gddVMmbdxmTUXKFepLAnbDhQjuUL & VoxcJRiMnCjlqEzftKtXfoudojNYA.Children) != VoxcJRiMnCjlqEzftKtXfoudojNYA.None)
			{
				KtnXeSpAhWucwrSEXJRoWSbrTaGb(P_0, P_1, DIbgSUVYdBBKHdAvDaAQrXWAEbkb);
			}
			else
			{
				mKNIhyiGRhrEXKxQDuaJFRiTMRVl(P_0, P_1, DIbgSUVYdBBKHdAvDaAQrXWAEbkb);
			}
		}
		else if ((gddVMmbdxmTUXKFepLAnbDhQjuUL & VoxcJRiMnCjlqEzftKtXfoudojNYA.Children) != VoxcJRiMnCjlqEzftKtXfoudojNYA.None)
		{
			IeBxGwaYJvJeFMonfvRJbClTGCfb(P_0, P_1, DIbgSUVYdBBKHdAvDaAQrXWAEbkb);
		}
	}

	public void SPGTRPyvIslcMdbPTItsewSLRPxx()
	{
		if (HtaXGKSwgRsWaFcEpcKcAcuoUpZnA)
		{
			DIbgSUVYdBBKHdAvDaAQrXWAEbkb.Clear();
		}
	}

	private static void KtnXeSpAhWucwrSEXJRoWSbrTaGb(Transform P_0, bool P_1, Dictionary<int, XHejpVnSbfUpuyDqoBlKSyRMokeN> P_2)
	{
		if (!(P_0 == null))
		{
			mKNIhyiGRhrEXKxQDuaJFRiTMRVl(P_0, P_1, P_2);
			IeBxGwaYJvJeFMonfvRJbClTGCfb(P_0, P_1, P_2);
		}
	}

	private static void IeBxGwaYJvJeFMonfvRJbClTGCfb(Transform P_0, bool P_1, Dictionary<int, XHejpVnSbfUpuyDqoBlKSyRMokeN> P_2)
	{
		if (!(P_0 == null))
		{
			int childCount = P_0.childCount;
			for (int i = 0; i < childCount; i++)
			{
				KtnXeSpAhWucwrSEXJRoWSbrTaGb(P_0.GetChild(i), P_1, P_2);
			}
		}
	}

	private static void mKNIhyiGRhrEXKxQDuaJFRiTMRVl(Transform P_0, bool P_1, Dictionary<int, XHejpVnSbfUpuyDqoBlKSyRMokeN> P_2)
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
			value = new XHejpVnSbfUpuyDqoBlKSyRMokeN();
			value.LlevBQcpSujocBrvshNwYerLCJUCA = flag;
			P_2.Add(instanceID, value);
		}
		if ((value.EfrikaWzhFnFburDTATKSVZaIsQl && flag == value.LlevBQcpSujocBrvshNwYerLCJUCA) || (!value.EfrikaWzhFnFburDTATKSVZaIsQl && flag != value.LlevBQcpSujocBrvshNwYerLCJUCA))
		{
			value.EfrikaWzhFnFburDTATKSVZaIsQl = false;
			value.WarGBpLIhEHKhmAaiDPvZhpgiUVC = false;
			value.LlevBQcpSujocBrvshNwYerLCJUCA = flag;
			if (!flag)
			{
				P_2.Remove(instanceID);
				return;
			}
		}
		if (P_1 != flag && value.LlevBQcpSujocBrvshNwYerLCJUCA)
		{
			if (value.LlevBQcpSujocBrvshNwYerLCJUCA == P_1)
			{
				value.EfrikaWzhFnFburDTATKSVZaIsQl = false;
				value.WarGBpLIhEHKhmAaiDPvZhpgiUVC = false;
			}
			else
			{
				value.EfrikaWzhFnFburDTATKSVZaIsQl = true;
				value.WarGBpLIhEHKhmAaiDPvZhpgiUVC = P_1;
			}
			UnityTools.externalTools.UnityUI_Graphic_SetRaycastTarget(component, P_1);
		}
	}
}
