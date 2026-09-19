using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 85)]
	public readonly struct MetaConstant85 : IMetaConstant
	{
		public int ExpectedSize => 85;
	}
}
