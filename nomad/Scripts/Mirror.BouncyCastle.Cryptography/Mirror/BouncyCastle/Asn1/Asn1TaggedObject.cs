using System;
using System.IO;
using Mirror.BouncyCastle.Utilities;

namespace Mirror.BouncyCastle.Asn1
{
	public abstract class Asn1TaggedObject : Asn1Object, IAsn1Convertible
	{
		internal readonly int m_explicitness;

		internal readonly int m_tagClass;

		internal readonly int m_tagNo;

		internal readonly Asn1Encodable m_object;

		public int TagClass => m_tagClass;

		public int TagNo => m_tagNo;

		protected Asn1TaggedObject(bool isExplicit, int tagNo, Asn1Encodable obj)
			: this(isExplicit, 128, tagNo, obj)
		{
		}

		protected Asn1TaggedObject(bool isExplicit, int tagClass, int tagNo, Asn1Encodable obj)
			: this(isExplicit ? 1 : 2, tagClass, tagNo, obj)
		{
		}

		internal Asn1TaggedObject(int explicitness, int tagClass, int tagNo, Asn1Encodable obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (tagClass == 0 || (tagClass & 0xC0) != tagClass)
			{
				throw new ArgumentException("invalid tag class: " + tagClass, "tagClass");
			}
			m_explicitness = ((obj is IAsn1Choice) ? 1 : explicitness);
			m_tagClass = tagClass;
			m_tagNo = tagNo;
			m_object = obj;
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (!(asn1Object is Asn1TaggedObject asn1TaggedObject) || m_tagNo != asn1TaggedObject.m_tagNo || m_tagClass != asn1TaggedObject.m_tagClass)
			{
				return false;
			}
			if (m_explicitness != asn1TaggedObject.m_explicitness && IsExplicit() != asn1TaggedObject.IsExplicit())
			{
				return false;
			}
			Asn1Object asn1Object2 = m_object.ToAsn1Object();
			Asn1Object asn1Object3 = asn1TaggedObject.m_object.ToAsn1Object();
			if (asn1Object2 == asn1Object3)
			{
				return true;
			}
			if (!IsExplicit())
			{
				try
				{
					byte[] encoded = GetEncoded();
					byte[] encoded2 = asn1TaggedObject.GetEncoded();
					return Arrays.AreEqual(encoded, encoded2);
				}
				catch (IOException)
				{
					return false;
				}
			}
			return asn1Object2.CallAsn1Equals(asn1Object3);
		}

		protected override int Asn1GetHashCode()
		{
			return (m_tagClass * 7919) ^ m_tagNo ^ (IsExplicit() ? 15 : 240) ^ m_object.ToAsn1Object().CallAsn1GetHashCode();
		}

		public bool HasTagClass(int tagClass)
		{
			return m_tagClass == tagClass;
		}

		public bool IsExplicit()
		{
			int explicitness = m_explicitness;
			if (explicitness == 1 || explicitness == 3)
			{
				return true;
			}
			return false;
		}

		public Asn1Encodable GetBaseObject()
		{
			return m_object;
		}

		public Asn1Encodable GetExplicitBaseObject()
		{
			if (!IsExplicit())
			{
				throw new InvalidOperationException("object implicit - explicit expected.");
			}
			return m_object;
		}

		internal Asn1Object GetBaseUniversal(bool declaredExplicit, Asn1UniversalType universalType)
		{
			if (declaredExplicit)
			{
				if (!IsExplicit())
				{
					throw new InvalidOperationException("object explicit - implicit expected.");
				}
				return universalType.CheckedCast(m_object.ToAsn1Object());
			}
			if (1 == m_explicitness)
			{
				throw new InvalidOperationException("object explicit - implicit expected.");
			}
			Asn1Object asn1Object = m_object.ToAsn1Object();
			switch (m_explicitness)
			{
			case 3:
				return universalType.FromImplicitConstructed(RebuildConstructed(asn1Object));
			case 4:
				if (asn1Object is Asn1Sequence sequence)
				{
					return universalType.FromImplicitConstructed(sequence);
				}
				return universalType.FromImplicitPrimitive((DerOctetString)asn1Object);
			default:
				return universalType.CheckedCast(asn1Object);
			}
		}

		public override string ToString()
		{
			return Asn1Utilities.GetTagText(m_tagClass, m_tagNo) + m_object;
		}

		internal abstract Asn1Sequence RebuildConstructed(Asn1Object asn1Object);

		internal static Asn1Object CreateConstructedDL(int tagClass, int tagNo, Asn1EncodableVector contentsElements)
		{
			if (contentsElements.Count != 1)
			{
				return new DLTaggedObject(4, tagClass, tagNo, DLSequence.FromVector(contentsElements));
			}
			return new DLTaggedObject(3, tagClass, tagNo, contentsElements[0]);
		}

		internal static Asn1Object CreateConstructedIL(int tagClass, int tagNo, Asn1EncodableVector contentsElements)
		{
			if (contentsElements.Count != 1)
			{
				return new BerTaggedObject(4, tagClass, tagNo, BerSequence.FromVector(contentsElements));
			}
			return new BerTaggedObject(3, tagClass, tagNo, contentsElements[0]);
		}

		internal static Asn1Object CreatePrimitive(int tagClass, int tagNo, byte[] contentsOctets)
		{
			return new DLTaggedObject(4, tagClass, tagNo, new DerOctetString(contentsOctets));
		}
	}
}
