using UnityEngine;

public class FearHandler : MonoBehaviour
{
	public float m_volumeFactor = 0.25f;

	private AudioLoop[] sources;

	public SFX_Instance scareOneShot;

	private float afraidAmount;

	private float shakeCounter;

	private Bot fearBot;

	private float counter = 10f;

	private void Start()
	{
		sources = GetComponentsInChildren<AudioLoop>();
	}

	private void Update()
	{
		counter += Time.deltaTime;
		if (!(Player.localPlayer == null))
		{
			if ((bool)fearBot)
			{
				CheckFearBot();
			}
			else
			{
				GetFearBot();
			}
			CalculateFear();
			ApplyFear();
		}
	}

	private void CheckFearBot()
	{
		if (fearBot.targetPlayer != Player.localPlayer)
		{
			fearBot = null;
		}
	}

	private void GetFearBot()
	{
		for (int i = 0; i < Player.localPlayer.data.fearList.Count; i++)
		{
			Bot bot = Player.localPlayer.data.fearList[i];
			bool flag = true;
			if (!bot.aggro)
			{
				flag = false;
			}
			if (bot.sinceLastSawTarget > 2f)
			{
				flag = false;
			}
			if (Vector3.Angle(Player.localPlayer.data.lookDirection, bot.Center() - Player.localPlayer.Center()) > 80f)
			{
				flag = false;
			}
			if (Vector3.Distance(bot.Center(), Player.localPlayer.Center()) > 30f)
			{
				flag = false;
			}
			if (flag)
			{
				if (fearBot == null && counter > 5f)
				{
					scareOneShot.Play(bot.Center());
					counter = 0f;
				}
				fearBot = bot;
				break;
			}
		}
	}

	private void CalculateFear()
	{
		if ((bool)fearBot)
		{
			afraidAmount = Mathf.MoveTowards(afraidAmount, 1f, Time.deltaTime);
		}
		else
		{
			afraidAmount = Mathf.MoveTowards(afraidAmount, 0f, Time.deltaTime * 0.3f);
		}
	}

	private void ApplyFear()
	{
		if (afraidAmount > 0f)
		{
			shakeCounter += Time.deltaTime;
			if (shakeCounter > 0.2f)
			{
				GamefeelHandler.instance.perlin.AddShake(0.05f * afraidAmount, 0.4f);
			}
		}
		for (int i = 0; i < sources.Length; i++)
		{
			sources[i].volume = afraidAmount * m_volumeFactor;
			if ((bool)fearBot)
			{
				sources[i].transform.position = fearBot.Center();
			}
		}
	}
}
