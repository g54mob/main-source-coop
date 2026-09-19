using System;
using System.Runtime.InteropServices;

namespace Fusion
{
	internal static class MetaAdd
	{
		internal static Type Get(Type a, Type b)
		{
			return typeof(MetaAdd<, >).MakeGenericType(a, b);
		}
	}
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct MetaAdd<A, B> : IMetaConstant where A : unmanaged, IMetaConstant where B : unmanaged, IMetaConstant
	{
		private readonly A _0;

		private readonly B _1;

		public int ExpectedSize => _0.ExpectedSize + _1.ExpectedSize;
	}
}
