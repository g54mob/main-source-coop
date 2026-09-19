using System;

namespace Fusion
{
	[AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public sealed class NetworkInputWeavedAttribute : Attribute
	{
		public int WordCount { get; }

		public uint Key { get; }

		public NetworkInputWeavedAttribute(uint key, int wordCount)
		{
			WordCount = wordCount;
			Key = key;
		}
	}
}
