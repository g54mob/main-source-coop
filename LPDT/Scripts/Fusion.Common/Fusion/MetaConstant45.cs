using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 45)]
	public readonly struct MetaConstant45 : IMetaConstant
	{
		public int ExpectedSize => 45;
	}
}
