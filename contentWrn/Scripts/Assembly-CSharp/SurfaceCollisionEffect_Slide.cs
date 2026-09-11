using System.Collections.Generic;
using UnityEngine;

public class SurfaceCollisionEffect_Slide : SurfaceCollisionEffect
{
	public float fall = 2f;

	public float force = 35f;

	public float damage = 51f;

	public float playerCooldown = 1f;

	private List<Player> hitPlayers = new List<Player>();

	private Transform point;

	private void Start()
	{
		point = base.transform.GetChild(0);
	}

	public override void CollideWithSurface(Collision col, Bodypart part)
	{
		Player componentInParent = part.transform.GetComponentInParent<Player>();
		if ((bool)componentInParent && !componentInParent.ai && !(componentInParent.data.sinceGotBackUp < 2f) && !(Vector3.Distance(part.rig.position, point.position) > 4f) && !(Vector3.Angle(col.contacts[0].normal, Vector3.up) > 60f))
		{
			componentInParent.refs.ragdoll.Fall(3f);
		}
	}
}
