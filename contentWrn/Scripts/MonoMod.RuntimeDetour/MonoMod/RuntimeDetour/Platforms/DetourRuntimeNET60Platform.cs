using System;

namespace MonoMod.RuntimeDetour.Platforms
{
	public class DetourRuntimeNET60Platform : DetourRuntimeNETCore30Platform
	{
		public new static readonly Guid JitVersionGuid = new Guid("5ed35c58-857b-48dd-a818-7c0136dc9f73");
	}
}
