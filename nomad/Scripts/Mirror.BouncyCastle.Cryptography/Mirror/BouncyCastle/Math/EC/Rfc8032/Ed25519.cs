using System;
using Mirror.BouncyCastle.Crypto;
using Mirror.BouncyCastle.Crypto.Digests;
using Mirror.BouncyCastle.Math.EC.Rfc7748;
using Mirror.BouncyCastle.Math.Raw;

namespace Mirror.BouncyCastle.Math.EC.Rfc8032
{
	public static class Ed25519
	{
		public sealed class PublicPoint
		{
			internal readonly int[] m_data;

			internal PublicPoint(int[] data)
			{
				m_data = data;
			}
		}

		private struct PointAccum
		{
			internal int[] x;

			internal int[] y;

			internal int[] z;

			internal int[] u;

			internal int[] v;
		}

		private struct PointAffine
		{
			internal int[] x;

			internal int[] y;
		}

		private struct PointExtended
		{
			internal int[] x;

			internal int[] y;

			internal int[] z;

			internal int[] t;
		}

		private struct PointPrecomp
		{
			internal int[] ymx_h;

			internal int[] ypx_h;

			internal int[] xyd;
		}

		private struct PointTemp
		{
			internal int[] r0;

			internal int[] r1;
		}

		public static readonly int PrehashSize = 64;

		public static readonly int PublicKeySize = 32;

		public static readonly int SecretKeySize = 32;

		public static readonly int SignatureSize = 64;

		private static readonly byte[] Dom2Prefix = new byte[32]
		{
			83, 105, 103, 69, 100, 50, 53, 53, 49, 57,
			32, 110, 111, 32, 69, 100, 50, 53, 53, 49,
			57, 32, 99, 111, 108, 108, 105, 115, 105, 111,
			110, 115
		};

		private static readonly uint[] P = new uint[8] { 4294967277u, 4294967295u, 4294967295u, 4294967295u, 4294967295u, 4294967295u, 4294967295u, 2147483647u };

		private static readonly uint[] Order8_y1 = new uint[8] { 1886001095u, 1339575613u, 1980447930u, 258412557u, 4199751722u, 3335272748u, 2013120334u, 2047061138u };

		private static readonly uint[] Order8_y2 = new uint[8] { 2408966182u, 2955391682u, 2314519365u, 4036554738u, 95215573u, 959694547u, 2281846961u, 100422509u };

		private static readonly int[] B_x = new int[10] { 52811034, 25909283, 8072341, 50637101, 13785486, 30858332, 20483199, 20966410, 43936626, 4379245 };

		private static readonly int[] B_y = new int[10] { 40265304, 26843545, 6710886, 53687091, 13421772, 40265318, 26843545, 6710886, 53687091, 13421772 };

		private static readonly int[] B128_x = new int[10] { 12052516, 1174424, 4087752, 38672185, 20040971, 21899680, 55468344, 20105554, 66708015, 9981791 };

		private static readonly int[] B128_y = new int[10] { 66430571, 45040722, 4842939, 15895846, 18981244, 46308410, 4697481, 8903007, 53646190, 12474675 };

		private static readonly int[] C_d = new int[10] { 56195235, 47411844, 25868126, 40503822, 57364, 58321048, 30416477, 31930572, 57760639, 10749657 };

		private static readonly int[] C_d2 = new int[10] { 45281625, 27714825, 18181821, 13898781, 114729, 49533232, 60832955, 30306712, 48412415, 4722099 };

		private static readonly int[] C_d4 = new int[10] { 23454386, 55429651, 2809210, 27797563, 229458, 31957600, 54557047, 27058993, 29715967, 9444199 };

		private static readonly object PrecompLock = new object();

		private static PointPrecomp[] PrecompBaseWnaf = null;

		private static PointPrecomp[] PrecompBase128Wnaf = null;

		private static int[] PrecompBaseComb = null;

		private static int CheckPoint(ref PointAffine p)
		{
			int[] array = X25519Field.Create();
			int[] array2 = X25519Field.Create();
			int[] array3 = X25519Field.Create();
			X25519Field.Sqr(p.x, array2);
			X25519Field.Sqr(p.y, array3);
			X25519Field.Mul(array2, array3, array);
			X25519Field.Sub(array2, array3, array2);
			X25519Field.Mul(array, C_d, array);
			X25519Field.AddOne(array);
			X25519Field.Add(array, array2, array);
			X25519Field.Normalize(array);
			X25519Field.Normalize(array3);
			return X25519Field.IsZero(array) & ~X25519Field.IsZero(array3);
		}

		private static int CheckPoint(PointAccum p)
		{
			int[] array = X25519Field.Create();
			int[] array2 = X25519Field.Create();
			int[] array3 = X25519Field.Create();
			int[] array4 = X25519Field.Create();
			X25519Field.Sqr(p.x, array2);
			X25519Field.Sqr(p.y, array3);
			X25519Field.Sqr(p.z, array4);
			X25519Field.Mul(array2, array3, array);
			X25519Field.Sub(array2, array3, array2);
			X25519Field.Mul(array2, array4, array2);
			X25519Field.Sqr(array4, array4);
			X25519Field.Mul(array, C_d, array);
			X25519Field.Add(array, array4, array);
			X25519Field.Add(array, array2, array);
			X25519Field.Normalize(array);
			X25519Field.Normalize(array3);
			X25519Field.Normalize(array4);
			return X25519Field.IsZero(array) & ~X25519Field.IsZero(array3) & ~X25519Field.IsZero(array4);
		}

		private static bool CheckPointFullVar(byte[] p)
		{
			uint num2;
			uint num = (num2 = Codec.Decode32(p, 28) & 0x7FFFFFFF);
			uint num3 = num ^ P[7];
			uint num4 = num ^ Order8_y1[7];
			uint num5 = num ^ Order8_y2[7];
			for (int num6 = 6; num6 > 0; num6--)
			{
				uint num7 = Codec.Decode32(p, num6 * 4);
				num2 |= num7;
				num3 |= num7 ^ P[num6];
				num4 |= num7 ^ Order8_y1[num6];
				num5 |= num7 ^ Order8_y2[num6];
			}
			uint num8 = Codec.Decode32(p, 0);
			if (num2 == 0 && num8 <= 1)
			{
				return false;
			}
			if (num3 == 0 && num8 >= P[0] - 1)
			{
				return false;
			}
			num4 |= num8 ^ Order8_y1[0];
			num5 |= num8 ^ Order8_y2[0];
			return num4 != 0 && num5 != 0;
		}

		private static byte[] Copy(byte[] buf, int off, int len)
		{
			byte[] array = new byte[len];
			Array.Copy(buf, off, array, 0, len);
			return array;
		}

		private static IDigest CreateDigest()
		{
			Sha512Digest sha512Digest = new Sha512Digest();
			if (sha512Digest.GetDigestSize() != 64)
			{
				throw new InvalidOperationException();
			}
			return sha512Digest;
		}

		private static bool DecodePointVar(byte[] p, bool negate, ref PointAffine r)
		{
			int num = (p[31] & 0x80) >> 7;
			X25519Field.Decode(p, r.y);
			int[] array = X25519Field.Create();
			int[] array2 = X25519Field.Create();
			X25519Field.Sqr(r.y, array);
			X25519Field.Mul(C_d, array, array2);
			X25519Field.SubOne(array);
			X25519Field.AddOne(array2);
			if (!X25519Field.SqrtRatioVar(array, array2, r.x))
			{
				return false;
			}
			X25519Field.Normalize(r.x);
			if (num == 1 && X25519Field.IsZeroVar(r.x))
			{
				return false;
			}
			if (negate ^ (num != (r.x[0] & 1)))
			{
				X25519Field.Negate(r.x, r.x);
				X25519Field.Normalize(r.x);
			}
			return true;
		}

		public static void EncodePublicPoint(PublicPoint publicPoint, byte[] pk, int pkOff)
		{
			X25519Field.Encode(publicPoint.m_data, 10, pk, pkOff);
			pk[pkOff + 32 - 1] |= (byte)((publicPoint.m_data[0] & 1) << 7);
		}

		private static PublicPoint ExportPoint(ref PointAffine p)
		{
			int[] array = new int[20];
			X25519Field.Copy(p.x, 0, array, 0);
			X25519Field.Copy(p.y, 0, array, 10);
			return new PublicPoint(array);
		}

		public static PublicPoint GeneratePublicKey(byte[] sk, int skOff)
		{
			IDigest digest = CreateDigest();
			byte[] array = new byte[64];
			digest.BlockUpdate(sk, skOff, SecretKeySize);
			digest.DoFinal(array, 0);
			byte[] array2 = new byte[32];
			PruneScalar(array, 0, array2);
			Init(out PointAccum r);
			ScalarMultBase(array2, ref r);
			Init(out PointAffine r2);
			NormalizeToAffine(ref r, ref r2);
			if (CheckPoint(ref r2) == 0)
			{
				throw new InvalidOperationException();
			}
			return ExportPoint(ref r2);
		}

		private static void GroupCombBits(uint[] n)
		{
			for (int i = 0; i < n.Length; i++)
			{
				n[i] = Interleave.Shuffle2(n[i]);
			}
		}

		private static void Init(out PointAccum r)
		{
			r.x = X25519Field.Create();
			r.y = X25519Field.Create();
			r.z = X25519Field.Create();
			r.u = X25519Field.Create();
			r.v = X25519Field.Create();
		}

		private static void Init(out PointAffine r)
		{
			r.x = X25519Field.Create();
			r.y = X25519Field.Create();
		}

		private static void Init(out PointExtended r)
		{
			r.x = X25519Field.Create();
			r.y = X25519Field.Create();
			r.z = X25519Field.Create();
			r.t = X25519Field.Create();
		}

		private static void Init(out PointPrecomp r)
		{
			r.ymx_h = X25519Field.Create();
			r.ypx_h = X25519Field.Create();
			r.xyd = X25519Field.Create();
		}

		private static void Init(out PointTemp r)
		{
			r.r0 = X25519Field.Create();
			r.r1 = X25519Field.Create();
		}

		private static void InvertDoubleZs(PointExtended[] points)
		{
			int num = points.Length;
			int[] array = X25519Field.CreateTable(num);
			int[] array2 = X25519Field.Create();
			X25519Field.Copy(points[0].z, 0, array2, 0);
			X25519Field.Copy(array2, 0, array, 0);
			int num2 = 0;
			while (++num2 < num)
			{
				X25519Field.Mul(array2, points[num2].z, array2);
				X25519Field.Copy(array2, 0, array, num2 * 10);
			}
			X25519Field.Add(array2, array2, array2);
			X25519Field.InvVar(array2, array2);
			num2--;
			int[] array3 = X25519Field.Create();
			while (num2 > 0)
			{
				int num3 = num2--;
				X25519Field.Copy(array, num2 * 10, array3, 0);
				X25519Field.Mul(array3, array2, array3);
				X25519Field.Mul(array2, points[num3].z, array2);
				X25519Field.Copy(array3, 0, points[num3].z, 0);
			}
			X25519Field.Copy(array2, 0, points[0].z, 0);
		}

		private static void NormalizeToAffine(ref PointAccum p, ref PointAffine r)
		{
			X25519Field.Inv(p.z, r.y);
			X25519Field.Mul(r.y, p.x, r.x);
			X25519Field.Mul(r.y, p.y, r.y);
			X25519Field.Normalize(r.x);
			X25519Field.Normalize(r.y);
		}

		private static void PointAdd(ref PointExtended p, ref PointExtended q, ref PointExtended r, ref PointTemp t)
		{
			int[] x = r.x;
			int[] y = r.y;
			int[] r2 = t.r0;
			int[] r3 = t.r1;
			int[] array = x;
			int[] array2 = r2;
			int[] array3 = r3;
			int[] array4 = y;
			X25519Field.Apm(p.y, p.x, y, x);
			X25519Field.Apm(q.y, q.x, r3, r2);
			X25519Field.Mul(x, r2, x);
			X25519Field.Mul(y, r3, y);
			X25519Field.Mul(p.t, q.t, r2);
			X25519Field.Mul(r2, C_d2, r2);
			X25519Field.Add(p.z, p.z, r3);
			X25519Field.Mul(r3, q.z, r3);
			X25519Field.Apm(y, x, array4, array);
			X25519Field.Apm(r3, r2, array3, array2);
			X25519Field.Mul(array, array4, r.t);
			X25519Field.Mul(array2, array3, r.z);
			X25519Field.Mul(array, array2, r.x);
			X25519Field.Mul(array4, array3, r.y);
		}

		private static void PointAdd(ref PointPrecomp p, ref PointAccum r, ref PointTemp t)
		{
			int[] x = r.x;
			int[] y = r.y;
			int[] r2 = t.r0;
			int[] u = r.u;
			int[] array = x;
			int[] array2 = y;
			int[] v = r.v;
			X25519Field.Apm(r.y, r.x, y, x);
			X25519Field.Mul(x, p.ymx_h, x);
			X25519Field.Mul(y, p.ypx_h, y);
			X25519Field.Mul(r.u, r.v, r2);
			X25519Field.Mul(r2, p.xyd, r2);
			X25519Field.Apm(y, x, v, u);
			X25519Field.Apm(r.z, r2, array2, array);
			X25519Field.Mul(array, array2, r.z);
			X25519Field.Mul(array, u, r.x);
			X25519Field.Mul(array2, v, r.y);
		}

		private static void PointCopy(ref PointAccum p, ref PointExtended r)
		{
			X25519Field.Copy(p.x, 0, r.x, 0);
			X25519Field.Copy(p.y, 0, r.y, 0);
			X25519Field.Copy(p.z, 0, r.z, 0);
			X25519Field.Mul(p.u, p.v, r.t);
		}

		private static void PointCopy(ref PointAffine p, ref PointExtended r)
		{
			X25519Field.Copy(p.x, 0, r.x, 0);
			X25519Field.Copy(p.y, 0, r.y, 0);
			X25519Field.One(r.z);
			X25519Field.Mul(p.x, p.y, r.t);
		}

		private static void PointDouble(ref PointAccum r)
		{
			int[] x = r.x;
			int[] y = r.y;
			int[] z = r.z;
			int[] u = r.u;
			int[] array = x;
			int[] array2 = y;
			int[] v = r.v;
			X25519Field.Add(r.x, r.y, u);
			X25519Field.Sqr(r.x, x);
			X25519Field.Sqr(r.y, y);
			X25519Field.Sqr(r.z, z);
			X25519Field.Add(z, z, z);
			X25519Field.Apm(x, y, v, array2);
			X25519Field.Sqr(u, u);
			X25519Field.Sub(v, u, u);
			X25519Field.Add(z, array2, array);
			X25519Field.Carry(array);
			X25519Field.Mul(array, array2, r.z);
			X25519Field.Mul(array, u, r.x);
			X25519Field.Mul(array2, v, r.y);
		}

		private static void PointLookup(int block, int index, ref PointPrecomp p)
		{
			int num = block * 8 * 3 * 10;
			for (int i = 0; i < 8; i++)
			{
				int cond = (i ^ index) - 1 >> 31;
				X25519Field.CMov(cond, PrecompBaseComb, num, p.ymx_h, 0);
				num += 10;
				X25519Field.CMov(cond, PrecompBaseComb, num, p.ypx_h, 0);
				num += 10;
				X25519Field.CMov(cond, PrecompBaseComb, num, p.xyd, 0);
				num += 10;
			}
		}

		private static void PointPrecompute(ref PointAffine p, PointExtended[] points, int pointsOff, int pointsLen, ref PointTemp t)
		{
			Init(out points[pointsOff]);
			PointCopy(ref p, ref points[pointsOff]);
			Init(out PointExtended r);
			PointAdd(ref points[pointsOff], ref points[pointsOff], ref r, ref t);
			for (int i = 1; i < pointsLen; i++)
			{
				Init(out points[pointsOff + i]);
				PointAdd(ref points[pointsOff + i - 1], ref r, ref points[pointsOff + i], ref t);
			}
		}

		private static void PointSetNeutral(ref PointAccum p)
		{
			X25519Field.Zero(p.x);
			X25519Field.One(p.y);
			X25519Field.One(p.z);
			X25519Field.Zero(p.u);
			X25519Field.One(p.v);
		}

		public static void Precompute()
		{
			lock (PrecompLock)
			{
				if (PrecompBaseComb != null)
				{
					return;
				}
				int num = 16;
				int num2 = 64;
				int num3 = num * 2 + num2;
				PointExtended[] array = new PointExtended[num3];
				Init(out PointTemp r);
				Init(out PointAffine r2);
				X25519Field.Copy(B_x, 0, r2.x, 0);
				X25519Field.Copy(B_y, 0, r2.y, 0);
				PointPrecompute(ref r2, array, 0, num, ref r);
				Init(out PointAffine r3);
				X25519Field.Copy(B128_x, 0, r3.x, 0);
				X25519Field.Copy(B128_y, 0, r3.y, 0);
				PointPrecompute(ref r3, array, num, num, ref r);
				Init(out PointAccum r4);
				X25519Field.Copy(B_x, 0, r4.x, 0);
				X25519Field.Copy(B_y, 0, r4.y, 0);
				X25519Field.One(r4.z);
				X25519Field.Copy(B_x, 0, r4.u, 0);
				X25519Field.Copy(B_y, 0, r4.v, 0);
				int num4 = num * 2;
				PointExtended[] array2 = new PointExtended[4];
				for (int i = 0; i < 4; i++)
				{
					Init(out array2[i]);
				}
				Init(out PointExtended r5);
				for (int j = 0; j < 8; j++)
				{
					ref PointExtended reference = ref array[num4++];
					Init(out reference);
					for (int k = 0; k < 4; k++)
					{
						if (k == 0)
						{
							PointCopy(ref r4, ref reference);
						}
						else
						{
							PointCopy(ref r4, ref r5);
							PointAdd(ref reference, ref r5, ref reference, ref r);
						}
						PointDouble(ref r4);
						PointCopy(ref r4, ref array2[k]);
						if (j + k != 10)
						{
							for (int l = 1; l < 8; l++)
							{
								PointDouble(ref r4);
							}
						}
					}
					X25519Field.Negate(reference.x, reference.x);
					X25519Field.Negate(reference.t, reference.t);
					for (int m = 0; m < 3; m++)
					{
						int num5 = 1 << m;
						int num6 = 0;
						while (num6 < num5)
						{
							Init(out array[num4]);
							PointAdd(ref array[num4 - num5], ref array2[m], ref array[num4], ref r);
							num6++;
							num4++;
						}
					}
				}
				InvertDoubleZs(array);
				PrecompBaseWnaf = new PointPrecomp[num];
				for (int n = 0; n < num; n++)
				{
					ref PointExtended reference2 = ref array[n];
					ref PointPrecomp reference3 = ref PrecompBaseWnaf[n];
					Init(out reference3);
					X25519Field.Mul(reference2.x, reference2.z, reference2.x);
					X25519Field.Mul(reference2.y, reference2.z, reference2.y);
					X25519Field.Apm(reference2.y, reference2.x, reference3.ypx_h, reference3.ymx_h);
					X25519Field.Mul(reference2.x, reference2.y, reference3.xyd);
					X25519Field.Mul(reference3.xyd, C_d4, reference3.xyd);
					X25519Field.Normalize(reference3.ymx_h);
					X25519Field.Normalize(reference3.ypx_h);
					X25519Field.Normalize(reference3.xyd);
				}
				PrecompBase128Wnaf = new PointPrecomp[num];
				for (int num7 = 0; num7 < num; num7++)
				{
					ref PointExtended reference4 = ref array[num + num7];
					ref PointPrecomp reference5 = ref PrecompBase128Wnaf[num7];
					Init(out reference5);
					X25519Field.Mul(reference4.x, reference4.z, reference4.x);
					X25519Field.Mul(reference4.y, reference4.z, reference4.y);
					X25519Field.Apm(reference4.y, reference4.x, reference5.ypx_h, reference5.ymx_h);
					X25519Field.Mul(reference4.x, reference4.y, reference5.xyd);
					X25519Field.Mul(reference5.xyd, C_d4, reference5.xyd);
					X25519Field.Normalize(reference5.ymx_h);
					X25519Field.Normalize(reference5.ypx_h);
					X25519Field.Normalize(reference5.xyd);
				}
				PrecompBaseComb = X25519Field.CreateTable(num2 * 3);
				Init(out PointPrecomp r6);
				int num8 = 0;
				for (int num9 = num * 2; num9 < num3; num9++)
				{
					ref PointExtended reference6 = ref array[num9];
					X25519Field.Mul(reference6.x, reference6.z, reference6.x);
					X25519Field.Mul(reference6.y, reference6.z, reference6.y);
					X25519Field.Apm(reference6.y, reference6.x, r6.ypx_h, r6.ymx_h);
					X25519Field.Mul(reference6.x, reference6.y, r6.xyd);
					X25519Field.Mul(r6.xyd, C_d4, r6.xyd);
					X25519Field.Normalize(r6.ymx_h);
					X25519Field.Normalize(r6.ypx_h);
					X25519Field.Normalize(r6.xyd);
					X25519Field.Copy(r6.ymx_h, 0, PrecompBaseComb, num8);
					num8 += 10;
					X25519Field.Copy(r6.ypx_h, 0, PrecompBaseComb, num8);
					num8 += 10;
					X25519Field.Copy(r6.xyd, 0, PrecompBaseComb, num8);
					num8 += 10;
				}
			}
		}

		private static void PruneScalar(byte[] n, int nOff, byte[] r)
		{
			Array.Copy(n, nOff, r, 0, 32);
			r[0] &= 248;
			r[31] &= 127;
			r[31] |= 64;
		}

		private static void ScalarMultBase(byte[] k, ref PointAccum r)
		{
			Precompute();
			uint[] array = new uint[8];
			Scalar25519.Decode(k, array);
			Scalar25519.ToSignedDigits(256, array);
			GroupCombBits(array);
			Init(out PointPrecomp r2);
			Init(out PointTemp r3);
			PointSetNeutral(ref r);
			int num = 0;
			int num2 = 28;
			while (true)
			{
				for (int i = 0; i < 8; i++)
				{
					uint num3 = array[i] >> num2;
					int num4 = (int)((num3 >> 3) & 1);
					int index = (int)((num3 ^ (uint)(-num4)) & 7);
					PointLookup(i, index, ref r2);
					X25519Field.CNegate(num ^ num4, r.x);
					X25519Field.CNegate(num ^ num4, r.u);
					num = num4;
					PointAdd(ref r2, ref r, ref r3);
				}
				if ((num2 -= 4) < 0)
				{
					break;
				}
				PointDouble(ref r);
			}
			X25519Field.CNegate(num, r.x);
			X25519Field.CNegate(num, r.u);
		}

		internal static void ScalarMultBaseYZ(byte[] k, int kOff, int[] y, int[] z)
		{
			byte[] array = new byte[32];
			PruneScalar(k, kOff, array);
			Init(out PointAccum r);
			ScalarMultBase(array, ref r);
			if (CheckPoint(r) == 0)
			{
				throw new InvalidOperationException();
			}
			X25519Field.Copy(r.y, 0, y, 0);
			X25519Field.Copy(r.z, 0, z, 0);
		}

		public static PublicPoint ValidatePublicKeyPartialExport(byte[] pk, int pkOff)
		{
			byte[] p = Copy(pk, pkOff, PublicKeySize);
			if (!CheckPointFullVar(p))
			{
				return null;
			}
			Init(out PointAffine r);
			if (!DecodePointVar(p, negate: false, ref r))
			{
				return null;
			}
			return ExportPoint(ref r);
		}
	}
}
