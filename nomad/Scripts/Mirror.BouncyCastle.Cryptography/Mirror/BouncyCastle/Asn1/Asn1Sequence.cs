using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mirror.BouncyCastle.Utilities;
using Mirror.BouncyCastle.Utilities.Collections;

namespace Mirror.BouncyCastle.Asn1
{
	public abstract class Asn1Sequence : Asn1Object, IEnumerable<Asn1Encodable>, IEnumerable
	{
		internal class Meta : Asn1UniversalType
		{
			internal static readonly Asn1UniversalType Instance = new Meta();

			private Meta()
				: base(typeof(Asn1Sequence), 16)
			{
			}

			internal override Asn1Object FromImplicitConstructed(Asn1Sequence sequence)
			{
				return sequence;
			}
		}

		internal readonly Asn1Encodable[] m_elements;

		public virtual Asn1Encodable this[int index] => m_elements[index];

		public virtual int Count => m_elements.Length;

		public static Asn1Sequence GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is Asn1Sequence result)
			{
				return result;
			}
			if (obj is IAsn1Convertible asn1Convertible)
			{
				if (asn1Convertible.ToAsn1Object() is Asn1Sequence result2)
				{
					return result2;
				}
			}
			else if (obj is byte[] bytes)
			{
				try
				{
					return (Asn1Sequence)Meta.Instance.FromByteArray(bytes);
				}
				catch (IOException ex)
				{
					throw new ArgumentException("failed to construct sequence from byte[]: " + ex.Message);
				}
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		protected internal Asn1Sequence()
		{
			m_elements = Asn1EncodableVector.EmptyElements;
		}

		protected internal Asn1Sequence(Asn1Encodable element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			m_elements = new Asn1Encodable[1] { element };
		}

		protected internal Asn1Sequence(Asn1Encodable element1, Asn1Encodable element2)
		{
			if (element1 == null)
			{
				throw new ArgumentNullException("element1");
			}
			if (element2 == null)
			{
				throw new ArgumentNullException("element2");
			}
			m_elements = new Asn1Encodable[2] { element1, element2 };
		}

		protected internal Asn1Sequence(params Asn1Encodable[] elements)
		{
			if (Arrays.IsNullOrContainsNull(elements))
			{
				throw new NullReferenceException("'elements' cannot be null, or contain null");
			}
			m_elements = Asn1EncodableVector.CloneElements(elements);
		}

		protected internal Asn1Sequence(Asn1EncodableVector elementVector)
		{
			if (elementVector == null)
			{
				throw new ArgumentNullException("elementVector");
			}
			m_elements = elementVector.TakeElements();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public virtual IEnumerator<Asn1Encodable> GetEnumerator()
		{
			return ((IEnumerable<Asn1Encodable>)m_elements).GetEnumerator();
		}

		public virtual T[] MapElements<T>(Func<Asn1Encodable, T> func)
		{
			int count = Count;
			T[] array = new T[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = func(m_elements[i]);
			}
			return array;
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
			if (!(asn1Object is Asn1Sequence asn1Sequence))
			{
				return false;
			}
			int count = Count;
			if (asn1Sequence.Count != count)
			{
				return false;
			}
			for (int i = 0; i < count; i++)
			{
				Asn1Object asn1Object2 = m_elements[i].ToAsn1Object();
				Asn1Object other = asn1Sequence.m_elements[i].ToAsn1Object();
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

		internal DerBitString[] GetConstructedBitStrings()
		{
			return MapElements(DerBitString.GetInstance);
		}

		internal Asn1OctetString[] GetConstructedOctetStrings()
		{
			return MapElements(Asn1OctetString.GetInstance);
		}

		internal abstract DerBitString ToAsn1BitString();

		internal abstract DerExternal ToAsn1External();

		internal abstract Asn1OctetString ToAsn1OctetString();

		internal abstract Asn1Set ToAsn1Set();
	}
}
