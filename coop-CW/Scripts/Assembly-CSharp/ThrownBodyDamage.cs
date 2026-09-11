using System;
using UnityEngine;

public class ThrownBodyDamage : MonoBehaviour
{
	public float damage;

	public float force;

	internal Vector3 direction;

	private Player player;

	private bool done;

	private void Start()
	{
		player = GetComponent<Player>();
		PlayerRagdoll ragdoll = player.refs.ragdoll;
		ragdoll.collisionAction = (Action<Collision, Bodypart>)Delegate.Combine(ragdoll.collisionAction, new Action<Collision, Bodypart>(Collide));
	}

	private void OnDestroy()
	{
		PlayerRagdoll ragdoll = player.refs.ragdoll;
		ragdoll.collisionAction = (Action<Collision, Bodypart>)Delegate.Remove(ragdoll.collisionAction, new Action<Collision, Bodypart>(Collide));
	}

	private void Collide(Collision collision, Bodypart bodypart)
	{
		if (done)
		{
			return;
		}
		if ((bool)collision.rigidbody)
		{
			Player component = collision.transform.root.GetComponent<Player>();
			if ((bool)component)
			{
				if (component.ai)
				{
					return;
				}
				component.CallTakeDamageAndAddForceAndFall(damage, force * direction.normalized, 3f);
			}
		}
		player.CallTakeDamageAndAddForceAndFall(damage, Vector3.zero, 2f);
		done = true;
		UnityEngine.Object.Destroy(this);
	}
}
