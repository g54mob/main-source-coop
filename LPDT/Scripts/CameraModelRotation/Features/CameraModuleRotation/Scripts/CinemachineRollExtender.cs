using Unity.Cinemachine;
using UnityEngine;

namespace Features.CameraModuleRotation.Scripts
{
	public class CinemachineRollExtender : CinemachineExtension
	{
		[field: SerializeField]
		public float RollValue { get; set; }

		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
			if (stage == CinemachineCore.Stage.Finalize)
			{
				Quaternion rawOrientation = state.RawOrientation;
				rawOrientation *= Quaternion.Euler(0f, 0f, RollValue);
				state.RawOrientation = rawOrientation;
			}
		}
	}
}
