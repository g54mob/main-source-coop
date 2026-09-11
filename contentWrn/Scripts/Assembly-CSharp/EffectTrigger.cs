using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectTrigger : MonoBehaviour
{
	public float damage;

	public float tase;

	public float fall;

	public UnityEvent eventToCall;

	public float perTargetCooldown = 1f;

	internal List<Player> ignoredPlayers = new List<Player>();

	private void OnTriggerStay(Collider other)
	{
		if (!(other.transform.root == base.transform.root) && !other.isTrigger)
		{
			Player component = other.transform.root.GetComponent<Player>();
			if ((bool)component && !other.GetComponentInParent<ItemInstance>() && !ignoredPlayers.Contains(component))
			{
				component.CallTakeDamageAndTase(damage, tase);
				eventToCall.Invoke();
				StartCoroutine(IgnoreForAbit(component));
			}
		}
		IEnumerator IgnoreForAbit(Player player)
		{
			if ((bool)player)
			{
				ignoredPlayers.Add(player);
			}
			yield return new WaitForSeconds(perTargetCooldown);
			if ((bool)player && ignoredPlayers.Contains(player))
			{
				ignoredPlayers.Remove(player);
			}
		}
	}

	private void OnDisable()
	{
		ignoredPlayers.Clear();
	}
}
