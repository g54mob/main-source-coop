using Cinemachine;
using UnityEngine;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")]
public class LockCameraX : CinemachineExtension
{
	[Tooltip("Lock the camera's X position to this value")]
	public float m_XPosition = 10f;

	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (stage == CinemachineCore.Stage.Body)
		{
			Vector3 rawPosition = state.RawPosition;
			rawPosition.x = m_XPosition;
			state.RawPosition = rawPosition;
		}
	}
}
