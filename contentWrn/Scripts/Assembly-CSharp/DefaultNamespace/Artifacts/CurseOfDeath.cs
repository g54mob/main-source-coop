using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace.Artifacts
{
	public class CurseOfDeath : MonoBehaviour, IArtifactCurse
	{
		public Player cursedPlayer;

		public ItemInstanceBehaviour itemSource;

		private PhotonView view_g;

		public float minHoldTime = 3f;

		private float timeInHand;

		public void CastCurse(ItemInstanceBehaviour cursedItem, Player playerHoldingItem)
		{
			itemSource = cursedItem;
			cursedPlayer = playerHoldingItem;
			Debug.Log($"CastCurse {base.gameObject.name} from {cursedItem} on {playerHoldingItem.name}", cursedPlayer);
			if (playerHoldingItem.refs.view.IsMine)
			{
				view_g.RPC("RPCA_AttachToPlayer", RpcTarget.All, playerHoldingItem.refs.view.OwnerActorNr);
			}
		}

		private void Update()
		{
			if (!(cursedPlayer == null) && cursedPlayer.refs.view.IsMine)
			{
				timeInHand += Time.deltaTime;
				Debug.Log("timeInHand: " + timeInHand);
				if (itemSource == null || !itemSource.isHeld)
				{
					Debug.Log("Calling Die on player holding item.");
					PhotonNetwork.Destroy(base.gameObject);
				}
				if (timeInHand > minHoldTime)
				{
					Debug.Log("Calling Die on player holding item.");
					cursedPlayer.CallDie();
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
				base.transform.parent = o.refs.curses.transform;
			}
		}
	}
}
