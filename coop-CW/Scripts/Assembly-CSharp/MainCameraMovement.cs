using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{
	private void LateUpdate()
	{
		if (!(Player.localPlayer == null))
		{
			base.transform.SetPositionAndRotation(Player.localPlayer.refs.cameraPos.transform.position, Quaternion.LookRotation(Player.localPlayer.data.lookDirection));
			if (Player.localPlayer.data.CameraPhysicsAmount() > 0.01f)
			{
				Quaternion rotation = Player.localPlayer.refs.ragdoll.GetBodypart(BodypartType.Head).rig.transform.rotation;
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, rotation, Player.localPlayer.data.CameraPhysicsAmount());
				base.transform.Rotate(base.transform.right * ((0f - Player.localPlayer.data.playerLookValues.y) * Player.localPlayer.data.CameraPhysicsAmount()), Space.World);
			}
			base.transform.localEulerAngles += GamefeelHandler.instance.GetEulerOffsets();
			base.transform.position += GamefeelHandler.instance.GetPositionOffsets();
		}
	}
}
