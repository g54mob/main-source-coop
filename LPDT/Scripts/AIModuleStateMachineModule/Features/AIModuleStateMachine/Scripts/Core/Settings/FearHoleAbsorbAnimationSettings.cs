using DG.Tweening;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Core.Settings
{
	[CreateAssetMenu(fileName = "FearHoleAbsorbAnimationSettings_Default", menuName = "Configurations/AIModuleStateMachine/Core/FearHoleAbsorbAnimationSettings")]
	public class FearHoleAbsorbAnimationSettings : ScriptableObject
	{
		[field: SerializeField]
		public float StartDistance { get; private set; } = 2f;

		[field: SerializeField]
		public float ScaleDuration { get; private set; } = 0.35f;

		[field: SerializeField]
		public float EndScale { get; private set; } = 0.05f;

		[field: SerializeField]
		public Ease ScaleEase { get; private set; } = Ease.InQuad;
	}
}
