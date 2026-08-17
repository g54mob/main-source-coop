using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

internal class aNAOgsrbsDgohUMWDwFdewVSZBBb : UTTJNIeEjOgUBSkCqZFvGRXYKUTD<TsGzFMVAJHKqGDKZvhDuOjYnBmiI, jFYsosqqDXkIDTDejBpiddroaHtn>
{
	private static readonly List<ZzZAyJzDNoaZOFsixNkrNiGpMTdZ> sBzpfTHvSaHzegvejRpwAyKlokkH;

	[CompilerGenerated]
	private List<ZzZAyJzDNoaZOFsixNkrNiGpMTdZ> IlUNYZfXJtahOisEXycKJlGkfxjd;

	public List<ZzZAyJzDNoaZOFsixNkrNiGpMTdZ> imyWGTKcVPRzigLcCoccWEiaqvxb => sBzpfTHvSaHzegvejRpwAyKlokkH;

	public List<ZzZAyJzDNoaZOFsixNkrNiGpMTdZ> fnTXloYENVboyIyggzGlpdRYbJzQA
	{
		[CompilerGenerated]
		get
		{
			return IlUNYZfXJtahOisEXycKJlGkfxjd;
		}
		[CompilerGenerated]
		private set
		{
			IlUNYZfXJtahOisEXycKJlGkfxjd = ilUNYZfXJtahOisEXycKJlGkfxjd;
		}
	}

	static aNAOgsrbsDgohUMWDwFdewVSZBBb()
	{
		sBzpfTHvSaHzegvejRpwAyKlokkH = new List<ZzZAyJzDNoaZOFsixNkrNiGpMTdZ>(256);
		foreach (object value in Enum.GetValues(typeof(ZzZAyJzDNoaZOFsixNkrNiGpMTdZ)))
		{
			sBzpfTHvSaHzegvejRpwAyKlokkH.Add((ZzZAyJzDNoaZOFsixNkrNiGpMTdZ)value);
		}
	}

	public aNAOgsrbsDgohUMWDwFdewVSZBBb()
	{
		fnTXloYENVboyIyggzGlpdRYbJzQA = new List<ZzZAyJzDNoaZOFsixNkrNiGpMTdZ>(16);
	}

	public bool yonflaqUBWcBueggGjazJAkIbsSZA(ZzZAyJzDNoaZOFsixNkrNiGpMTdZ P_0)
	{
		return fnTXloYENVboyIyggzGlpdRYbJzQA.Contains(P_0);
	}

	public void Update(jFYsosqqDXkIDTDejBpiddroaHtn P_0)
	{
		if (P_0.oJJtkubmWunbIPecXeOGesqzbHoT != ZzZAyJzDNoaZOFsixNkrNiGpMTdZ.Unknown)
		{
			bool flag = yonflaqUBWcBueggGjazJAkIbsSZA(P_0.oJJtkubmWunbIPecXeOGesqzbHoT);
			if (P_0.yonflaqUBWcBueggGjazJAkIbsSZA && !flag)
			{
				fnTXloYENVboyIyggzGlpdRYbJzQA.Add(P_0.oJJtkubmWunbIPecXeOGesqzbHoT);
			}
			else if (P_0.HorBBHLvWYaJzYEmGdEhJpcTazstA && flag)
			{
				fnTXloYENVboyIyggzGlpdRYbJzQA.Remove(P_0.oJJtkubmWunbIPecXeOGesqzbHoT);
			}
		}
	}

	public unsafe void MarshalFrom(IntPtr P_0)
	{
		fnTXloYENVboyIyggzGlpdRYbJzQA.Clear();
		TsGzFMVAJHKqGDKZvhDuOjYnBmiI* ptr = (TsGzFMVAJHKqGDKZvhDuOjYnBmiI*)(void*)P_0;
		jFYsosqqDXkIDTDejBpiddroaHtn jFYsosqqDXkIDTDejBpiddroaHtn2 = default(jFYsosqqDXkIDTDejBpiddroaHtn);
		byte* ptr2 = &ptr->enOAkVuPujrMBAlhMfFtkVnFqJchb.aRusOMbqHHhlBGyNvRYRklEHQRREb;
		for (int i = 0; i < 256; i++)
		{
			jFYsosqqDXkIDTDejBpiddroaHtn2.VsddSgHdoGiQLVZLCiqGfqNdUgkd = i;
			jFYsosqqDXkIDTDejBpiddroaHtn2.lldCVvRkHRfVnMGBtiZlGoIQhngM = ptr2[i];
			if (jFYsosqqDXkIDTDejBpiddroaHtn2.yonflaqUBWcBueggGjazJAkIbsSZA)
			{
				fnTXloYENVboyIyggzGlpdRYbJzQA.Add(jFYsosqqDXkIDTDejBpiddroaHtn2.oJJtkubmWunbIPecXeOGesqzbHoT);
			}
		}
	}

	public virtual string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		return string.Format(CultureInfo.InvariantCulture, "PressedKeys: {0}", new object[1] { aOYtALpBiXtNUYUxrmqpvmqCyvKG.beeAFlkTibRtDifoecnQBvpZYYuV(",", fnTXloYENVboyIyggzGlpdRYbJzQA) });
	}
}
