using System.Runtime.CompilerServices;

namespace GameKit.Utilities
{
	public static class ResettableObjectCaches<T> where T : IResettable
	{
		public static T Retrieve()
		{
			T result = ObjectCaches<T>.Retrieve();
			result.InitializeState();
			return result;
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
			value.ResetState();
			ObjectCaches<T>.Store(value);
		}
	}
}
