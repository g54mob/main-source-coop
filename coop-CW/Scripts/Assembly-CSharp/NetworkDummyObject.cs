using Photon.Pun;
using UnityEngine;

public class NetworkDummyObject : MonoBehaviour
{
	public string objectName;

	private void Start()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.Instantiate(objectName, base.transform.position, base.transform.rotation, 0);
		}
		Object.Destroy(base.gameObject);
	}
}
