using UnityEngine;

public class TurnToLocalPlayer : MonoBehaviour
{
	private void Update()
	{
		if (!(Player.localPlayer == null))
		{
			base.transform.rotation = Quaternion.LookRotation(MainCamera.instance.transform.position - base.transform.position);
		}
	}
}
