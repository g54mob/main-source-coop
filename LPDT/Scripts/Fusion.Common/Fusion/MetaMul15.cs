using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct MetaMul15<T> : IMetaConstant where T : unmanaged, IMetaConstant
	{
		private readonly T _0;

		private readonly T _1;

		private readonly T _2;

		private readonly T _3;

		private readonly T _4;

		private readonly T _5;

		private readonly T _6;

		private readonly T _7;

		private readonly T _8;

		private readonly T _9;

		private readonly T _10;

		private readonly T _11;

		private readonly T _12;

		private readonly T _13;

		private readonly T _14;

		public int ExpectedSize => 15 * _0.ExpectedSize;
	}
}
