using UnityEngine;

namespace DunGen.Collision
{
	public delegate bool AdditionalCollisionsPredicate(Bounds tileBounds, bool isCollidingWithDungeon);
}
