using UnityEngine;

public class PlayerGroundPositionTransform : MonoBehaviour
{
	private Player player;

	private void Start()
	{
		player = GetComponentInParent<Player>();
	}

	private void LateUpdate()
	{
		base.transform.position = player.CenterGroundPos();
	}
}
