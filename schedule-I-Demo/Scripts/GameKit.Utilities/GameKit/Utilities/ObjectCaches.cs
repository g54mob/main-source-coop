using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameKit.Utilities
{
	public static class ObjectCaches<T>
	{
		private static readonly Stack<T> _stack = new Stack<T>();

		public static T Retrieve()
		{
			if (_stack.Count == 0)
			{
				return Activator.CreateInstance<T>();
			}
			return _stack.Pop();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void StoreAndDefault(ref T value)
		{
			if (value != null)
			{
				Store(value);
				value = default(T);
			}
		}

		public static void Store(T value)
		{
			_stack.Push(value);
		}
	}
}
