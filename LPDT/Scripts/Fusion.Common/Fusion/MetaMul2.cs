using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct MetaMul2<T> : IMetaConstant where T : unmanaged, IMetaConstant
	{
		private readonly T _0;

		private readonly T _1;

		public int ExpectedSize => 2 * _0.ExpectedSize;
	}
}
