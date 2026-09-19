namespace Fusion
{
	internal interface IIntrusiveLinkedListPtr<T> : IIntrusiveLinkedList where T : unmanaged
	{
		unsafe bool Remove(T* item);

		unsafe void AddFirst(T* item);

		unsafe void AddLast(T* item);

		unsafe T* RemoveFirst();

		unsafe bool TryRemoveFirst(out T* first);

		unsafe bool TryPeekFirst(out T* first);

		unsafe T* Next(T* current);

		unsafe T* Prev(T* current);

		unsafe T*[] ToArray();

		void FreeAll();
	}
}
