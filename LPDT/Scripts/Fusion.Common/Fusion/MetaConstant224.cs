using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 224)]
	public readonly struct MetaConstant224 : IMetaConstant
	{
		public int ExpectedSize => 224;
	}
}
