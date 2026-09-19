using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.HeadcrabEnemy.Settings
{
	[CreateAssetMenu(fileName = "HeadcrabVignetteSettings_Default", menuName = "Configurations/AIModuleStateMachine/Headcrab/HeadcrabVignetteSettings")]
	public class HeadcrabVignetteSettings : ScriptableObject
	{
		[field: SerializeField]
		public AnimationCurve VignetteFadeCurve { get; private set; } = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
	}
}
