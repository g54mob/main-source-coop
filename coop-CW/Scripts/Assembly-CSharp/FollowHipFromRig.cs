using UnityEngine;

public class FollowHipFromRig : MonoBehaviour
{
	public Transform main;

	private Player player;

	private void Start()
	{
		player = main.GetComponent<Player>();
	}

	private void LateUpdate()
	{
		if ((bool)player)
		{
			base.transform.position = player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.transform.position;
		}
	}
}
