using UnityEngine;

public class SimplePlayerAnimations : MonoBehaviour
{
	private SimplePlayer player;

	private void Start()
	{
		player = GetComponent<SimplePlayer>();
	}

	private void Update()
	{
		player.refs.modelRoot.rotation = Quaternion.LookRotation(player.data.playerLookForward.Flat());
	}
}
