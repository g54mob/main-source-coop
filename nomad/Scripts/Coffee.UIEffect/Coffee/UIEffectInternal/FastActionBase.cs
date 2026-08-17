using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIEffectInternal
{
	internal class FastActionBase<T>
	{
		private static readonly InternalObjectPool<LinkedListNode<T>> s_NodePool = new InternalObjectPool<LinkedListNode<T>>(() => new LinkedListNode<T>(default(T)), (LinkedListNode<T> _) => true, delegate(LinkedListNode<T> x)
		{
			x.Value = default(T);
		});

		private readonly LinkedList<T> _delegates = new LinkedList<T>();

		public void Add(T rhs)
		{
			if (rhs != null)
			{
				LinkedListNode<T> linkedListNode = s_NodePool.Rent();
				linkedListNode.Value = rhs;
				_delegates.AddLast(linkedListNode);
			}
		}

		public void Remove(T rhs)
		{
			if (rhs != null)
			{
				LinkedListNode<T> instance = _delegates.Find(rhs);
				if (instance != null)
				{
					_delegates.Remove(instance);
					s_NodePool.Return(ref instance);
				}
			}
		}

		protected void Invoke(Action<T> callback)
		{
			for (LinkedListNode<T> linkedListNode = _delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				try
				{
					callback(linkedListNode.Value);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		public void Clear()
		{
			_delegates.Clear();
		}
	}
}
