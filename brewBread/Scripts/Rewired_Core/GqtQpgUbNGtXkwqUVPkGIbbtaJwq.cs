using System;
using System.Runtime.CompilerServices;
using Rewired;

internal struct GqtQpgUbNGtXkwqUVPkGIbbtaJwq : IEquatable<GqtQpgUbNGtXkwqUVPkGIbbtaJwq>
{
	public KeyboardKeyCode ZUEGwJAICpdPILQnNmgcQErtxBVoA;

	public ModifierKey uTQDUHBOiryPBqHLLXEKKjbRBBus;

	public ModifierKey BpuVLSKErRUyEXnNoenFvstYfATV;

	public ModifierKey pmQKuWfkHtqDXVkQqroayukudkkH;

	public GqtQpgUbNGtXkwqUVPkGIbbtaJwq(KeyboardKeyCode P_0, ModifierKey P_1, ModifierKey P_2, ModifierKey P_3)
	{
		ZUEGwJAICpdPILQnNmgcQErtxBVoA = P_0;
		uTQDUHBOiryPBqHLLXEKKjbRBBus = P_1;
		BpuVLSKErRUyEXnNoenFvstYfATV = P_2;
		pmQKuWfkHtqDXVkQqroayukudkkH = P_3;
	}

	public void SPGTRPyvIslcMdbPTItsewSLRPxx()
	{
		if (ZUEGwJAICpdPILQnNmgcQErtxBVoA != KeyboardKeyCode.None)
		{
			ZUEGwJAICpdPILQnNmgcQErtxBVoA = KeyboardKeyCode.None;
		}
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

	public bool Equals(GqtQpgUbNGtXkwqUVPkGIbbtaJwq other)
	{
		if (ZUEGwJAICpdPILQnNmgcQErtxBVoA == other.ZUEGwJAICpdPILQnNmgcQErtxBVoA && uTQDUHBOiryPBqHLLXEKKjbRBBus == other.uTQDUHBOiryPBqHLLXEKKjbRBBus && BpuVLSKErRUyEXnNoenFvstYfATV == other.BpuVLSKErRUyEXnNoenFvstYfATV)
		{
			return pmQKuWfkHtqDXVkQqroayukudkkH == other.pmQKuWfkHtqDXVkQqroayukudkkH;
		}
		return false;
	}

	public bool IvmEuMiWjofpBMhUdDMogdquDHke(object P_0)
	{
		if (P_0 == null || !(P_0 is GqtQpgUbNGtXkwqUVPkGIbbtaJwq))
		{
			return false;
		}
		return Equals((GqtQpgUbNGtXkwqUVPkGIbbtaJwq)P_0);
	}

	public int gRxrPIbeYAPURsPpfeVTHdqQzmVFA()
	{
		return (((17 * 29 + ZUEGwJAICpdPILQnNmgcQErtxBVoA.GetHashCode()) * 29 + uTQDUHBOiryPBqHLLXEKKjbRBBus.GetHashCode()) * 29 + BpuVLSKErRUyEXnNoenFvstYfATV.GetHashCode()) * 29 + pmQKuWfkHtqDXVkQqroayukudkkH.GetHashCode();
	}

	[SpecialName]
	public static bool LIAzMZCoeYXHaSTBwADWuJxIpNnV(GqtQpgUbNGtXkwqUVPkGIbbtaJwq P_0, GqtQpgUbNGtXkwqUVPkGIbbtaJwq P_1)
	{
		if (P_0.ZUEGwJAICpdPILQnNmgcQErtxBVoA == P_1.ZUEGwJAICpdPILQnNmgcQErtxBVoA && P_0.uTQDUHBOiryPBqHLLXEKKjbRBBus == P_1.uTQDUHBOiryPBqHLLXEKKjbRBBus && P_0.BpuVLSKErRUyEXnNoenFvstYfATV == P_1.BpuVLSKErRUyEXnNoenFvstYfATV)
		{
			return P_0.pmQKuWfkHtqDXVkQqroayukudkkH == P_1.pmQKuWfkHtqDXVkQqroayukudkkH;
		}
		return false;
	}

	[SpecialName]
	public static bool pVsQmQHIrOcxAeSyBdXPdNwLYtOe(GqtQpgUbNGtXkwqUVPkGIbbtaJwq P_0, GqtQpgUbNGtXkwqUVPkGIbbtaJwq P_1)
	{
		return !LIAzMZCoeYXHaSTBwADWuJxIpNnV(P_0, P_1);
	}
}
