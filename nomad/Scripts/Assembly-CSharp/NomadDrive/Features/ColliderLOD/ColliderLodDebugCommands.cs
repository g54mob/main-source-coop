using QFSW.QC;

namespace NomadDrive.Features.ColliderLOD
{
	public static class ColliderLodDebugCommands
	{
		[Command("lod.toggle", "Toggle the collider LOD system on/off (kill-switch for Profiler A/B).", Platform.AllPlatforms, MonoTargetType.Single)]
		public static void Toggle()
		{
			if (TryGet(out var manager))
			{
				manager.SetEnabled(!manager.IsEnabled);
			}
		}

		[Command("lod.on", "Enable the collider LOD system.", Platform.AllPlatforms, MonoTargetType.Single)]
		public static void On()
		{
			if (TryGet(out var manager))
			{
				manager.SetEnabled(enabled: true);
			}
		}

		[Command("lod.off", "Disable the collider LOD system (restores all colliders).", Platform.AllPlatforms, MonoTargetType.Single)]
		public static void Off()
		{
			if (TryGet(out var manager))
			{
				manager.SetEnabled(enabled: false);
			}
		}

		[Command("lod.stats", "Print collider LOD stats (managed / active / players / enabled).", Platform.AllPlatforms, MonoTargetType.Single)]
		public static void Stats()
		{
			TryGet(out var _);
		}

		[Command("lod.setRadius", "Set the base activation radius in metres (re-resolves all entries).", Platform.AllPlatforms, MonoTargetType.Single)]
		public static void SetRadius(float radius)
		{
			if (TryGet(out var manager))
			{
				manager.SetActivationRadiusRuntime(radius);
			}
		}

		private static bool TryGet(out ColliderLODManager manager)
		{
			manager = ColliderLODManager.Instance;
			if (manager == null)
			{
				return false;
			}
			return true;
		}
	}
}
