using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace.Artifacts
{
	public class CurseOfGravityFlip : MonoBehaviour, IArtifactCurse
	{
		public float upAmount = 1f;

		[SerializeField]
		private float rarity;

		[SerializeField]
		private float budgetCost;

		public Player cursedPlayer;

		public ItemInstanceBehaviour itemInstanceBehaviourSource;

		public Item itemSource;

		private PhotonView view_g;

		public float killTime = 60f;

		private float timeSinceLastDamage;

		public float Rarity => rarity;

		public float BudgetCost => budgetCost;

		public void CastCurse(ItemInstanceBehaviour cursedItem, Player playerHoldingItem)
		{
			itemInstanceBehaviourSource = cursedItem;
			itemSource = itemInstanceBehaviourSource.itemInstance.item;
			Debug.Log($"CastCurse {base.gameObject.name} from {cursedItem} on {playerHoldingItem.name}", cursedPlayer);
			if (playerHoldingItem.refs.view.IsMine)
			{
				view_g.RPC("RPCA_AttachToPlayer", RpcTarget.All, playerHoldingItem.refs.view.OwnerActorNr);
			}
		}

		private void FixedUpdate()
		{
			if (!(cursedPlayer == null))
			{
				Debug.Log("Player has Flippted Gravity " + Time.frameCount);
				cursedPlayer.refs.ragdoll.AddForce(Vector3.up * upAmount, ForceMode.Acceleration);
				cursedPlayer.data.sinceGrounded = Mathf.Clamp(Player.localPlayer.data.sinceGrounded * 5f, 0f, 1f);
			}
		}

		private void Update()
		{
			if (!(cursedPlayer == null))
			{
				timeSinceLastDamage += Time.deltaTime;
				if (timeSinceLastDamage > 1f)
				{
					float damage = Player.PlayerData.maxHealth / killTime;
					cursedPlayer.CallTakeDamage(damage);
					timeSinceLastDamage = 0f;
				}
				if (cursedPlayer.data.dead)
				{
					PhotonNetwork.Destroy(base.gameObject);
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
				Debug.Log("Attaching to player " + o.name, o);
				cursedPlayer = o;
				base.transform.parent = o.refs.curses.transform;
			}
		}
	}
}
