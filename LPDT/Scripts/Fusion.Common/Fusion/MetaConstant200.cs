using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 200)]
	public readonly struct MetaConstant200 : IMetaConstant
	{
		public int ExpectedSize => 200;
	}
}
