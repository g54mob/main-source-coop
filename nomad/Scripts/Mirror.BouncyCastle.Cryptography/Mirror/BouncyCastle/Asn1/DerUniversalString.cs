using System;
using System.Text;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Asn1
{
	public class DerUniversalString : DerStringBase
	{
		private static readonly char[] table = new char[16]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F'
		};

		private readonly byte[] m_contents;

		internal DerUniversalString(byte[] contents, bool clone)
		{
			if (contents == null)
			{
				throw new ArgumentNullException("contents");
			}
			m_contents = (clone ? Arrays.Clone(contents) : contents);
		}

		public override string GetString()
		{
			int num = m_contents.Length;
			int capacity = 3 + 2 * (Asn1OutputStream.GetLengthOfDL(num) + num);
			StringBuilder stringBuilder = new StringBuilder("#1C", capacity);
			EncodeHexDL(stringBuilder, num);
			for (int i = 0; i < num; i++)
			{
				EncodeHexByte(stringBuilder, m_contents[i]);
			}
			return stringBuilder.ToString();
		}

		internal override IAsn1Encoding GetEncoding(int encoding)
		{
			return new PrimitiveEncoding(0, 28, m_contents);
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			return new PrimitiveEncoding(tagClass, tagNo, m_contents);
		}

		internal sealed override DerEncoding GetEncodingDer()
		{
			return new PrimitiveDerEncoding(0, 28, m_contents);
		}

		internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
		{
			return new PrimitiveDerEncoding(tagClass, tagNo, m_contents);
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (asn1Object is DerUniversalString derUniversalString)
			{
				return Arrays.AreEqual(m_contents, derUniversalString.m_contents);
			}
			return false;
		}

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(m_contents);
		}

		internal static DerUniversalString CreatePrimitive(byte[] contents)
		{
			return new DerUniversalString(contents, clone: false);
		}

		private static void EncodeHexByte(StringBuilder buf, int i)
		{
			buf.Append(table[(i >> 4) & 0xF]);
			buf.Append(table[i & 0xF]);
		}

		private static void EncodeHexDL(StringBuilder buf, int dl)
		{
			if (dl < 128)
			{
				EncodeHexByte(buf, dl);
				return;
			}
			byte[] array = new byte[5];
			int num = 5;
			do
			{
				array[--num] = (byte)dl;
				dl >>= 8;
			}
			while (dl != 0);
			int num2 = array.Length - num;
			array[--num] = (byte)(0x80 | num2);
			do
			{
				EncodeHexByte(buf, array[num++]);
			}
			while (num < array.Length);
		}
	}
}
