using UnityEngine;

public class BlackHole : MonoBehaviour
{
	private Transform inner;

	private Transform scale;

	public float force;

	private float shakeCounter;

	private void Start()
	{
		scale = base.transform.Find("Scale");
		inner = base.transform.Find("Scale/Inner");
	}

	private void FixedUpdate()
	{
		float range = scale.localScale.x * 0.5f;
		float innerRange = range * inner.localScale.x;
		range = range * 1.2f + 20f;
		float value = Vector3.Distance(base.transform.position, MainCamera.instance.transform.position);
		float num = Mathf.InverseLerp(range, innerRange, value);
		shakeCounter += Time.deltaTime;
		if (shakeCounter > 0.5f)
		{
			shakeCounter = 0f;
			if (num > 0f)
			{
				GamefeelHandler.instance.perlin.AddShake(num * num * 5f, 1f);
			}
		}
		for (int i = 0; i < PlayerHandler.instance.players.Count; i++)
		{
			ActOnPlayer(PlayerHandler.instance.players[i]);
		}
		for (int j = 0; j < BotHandler.instance.bots.Count; j++)
		{
			Player component = BotHandler.instance.bots[j].transform.root.GetComponent<Player>();
			if ((bool)component)
			{
				ActOnPlayer(component);
			}
		}
		void ActOnPlayer(Player player)
		{
			if (player.gameObject.activeSelf)
			{
				float num2 = Vector3.Distance(base.transform.position, player.Center());
				if (!(num2 > range))
				{
					float num3 = Mathf.InverseLerp(range, innerRange, num2);
					if (num2 < innerRange * 0.9f && innerRange > 3f)
					{
						if (player.ai)
						{
							player.gameObject.SetActive(value: false);
						}
						else if (player.IsLocal && !player.data.dead)
						{
							player.CallDie();
						}
					}
					Vector3 vector = base.transform.position - player.Center();
					player.GetRig(BodypartType.Hip).AddForce(vector * (force * num3 * num3), ForceMode.Acceleration);
				}
			}
		}
	}
}
