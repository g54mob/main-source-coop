using System;

namespace DunGen.Collision
{
	[Serializable]
	public abstract class BroadphaseSettings
	{
		public abstract ICollisionBroadphase Create();
	}
}
