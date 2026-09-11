using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Photon.Pun;
using Photon.Voice.PUN;
using UnityEngine;
using UnityEngine.Serialization;

public class Bot_Angler : MonoBehaviour
{
	public Bot bot;

	public Bot_AnglerMimic mimic;

	public float aggroRange = 50f;

	public float defaultMimicDistance = 3f;

	public Collider suckCollider;

	public ParticleSystem suckPart;

	public Animator suckAnim;

	private Antenna antenna;

	public float suckingRange = 20f;

	public Player mimicingPlayer;

	private bool isSucking;

	private PhotonView m_PhotonView;

	public VoiceRemoteMimic m_RemoteMimic;

	[FormerlySerializedAs("suchPoint")]
	public Transform suckPoint;

	public float minDistanceForPlayerToBeConsideredToBeAlone = 50f;

	public float aggroRangeIfSeperated = 150f;

	public bool IsSucking
	{
		get
		{
			return isSucking;
		}
		set
		{
			if (value != isSucking)
			{
				if (value)
				{
					RPCA_Suck();
				}
				else
				{
					RPCA_StopSuck();
				}
			}
		}
	}

	private void Start()
	{
		m_PhotonView = GetComponent<PhotonView>();
		bot = GetComponent<Bot>();
		m_RemoteMimic = new VoiceRemoteMimic();
		antenna = GetComponent<Antenna>();
		FindMimic();
	}

	public float GetMinDistanceToOtherPlayers(Player player)
	{
		float num = float.MaxValue;
		foreach (Player item in PlayerHandler.instance.playersAlive)
		{
			if (!(item == player))
			{
				float num2 = Vector3.Distance(player.Center(), item.Center());
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	public bool SomeoneIsAlone()
	{
		float num = float.MinValue;
		if (PlayerHandler.instance.playersAlive.Count < 2)
		{
			return true;
		}
		foreach (Player item in PlayerHandler.instance.playersAlive)
		{
			float minDistanceToOtherPlayers = GetMinDistanceToOtherPlayers(item);
			if (minDistanceToOtherPlayers > num)
			{
				num = minDistanceToOtherPlayers;
			}
		}
		if (bot.targetPlayer != null)
		{
			return num + 5f > minDistanceForPlayerToBeConsideredToBeAlone;
		}
		return num > minDistanceForPlayerToBeConsideredToBeAlone;
	}

	private void Update()
	{
		if (mimic == null || !m_PhotonView.IsMine)
		{
			return;
		}
		bot.LookForBetterTarget(bot.Center(), suckingRange, 360f);
		if (bot.targetPlayer != null && mimicingPlayer == null)
		{
			FindAndSetPlayerToMimic();
		}
		if (bot.targetPlayer == null && mimicingPlayer != null)
		{
			Debug.Log("RPCA_RemoveMimic");
			m_PhotonView.RPC("RPCA_RemoveMimic", RpcTarget.All);
		}
		if (!IsSucking && bot.targetPlayer != null && bot.distanceToTarget_Flat <= suckingRange)
		{
			m_PhotonView.RPC("RPCA_Suck", RpcTarget.All);
		}
		if (IsSucking)
		{
			if (bot.targetPlayer == null || bot.distanceToTarget_Flat > suckingRange)
			{
				m_PhotonView.RPC("RPCA_StopSuck", RpcTarget.All);
			}
			foreach (Player item in PlayerHandler.instance.GetAlivePlayersWithinFlatDistanceFromPoint(suckPoint.position, 1f))
			{
				if (item == bot.targetPlayer)
				{
					bot.LoseTarget();
				}
				if (m_PhotonView.IsMine)
				{
					item.Die();
				}
				HelperFunctions.AttachPositionJoint(suckPoint.GetComponent<Rigidbody>(), item.refs.ragdoll.GetBodypart(BodypartType.Torso).GetComponent<Rigidbody>());
			}
		}
		if (bot.targetPlayer == null)
		{
			bot.StandStill();
			bot.LookForTarget(bot.Center(), aggroRange, 360f);
			if (bot.targetPlayer == null)
			{
				List<Player> alivePlayersWithinFlatDistanceFromPoint = PlayerHandler.instance.GetAlivePlayersWithinFlatDistanceFromPoint(base.transform.position, aggroRangeIfSeperated);
				Debug.Log(alivePlayersWithinFlatDistanceFromPoint.Count);
				if (alivePlayersWithinFlatDistanceFromPoint.Count > 0 && SomeoneIsAlone())
				{
					bot.SetTargetPlayer(PlayerHandler.FindClosest(base.transform.position, alivePlayersWithinFlatDistanceFromPoint));
					Debug.Log("found Close player");
				}
			}
			if (bot.targetPlayer == null)
			{
				Player closestAlivePlayerToPoint = PlayerHandler.instance.GetClosestAlivePlayerToPoint(base.transform.position);
				if (closestAlivePlayerToPoint != null)
				{
					bot.LookAt(closestAlivePlayerToPoint.Center());
				}
			}
			return;
		}
		bot.StandStill();
		bot.LookAt(bot.targetPlayer.Center());
		if (bot.distanceToTarget_Flat > aggroRange + 10f)
		{
			if (!SomeoneIsAlone())
			{
				Debug.Log("Lost ");
				bot.LoseTarget();
			}
			else if (bot.distanceToTarget_Flat > aggroRangeIfSeperated + 10f)
			{
				bot.LoseTarget();
			}
		}
	}

	private void FindAndSetPlayerToMimic()
	{
		PhotonView component = bot.targetPlayer.GetComponent<PhotonView>();
		Player furthestPlayerFromPlayer = PlayerHandler.instance.GetFurthestPlayerFromPlayer(bot.targetPlayer);
		if (!(furthestPlayerFromPlayer == null))
		{
			Debug.Log("Calling RPC_Mimic");
			m_PhotonView.RPC("RPC_Mimic", RpcTarget.All, component.ViewID, furthestPlayerFromPlayer.refs.view.ViewID);
		}
	}

	private void OnDestroy()
	{
		RPCA_RemoveMimic();
	}

	[PunRPC]
	public void RPCA_Suck()
	{
		suckCollider.enabled = true;
		suckPart.Play();
		suckAnim.SetBool("Inhale", value: true);
		isSucking = true;
	}

	[PunRPC]
	public void RPCA_StopSuck()
	{
		suckCollider.enabled = false;
		suckPart.Stop();
		suckAnim.SetBool("Inhale", value: false);
		isSucking = false;
	}

	private void SpawnMimic()
	{
		Vector3 vector = base.transform.position + base.transform.forward * 10f;
		vector = HelperFunctions.GetGroundPos(vector + Vector3.up * 10f, HelperFunctions.LayerType.TerrainProp);
		PhotonNetwork.Instantiate("AnglerMimic", vector, base.transform.rotation, 0).GetComponentInChildren<Bot_AnglerMimic>().AssignAngler(this);
	}

	private void FindMimic()
	{
		float num = float.MaxValue;
		Bot_AnglerMimic bot_AnglerMimic = null;
		foreach (Bot_AnglerMimic item in Object.FindObjectsByType<Bot_AnglerMimic>(FindObjectsSortMode.None).ToList())
		{
			if (!item.HasAnglerDaddy)
			{
				float num2 = Vector3.Distance(item.transform.position, base.transform.position);
				if (num2 < num)
				{
					num = num2;
					bot_AnglerMimic = item;
				}
			}
		}
		mimic = bot_AnglerMimic;
		if (mimic == null)
		{
			PhotonNetwork.Destroy(bot.transform.root.gameObject);
		}
		else
		{
			mimic.AssignAngler(this);
		}
	}

	[PunRPC]
	private void RPCA_RemoveMimic()
	{
		Debug.Log("RPCA_RemoeMimic");
		m_RemoteMimic.ResetMimicTargets();
		mimicingPlayer = null;
	}

	[PunRPC]
	private void RPC_Mimic(int targetViewID, int mimicViewID)
	{
		RPCA_RemoveMimic();
		PhotonView photonView = PhotonView.Find(targetViewID);
		PhotonView photonView2 = PhotonView.Find(mimicViewID);
		mimicingPlayer = photonView2.GetComponentInParent<Player>();
		PlayerVisor visor = mimicingPlayer.refs.visor;
		mimic.transform.root.GetComponentInChildren<PlayerVisor>().SetAllFaceSettings(visor.hue.Value, visor.visorColorIndex, visor.visorFaceText.text, visor.FaceRotation, visor.FaceSize);
		Debug.Log("Got RPC Mimic: Target: " + photonView?.ToString() + " MIMIC: " + photonView2);
		int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
		if (actorNumber != photonView.OwnerActorNr && actorNumber != photonView2.OwnerActorNr)
		{
			Debug.Log("Got Angler Mimic event but is not target nor Mimic");
			return;
		}
		if (actorNumber == photonView2.OwnerActorNr)
		{
			Debug.Log("I AM THE MIMIC, I SHOULD HEAR THE TARGETS VOICE " + photonView);
			Transform parent = photonView2.GetComponentInChildren<PhotonVoiceView>().transform.parent;
			m_RemoteMimic.MakeMimicTargets(photonView, parent);
			m_RemoteMimic.SwitchMimics();
			return;
		}
		if (actorNumber == photonView.OwnerActorNr)
		{
			Debug.Log("I AM THE ANGLER TARGET, I SHOULD HEAR: " + photonView2?.ToString() + " s VOICE FROM THE MIMIC");
		}
		else
		{
			Debug.Log("I AM CLOSE TO THE ANGLER TARGET, I SHOULD HEAR: " + photonView2?.ToString() + " s VOICE FROM THE MIMIC");
		}
		m_RemoteMimic.MakeMimicTargets(photonView2, mimic.transform);
		m_RemoteMimic.SwitchMimics();
	}
}
