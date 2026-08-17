using System;
using Mirror.BouncyCastle.Security;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Math
{
	public static class Primes
	{
		public static readonly int SmallFactorLimit = 211;

		private static readonly BigInteger One = BigInteger.One;

		private static readonly BigInteger Two = BigInteger.Two;

		private static readonly BigInteger Three = BigInteger.Three;

		public static bool HasAnySmallFactors(BigInteger candidate)
		{
			CheckCandidate(candidate, "candidate");
			return ImplHasAnySmallFactors(candidate);
		}

		public static bool IsMRProbablePrime(BigInteger candidate, SecureRandom random, int iterations)
		{
			CheckCandidate(candidate, "candidate");
			if (random == null)
			{
				throw new ArgumentException("cannot be null", "random");
			}
			if (iterations < 1)
			{
				throw new ArgumentException("must be > 0", "iterations");
			}
			if (candidate.BitLength == 2)
			{
				return true;
			}
			if (!candidate.TestBit(0))
			{
				return false;
			}
			BigInteger bigInteger = candidate.Subtract(One);
			BigInteger max = candidate.Subtract(Two);
			int lowestSetBit = bigInteger.GetLowestSetBit();
			BigInteger m = bigInteger.ShiftRight(lowestSetBit);
			for (int i = 0; i < iterations; i++)
			{
				BigInteger b = BigIntegers.CreateRandomInRange(Two, max, random);
				if (!ImplMRProbablePrimeToBase(candidate, bigInteger, m, lowestSetBit, b))
				{
					return false;
				}
			}
			return true;
		}

		private static void CheckCandidate(BigInteger n, string name)
		{
			if (n == null || n.SignValue < 1 || n.BitLength < 2)
			{
				throw new ArgumentException("must be non-null and >= 2", name);
			}
		}

		private static bool ImplHasAnySmallFactors(BigInteger x)
		{
			int value = 223092870;
			int intValue = x.Mod(BigInteger.ValueOf(value)).IntValue;
			if (intValue % 2 == 0 || intValue % 3 == 0 || intValue % 5 == 0 || intValue % 7 == 0 || intValue % 11 == 0 || intValue % 13 == 0 || intValue % 17 == 0 || intValue % 19 == 0 || intValue % 23 == 0)
			{
				return true;
			}
			value = 58642669;
			intValue = x.Mod(BigInteger.ValueOf(value)).IntValue;
			if (intValue % 29 == 0 || intValue % 31 == 0 || intValue % 37 == 0 || intValue % 41 == 0 || intValue % 43 == 0)
			{
				return true;
			}
			value = 600662303;
			intValue = x.Mod(BigInteger.ValueOf(value)).IntValue;
			if (intValue % 47 == 0 || intValue % 53 == 0 || intValue % 59 == 0 || intValue % 61 == 0 || intValue % 67 == 0)
			{
				return true;
			}
			value = 33984931;
			intValue = x.Mod(BigInteger.ValueOf(value)).IntValue;
			if (intValue % 71 == 0 || intValue % 73 == 0 || intValue % 79 == 0 || intValue % 83 == 0)
			{
				return true;
			}
			value = 89809099;
			intValue = x.Mod(BigInteger.ValueOf(value)).IntValue;
			if (intValue % 89 == 0 || intValue % 97 == 0 || intValue % 101 == 0 || intValue % 103 == 0)
			{
				return true;
			}
			value = 167375713;
			intValue = x.Mod(BigInteger.ValueOf(value)).IntValue;
			if (intValue % 107 == 0 || intValue % 109 == 0 || intValue % 113 == 0 || intValue % 127 == 0)
			{
				return true;
			}
			value = 371700317;
			intValue = x.Mod(BigInteger.ValueOf(value)).IntValue;
			if (intValue % 131 == 0 || intValue % 137 == 0 || intValue % 139 == 0 || intValue % 149 == 0)
			{
				return true;
			}
			value = 645328247;
			intValue = x.Mod(BigInteger.ValueOf(value)).IntValue;
			if (intValue % 151 == 0 || intValue % 157 == 0 || intValue % 163 == 0 || intValue % 167 == 0)
			{
				return true;
			}
			value = 1070560157;
			intValue = x.Mod(BigInteger.ValueOf(value)).IntValue;
			if (intValue % 173 == 0 || intValue % 179 == 0 || intValue % 181 == 0 || intValue % 191 == 0)
			{
				return true;
			}
			value = 1596463769;
			intValue = x.Mod(BigInteger.ValueOf(value)).IntValue;
			if (intValue % 193 == 0 || intValue % 197 == 0 || intValue % 199 == 0 || intValue % 211 == 0)
			{
				return true;
			}
			return false;
		}

		private static bool ImplMRProbablePrimeToBase(BigInteger w, BigInteger wSubOne, BigInteger m, int a, BigInteger b)
		{
			BigInteger bigInteger = b.ModPow(m, w);
			if (bigInteger.Equals(One) || bigInteger.Equals(wSubOne))
			{
				return true;
			}
			for (int i = 1; i < a; i++)
			{
				bigInteger = bigInteger.Square().Mod(w);
				if (bigInteger.Equals(wSubOne))
				{
					return true;
				}
				if (bigInteger.Equals(One))
				{
					return false;
				}
			}
			return false;
		}
	}
}
