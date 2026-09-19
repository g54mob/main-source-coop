using System;

namespace Fusion
{
	internal interface IIntrusiveLinkedList
	{
		int Count { get; }

		bool IsEmpty { get; }

		void CopyTo(Array array, int index);
	}
	internal interface IIntrusiveLinkedList<T> : IIntrusiveLinkedList where T : class
	{
		bool Remove(T item);

		void AddFirst(T item);

		void AddLast(T item);

		T RemoveFirst();

		bool TryRemoveFirst(out T first);

		bool TryPeekFirst(out T first);

		T Next(T current);

		T Prev(T current);

		T[] ToArray();
	}
}
