using System.Collections.Generic;

namespace Features.PhysicsVolumeModule.Scripts
{
	public static class CartFlowZoneProvider
	{
		private static readonly List<CartFlowZone> _zones = new List<CartFlowZone>();

		public static IReadOnlyList<CartFlowZone> Zones => _zones;

		public static void Register(CartFlowZone zone)
		{
			if (!_zones.Contains(zone))
			{
				_zones.Add(zone);
			}
		}

		public static void Unregister(CartFlowZone zone)
		{
			_zones.Remove(zone);
		}
	}
}
