using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rewired.Internal;
using Rewired.Internal.Glyphs;
using Rewired.Utils.Classes.Data;

internal abstract class ZCteKPZRxTbUmaKieiJZhAaEJCygB : IPrefetch
{
	protected struct RTLJigdPBumdJaIlwNYFaZzbdnrR : IEquatable<RTLJigdPBumdJaIlwNYFaZzbdnrR>
	{
		public KeyedGlyph bgcEhOhwfuxKZtuHkopcUGIuSvyA;

		public int DIHNfKksobcWOkNuxgpzBCSCMBKR;

		public RTLJigdPBumdJaIlwNYFaZzbdnrR(KeyedGlyph P_0, int P_1)
		{
			bgcEhOhwfuxKZtuHkopcUGIuSvyA = P_0;
			DIHNfKksobcWOkNuxgpzBCSCMBKR = P_1;
		}

		public bool ODaDVHuwyxngGLnmeYpVVHGtwjSc(object P_0)
		{
			if (!(P_0 is RTLJigdPBumdJaIlwNYFaZzbdnrR rTLJigdPBumdJaIlwNYFaZzbdnrR))
			{
				return false;
			}
			if (rTLJigdPBumdJaIlwNYFaZzbdnrR.bgcEhOhwfuxKZtuHkopcUGIuSvyA == bgcEhOhwfuxKZtuHkopcUGIuSvyA)
			{
				return rTLJigdPBumdJaIlwNYFaZzbdnrR.DIHNfKksobcWOkNuxgpzBCSCMBKR == DIHNfKksobcWOkNuxgpzBCSCMBKR;
			}
			return false;
		}

		public int JNLYZJGhCMgSrhZSRMKyWBZrqOuQ()
		{
			return (17 * 29 + bgcEhOhwfuxKZtuHkopcUGIuSvyA.GetHashCode()) * 29 + DIHNfKksobcWOkNuxgpzBCSCMBKR.GetHashCode();
		}

		public bool Equals(RTLJigdPBumdJaIlwNYFaZzbdnrR other)
		{
			if (bgcEhOhwfuxKZtuHkopcUGIuSvyA == other.bgcEhOhwfuxKZtuHkopcUGIuSvyA)
			{
				return DIHNfKksobcWOkNuxgpzBCSCMBKR == other.DIHNfKksobcWOkNuxgpzBCSCMBKR;
			}
			return false;
		}

		bool IEquatable<RTLJigdPBumdJaIlwNYFaZzbdnrR>.Equals(RTLJigdPBumdJaIlwNYFaZzbdnrR other)
		{
			//ILSpy generated this explicit interface implementation from .override directive in Equals
			return this.Equals(other);
		}

		[SpecialName]
		public static bool hxVxYgEQviJcTJNVedGsCNyqYCUkA(RTLJigdPBumdJaIlwNYFaZzbdnrR P_0, RTLJigdPBumdJaIlwNYFaZzbdnrR P_1)
		{
			return P_0.Equals(P_1);
		}

		[SpecialName]
		public static bool FYYvGOUIBxdyJNobKaZmtCyjcOjAA(RTLJigdPBumdJaIlwNYFaZzbdnrR P_0, RTLJigdPBumdJaIlwNYFaZzbdnrR P_1)
		{
			return !P_0.Equals(P_1);
		}
	}

	private HQqbZoQigscgVQcdQGCMdxuNvzzS FRfMhwGeNlKDvAptvjzccrAoAijn;

	protected readonly KeyedGlyph DFRsEWxMCYnvJvYKEiwuybaAgMShA;

	private Id bCUVJsPglFmfWRdPnINkBCZebPzL;

	private readonly Dictionary<int, List<RTLJigdPBumdJaIlwNYFaZzbdnrR>> vFZXEjYqlFhqfRfDBVCyApEOnyWg;

	private bool FdZzuEIjkSDIlRHPMbikkNdcjAvyA;

	protected bool taeNyrJjqaFSMWGGTBowjOrLyHrt => FdZzuEIjkSDIlRHPMbikkNdcjAvyA;

	public abstract object VMAqNtDPmJybBXpPjPFicHThBxQD { get; }

	public abstract string QDOOogoJEZzDwhcfyDMVrQliiZaQ { get; }

	protected ZCteKPZRxTbUmaKieiJZhAaEJCygB()
	{
		DFRsEWxMCYnvJvYKEiwuybaAgMShA = new KeyedGlyph();
		vFZXEjYqlFhqfRfDBVCyApEOnyWg = new Dictionary<int, List<RTLJigdPBumdJaIlwNYFaZzbdnrR>>();
	}

	protected ZCteKPZRxTbUmaKieiJZhAaEJCygB(HQqbZoQigscgVQcdQGCMdxuNvzzS P_0)
		: this()
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("dataSource");
		}
		FRfMhwGeNlKDvAptvjzccrAoAijn = P_0;
	}

	public void TvRDOmUSVtCEeOePggfyBwqELGfo()
	{
		WDUJTcrrlFsnJzdVBbdBBDaoMdUs();
		if (GlyphManager.isEnabled && GlyphManager.autoPrefetch)
		{
			tCgFabDNaInoTqPiHLGuoxNDlTKN();
		}
	}

	protected virtual void WDUJTcrrlFsnJzdVBbdBBDaoMdUs()
	{
		VihglnFXAIcvsRAhnQytBFEgiPDE();
		snksrTQOsEKEXIrfxsDFgVraoAEW();
		GlyphManager.Add(this, ref bCUVJsPglFmfWRdPnINkBCZebPzL);
		FdZzuEIjkSDIlRHPMbikkNdcjAvyA = true;
	}

	public virtual void VihglnFXAIcvsRAhnQytBFEgiPDE()
	{
		nMCFRTIaMgJLhkpNjDKiKWkSjEtlA();
		GlyphManager.Remove(ref bCUVJsPglFmfWRdPnINkBCZebPzL);
		FdZzuEIjkSDIlRHPMbikkNdcjAvyA = false;
	}

	public virtual void QAxXyYCvMGLPopqbVHETwtyFMYp(HQqbZoQigscgVQcdQGCMdxuNvzzS P_0)
	{
		if (P_0 == null)
		{
			throw new ArgumentNullException("value");
		}
		if (P_0 != FRfMhwGeNlKDvAptvjzccrAoAijn)
		{
			if (FRfMhwGeNlKDvAptvjzccrAoAijn != null)
			{
				nMCFRTIaMgJLhkpNjDKiKWkSjEtlA();
			}
			FRfMhwGeNlKDvAptvjzccrAoAijn = P_0;
			TvRDOmUSVtCEeOePggfyBwqELGfo();
		}
	}

	public virtual void xCVyKqoAIbQLjSeZueDmIqhArwCN()
	{
		DFRsEWxMCYnvJvYKEiwuybaAgMShA.Clear();
	}

	public virtual bool qCFiRDjEOHkqdfmCYPvVKSPPChWf(ZCteKPZRxTbUmaKieiJZhAaEJCygB P_0)
	{
		if (P_0 == null)
		{
			return false;
		}
		if (!object.Equals(GetType(), P_0.GetType()))
		{
			return false;
		}
		if (FRfMhwGeNlKDvAptvjzccrAoAijn == null != (P_0.FRfMhwGeNlKDvAptvjzccrAoAijn == null))
		{
			return false;
		}
		if (FRfMhwGeNlKDvAptvjzccrAoAijn != null && (!string.Equals(FRfMhwGeNlKDvAptvjzccrAoAijn.keyCategory, P_0.FRfMhwGeNlKDvAptvjzccrAoAijn.keyCategory, StringComparison.Ordinal) || !string.Equals(FRfMhwGeNlKDvAptvjzccrAoAijn.key, P_0.FRfMhwGeNlKDvAptvjzccrAoAijn.key, StringComparison.Ordinal)))
		{
			return false;
		}
		return true;
	}

	protected virtual void nMCFRTIaMgJLhkpNjDKiKWkSjEtlA()
	{
		DFRsEWxMCYnvJvYKEiwuybaAgMShA.Clear();
		vFZXEjYqlFhqfRfDBVCyApEOnyWg.Clear();
	}

	protected HQqbZoQigscgVQcdQGCMdxuNvzzS mTZTIPnsHjouJscecTzhOxkyLBCX()
	{
		return FRfMhwGeNlKDvAptvjzccrAoAijn;
	}

	protected virtual void tCgFabDNaInoTqPiHLGuoxNDlTKN()
	{
		_ = VMAqNtDPmJybBXpPjPFicHThBxQD;
	}

	void IPrefetch.Prefetch()
	{
		tCgFabDNaInoTqPiHLGuoxNDlTKN();
	}

	protected virtual void ctOHDIDBxuFUMdxJJZkKswSfEPreA(int P_0)
	{
	}

	protected virtual void snksrTQOsEKEXIrfxsDFgVraoAEW()
	{
	}

	protected virtual void piakUSCLcudJRhZilcQmLsckObsUA(int P_0, RTLJigdPBumdJaIlwNYFaZzbdnrR P_1)
	{
		for (int i = 0; i < 32; i++)
		{
			int num = 1 << i;
			if ((P_0 & num) != 0)
			{
				if (!vFZXEjYqlFhqfRfDBVCyApEOnyWg.TryGetValue(num, out var value))
				{
					value = new List<RTLJigdPBumdJaIlwNYFaZzbdnrR>();
					vFZXEjYqlFhqfRfDBVCyApEOnyWg[num] = value;
				}
				if (!value.Contains(P_1))
				{
					value.Add(P_1);
				}
			}
		}
	}

	protected virtual void PgZYpFfbsufOVHcdXJVCSqcxLwyA(int P_0, RTLJigdPBumdJaIlwNYFaZzbdnrR P_1)
	{
		for (int i = 0; i < 32; i++)
		{
			int num = 1 << i;
			if ((P_0 & num) == 0 || !vFZXEjYqlFhqfRfDBVCyApEOnyWg.TryGetValue(num, out var value))
			{
				continue;
			}
			for (int num2 = value.Count - 1; num2 >= 0; num2--)
			{
				if (RTLJigdPBumdJaIlwNYFaZzbdnrR.hxVxYgEQviJcTJNVedGsCNyqYCUkA(value[num2], P_1))
				{
					value.RemoveAt(num2);
				}
			}
		}
	}

	protected virtual void QsUCZNOSicazMKHKLTixYAhXAyft(int P_0)
	{
		for (int i = 0; i < 32; i++)
		{
			int num = 1 << i;
			if ((P_0 & num) == 0 || !vFZXEjYqlFhqfRfDBVCyApEOnyWg.TryGetValue(num, out var value))
			{
				continue;
			}
			int count = value.Count;
			for (int j = 0; j < count; j++)
			{
				if (value[j].DIHNfKksobcWOkNuxgpzBCSCMBKR != 0)
				{
					ctOHDIDBxuFUMdxJJZkKswSfEPreA(value[j].DIHNfKksobcWOkNuxgpzBCSCMBKR);
				}
				if (value[j].bgcEhOhwfuxKZtuHkopcUGIuSvyA != null)
				{
					value[j].bgcEhOhwfuxKZtuHkopcUGIuSvyA.Clear();
				}
			}
		}
	}
}
