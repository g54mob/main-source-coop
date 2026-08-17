using System;
using System.Globalization;
using System.Text;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Asn1
{
	public class Asn1UtcTime : Asn1Object
	{
		private readonly string m_timeString;

		private readonly DateTime m_dateTime;

		private readonly bool m_dateTimeLocked;

		private readonly int m_twoDigitYearMax;

		public Asn1UtcTime(string timeString)
		{
			m_timeString = timeString ?? throw new ArgumentNullException("timeString");
			try
			{
				m_dateTime = FromString(timeString, out m_twoDigitYearMax);
				m_dateTimeLocked = false;
			}
			catch (FormatException ex)
			{
				throw new ArgumentException("invalid date string: " + ex.Message);
			}
		}

		internal Asn1UtcTime(byte[] contents)
			: this(Encoding.ASCII.GetString(contents))
		{
		}

		internal byte[] GetContents(int encoding)
		{
			if (encoding == 3 && m_timeString.Length != 13)
			{
				string s = ToStringCanonical(m_dateTime);
				return Encoding.ASCII.GetBytes(s);
			}
			return Encoding.ASCII.GetBytes(m_timeString);
		}

		internal override IAsn1Encoding GetEncoding(int encoding)
		{
			return new PrimitiveEncoding(0, 23, GetContents(encoding));
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			return new PrimitiveEncoding(tagClass, tagNo, GetContents(encoding));
		}

		internal sealed override DerEncoding GetEncodingDer()
		{
			return new PrimitiveDerEncoding(0, 23, GetContents(3));
		}

		internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
		{
			return new PrimitiveDerEncoding(tagClass, tagNo, GetContents(3));
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (!(asn1Object is Asn1UtcTime asn1UtcTime))
			{
				return false;
			}
			return Arrays.AreEqual(GetContents(3), asn1UtcTime.GetContents(3));
		}

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(GetContents(3));
		}

		public override string ToString()
		{
			return m_timeString;
		}

		internal static Asn1UtcTime CreatePrimitive(byte[] contents)
		{
			return new Asn1UtcTime(contents);
		}

		private static DateTime FromString(string s, out int twoDigitYearMax)
		{
			DateTimeFormatInfo invariantInfo = DateTimeFormatInfo.InvariantInfo;
			twoDigitYearMax = invariantInfo.Calendar.TwoDigitYearMax;
			return s.Length switch
			{
				11 => DateTime.ParseExact(s, "yyMMddHHmm\\Z", invariantInfo, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal), 
				13 => DateTime.ParseExact(s, "yyMMddHHmmss\\Z", invariantInfo, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal), 
				15 => DateTime.ParseExact(s, "yyMMddHHmmzzz", invariantInfo, DateTimeStyles.AdjustToUniversal), 
				17 => DateTime.ParseExact(s, "yyMMddHHmmsszzz", invariantInfo, DateTimeStyles.AdjustToUniversal), 
				_ => throw new FormatException(), 
			};
		}

		private static string ToStringCanonical(DateTime dateTime)
		{
			return dateTime.ToString("yyMMddHHmmss\\Z", DateTimeFormatInfo.InvariantInfo);
		}
	}
}
