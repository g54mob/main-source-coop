using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 44)]
	public readonly struct MetaConstant44 : IMetaConstant
	{
		public int ExpectedSize => 44;
	}
}
