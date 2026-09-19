using System;

namespace MessagePack
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	public class UnionAttribute : Attribute
	{
		public int Key { get; }

		public Type SubType { get; }

		public UnionAttribute(int key, Type subType)
		{
			Key = key;
			SubType = subType ?? throw new ArgumentNullException("subType");
		}

		public UnionAttribute(int key, string subType)
		{
			Key = key;
			SubType = Type.GetType(subType, throwOnError: true);
		}
	}
}
