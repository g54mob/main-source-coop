using DefaultNamespace.Artifacts;
using Photon.Pun;
using UnityEngine;

public class CursedPlayerBoss : MonoBehaviour
{
	private Player player_gp;

	public void SpawnCurse(ItemInstanceBehaviour artifact, GameObject cursePrefab)
	{
		Debug.Log("SpawnCurse");
		GameObject obj = PhotonNetwork.Instantiate(cursePrefab.name, Vector3.zero, Quaternion.identity, 0);
		Debug.Log("SpawnedCurse");
		obj.GetComponent<IArtifactCurse>().CastCurse(artifact, player_gp);
	}

	private void Awake()
	{
		player_gp = GetComponentInParent<Player>();
	}
}
