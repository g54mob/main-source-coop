using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct MetaMul1<T> : IMetaConstant where T : unmanaged, IMetaConstant
	{
		private readonly T _0;

		public int ExpectedSize => _0.ExpectedSize;
	}
}
