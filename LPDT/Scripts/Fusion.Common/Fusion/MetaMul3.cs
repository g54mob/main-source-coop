using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct MetaMul3<T> : IMetaConstant where T : unmanaged, IMetaConstant
	{
		private readonly T _0;

		private readonly T _1;

		private readonly T _2;

		public int ExpectedSize => 3 * _0.ExpectedSize;
	}
}
