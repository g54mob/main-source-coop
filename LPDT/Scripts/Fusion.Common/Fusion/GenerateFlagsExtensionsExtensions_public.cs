using System.Runtime.CompilerServices;

namespace Fusion
{
	public static class GenerateFlagsExtensionsExtensions_public
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this FusionScriptingBackend flag, FusionScriptingBackend value)
		{
			return (flag & value) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has(this FusionScriptingBackend flag, FusionScriptingBackend value, FusionScriptingBackend mask)
		{
			return (flag & mask) == value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this FusionScriptingBackend flag, FusionScriptingBackend value)
		{
			return (flag & value) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNot(this FusionScriptingBackend flag, FusionScriptingBackend value, FusionScriptingBackend mask)
		{
			return (flag & mask) != value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAny(this FusionScriptingBackend flag, FusionScriptingBackend value)
		{
			return (flag & value) != 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNone(this FusionScriptingBackend flag, FusionScriptingBackend value)
		{
			return (flag & value) == 0;
		}
	}
}
