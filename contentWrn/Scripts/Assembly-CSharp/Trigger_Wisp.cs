using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Wisp : MonoBehaviour
{
	public float cooldownPerPlayer = 0.5f;

	public float damage = 6f;

	public float force = 10f;

	private List<Player> hitPlayers = new List<Player>();

	private Bot_ToolkitBoy tool;

	private void Start()
	{
		tool = base.transform.root.GetComponentInChildren<Bot_ToolkitBoy>();
	}

	private void OnTriggerStay(Collider col)
	{
		Player componentInParent = col.GetComponentInParent<Player>();
		if (!col.isTrigger && (bool)componentInParent && componentInParent.refs.view.IsMine && !hitPlayers.Contains(componentInParent) && !(col.transform.root == base.transform.root) && tool.isCharging)
		{
			Vector3 normalized = (componentInParent.Center() - tool.bot.Center()).Flat().normalized;
			normalized.y = 0.5f;
			componentInParent.CallTakeDamageAndAddForceAndFall(damage, normalized * force, 2.5f);
			GamefeelHandler.instance.perlin.AddShake(5f, 0.6f);
			StartCoroutine(IHoldPlayer(componentInParent));
		}
		IEnumerator IHoldPlayer(Player p)
		{
			hitPlayers.Add(p);
			yield return new WaitForSeconds(cooldownPerPlayer);
			hitPlayers.Remove(p);
		}
	}
}
