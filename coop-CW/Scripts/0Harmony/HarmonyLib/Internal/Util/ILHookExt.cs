using System.Reflection;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;

namespace HarmonyLib.Internal.Util
{
	internal class ILHookExt : ILHook
	{
		public string dumpPath;

		public ILHookExt(MethodBase from, ILContext.Manipulator manipulator, ILHookConfig config)
			: base(from, manipulator, config)
		{
		}
	}
}
