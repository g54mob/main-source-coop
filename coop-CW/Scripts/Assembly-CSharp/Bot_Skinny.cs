using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Bot_Skinny : MonoBehaviour
{
	public UnityEvent turnBlackEvent;

	public float untilNextSwitch = 10f;

	public bool nextWillBeFull;

	private float inDimentionFor = 10f;

	public bool isInDimention;

	private float attackingFor;

	private float dieCounter;

	public Light light;

	public Volume post;

	public Volume post_death;

	public Dictionary<Player, ParticleSystem> targetPlayers = new Dictionary<Player, ParticleSystem>();

	public Transform eyePos;

	private PhotonView view;

	private Player player;

	private Bot bot;

	private bool attacking;

	public ParticleSystem part;

	public bool fullyInDimention;

	private void Start()
	{
		player = GetComponentInParent<Player>();
		bot = GetComponent<Bot>();
		view = GetComponent<PhotonView>();
		SetInDimetion(setInDimention: false);
		light.intensity = 0f;
	}

	private void Update()
	{
		inDimentionFor += Time.deltaTime;
		if (isInDimention)
		{
			Combat();
		}
		if (!fullyInDimention)
		{
			untilNextSwitch -= Time.deltaTime;
		}
		ConfigPost();
		if (!view.IsMine)
		{
			return;
		}
		if (fullyInDimention)
		{
			if (!bot.targetPlayer)
			{
				bot.LookForTarget(eyePos.position, 50f, 400f);
			}
			else
			{
				bot.LookAt(bot.targetPlayer.Center());
				bot.StandStill();
			}
			if (inDimentionFor > 10f || (targetPlayers.Count == 0 && inDimentionFor > 3f))
			{
				ExitDimentionFully();
			}
			if (attacking)
			{
				attackingFor -= Time.deltaTime;
				if (attackingFor <= 0f)
				{
					view.RPC("RPCA_StopAttacking", RpcTarget.All);
				}
			}
			if ((bool)bot.targetPlayer)
			{
				Stare();
			}
		}
		else
		{
			DimentionSwitching();
			_ = isInDimention;
			Patrol();
		}
	}

	private void Patrol()
	{
		Player player = bot.TryToReturnTarget(eyePos.position, 60f, 400f);
		if ((bool)player)
		{
			bot.Patrol(look: false, walk: false, 3f, listenToNoise: false, default(Vector3), alertable: false);
			bot.LookAt(player.Center());
			bot.SetMovementWorld(bot.navDirection_Read);
		}
		else
		{
			bot.Patrol(look: true, walk: true, 3f, listenToNoise: true, default(Vector3), alertable: false);
		}
	}

	private void DimentionSwitching()
	{
		if (untilNextSwitch < 0f)
		{
			view.RPC("DoSwitch", RpcTarget.All, Random.Range(1, 6), nextWillBeFull);
			bool flag = Random.value < 0.5f;
			if (bot.TryToReturnTarget(eyePos.position, 20f, 400f) == null)
			{
				flag = false;
			}
			nextWillBeFull = flag;
			float num = Random.Range(3f, 15f);
			if (Random.value < 0.2f)
			{
				num = Random.Range(15f, 40f);
			}
			if (nextWillBeFull)
			{
				num = 5f;
			}
			view.RPC("SetSwitchData", RpcTarget.All, nextWillBeFull, num);
		}
	}

	[PunRPC]
	private void SetSwitchData(bool setNextWillBeSwitch, float setUntilNext)
	{
		nextWillBeFull = setNextWillBeSwitch;
		untilNextSwitch = setUntilNext;
	}

	[PunRPC]
	private void DoSwitch(int switches, bool endUpInDimention)
	{
		StartCoroutine(DoSwitch(switches, endUpInDimention));
		IEnumerator DoSwitch(int num, bool flag)
		{
			for (int i = 0; i < num; i++)
			{
				yield return new WaitForSeconds(Random.Range(0.02f, 0.1f));
				turnBlackEvent.Invoke();
				Level.currentLevel.ToggleLights(setLightsOn: false, base.transform.position, 60f);
				SetInDimetion(setInDimention: false);
				yield return new WaitForSeconds(Random.Range(0.02f, 0.1f));
				Level.currentLevel.ToggleLights(setLightsOn: true, base.transform.position, 60f);
				if (i < num - 1)
				{
					SetInDimetion(Random.value < 0.5f);
				}
				else if (flag)
				{
					SetInDimetion(flag);
					EnterDimentionFully();
				}
				else
				{
					SetInDimetion(setInDimention: false);
				}
			}
		}
	}

	private void EnterDimentionFully()
	{
		fullyInDimention = true;
	}

	private void ExitDimentionFully()
	{
		view.RPC("RPCA_ExitDimentionFully", RpcTarget.All);
	}

	[PunRPC]
	public void RPCA_ExitDimentionFully()
	{
		foreach (Player key in targetPlayers.Keys)
		{
			if (key.data.possession > 5f)
			{
				if (key.refs.view.IsMine)
				{
					key.Die();
				}
			}
			else
			{
				key.data.possession = 0f;
			}
		}
		fullyInDimention = false;
		SetInDimetion(setInDimention: false);
		ClearTargets();
		StartCoroutine(DoSwitch());
		light.intensity = 0f;
		IEnumerator DoSwitch()
		{
			yield return new WaitForSeconds(Random.Range(0.02f, 0.1f));
			turnBlackEvent.Invoke();
			Level.currentLevel.ToggleLights(setLightsOn: false, base.transform.position, 60f);
			yield return new WaitForSeconds(Random.Range(0.02f, 0.1f));
			Level.currentLevel.ToggleLights(setLightsOn: true, base.transform.position, 60f);
		}
	}

	private void SetInDimetion(bool setInDimention)
	{
		isInDimention = setInDimention;
		player.refs.bodyMeshRenderer.enabled = isInDimention;
		if (setInDimention)
		{
			inDimentionFor = 0f;
			return;
		}
		light.intensity = 0f;
		ClearTargets();
	}

	private void ClearTargets()
	{
		foreach (ParticleSystem value in targetPlayers.Values)
		{
			Object.Destroy(value.gameObject);
		}
		targetPlayers.Clear();
	}

	private void ConfigPost()
	{
		if (targetPlayers.ContainsKey(Player.localPlayer))
		{
			if (dieCounter > 2f)
			{
				post.weight = Mathf.MoveTowards(post.weight, 0.7f, Time.deltaTime * 0.2f);
			}
			if (post.weight < 0.99f)
			{
				post.enabled = true;
			}
			post_death.weight = Mathf.Clamp(dieCounter * 0.1f, 0f, 1f) * 0.28f;
			if (post_death.weight > 0.01f)
			{
				post_death.enabled = true;
			}
			dieCounter += Time.deltaTime;
		}
		else
		{
			post.weight = Mathf.MoveTowards(post.weight, 1f, Time.deltaTime * 0.3f);
			if (post.weight > 0.99f)
			{
				post.enabled = false;
			}
			post_death.weight = Mathf.MoveTowards(post_death.weight, 0f, Time.deltaTime * 0.3f);
			if (post_death.weight < 0.01f)
			{
				post_death.enabled = false;
			}
			dieCounter = 0f;
		}
	}

	private void Combat()
	{
		if (attacking)
		{
			DoAttackingLocal();
		}
		else
		{
			ResetAttackingLocal();
		}
	}

	private void Stare()
	{
		bot.LookAt(bot.targetPlayer.Center());
		bot.StandStill();
		if (inDimentionFor > 0.15f)
		{
			attackingFor = 20f;
			view.RPC("RPCA_StartAttack", RpcTarget.All);
		}
	}

	[PunRPC]
	private void RPCA_StopAttacking()
	{
		attacking = false;
		bot.LoseTarget();
		foreach (Player key in targetPlayers.Keys)
		{
			key.data.overrideMovementInput = new Vector2(0f, 0f);
			key.data.inputOverideAmount = 0f;
		}
		ClearTargets();
	}

	[PunRPC]
	private void RPCA_StartAttack()
	{
		attacking = true;
	}

	private void ResetAttackingLocal()
	{
		ClearTargets();
		light.intensity = Mathf.MoveTowards(light.intensity, 0f, Time.deltaTime * 300f);
	}

	private void DoAttackingLocal()
	{
		light.intensity = Mathf.MoveTowards(light.intensity, 3000f, Time.deltaTime * 200f);
		if (PlayerHandler.instance.playersAlive.Count != 0)
		{
			for (int i = 0; i < PlayerHandler.instance.playersAlive.Count; i++)
			{
				Player p = PlayerHandler.instance.playersAlive[i];
				TryAttackPlayer(p);
			}
		}
	}

	private void TryAttackPlayer(Player p)
	{
		bool flag = p.CanSee(eyePos.position, 120f);
		if (Vector3.Distance(bot.Center(), p.Center()) > 35f)
		{
			flag = false;
		}
		if (!flag && p.data.possession < 1f)
		{
			FailToAttackPlayer(p);
		}
		else
		{
			AttackPlayer(p);
		}
	}

	private void FailToAttackPlayer(Player p)
	{
		if ((bool)p && targetPlayers != null && targetPlayers.ContainsKey(p))
		{
			targetPlayers[p].Stop();
			Object.Destroy(targetPlayers[p].gameObject);
			targetPlayers.Remove(p);
			p.data.overrideMovementInput = new Vector2(0f, 0f);
			p.data.inputOverideAmount = 0f;
		}
	}

	private void AttackPlayer(Player p)
	{
		if (!targetPlayers.ContainsKey(p))
		{
			GameObject gameObject = Object.Instantiate(part.gameObject, part.transform.parent);
			gameObject.SetActive(value: true);
			targetPlayers.Add(p, gameObject.GetComponent<ParticleSystem>());
		}
		ParticleSystem particleSystem = targetPlayers[p];
		Vector3 vector = eyePos.position - p.HeadPosition();
		float value = Vector3.Angle(p.data.lookDirection, vector);
		float num = Mathf.InverseLerp(90f, 30f, value);
		GamefeelHandler.instance.perlin.AddShake(num, 0.02f);
		p.data.overrideMovementInput = new Vector2(0f, 0f);
		p.data.inputOverideAmount = num;
		num *= Mathf.Clamp01(inDimentionFor * 0.2f);
		p.data.possession += Time.deltaTime * 3f * num;
		num = Mathf.Lerp(num, 1f, p.data.possession);
		p.SetLookDirection(Vector3.MoveTowards(p.data.lookDirection, vector, Time.deltaTime * 25f * num));
		particleSystem.transform.position = p.Center();
		ParticleSystem.ShapeModule shape = particleSystem.shape;
		shape.skinnedMeshRenderer = p.refs.bodyMeshRenderer;
		if (!particleSystem.isPlaying)
		{
			particleSystem.Play();
		}
	}
}
