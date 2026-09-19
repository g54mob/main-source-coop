using System;

namespace Fusion
{
	internal readonly struct NetworkPropertyMeta
	{
		public readonly int WordOffset;

		public readonly int WordCount;

		public readonly int Capacity;

		public readonly Type PropertyType;

		public readonly Type ValueReaderWriterType;

		public readonly Type KeyReaderWriterType;

		public readonly NetworkPropertyMetaFlags Flags;

		public NetworkPropertyMeta(int wordOffset, int wordCount, int capacity, Type propertyType, Type valueReaderWriterType, Type keyReaderWriterType, NetworkPropertyMetaFlags flags)
		{
			WordOffset = wordOffset;
			WordCount = wordCount;
			Capacity = capacity;
			PropertyType = propertyType;
			ValueReaderWriterType = valueReaderWriterType;
			KeyReaderWriterType = keyReaderWriterType;
			Flags = flags;
		}
	}
}
