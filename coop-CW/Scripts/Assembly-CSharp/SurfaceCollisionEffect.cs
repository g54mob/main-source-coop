using UnityEngine;

public abstract class SurfaceCollisionEffect : MonoBehaviour
{
	public bool stopRagdollSounds;

	public abstract void CollideWithSurface(Collision col, Bodypart part);
}
