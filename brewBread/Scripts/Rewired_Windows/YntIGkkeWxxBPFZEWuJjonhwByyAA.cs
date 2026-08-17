using System.Collections.Generic;
using Rewired.Utils;

internal class YntIGkkeWxxBPFZEWuJjonhwByyAA : TuhurQJVxqUSFhIAeAwMMcfbsgih
{
	private List<KrMUAsyffeCsEDyEiBXwBxbsmhaHb> aImuJHDevSvBqBWwEmrbWhsCgBqy;

	private KrMUAsyffeCsEDyEiBXwBxbsmhaHb[] zlJTbPIiPhzxDnxKUnogiFIEzcKW;

	private bool yVZLOspvZOVRocKUGWRryuzSUkei;

	public YntIGkkeWxxBPFZEWuJjonhwByyAA()
	{
		aImuJHDevSvBqBWwEmrbWhsCgBqy = new List<KrMUAsyffeCsEDyEiBXwBxbsmhaHb>();
	}

	public override void DSbMYqAOYokYmQApZPoPIubEREq(KrMUAsyffeCsEDyEiBXwBxbsmhaHb P_0)
	{
		aImuJHDevSvBqBWwEmrbWhsCgBqy.Add(P_0);
	}

	public float mPKaIHfzhogaSXQJCwZlMPTETFwmA(int P_0)
	{
		if (P_0 < 0 || P_0 >= zlJTbPIiPhzxDnxKUnogiFIEzcKW.Length)
		{
			return 0f;
		}
		return FdAIjbgcfatFrghHnIipFWDBvhqyA(zlJTbPIiPhzxDnxKUnogiFIEzcKW[P_0].lldCVvRkHRfVnMGBtiZlGoIQhngM);
	}

	public int EYfGGXiEAfJbAhSFicTjkSxKfbvgA(int P_0)
	{
		if (P_0 < 0 || P_0 >= zlJTbPIiPhzxDnxKUnogiFIEzcKW.Length)
		{
			return 0;
		}
		return (int)zlJTbPIiPhzxDnxKUnogiFIEzcKW[P_0].AnebwuHTCIhUVphVZsCAuxNDYiqd;
	}

	public override void rTHCJLRzwrGcUgfZvFujrHdcCQwh()
	{
		if (!yVZLOspvZOVRocKUGWRryuzSUkei)
		{
			yVZLOspvZOVRocKUGWRryuzSUkei = true;
			zlJTbPIiPhzxDnxKUnogiFIEzcKW = aImuJHDevSvBqBWwEmrbWhsCgBqy.ToArray();
			aImuJHDevSvBqBWwEmrbWhsCgBqy = null;
		}
	}

	private float FdAIjbgcfatFrghHnIipFWDBvhqyA(int P_0)
	{
		if (P_0 == 0)
		{
			return 0f;
		}
		return MathTools.Clamp((float)MathTools.Abs(P_0) / 65535f * (float)MathTools.Sign(P_0), -1f, 1f);
	}
}
