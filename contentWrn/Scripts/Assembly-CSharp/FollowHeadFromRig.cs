using UnityEngine;

public class FollowHeadFromRig : MonoBehaviour
{
	public Transform main;

	private Player player;

	private void Start()
	{
		player = main.GetComponent<Player>();
	}

	private void Update()
	{
		if ((bool)player)
		{
			base.transform.position = player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.position;
		}
	}
}
