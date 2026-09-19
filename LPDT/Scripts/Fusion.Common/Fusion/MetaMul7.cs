using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct MetaMul7<T> : IMetaConstant where T : unmanaged, IMetaConstant
	{
		private readonly T _0;

		private readonly T _1;

		private readonly T _2;

		private readonly T _3;

		private readonly T _4;

		private readonly T _5;

		private readonly T _6;

		public int ExpectedSize => 7 * _0.ExpectedSize;
	}
}
