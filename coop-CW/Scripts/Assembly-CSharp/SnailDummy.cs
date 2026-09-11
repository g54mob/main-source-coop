using System.Collections;
using Photon.Pun;
using UnityEngine;

public class SnailDummy : MonoBehaviour
{
	private PhotonView view;

	public GameObject snailDeleter;

	private bool done;

	private void Awake()
	{
		view = GetComponent<PhotonView>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.isTrigger && !done && view.IsMine)
		{
			Player component = other.transform.root.GetComponent<Player>();
			if ((bool)component && !component.ai)
			{
				StartCoroutine(ISpawn());
				done = true;
			}
		}
		IEnumerator ISpawn()
		{
			yield return new WaitForEndOfFrame();
			PhotonView component2 = PhotonNetwork.Instantiate("Zombe", base.transform.position, base.transform.rotation, 0).GetComponent<PhotonView>();
			view.RPC("RPCA_SpawnSnail", RpcTarget.All, component2.ViewID);
		}
	}

	[PunRPC]
	private void RPCA_SpawnSnail(int snailView)
	{
		PhotonView photonView = PhotonNetwork.GetPhotonView(snailView);
		Object.Instantiate(snailDeleter, photonView.transform.position, photonView.transform.rotation, photonView.transform);
		base.gameObject.SetActive(value: false);
		MonoFunctions.instance.PhotonDestroy(base.gameObject, 1f);
	}

	public void RemoveSnail()
	{
		if (view.IsMine)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
