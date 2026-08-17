using System;
using System.Collections.Generic;

namespace NomadDrive.Features.ColliderLOD
{
	public static class ColliderLodRegistry
	{
		private static readonly HashSet<IColliderLodTarget> Registered = new HashSet<IColliderLodTarget>();

		public static IReadOnlyCollection<IColliderLodTarget> Targets => Registered;

		public static event Action<IColliderLodTarget> OnRegistered;

		public static event Action<IColliderLodTarget> OnUnregistered;

		public static void Register(IColliderLodTarget target)
		{
			if (target != null && Registered.Add(target))
			{
				ColliderLodRegistry.OnRegistered?.Invoke(target);
			}
		}

		public static void Unregister(IColliderLodTarget target)
		{
			if (target != null && Registered.Remove(target))
			{
				ColliderLodRegistry.OnUnregistered?.Invoke(target);
			}
		}
	}
}
