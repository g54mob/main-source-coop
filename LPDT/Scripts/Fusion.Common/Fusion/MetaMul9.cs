using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct MetaMul9<T> : IMetaConstant where T : unmanaged, IMetaConstant
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

		public int ExpectedSize => 9 * _0.ExpectedSize;
	}
}
