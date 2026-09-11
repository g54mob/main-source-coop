using Photon.Pun;
using UnityEngine;

public class SimplePlayerSyncer : MonoBehaviour, IPunObservable
{
	private SimplePlayer player;

	private Vector3 targetPos;

	private float distance;

	private Vector2 targetLook;

	private float lookDistance;

	private void Start()
	{
		player = GetComponent<SimplePlayer>();
		if (!player.data.isLocalPlayer)
		{
			player.refs.rig.isKinematic = true;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(player.data.playerLookValues);
			return;
		}
		targetPos = (Vector3)stream.ReceiveNext();
		distance = Vector3.Distance(base.transform.position, targetPos);
		targetLook = (Vector2)stream.ReceiveNext();
		lookDistance = Vector2.Distance(player.data.playerLookValues, targetLook);
	}

	private void Update()
	{
		if (!player.data.isLocalPlayer)
		{
			float num = 1f / (float)PhotonNetwork.SerializationRate;
			float num2 = distance / num;
			base.transform.position = Vector3.MoveTowards(base.transform.position, targetPos, num2 * Time.deltaTime);
			float num3 = lookDistance / num;
			player.data.playerLookValues = Vector2.MoveTowards(player.data.playerLookValues, targetLook, num3 * Time.deltaTime);
		}
	}
}
