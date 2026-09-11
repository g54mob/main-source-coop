using UnityEngine;
using UnityEngine.Rendering;

public class FadeToBlackTransition : MonoBehaviour
{
	private Volume vol;

	private float playerDeadFor;

	private void Start()
	{
		vol = GetComponent<Volume>();
	}

	private void Update()
	{
		if (!Player.localPlayer)
		{
			return;
		}
		bool flag = false;
		if (Player.localPlayer.data.dead && !Spectate.spectating)
		{
			playerDeadFor += Time.deltaTime;
			if (playerDeadFor > 2f)
			{
				flag = true;
			}
		}
		else
		{
			playerDeadFor = 0f;
		}
		if (flag)
		{
			vol.weight = Mathf.MoveTowards(vol.weight, 1f, Time.deltaTime);
		}
		else
		{
			vol.weight = Mathf.MoveTowards(vol.weight, 0f, Time.deltaTime);
		}
	}
}
