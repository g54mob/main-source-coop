using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	public static class MapBounds
	{
		public static bool AnyBaked
		{
			get
			{
				IReadOnlyList<MapBoundsVolume> active = MapBoundsVolume.Active;
				for (int i = 0; i < active.Count; i++)
				{
					if (active[i] != null && active[i].IsBaked)
					{
						return true;
					}
				}
				return false;
			}
		}

		public static bool IsInsidePlayArea(Vector3 worldPoint)
		{
			IReadOnlyList<MapBoundsVolume> active = MapBoundsVolume.Active;
			bool flag = false;
			for (int i = 0; i < active.Count; i++)
			{
				MapBoundsVolume mapBoundsVolume = active[i];
				if (!(mapBoundsVolume == null) && mapBoundsVolume.IsBaked)
				{
					flag = true;
					if (mapBoundsVolume.Contains(worldPoint))
					{
						return true;
					}
				}
			}
			return !flag;
		}

		public static Vector3 SamplePointFor(Transform player)
		{
			if (player == null)
			{
				return Vector3.zero;
			}
			CharacterController component = player.GetComponent<CharacterController>();
			if (!(component != null))
			{
				return player.position;
			}
			return player.position + player.TransformVector(component.center);
		}
	}
}
