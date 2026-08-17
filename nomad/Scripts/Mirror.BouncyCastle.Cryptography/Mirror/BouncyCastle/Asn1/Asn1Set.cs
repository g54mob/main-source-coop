using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Mirror.BouncyCastle.Utilities.Collections;

namespace Mirror.BouncyCastle.Asn1
{
	[DefaultMember("Item")]
	public abstract class Asn1Set : Asn1Object, IEnumerable<Asn1Encodable>, IEnumerable
	{
		internal class Meta : Asn1UniversalType
		{
			internal static readonly Asn1UniversalType Instance = new Meta();

			private Meta()
				: base(typeof(Asn1Set), 17)
			{
			}

			internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence)
			{
				return sequence.ToAsn1Set();
			}
		}

		internal readonly Asn1Encodable[] m_elements;

		internal DerEncoding[] m_sortedDerEncodings;

		public virtual int Count => m_elements.Length;

		public static Asn1Set GetInstance(Asn1TaggedObject taggedObject, bool declaredExplicit)
		{
			return (Asn1Set)Meta.Instance.GetContextInstance(taggedObject, declaredExplicit);
		}

		protected internal Asn1Set()
		{
			m_elements = Asn1EncodableVector.EmptyElements;
			m_sortedDerEncodings = null;
		}

		protected internal Asn1Set(Asn1EncodableVector elementVector, bool doSort)
		{
			if (elementVector == null)
			{
				throw new ArgumentNullException("elementVector");
			}
			Asn1Encodable[] elements;
			DerEncoding[] sortedDerEncodings;
			if (doSort && elementVector.Count > 1)
			{
				elements = elementVector.CopyElements();
				sortedDerEncodings = SortElements(elements);
			}
			else
			{
				elements = elementVector.TakeElements();
				sortedDerEncodings = null;
			}
			m_elements = elements;
			m_sortedDerEncodings = sortedDerEncodings;
		}

		protected internal Asn1Set(bool isSorted, Asn1Encodable[] elements)
		{
			m_elements = elements;
			m_sortedDerEncodings = null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public virtual IEnumerator<Asn1Encodable> GetEnumerator()
		{
			return ((IEnumerable<Asn1Encodable>)m_elements).GetEnumerator();
		}

		protected override int Asn1GetHashCode()
		{
			int num = Count;
			int num2 = num + 1;
			while (--num >= 0)
			{
				num2 *= 257;
				num2 ^= m_elements[num].ToAsn1Object().CallAsn1GetHashCode();
			}
			return num2;
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			if (!(asn1Object is Asn1Set asn1Set))
			{
				return false;
			}
			int count = Count;
			if (asn1Set.Count != count)
			{
				return false;
			}
			for (int i = 0; i < count; i++)
			{
				Asn1Object asn1Object2 = m_elements[i].ToAsn1Object();
				Asn1Object other = asn1Set.m_elements[i].ToAsn1Object();
				if (!asn1Object2.Equals(other))
				{
					return false;
				}
			}
			return true;
		}

		public override string ToString()
		{
			return CollectionUtilities.ToString(m_elements);
		}

		private static DerEncoding[] SortElements(Asn1Encodable[] elements)
		{
			DerEncoding[] contentsEncodingsDer = Asn1OutputStream.GetContentsEncodingsDer(elements);
			Array.Sort(contentsEncodingsDer, elements);
			return contentsEncodingsDer;
		}
	}
}
