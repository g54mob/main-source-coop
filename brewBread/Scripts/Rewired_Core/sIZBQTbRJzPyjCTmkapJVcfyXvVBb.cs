using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Rewired;

[DefaultMember("Item")]
internal struct sIZBQTbRJzPyjCTmkapJVcfyXvVBb : IEquatable<sIZBQTbRJzPyjCTmkapJVcfyXvVBb>
{
	public ModifierKey uTQDUHBOiryPBqHLLXEKKjbRBBus;

	public ModifierKey BpuVLSKErRUyEXnNoenFvstYfATV;

	public ModifierKey pmQKuWfkHtqDXVkQqroayukudkkH;

	private ModifierKey xKHWVyyMFvFkZzVdcahSgmCfFoFGb
	{
		get
		{
			if (P_0 <= 0)
			{
				return uTQDUHBOiryPBqHLLXEKKjbRBBus;
			}
			if (P_0 == 1)
			{
				return BpuVLSKErRUyEXnNoenFvstYfATV;
			}
			if (P_0 >= 2)
			{
				return pmQKuWfkHtqDXVkQqroayukudkkH;
			}
			return uTQDUHBOiryPBqHLLXEKKjbRBBus;
		}
		set
		{
			if (num <= 0)
			{
				uTQDUHBOiryPBqHLLXEKKjbRBBus = bpuVLSKErRUyEXnNoenFvstYfATV;
			}
			if (num == 1)
			{
				BpuVLSKErRUyEXnNoenFvstYfATV = bpuVLSKErRUyEXnNoenFvstYfATV;
			}
			if (num >= 2)
			{
				pmQKuWfkHtqDXVkQqroayukudkkH = bpuVLSKErRUyEXnNoenFvstYfATV;
			}
		}
	}

	public sIZBQTbRJzPyjCTmkapJVcfyXvVBb(ModifierKey P_0, ModifierKey P_1, ModifierKey P_2)
	{
		uTQDUHBOiryPBqHLLXEKKjbRBBus = P_0;
		BpuVLSKErRUyEXnNoenFvstYfATV = P_1;
		pmQKuWfkHtqDXVkQqroayukudkkH = P_2;
	}

	public void SPGTRPyvIslcMdbPTItsewSLRPxx()
	{
		if (uTQDUHBOiryPBqHLLXEKKjbRBBus != ModifierKey.None)
		{
			uTQDUHBOiryPBqHLLXEKKjbRBBus = ModifierKey.None;
		}
		if (BpuVLSKErRUyEXnNoenFvstYfATV != ModifierKey.None)
		{
			BpuVLSKErRUyEXnNoenFvstYfATV = ModifierKey.None;
		}
		if (pmQKuWfkHtqDXVkQqroayukudkkH != ModifierKey.None)
		{
			pmQKuWfkHtqDXVkQqroayukudkkH = ModifierKey.None;
		}
	}

	public static sIZBQTbRJzPyjCTmkapJVcfyXvVBb IsSgiyIILrCIJkNwCUzKaexlZlhV(ModifierKeyFlags P_0)
	{
		sIZBQTbRJzPyjCTmkapJVcfyXvVBb result = default(sIZBQTbRJzPyjCTmkapJVcfyXvVBb);
		int num = 0;
		if (Keyboard.ModifierKeyFlagsContain(P_0, ModifierKey.Control))
		{
			result.DbTdhyBCZzPmwrwbRmklmOASylcc(num++, ModifierKey.Control);
		}
		if (Keyboard.ModifierKeyFlagsContain(P_0, ModifierKey.Command))
		{
			result.DbTdhyBCZzPmwrwbRmklmOASylcc(num++, ModifierKey.Command);
		}
		if (Keyboard.ModifierKeyFlagsContain(P_0, ModifierKey.Alt))
		{
			result.DbTdhyBCZzPmwrwbRmklmOASylcc(num++, ModifierKey.Alt);
		}
		if (num >= 3)
		{
			return result;
		}
		if (Keyboard.ModifierKeyFlagsContain(P_0, ModifierKey.Shift))
		{
			result.DbTdhyBCZzPmwrwbRmklmOASylcc(num++, ModifierKey.Shift);
		}
		return result;
	}

	public bool Equals(sIZBQTbRJzPyjCTmkapJVcfyXvVBb other)
	{
		if (uTQDUHBOiryPBqHLLXEKKjbRBBus == other.uTQDUHBOiryPBqHLLXEKKjbRBBus && BpuVLSKErRUyEXnNoenFvstYfATV == other.BpuVLSKErRUyEXnNoenFvstYfATV)
		{
			return pmQKuWfkHtqDXVkQqroayukudkkH == other.pmQKuWfkHtqDXVkQqroayukudkkH;
		}
		return false;
	}

	public bool IvmEuMiWjofpBMhUdDMogdquDHke(object P_0)
	{
		if (P_0 == null || !(P_0 is sIZBQTbRJzPyjCTmkapJVcfyXvVBb))
		{
			return false;
		}
		return Equals((sIZBQTbRJzPyjCTmkapJVcfyXvVBb)P_0);
	}

	public int gRxrPIbeYAPURsPpfeVTHdqQzmVFA()
	{
		return ((17 * 29 + uTQDUHBOiryPBqHLLXEKKjbRBBus.GetHashCode()) * 29 + BpuVLSKErRUyEXnNoenFvstYfATV.GetHashCode()) * 29 + pmQKuWfkHtqDXVkQqroayukudkkH.GetHashCode();
	}

	[SpecialName]
	public static bool LIAzMZCoeYXHaSTBwADWuJxIpNnV(sIZBQTbRJzPyjCTmkapJVcfyXvVBb P_0, sIZBQTbRJzPyjCTmkapJVcfyXvVBb P_1)
	{
		if (P_0.uTQDUHBOiryPBqHLLXEKKjbRBBus == P_1.uTQDUHBOiryPBqHLLXEKKjbRBBus && P_0.BpuVLSKErRUyEXnNoenFvstYfATV == P_1.BpuVLSKErRUyEXnNoenFvstYfATV)
		{
			return P_0.pmQKuWfkHtqDXVkQqroayukudkkH == P_1.pmQKuWfkHtqDXVkQqroayukudkkH;
		}
		return false;
	}

	[SpecialName]
	public static bool pVsQmQHIrOcxAeSyBdXPdNwLYtOe(sIZBQTbRJzPyjCTmkapJVcfyXvVBb P_0, sIZBQTbRJzPyjCTmkapJVcfyXvVBb P_1)
	{
		return !LIAzMZCoeYXHaSTBwADWuJxIpNnV(P_0, P_1);
	}
}
