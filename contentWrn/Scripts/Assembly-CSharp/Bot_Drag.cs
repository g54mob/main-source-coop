using UnityEngine;

public class Bot_Drag : MonoBehaviour
{
	public float handForce = 10f;

	public float dragForce = 10f;

	public float range = 1.5f;

	private Bot bot;

	private Player player;

	private Bot_Zombie zom;

	private float strength = 3f;

	private float counter;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		zom = GetComponent<Bot_Zombie>();
		range *= range;
	}

	private void FixedUpdate()
	{
		if (player.NoControl())
		{
			return;
		}
		bool flag = false;
		if ((bool)bot.targetPlayer && bot.attacking && Vector3.SqrMagnitude(bot.Center() - bot.targetPlayer.Center()) < range)
		{
			float num = Mathf.Clamp(strength, 0f, 2f);
			flag = true;
			Rigidbody rig = bot.targetPlayer.refs.ragdoll.GetBodypart(BodypartType.Torso).rig;
			Vector3 normalized = (bot.Center() - rig.position).normalized;
			rig.AddForce(normalized * (num * dragForce), ForceMode.Acceleration);
			Rigidbody rig2 = player.refs.ragdoll.GetBodypart(BodypartType.Elbow_L).rig;
			Rigidbody rig3 = player.refs.ragdoll.GetBodypart(BodypartType.Elbow_R).rig;
			Vector3 normalized2 = (rig.position - rig2.worldCenterOfMass).normalized;
			Vector3 normalized3 = (rig.position - rig3.worldCenterOfMass).normalized;
			rig2.AddForce(normalized2 * (num * handForce), ForceMode.Acceleration);
			rig3.AddForce(normalized3 * (num * handForce), ForceMode.Acceleration);
			counter += Time.fixedDeltaTime;
			if (counter > 0.75f)
			{
				if (bot.targetPlayer.IsLocal)
				{
					bot.targetPlayer.CallTakeDamage(4f);
				}
				counter = 0f;
			}
			strength = Mathf.MoveTowards(strength, 0.5f, Time.fixedDeltaTime * 0.5f);
		}
		if (!flag)
		{
			counter = Mathf.MoveTowards(counter, 0f, Time.fixedDeltaTime);
			strength = Mathf.MoveTowards(strength, 3f, Time.fixedDeltaTime);
		}
	}
}
