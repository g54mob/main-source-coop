using Photon.Pun;
using TMPro;
using UnityEngine;

public class Attacks_BlackHole : MonoBehaviour
{
	public GameObject toSpawn;

	public TextMeshPro[] texts;

	private Bot bot;

	private Player player;

	private PhotonView view;

	private MonsterAnimationHandler anim;

	private float counter = 10f;

	public float range = 5f;

	private float angryFor;

	private bool counting;

	private bool exploding;

	private float explodeTime;

	private bool done;

	private bool isAggro;

	private void Start()
	{
		anim = GetComponentInParent<MonsterAnimationHandler>();
		view = GetComponent<PhotonView>();
		bot = GetComponent<Bot>();
		player = GetComponentInParent<Player>();
	}

	[PunRPC]
	private void RPCA_SpawnBlackHole()
	{
		Object.Instantiate(toSpawn, bot.Center(), Quaternion.identity);
	}

	private void Update()
	{
		if (!done)
		{
			if (view.IsMine)
			{
				SyncAggro();
			}
			LocalCode();
		}
	}

	private void SetTexts(string text)
	{
		for (int i = 0; i < texts.Length; i++)
		{
			texts[i].text = text;
		}
	}

	private void LocalCode()
	{
		if (exploding)
		{
			explodeTime += Time.deltaTime;
			SetTexts(">:D");
			if (explodeTime > 1f)
			{
				if (view.IsMine)
				{
					view.RPC("RPCA_SpawnBlackHole", RpcTarget.All);
				}
				done = true;
			}
			return;
		}
		if (counting)
		{
			if (bot.distanceToTarget < range + 4f)
			{
				angryFor = 0f;
				counter = Mathf.MoveTowards(counter, 0f, Time.deltaTime);
				SetTexts(Mathf.Ceil(counter).ToString());
				if (counter <= 0.1f)
				{
					exploding = true;
				}
				return;
			}
			SetTexts(">:(");
			angryFor += Time.deltaTime;
			if (angryFor > 3f)
			{
				counting = false;
				counter = 10f;
				bot.attacking = false;
			}
			return;
		}
		counter = 10f;
		SetTexts(">:)");
		if (bot.aggro && bot.distanceToTarget < range)
		{
			if (view.IsMine)
			{
				bot.StandStill();
			}
			counting = true;
			bot.attacking = true;
		}
	}

	private void SyncAggro()
	{
		if (bot.aggro != isAggro)
		{
			view.RPC("RPCA_SetAggro", RpcTarget.All, bot.aggro);
		}
	}

	[PunRPC]
	private void RPCA_SetAggro(bool setAggro)
	{
		isAggro = setAggro;
		bot.aggro = setAggro;
	}
}
