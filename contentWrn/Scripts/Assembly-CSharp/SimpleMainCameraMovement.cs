using UnityEngine;

public class SimpleMainCameraMovement : MonoBehaviour
{
	private bool inited;

	private Vector3 wobbleForward;

	private Vector3 vel;

	private void Start()
	{
		if ((bool)SimplePlayer.localPlayer && !inited)
		{
			inited = true;
			wobbleForward = SimplePlayer.localPlayer.data.playerLookForward;
		}
	}

	private void LateUpdate()
	{
		if (!inited)
		{
			Start();
			return;
		}
		base.transform.position = SimplePlayer.localPlayer.refs.cameraPoint.transform.position;
		FRILerp.DirectionSpring(ref wobbleForward, SimplePlayer.localPlayer.data.playerLookForward, 5f, 10f, ref vel);
		base.transform.rotation = Quaternion.LookRotation(Vector3.Lerp(SimplePlayer.localPlayer.data.playerLookForward, wobbleForward, 0.1f));
		base.transform.localEulerAngles += GamefeelHandler.instance.GetEulerOffsets();
		base.transform.position += GamefeelHandler.instance.GetPositionOffsets();
	}
}
