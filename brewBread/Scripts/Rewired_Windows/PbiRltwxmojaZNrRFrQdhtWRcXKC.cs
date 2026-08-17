using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
[DefaultMember("Item")]
internal struct PbiRltwxmojaZNrRFrQdhtWRcXKC : IEquatable<PbiRltwxmojaZNrRFrQdhtWRcXKC>, IFormattable
{
	public static readonly int rYJCyXWikEyjUHXgSdyVqiubbGng = Marshal.SizeOf(typeof(PbiRltwxmojaZNrRFrQdhtWRcXKC));

	public static readonly PbiRltwxmojaZNrRFrQdhtWRcXKC VlWNMKqECXRzdRysMFqtzPIFTroe = default(PbiRltwxmojaZNrRFrQdhtWRcXKC);

	public static readonly PbiRltwxmojaZNrRFrQdhtWRcXKC aUVrZDnqQIuVzXJPViEvbzydDZlB = new PbiRltwxmojaZNrRFrQdhtWRcXKC(1f, 0f);

	public static readonly PbiRltwxmojaZNrRFrQdhtWRcXKC FMTMwdIyyYdhNYzufRnOmnCJeqafA = new PbiRltwxmojaZNrRFrQdhtWRcXKC(0f, 1f);

	public static readonly PbiRltwxmojaZNrRFrQdhtWRcXKC vWVwpUOaGBskOrFwxHHXWGwUBuqGA = new PbiRltwxmojaZNrRFrQdhtWRcXKC(1f, 1f);

	public float LeaAlLlLUVpUZEtsNQKSfcVaXRbL;

	public float nYgMBIIMubbuyaBAFyuFsqYaYOAtA;

	public bool TRyaTYRpYidpmUHOlsUNNcJWannj => dwFuEKaDLqkdeMSEflaKjgVWFsmj.hoKpMLOIJupuaMTotLOSqPDxnVRh(LeaAlLlLUVpUZEtsNQKSfcVaXRbL * LeaAlLlLUVpUZEtsNQKSfcVaXRbL + nYgMBIIMubbuyaBAFyuFsqYaYOAtA * nYgMBIIMubbuyaBAFyuFsqYaYOAtA);

	public bool flfwjkDCSbRtLTlfYMBgrBMebuOl
	{
		get
		{
			if (LeaAlLlLUVpUZEtsNQKSfcVaXRbL == 0f)
			{
				return nYgMBIIMubbuyaBAFyuFsqYaYOAtA == 0f;
			}
			return false;
		}
	}

	public float muarktpyJSWZctSRxmEoxarAcZUv
	{
		get
		{
			return P_0 switch
			{
				0 => LeaAlLlLUVpUZEtsNQKSfcVaXRbL, 
				1 => nYgMBIIMubbuyaBAFyuFsqYaYOAtA, 
				_ => throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive."), 
			};
		}
		set
		{
			switch (num)
			{
			case 0:
				LeaAlLlLUVpUZEtsNQKSfcVaXRbL = leaAlLlLUVpUZEtsNQKSfcVaXRbL;
				break;
			case 1:
				nYgMBIIMubbuyaBAFyuFsqYaYOAtA = leaAlLlLUVpUZEtsNQKSfcVaXRbL;
				break;
			default:
				throw new ArgumentOutOfRangeException("index", "Indices for Vector2 run from 0 to 1, inclusive.");
			}
		}
	}

	public PbiRltwxmojaZNrRFrQdhtWRcXKC(float P_0)
	{
		LeaAlLlLUVpUZEtsNQKSfcVaXRbL = P_0;
		nYgMBIIMubbuyaBAFyuFsqYaYOAtA = P_0;
	}

	public PbiRltwxmojaZNrRFrQdhtWRcXKC(float P_0, float P_1)
	{
		LeaAlLlLUVpUZEtsNQKSfcVaXRbL = P_0;
		nYgMBIIMubbuyaBAFyuFsqYaYOAtA = P_1;
	}

	public PbiRltwxmojaZNrRFrQdhtWRcXKC(float[] P_0)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("values");
		}
		if (P_0.Length != 2)
		{
			throw new ArgumentOutOfRangeException("values", "There must be two and only two input values for Vector2.");
		}
		LeaAlLlLUVpUZEtsNQKSfcVaXRbL = P_0[0];
		nYgMBIIMubbuyaBAFyuFsqYaYOAtA = P_0[1];
	}

	public float oRjNdVSumbQrnPpjzCOLcZisplBNA()
	{
		return (float)Math.Sqrt(LeaAlLlLUVpUZEtsNQKSfcVaXRbL * LeaAlLlLUVpUZEtsNQKSfcVaXRbL + nYgMBIIMubbuyaBAFyuFsqYaYOAtA * nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public float kpwzhVoaUuIHpwlsifNSDvUColXnA()
	{
		return LeaAlLlLUVpUZEtsNQKSfcVaXRbL * LeaAlLlLUVpUZEtsNQKSfcVaXRbL + nYgMBIIMubbuyaBAFyuFsqYaYOAtA * nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
	}

	public void sjkdpdcfjNwSIFEMyHGtWWQrHwlnA()
	{
		float num = oRjNdVSumbQrnPpjzCOLcZisplBNA();
		if (!dwFuEKaDLqkdeMSEflaKjgVWFsmj.flfwjkDCSbRtLTlfYMBgrBMebuOl(num))
		{
			float num2 = 1f / num;
			LeaAlLlLUVpUZEtsNQKSfcVaXRbL *= num2;
			nYgMBIIMubbuyaBAFyuFsqYaYOAtA *= num2;
		}
	}

	public float[] RMlsqwloyMhysVSDSJhffPMMmlgm()
	{
		return new float[2] { LeaAlLlLUVpUZEtsNQKSfcVaXRbL, nYgMBIIMubbuyaBAFyuFsqYaYOAtA };
	}

	public static void SEoDdidVANZapFqnoMQIWbCXqlvLA(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		P_2 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC SEoDdidVANZapFqnoMQIWbCXqlvLA(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static void SEoDdidVANZapFqnoMQIWbCXqlvLA(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref float P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		P_2 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + P_1);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC SEoDdidVANZapFqnoMQIWbCXqlvLA(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, float P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + P_1);
	}

	public static void bpuYPVPuOpleIsKXugBOPXjRorhg(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		P_2 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC bpuYPVPuOpleIsKXugBOPXjRorhg(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static void bpuYPVPuOpleIsKXugBOPXjRorhg(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref float P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		P_2 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_1);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC bpuYPVPuOpleIsKXugBOPXjRorhg(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, float P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_1);
	}

	public static void bpuYPVPuOpleIsKXugBOPXjRorhg(ref float P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		P_2 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0 - P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0 - P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC bpuYPVPuOpleIsKXugBOPXjRorhg(float P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0 - P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0 - P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static void RLOJSbqkLfORpihCsCybiCnAqwiL(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, float P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		P_2 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * P_1);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC RLOJSbqkLfORpihCsCybiCnAqwiL(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, float P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * P_1);
	}

	public static void RLOJSbqkLfORpihCsCybiCnAqwiL(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		P_2 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC RLOJSbqkLfORpihCsCybiCnAqwiL(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static void HTtItiuSvTGIGdHgyeHsFkUhPfPrA(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, float P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		P_2 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL / P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA / P_1);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC HTtItiuSvTGIGdHgyeHsFkUhPfPrA(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, float P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL / P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA / P_1);
	}

	public static void HTtItiuSvTGIGdHgyeHsFkUhPfPrA(float P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		P_2 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0 / P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0 / P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC HTtItiuSvTGIGdHgyeHsFkUhPfPrA(float P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0 / P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0 / P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static void qOaIocDeOQqVWSrRPPSGxZNRCmRs(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		P_1 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(0f - P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, 0f - P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC qOaIocDeOQqVWSrRPPSGxZNRCmRs(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(0f - P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, 0f - P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static void SYinGnaruvHBjzKqzmsJSXrGQszS(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_2, float P_3, float P_4, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_5)
	{
		P_5 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_3 * (P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL) + P_4 * (P_2.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL), P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + P_3 * (P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA) + P_4 * (P_2.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA));
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC SYinGnaruvHBjzKqzmsJSXrGQszS(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, PbiRltwxmojaZNrRFrQdhtWRcXKC P_2, float P_3, float P_4)
	{
		SYinGnaruvHBjzKqzmsJSXrGQszS(ref P_0, ref P_1, ref P_2, P_3, P_4, out var result);
		return result;
	}

	public static void GWfixTOMXeWqvsnpqSaBaVgtnvGj(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_2, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_3)
	{
		float leaAlLlLUVpUZEtsNQKSfcVaXRbL = P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL;
		leaAlLlLUVpUZEtsNQKSfcVaXRbL = ((leaAlLlLUVpUZEtsNQKSfcVaXRbL > P_2.LeaAlLlLUVpUZEtsNQKSfcVaXRbL) ? P_2.LeaAlLlLUVpUZEtsNQKSfcVaXRbL : leaAlLlLUVpUZEtsNQKSfcVaXRbL);
		leaAlLlLUVpUZEtsNQKSfcVaXRbL = ((leaAlLlLUVpUZEtsNQKSfcVaXRbL < P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL) ? P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL : leaAlLlLUVpUZEtsNQKSfcVaXRbL);
		float num = P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
		num = ((num > P_2.nYgMBIIMubbuyaBAFyuFsqYaYOAtA) ? P_2.nYgMBIIMubbuyaBAFyuFsqYaYOAtA : num);
		num = ((num < P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA) ? P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA : num);
		P_3 = new PbiRltwxmojaZNrRFrQdhtWRcXKC(leaAlLlLUVpUZEtsNQKSfcVaXRbL, num);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC GWfixTOMXeWqvsnpqSaBaVgtnvGj(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		GWfixTOMXeWqvsnpqSaBaVgtnvGj(ref P_0, ref P_1, ref P_2, out var result);
		return result;
	}

	public void EcfyYAzCmDYNpYIUruwDrIgvQMEE()
	{
		LeaAlLlLUVpUZEtsNQKSfcVaXRbL = ((LeaAlLlLUVpUZEtsNQKSfcVaXRbL < 0f) ? 0f : ((LeaAlLlLUVpUZEtsNQKSfcVaXRbL > 1f) ? 1f : LeaAlLlLUVpUZEtsNQKSfcVaXRbL));
		nYgMBIIMubbuyaBAFyuFsqYaYOAtA = ((nYgMBIIMubbuyaBAFyuFsqYaYOAtA < 0f) ? 0f : ((nYgMBIIMubbuyaBAFyuFsqYaYOAtA > 1f) ? 1f : nYgMBIIMubbuyaBAFyuFsqYaYOAtA));
	}

	public static void TcGxGUXlYUeZsjNvkeJVAELHRIrZ(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, out float P_2)
	{
		float num = P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL;
		float num2 = P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
		P_2 = (float)Math.Sqrt(num * num + num2 * num2);
	}

	public static float TcGxGUXlYUeZsjNvkeJVAELHRIrZ(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		float num = P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL;
		float num2 = P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
		return (float)Math.Sqrt(num * num + num2 * num2);
	}

	public static void uoXRlSZpRLcalBwMLvsXtLDYNfcBb(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, out float P_2)
	{
		float num = P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL;
		float num2 = P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
		P_2 = num * num + num2 * num2;
	}

	public static float uoXRlSZpRLcalBwMLvsXtLDYNfcBb(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		float num = P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL;
		float num2 = P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
		return num * num + num2 * num2;
	}

	public static void oKlwNxFrNKhjvpMJqbeZkelNXLPpA(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, out float P_2)
	{
		P_2 = P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
	}

	public static float oKlwNxFrNKhjvpMJqbeZkelNXLPpA(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
	}

	public static void sjkdpdcfjNwSIFEMyHGtWWQrHwlnA(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		P_1 = P_0;
		P_1.sjkdpdcfjNwSIFEMyHGtWWQrHwlnA();
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC sjkdpdcfjNwSIFEMyHGtWWQrHwlnA(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0)
	{
		P_0.sjkdpdcfjNwSIFEMyHGtWWQrHwlnA();
		return P_0;
	}

	public static void YHhIJSyJTPVnVDbVLdvKINDwDrnk(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, float P_2, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_3)
	{
		P_3.LeaAlLlLUVpUZEtsNQKSfcVaXRbL = dwFuEKaDLqkdeMSEflaKjgVWFsmj.YHhIJSyJTPVnVDbVLdvKINDwDrnk(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_2);
		P_3.nYgMBIIMubbuyaBAFyuFsqYaYOAtA = dwFuEKaDLqkdeMSEflaKjgVWFsmj.YHhIJSyJTPVnVDbVLdvKINDwDrnk(P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA, P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA, P_2);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC YHhIJSyJTPVnVDbVLdvKINDwDrnk(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, float P_2)
	{
		YHhIJSyJTPVnVDbVLdvKINDwDrnk(ref P_0, ref P_1, P_2, out var result);
		return result;
	}

	public static void YmmNVvEfSqniCzsgjAcHxeCKIuFBA(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, float P_2, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_3)
	{
		P_2 = dwFuEKaDLqkdeMSEflaKjgVWFsmj.YmmNVvEfSqniCzsgjAcHxeCKIuFBA(P_2);
		YHhIJSyJTPVnVDbVLdvKINDwDrnk(ref P_0, ref P_1, P_2, out P_3);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC YmmNVvEfSqniCzsgjAcHxeCKIuFBA(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, float P_2)
	{
		YmmNVvEfSqniCzsgjAcHxeCKIuFBA(ref P_0, ref P_1, P_2, out var result);
		return result;
	}

	public static void OuFvwSgTeEdxQZOFjWkQJXVeAPXC(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_2, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_3, float P_4, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_5)
	{
		float num = P_4 * P_4;
		float num2 = P_4 * num;
		float num3 = 2f * num2 - 3f * num + 1f;
		float num4 = -2f * num2 + 3f * num;
		float num5 = num2 - 2f * num + P_4;
		float num6 = num2 - num;
		P_5.LeaAlLlLUVpUZEtsNQKSfcVaXRbL = P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * num3 + P_2.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * num4 + P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * num5 + P_3.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * num6;
		P_5.nYgMBIIMubbuyaBAFyuFsqYaYOAtA = P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * num3 + P_2.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * num4 + P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * num5 + P_3.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * num6;
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC OuFvwSgTeEdxQZOFjWkQJXVeAPXC(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, PbiRltwxmojaZNrRFrQdhtWRcXKC P_2, PbiRltwxmojaZNrRFrQdhtWRcXKC P_3, float P_4)
	{
		OuFvwSgTeEdxQZOFjWkQJXVeAPXC(ref P_0, ref P_1, ref P_2, ref P_3, P_4, out var result);
		return result;
	}

	public static void HXpblcCnHogSQUREcZJqahSIGAxd(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_2, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_3, float P_4, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_5)
	{
		float num = P_4 * P_4;
		float num2 = P_4 * num;
		P_5.LeaAlLlLUVpUZEtsNQKSfcVaXRbL = 0.5f * (2f * P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + (0f - P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_2.LeaAlLlLUVpUZEtsNQKSfcVaXRbL) * P_4 + (2f * P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - 5f * P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + 4f * P_2.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_3.LeaAlLlLUVpUZEtsNQKSfcVaXRbL) * num + (0f - P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + 3f * P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - 3f * P_2.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_3.LeaAlLlLUVpUZEtsNQKSfcVaXRbL) * num2);
		P_5.nYgMBIIMubbuyaBAFyuFsqYaYOAtA = 0.5f * (2f * P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + (0f - P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + P_2.nYgMBIIMubbuyaBAFyuFsqYaYOAtA) * P_4 + (2f * P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - 5f * P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + 4f * P_2.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_3.nYgMBIIMubbuyaBAFyuFsqYaYOAtA) * num + (0f - P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + 3f * P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - 3f * P_2.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + P_3.nYgMBIIMubbuyaBAFyuFsqYaYOAtA) * num2);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC HXpblcCnHogSQUREcZJqahSIGAxd(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, PbiRltwxmojaZNrRFrQdhtWRcXKC P_2, PbiRltwxmojaZNrRFrQdhtWRcXKC P_3, float P_4)
	{
		HXpblcCnHogSQUREcZJqahSIGAxd(ref P_0, ref P_1, ref P_2, ref P_3, P_4, out var result);
		return result;
	}

	public static void HHcbQCFMMAJJToMJswdAQoPFBzsE(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		P_2.LeaAlLlLUVpUZEtsNQKSfcVaXRbL = ((P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL > P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL) ? P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL : P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL);
		P_2.nYgMBIIMubbuyaBAFyuFsqYaYOAtA = ((P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA > P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA) ? P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA : P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC HHcbQCFMMAJJToMJswdAQoPFBzsE(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		HHcbQCFMMAJJToMJswdAQoPFBzsE(ref P_0, ref P_1, out var result);
		return result;
	}

	public static void WfFfwyNisMVMuTMOgcPsoLyntNsL(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		P_2.LeaAlLlLUVpUZEtsNQKSfcVaXRbL = ((P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL < P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL) ? P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL : P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL);
		P_2.nYgMBIIMubbuyaBAFyuFsqYaYOAtA = ((P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA < P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA) ? P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA : P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC WfFfwyNisMVMuTMOgcPsoLyntNsL(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		WfFfwyNisMVMuTMOgcPsoLyntNsL(ref P_0, ref P_1, out var result);
		return result;
	}

	public static void cPgjMIVfTpVMWQMvxCikVrDifyRhA(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_1, out PbiRltwxmojaZNrRFrQdhtWRcXKC P_2)
	{
		float num = P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
		P_2.LeaAlLlLUVpUZEtsNQKSfcVaXRbL = P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - 2f * num * P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL;
		P_2.nYgMBIIMubbuyaBAFyuFsqYaYOAtA = P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - 2f * num * P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA;
	}

	public static PbiRltwxmojaZNrRFrQdhtWRcXKC cPgjMIVfTpVMWQMvxCikVrDifyRhA(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		cPgjMIVfTpVMWQMvxCikVrDifyRhA(ref P_0, ref P_1, out var result);
		return result;
	}

	public static void jdSSvZJlWZzxcHSXKewadGFPHUDu(PbiRltwxmojaZNrRFrQdhtWRcXKC[] P_0, params PbiRltwxmojaZNrRFrQdhtWRcXKC[] P_1)
	{
		if (P_1 == null)
		{
			throw new ArgumentNullException("source");
		}
		if (P_0 == null)
		{
			throw new ArgumentNullException("destination");
		}
		if (P_0.Length < P_1.Length)
		{
			throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
		}
		for (int i = 0; i < P_1.Length; i++)
		{
			PbiRltwxmojaZNrRFrQdhtWRcXKC pbiRltwxmojaZNrRFrQdhtWRcXKC = P_1[i];
			for (int j = 0; j < i; j++)
			{
				pbiRltwxmojaZNrRFrQdhtWRcXKC = lgwFFWXwlOSAWyYJUEMqGmHANcRF(pbiRltwxmojaZNrRFrQdhtWRcXKC, gDgIvDNRTUsRVmxnvfJWFAHGdWqW(oKlwNxFrNKhjvpMJqbeZkelNXLPpA(P_0[j], pbiRltwxmojaZNrRFrQdhtWRcXKC) / oKlwNxFrNKhjvpMJqbeZkelNXLPpA(P_0[j], P_0[j]), P_0[j]));
			}
			P_0[i] = pbiRltwxmojaZNrRFrQdhtWRcXKC;
		}
	}

	public static void shlOvXwkLDMcwgHaiieoIhxLgEGm(PbiRltwxmojaZNrRFrQdhtWRcXKC[] P_0, params PbiRltwxmojaZNrRFrQdhtWRcXKC[] P_1)
	{
		if (P_1 == null)
		{
			throw new ArgumentNullException("source");
		}
		if (P_0 == null)
		{
			throw new ArgumentNullException("destination");
		}
		if (P_0.Length < P_1.Length)
		{
			throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");
		}
		for (int i = 0; i < P_1.Length; i++)
		{
			PbiRltwxmojaZNrRFrQdhtWRcXKC pbiRltwxmojaZNrRFrQdhtWRcXKC = P_1[i];
			for (int j = 0; j < i; j++)
			{
				pbiRltwxmojaZNrRFrQdhtWRcXKC = lgwFFWXwlOSAWyYJUEMqGmHANcRF(pbiRltwxmojaZNrRFrQdhtWRcXKC, gDgIvDNRTUsRVmxnvfJWFAHGdWqW(oKlwNxFrNKhjvpMJqbeZkelNXLPpA(P_0[j], pbiRltwxmojaZNrRFrQdhtWRcXKC), P_0[j]));
			}
			pbiRltwxmojaZNrRFrQdhtWRcXKC.sjkdpdcfjNwSIFEMyHGtWWQrHwlnA();
			P_0[i] = pbiRltwxmojaZNrRFrQdhtWRcXKC;
		}
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC QrsxZdSIARvnlmhlCmFxtHdEDQlGA(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC gDgIvDNRTUsRVmxnvfJWFAHGdWqW(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC DRUMoBhHftdgIpIayWqNVOgfjpZL(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0)
	{
		return P_0;
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC lgwFFWXwlOSAWyYJUEMqGmHANcRF(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC CVRcdrgGVLPWsxLWqBglwTmtjfeSA(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(0f - P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, 0f - P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC gDgIvDNRTUsRVmxnvfJWFAHGdWqW(float P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * P_0, P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * P_0);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC gDgIvDNRTUsRVmxnvfJWFAHGdWqW(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, float P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL * P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA * P_1);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC aTLJCIlOeUIQNhLmoLkNDhkaklwAB(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, float P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL / P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA / P_1);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC aTLJCIlOeUIQNhLmoLkNDhkaklwAB(float P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0 / P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0 / P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC aTLJCIlOeUIQNhLmoLkNDhkaklwAB(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL / P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA / P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC QrsxZdSIARvnlmhlCmFxtHdEDQlGA(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, float P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL + P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA + P_1);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC QrsxZdSIARvnlmhlCmFxtHdEDQlGA(float P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0 + P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0 + P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC lgwFFWXwlOSAWyYJUEMqGmHANcRF(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, float P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL - P_1, P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA - P_1);
	}

	[SpecialName]
	public static PbiRltwxmojaZNrRFrQdhtWRcXKC lgwFFWXwlOSAWyYJUEMqGmHANcRF(float P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return new PbiRltwxmojaZNrRFrQdhtWRcXKC(P_0 - P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, P_0 - P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
	}

	[SpecialName]
	public static bool OUvbuOHwkfdwNYEjxHMoxEMbFyifA(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return P_0.RiVeyXzIIJEVyClDkSYOlnCBsUpL(ref P_1);
	}

	[SpecialName]
	public static bool cJRKgPQzpjtShcOMUgofqgaeyvHQ(PbiRltwxmojaZNrRFrQdhtWRcXKC P_0, PbiRltwxmojaZNrRFrQdhtWRcXKC P_1)
	{
		return !P_0.RiVeyXzIIJEVyClDkSYOlnCBsUpL(ref P_1);
	}

	public string GFrJAlTMaWterKRJyEKenILZzzqq()
	{
		return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", new object[2] { LeaAlLlLUVpUZEtsNQKSfcVaXRbL, nYgMBIIMubbuyaBAFyuFsqYaYOAtA });
	}

	public string GFrJAlTMaWterKRJyEKenILZzzqq(string P_0)
	{
		if (P_0 == null)
		{
			return ToString();
		}
		return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", new object[2]
		{
			LeaAlLlLUVpUZEtsNQKSfcVaXRbL.ToString(P_0, CultureInfo.CurrentCulture),
			nYgMBIIMubbuyaBAFyuFsqYaYOAtA.ToString(P_0, CultureInfo.CurrentCulture)
		});
	}

	public string GFrJAlTMaWterKRJyEKenILZzzqq(IFormatProvider P_0)
	{
		return string.Format(P_0, "X:{0} Y:{1}", new object[2] { LeaAlLlLUVpUZEtsNQKSfcVaXRbL, nYgMBIIMubbuyaBAFyuFsqYaYOAtA });
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		if (format == null)
		{
			GFrJAlTMaWterKRJyEKenILZzzqq(formatProvider);
		}
		return string.Format(formatProvider, "X:{0} Y:{1}", new object[2]
		{
			LeaAlLlLUVpUZEtsNQKSfcVaXRbL.ToString(format, formatProvider),
			nYgMBIIMubbuyaBAFyuFsqYaYOAtA.ToString(format, formatProvider)
		});
	}

	public int vpCtZDiWtrmaqwDCmMjniXrnNrOD()
	{
		return (LeaAlLlLUVpUZEtsNQKSfcVaXRbL.GetHashCode() * 397) ^ nYgMBIIMubbuyaBAFyuFsqYaYOAtA.GetHashCode();
	}

	public bool RiVeyXzIIJEVyClDkSYOlnCBsUpL(ref PbiRltwxmojaZNrRFrQdhtWRcXKC P_0)
	{
		if (dwFuEKaDLqkdeMSEflaKjgVWFsmj.VdqgziRSvQXaUtiuplrQLEtOVtFC(P_0.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, LeaAlLlLUVpUZEtsNQKSfcVaXRbL))
		{
			return dwFuEKaDLqkdeMSEflaKjgVWFsmj.VdqgziRSvQXaUtiuplrQLEtOVtFC(P_0.nYgMBIIMubbuyaBAFyuFsqYaYOAtA, nYgMBIIMubbuyaBAFyuFsqYaYOAtA);
		}
		return false;
	}

	public bool Equals(PbiRltwxmojaZNrRFrQdhtWRcXKC other)
	{
		return RiVeyXzIIJEVyClDkSYOlnCBsUpL(ref other);
	}

	public bool RiVeyXzIIJEVyClDkSYOlnCBsUpL(object P_0)
	{
		if (!(P_0 is PbiRltwxmojaZNrRFrQdhtWRcXKC pbiRltwxmojaZNrRFrQdhtWRcXKC))
		{
			return false;
		}
		return RiVeyXzIIJEVyClDkSYOlnCBsUpL(ref pbiRltwxmojaZNrRFrQdhtWRcXKC);
	}
}
