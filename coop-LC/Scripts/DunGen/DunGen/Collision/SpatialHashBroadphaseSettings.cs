using System;
using System.ComponentModel;

namespace DunGen.Collision
{
	[Serializable]
	[DisplayName("Spatial Hashing")]
	public class SpatialHashBroadphaseSettings : BroadphaseSettings
	{
		public float CellSize = 40f;

		public override ICollisionBroadphase Create()
		{
			return new SpatialHashBroadphase();
		}
	}
}
