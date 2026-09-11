using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
	private Player player;

	private void Start()
	{
		player = GetComponentInParent<Player>();
	}

	private void LateUpdate()
	{
		base.transform.position = player.refs.ragdoll.GetBodypart(BodypartType.Hip).rig.position;
	}
}
