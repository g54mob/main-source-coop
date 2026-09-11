using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStickTrigger : MonoBehaviour
{
	public List<Player> ignoredPlayers = new List<Player>();

	private void OnTriggerStay(Collider other)
	{
		if (!other.isTrigger)
		{
			Player component = other.transform.root.GetComponent<Player>();
			if ((bool)component && !other.GetComponentInParent<ItemInstance>() && !ignoredPlayers.Contains(component))
			{
				StartCoroutine(IgnoreForAbit(component));
				GetComponentInParent<ShockStick>().OnShock(component);
			}
		}
		IEnumerator IgnoreForAbit(Player player)
		{
			if ((bool)player)
			{
				ignoredPlayers.Add(player);
			}
			yield return new WaitForSeconds(1f);
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
