using DefaultNamespace.Artifacts;
using Photon.Pun;
using Portningsbolaget.Platforms;
using UnityEngine;

public class CurseOfShroom : MonoBehaviour, IArtifactCurse
{
	public Mesh replacementMesh;

	public Material replacementMaterial;

	public Player cursedPlayer;

	public ItemInstanceBehaviour itemSource;

	private PhotonView view_g;

	public SFX_Instance[] transformSound;

	public SFX_Instance[] voiceSounds_Low;

	public SFX_Instance[] voiceSounds_Mid;

	public SFX_Instance[] voiceSounds_High;

	private float untilNextSound;

	private float sinceSound;

	private float cd = 0.5f;

	private float sinceAttack;

	private bool inited;

	private GenericAttack attack;

	private bool hasActivated;

	private float allDeadFor;

	public void CastCurse(ItemInstanceBehaviour cursedItem, Player playerHoldingItem)
	{
		itemSource = cursedItem;
		if (playerHoldingItem.refs.view.IsMine)
		{
			view_g.RPC("RPCA_AttachToPlayer", RpcTarget.All, playerHoldingItem.refs.view.OwnerActorNr);
		}
	}

	private void Update()
	{
		if (cursedPlayer == null || !inited)
		{
			return;
		}
		if (cursedPlayer.IsLocal && !cursedPlayer.data.dead && PlayerHandler.instance.playersAlive.Count < 2)
		{
			allDeadFor += Time.deltaTime;
			if (allDeadFor > 2f)
			{
				cursedPlayer.CallDie();
			}
		}
		if (!hasActivated)
		{
			if (!PlayerHandler.instance.CanAnAlivePlayerSeePoint(cursedPlayer.Center(), out var _, cursedPlayer))
			{
				ActivateShroom();
			}
			return;
		}
		if (!cursedPlayer.refs.view.IsMine)
		{
			Sounds();
		}
		if (cursedPlayer.refs.view.IsMine)
		{
			Combat();
		}
	}

	private void Sounds()
	{
		untilNextSound -= Time.deltaTime * cursedPlayer.data.microphoneValue * 2f;
		sinceSound += Time.deltaTime;
		if (untilNextSound < 0f && sinceSound > cd)
		{
			untilNextSound = 1f;
			sinceSound = 0f;
			if (cursedPlayer.data.microphoneValue > 0.8f)
			{
				voiceSounds_High[Random.Range(0, voiceSounds_High.Length)].Play(cursedPlayer.Center(), local: false, 1f, cursedPlayer.refs.ragdoll.GetBodypart(BodypartType.Torso).rig.transform);
			}
			else if (cursedPlayer.data.microphoneValue > 0.4f)
			{
				voiceSounds_Mid[Random.Range(0, voiceSounds_Mid.Length)].Play(cursedPlayer.Center(), local: false, 1f, cursedPlayer.refs.ragdoll.GetBodypart(BodypartType.Torso).rig.transform);
			}
			else
			{
				voiceSounds_Low[Random.Range(0, voiceSounds_Low.Length)].Play(cursedPlayer.Center(), local: false, 1f, cursedPlayer.refs.ragdoll.GetBodypart(BodypartType.Torso).rig.transform);
			}
		}
	}

	private void ActivateShroom()
	{
		if (!hasActivated)
		{
			for (int i = 0; i < transformSound.Length; i++)
			{
				transformSound[i].Play(cursedPlayer.refs.ragdoll.GetBodypart(BodypartType.Torso).rig.transform.position);
			}
		}
		if (!cursedPlayer.refs.view.IsMine)
		{
			base.transform.root.Find("CharacterModel/HeadRenderer").gameObject.SetActive(value: false);
			base.transform.root.Find("CharacterModel/HeadRendererShadow").gameObject.SetActive(value: false);
			SkinnedMeshRenderer component = base.transform.root.Find("CharacterModel/BodyRenderer").GetComponent<SkinnedMeshRenderer>();
			component.sharedMesh = replacementMesh;
			component.sharedMaterials = new Material[1] { replacementMaterial };
			cursedPlayer.data.voiceVolumeModifier = 0f;
			cursedPlayer.data.looksLikeShroomMonster = true;
			cursedPlayer.RPCA_EquipHat(-1);
		}
		hasActivated = true;
	}

	private void Combat()
	{
		float closestDistance;
		Player player = PlayerHandler.instance.FindClosestPlayerToPlayer(cursedPlayer, out closestDistance);
		sinceAttack += Time.deltaTime;
		if ((bool)player && !(closestDistance > 4f) && !(sinceAttack < 1f))
		{
			sinceAttack = 0f;
			if (cursedPlayer.IsLocal)
			{
				attack.CallAttack(player);
			}
		}
	}

	public void Awake()
	{
		view_g = GetComponent<PhotonView>();
	}

	[PunRPC]
	private void RPCA_AttachToPlayer(int playerid)
	{
		if (PlayerHandler.instance.TryGetPlayerFromOwnerID(playerid, out var o))
		{
			base.transform.parent = o.refs.curses.transform;
			cursedPlayer = o;
			attack = GetComponent<GenericAttack>();
			attack.enabled = true;
			inited = true;
			PlatformManager.UnlockAchievement(Achievements.ACH_SHROOM);
		}
	}
}
