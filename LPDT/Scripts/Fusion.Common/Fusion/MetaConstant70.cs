using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 70)]
	public readonly struct MetaConstant70 : IMetaConstant
	{
		public int ExpectedSize => 70;
	}
}
