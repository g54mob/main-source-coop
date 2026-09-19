using System;

namespace Fusion
{
	internal struct IntrusiveLinkedListNode<T> : IEquatable<IntrusiveLinkedListNode<T>> where T : class
	{
		public T Next;

		public T Prev;

		public readonly bool Equals(IntrusiveLinkedListNode<T> other)
		{
			return Next == other.Next && Prev == other.Prev;
		}

		public override readonly bool Equals(object obj)
		{
			return obj is IntrusiveLinkedListNode<T> other && Equals(other);
		}

		public override readonly int GetHashCode()
		{
			return HashCode.Combine(Next, Prev);
		}

		public static bool operator ==(IntrusiveLinkedListNode<T> a, IntrusiveLinkedListNode<T> b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(IntrusiveLinkedListNode<T> a, IntrusiveLinkedListNode<T> b)
		{
			return !a.Equals(b);
		}

		public override readonly string ToString()
		{
			return $"[Next:{Next}, Prev:{Prev}]";
		}
	}
}
