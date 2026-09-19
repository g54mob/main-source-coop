using System.Runtime.InteropServices;

namespace Fusion
{
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	public readonly struct MetaConstant4 : IMetaConstant
	{
		public int ExpectedSize => 4;
	}
}
