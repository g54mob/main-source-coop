using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceCollisionEffect_Piston : SurfaceCollisionEffect
{
	public float fall = 2f;

	public float force = 35f;

	public float damage = 51f;

	public float playerCooldown = 1f;

	private List<Player> hitPlayers = new List<Player>();

	public override void CollideWithSurface(Collision col, Bodypart part)
	{
		Player componentInParent = part.transform.GetComponentInParent<Player>();
		if ((bool)componentInParent && !componentInParent.ai && !hitPlayers.Contains(componentInParent) && componentInParent.refs.view.IsMine)
		{
			componentInParent.CallTakeDamageAndAddForceAndFall(damage, (componentInParent.Center() - base.transform.position).normalized * force, fall);
			StartCoroutine(IHoldPlayer(componentInParent));
		}
		IEnumerator IHoldPlayer(Player p)
		{
			hitPlayers.Add(p);
			yield return new WaitForSeconds(playerCooldown);
			hitPlayers.Remove(p);
		}
	}
}
