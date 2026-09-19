using UnityEngine;

namespace Features.CameraModelModule
{
	[CreateAssetMenu(fileName = "CameraTransitionPreset_Default", menuName = "Configurations/CamerModelModule/CameraTransitionPreset")]
	public class CameraTransitionPreset : ScriptableObject
	{
		[field: SerializeField]
		public CameraTransitionViewType TransitionViewType { get; private set; }

		[field: SerializeField]
		public PlayerRenderTransition PlayerRenderTransition { get; private set; }

		[field: SerializeField]
		public CameraSkinUpdateTiming CameraSkinUpdateTimingOverride { get; private set; }
	}
}
