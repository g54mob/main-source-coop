using System;
using Mirror.BouncyCastle.Math.EC.Endo;
using Mirror.BouncyCastle.Math.EC.Multiplier;
using Mirror.BouncyCastle.Math.Field;
using Mirror.BouncyCastle.Math.Raw;

namespace Mirror.BouncyCastle.Math.EC
{
	public class ECAlgorithms
	{
		public static bool IsF2mCurve(ECCurve c)
		{
			return IsF2mField(c.Field);
		}

		public static bool IsF2mField(IFiniteField field)
		{
			if (field.Dimension > 1 && field.Characteristic.Equals(BigInteger.Two))
			{
				return field is IPolynomialExtensionField;
			}
			return false;
		}

		public static bool IsFpCurve(ECCurve c)
		{
			return IsFpField(c.Field);
		}

		public static bool IsFpField(IFiniteField field)
		{
			return field.Dimension == 1;
		}

		public static ECPoint ImportPoint(ECCurve c, ECPoint p)
		{
			ECCurve curve = p.Curve;
			if (!c.Equals(curve))
			{
				throw new ArgumentException("Point must be on the same curve");
			}
			return c.ImportPoint(p);
		}

		public static void MontgomeryTrick(ECFieldElement[] zs, int off, int len, ECFieldElement scale)
		{
			ECFieldElement[] array = new ECFieldElement[len];
			array[0] = zs[off];
			int num = 0;
			while (++num < len)
			{
				array[num] = array[num - 1].Multiply(zs[off + num]);
			}
			num--;
			if (scale != null)
			{
				array[num] = array[num].Multiply(scale);
			}
			ECFieldElement eCFieldElement = array[num].Invert();
			while (num > 0)
			{
				int num2 = off + num--;
				ECFieldElement b = zs[num2];
				zs[num2] = array[num].Multiply(eCFieldElement);
				eCFieldElement = eCFieldElement.Multiply(b);
			}
			zs[off] = eCFieldElement;
		}

		public static ECPoint ReferenceMultiply(ECPoint p, BigInteger k)
		{
			BigInteger bigInteger = k.Abs();
			ECPoint eCPoint = p.Curve.Infinity;
			int bitLength = bigInteger.BitLength;
			if (bitLength > 0)
			{
				if (bigInteger.TestBit(0))
				{
					eCPoint = p;
				}
				for (int i = 1; i < bitLength; i++)
				{
					p = p.Twice();
					if (bigInteger.TestBit(i))
					{
						eCPoint = eCPoint.Add(p);
					}
				}
			}
			if (k.SignValue >= 0)
			{
				return eCPoint;
			}
			return eCPoint.Negate();
		}

		public static ECPoint CleanPoint(ECCurve c, ECPoint p)
		{
			ECCurve curve = p.Curve;
			if (!c.Equals(curve))
			{
				throw new ArgumentException("Point must be on the same curve", "p");
			}
			return c.DecodePoint(p.GetEncoded(compressed: false));
		}

		internal static ECPoint ImplCheckResult(ECPoint p)
		{
			if (!p.IsValidPartial())
			{
				throw new InvalidOperationException("Invalid result");
			}
			return p;
		}

		internal static ECPoint ImplShamirsTrickWNaf(ECPoint P, BigInteger k, ECPoint Q, BigInteger l)
		{
			bool flag = k.SignValue < 0;
			bool flag2 = l.SignValue < 0;
			BigInteger bigInteger = k.Abs();
			BigInteger bigInteger2 = l.Abs();
			int windowSize = WNafUtilities.GetWindowSize(bigInteger.BitLength, 8);
			int windowSize2 = WNafUtilities.GetWindowSize(bigInteger2.BitLength, 8);
			WNafPreCompInfo wNafPreCompInfo = WNafUtilities.Precompute(P, windowSize, includeNegated: true);
			WNafPreCompInfo wNafPreCompInfo2 = WNafUtilities.Precompute(Q, windowSize2, includeNegated: true);
			int combSize = FixedPointUtilities.GetCombSize(P.Curve);
			if (!flag && !flag2 && k.BitLength <= combSize && l.BitLength <= combSize && wNafPreCompInfo.IsPromoted && wNafPreCompInfo2.IsPromoted)
			{
				return ImplShamirsTrickFixedPoint(P, k, Q, l);
			}
			int width = System.Math.Min(8, wNafPreCompInfo.Width);
			int width2 = System.Math.Min(8, wNafPreCompInfo2.Width);
			ECPoint[] preCompP = (flag ? wNafPreCompInfo.PreCompNeg : wNafPreCompInfo.PreComp);
			ECPoint[] preCompQ = (flag2 ? wNafPreCompInfo2.PreCompNeg : wNafPreCompInfo2.PreComp);
			ECPoint[] preCompNegP = (flag ? wNafPreCompInfo.PreComp : wNafPreCompInfo.PreCompNeg);
			ECPoint[] preCompNegQ = (flag2 ? wNafPreCompInfo2.PreComp : wNafPreCompInfo2.PreCompNeg);
			byte[] wnafP = WNafUtilities.GenerateWindowNaf(width, bigInteger);
			byte[] wnafQ = WNafUtilities.GenerateWindowNaf(width2, bigInteger2);
			return ImplShamirsTrickWNaf(preCompP, preCompNegP, wnafP, preCompQ, preCompNegQ, wnafQ);
		}

		internal static ECPoint ImplShamirsTrickWNaf(ECEndomorphism endomorphism, ECPoint P, BigInteger k, BigInteger l)
		{
			bool flag = k.SignValue < 0;
			bool flag2 = l.SignValue < 0;
			k = k.Abs();
			l = l.Abs();
			int windowSize = WNafUtilities.GetWindowSize(System.Math.Max(k.BitLength, l.BitLength), 8);
			WNafPreCompInfo wNafPreCompInfo = WNafUtilities.Precompute(P, windowSize, includeNegated: true);
			WNafPreCompInfo wNafPreCompInfo2 = WNafUtilities.PrecomputeWithPointMap(EndoUtilities.MapPoint(endomorphism, P), endomorphism.PointMap, wNafPreCompInfo, includeNegated: true);
			int width = System.Math.Min(8, wNafPreCompInfo.Width);
			int width2 = System.Math.Min(8, wNafPreCompInfo2.Width);
			ECPoint[] preCompP = (flag ? wNafPreCompInfo.PreCompNeg : wNafPreCompInfo.PreComp);
			ECPoint[] preCompQ = (flag2 ? wNafPreCompInfo2.PreCompNeg : wNafPreCompInfo2.PreComp);
			ECPoint[] preCompNegP = (flag ? wNafPreCompInfo.PreComp : wNafPreCompInfo.PreCompNeg);
			ECPoint[] preCompNegQ = (flag2 ? wNafPreCompInfo2.PreComp : wNafPreCompInfo2.PreCompNeg);
			byte[] wnafP = WNafUtilities.GenerateWindowNaf(width, k);
			byte[] wnafQ = WNafUtilities.GenerateWindowNaf(width2, l);
			return ImplShamirsTrickWNaf(preCompP, preCompNegP, wnafP, preCompQ, preCompNegQ, wnafQ);
		}

		private static ECPoint ImplShamirsTrickWNaf(ECPoint[] preCompP, ECPoint[] preCompNegP, byte[] wnafP, ECPoint[] preCompQ, ECPoint[] preCompNegQ, byte[] wnafQ)
		{
			int num = System.Math.Max(wnafP.Length, wnafQ.Length);
			ECPoint infinity = preCompP[0].Curve.Infinity;
			ECPoint eCPoint = infinity;
			int num2 = 0;
			for (int num3 = num - 1; num3 >= 0; num3--)
			{
				int num4 = ((num3 < wnafP.Length) ? ((sbyte)wnafP[num3]) : 0);
				int num5 = ((num3 < wnafQ.Length) ? ((sbyte)wnafQ[num3]) : 0);
				if ((num4 | num5) == 0)
				{
					num2++;
				}
				else
				{
					ECPoint eCPoint2 = infinity;
					if (num4 != 0)
					{
						int num6 = System.Math.Abs(num4);
						ECPoint[] array = ((num4 < 0) ? preCompNegP : preCompP);
						eCPoint2 = eCPoint2.Add(array[num6 >> 1]);
					}
					if (num5 != 0)
					{
						int num7 = System.Math.Abs(num5);
						ECPoint[] array2 = ((num5 < 0) ? preCompNegQ : preCompQ);
						eCPoint2 = eCPoint2.Add(array2[num7 >> 1]);
					}
					if (num2 > 0)
					{
						eCPoint = eCPoint.TimesPow2(num2);
						num2 = 0;
					}
					eCPoint = eCPoint.TwicePlus(eCPoint2);
				}
			}
			if (num2 > 0)
			{
				eCPoint = eCPoint.TimesPow2(num2);
			}
			return eCPoint;
		}

		private static ECPoint ImplShamirsTrickFixedPoint(ECPoint p, BigInteger k, ECPoint q, BigInteger l)
		{
			ECCurve curve = p.Curve;
			int combSize = FixedPointUtilities.GetCombSize(curve);
			if (k.BitLength > combSize || l.BitLength > combSize)
			{
				throw new InvalidOperationException("fixed-point comb doesn't support scalars larger than the curve order");
			}
			FixedPointPreCompInfo fixedPointPreCompInfo = FixedPointUtilities.Precompute(p);
			FixedPointPreCompInfo fixedPointPreCompInfo2 = FixedPointUtilities.Precompute(q);
			ECLookupTable lookupTable = fixedPointPreCompInfo.LookupTable;
			ECLookupTable lookupTable2 = fixedPointPreCompInfo2.LookupTable;
			int width = fixedPointPreCompInfo.Width;
			int width2 = fixedPointPreCompInfo2.Width;
			if (width != width2)
			{
				FixedPointCombMultiplier fixedPointCombMultiplier = new FixedPointCombMultiplier();
				ECPoint eCPoint = fixedPointCombMultiplier.Multiply(p, k);
				ECPoint b = fixedPointCombMultiplier.Multiply(q, l);
				return eCPoint.Add(b);
			}
			int num = width;
			int num2 = (combSize + num - 1) / num;
			int num3 = num2 * num;
			uint[] array = Nat.FromBigInteger(num3, k);
			uint[] array2 = Nat.FromBigInteger(num3, l);
			ECPoint eCPoint2 = curve.Infinity;
			for (int i = 1; i <= num2; i++)
			{
				uint num4 = 0u;
				uint num5 = 0u;
				for (int num6 = num3 - i; num6 >= 0; num6 -= num2)
				{
					uint num7 = array[num6 >> 5] >> num6;
					num4 ^= num7 >> 1;
					num4 <<= 1;
					num4 ^= num7;
					uint num8 = array2[num6 >> 5] >> num6;
					num5 ^= num8 >> 1;
					num5 <<= 1;
					num5 ^= num8;
				}
				ECPoint eCPoint3 = lookupTable.LookupVar((int)num4);
				ECPoint b2 = lookupTable2.LookupVar((int)num5);
				ECPoint b3 = eCPoint3.Add(b2);
				eCPoint2 = eCPoint2.TwicePlus(b3);
			}
			return eCPoint2.Add(fixedPointPreCompInfo.Offset).Add(fixedPointPreCompInfo2.Offset);
		}
	}
}
