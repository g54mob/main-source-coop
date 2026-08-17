using System;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Asn1
{
	public class DerEnumerated : Asn1Object
	{
		private readonly byte[] contents;

		private readonly int start;

		private static readonly DerEnumerated[] cache = new DerEnumerated[12];

		internal DerEnumerated(byte[] contents, bool clone)
		{
			if (DerInteger.IsMalformed(contents))
			{
				throw new ArgumentException("malformed enumerated", "contents");
			}
			if ((contents[0] & 0x80) != 0)
			{
				throw new ArgumentException("enumerated must be non-negative", "contents");
			}
			this.contents = (clone ? Arrays.Clone(contents) : contents);
			start = DerInteger.SignBytesToSkip(this.contents);
		}

		internal override IAsn1Encoding GetEncoding(int encoding)
		{
			return new PrimitiveEncoding(0, 10, contents);
		}

		internal override IAsn1Encoding GetEncodingImplicit(int encoding, int tagClass, int tagNo)
		{
			return new PrimitiveEncoding(tagClass, tagNo, contents);
		}

		internal sealed override DerEncoding GetEncodingDer()
		{
			return new PrimitiveDerEncoding(0, 10, contents);
		}

		internal sealed override DerEncoding GetEncodingDerImplicit(int tagClass, int tagNo)
		{
			return new PrimitiveDerEncoding(tagClass, tagNo, contents);
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (!(asn1Object is DerEnumerated derEnumerated))
			{
				return false;
			}
			return Arrays.AreEqual(contents, derEnumerated.contents);
		}

		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(contents);
		}

		internal static DerEnumerated CreatePrimitive(byte[] contents, bool clone)
		{
			if (contents.Length > 1)
			{
				return new DerEnumerated(contents, clone);
			}
			if (contents.Length == 0)
			{
				throw new ArgumentException("ENUMERATED has zero length", "contents");
			}
			int num = contents[0];
			if (num >= cache.Length)
			{
				return new DerEnumerated(contents, clone);
			}
			DerEnumerated derEnumerated = cache[num];
			if (derEnumerated == null)
			{
				derEnumerated = (cache[num] = new DerEnumerated(contents, clone));
			}
			return derEnumerated;
		}
	}
}
