using Photon.Pun;
using UnityEngine;

public class RemoveAfterSeconds : MonoBehaviour
{
	public float seconds = 5f;

	public bool photonRemove;

	public bool disable;

	private float time;

	private void OnEnable()
	{
		time = seconds;
	}

	private void Update()
	{
		time -= Time.deltaTime;
		if (time < 0f)
		{
			if (photonRemove && GetComponent<PhotonView>().IsMine)
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
			else if (disable)
			{
				base.gameObject.SetActive(value: false);
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
