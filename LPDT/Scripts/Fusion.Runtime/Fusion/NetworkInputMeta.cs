using System;

namespace Fusion
{
	internal readonly struct NetworkInputMeta
	{
		public readonly uint Key;

		public readonly int WordCount;

		public readonly Type Type;

		public NetworkInputMeta(uint key, int wordCount, Type type)
		{
			Key = key;
			WordCount = wordCount;
			Type = type;
		}
	}
}
