using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeTrigger : MonoBehaviour
{
	public float initialDamage = 5f;

	internal List<Player> ignoredPlayers = new List<Player>();

	public bool ignoreForever = true;

	public float ignoreForSeconds;

	public void OnTriggerStay(Collider other)
	{
		if (other.isTrigger)
		{
			return;
		}
		Player componentInParent = other.GetComponentInParent<Player>();
		if ((bool)componentInParent && !componentInParent.ai && !ignoredPlayers.Contains(componentInParent) && componentInParent.IsLocal)
		{
			if (!ignoreForever)
			{
				StartCoroutine(IgnoreForAbit(componentInParent));
			}
			else
			{
				ignoredPlayers.Add(componentInParent);
			}
			componentInParent.CallTakeDamage(initialDamage);
		}
		IEnumerator IgnoreForAbit(Player player)
		{
			if ((bool)player)
			{
				ignoredPlayers.Add(player);
			}
			yield return new WaitForSeconds(ignoreForSeconds);
			if ((bool)player && ignoredPlayers.Contains(player))
			{
				ignoredPlayers.Remove(player);
			}
		}
	}
}
