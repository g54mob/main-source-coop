using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Tools
{
	public class TireRepairKit : RepairKit
	{
		[SerializeField]
		private Transform tool1Transform;

		[SerializeField]
		private Transform tool2Transform;

		[SerializeField]
		private Transform tool3Transform;

		public void ExecuteToolWalkingAnimations(float speed)
		{
			Tween.LocalRotation(endValue: Quaternion.Euler(new Vector3(0f, 0f, 20f)), target: tool1Transform, duration: speed, ease: Ease.InBounce, cycles: -1, cycleMode: CycleMode.Yoyo);
			Tween.LocalRotation(endValue: Quaternion.Euler(new Vector3(0f, 0f, -20f)), target: tool2Transform, duration: speed, ease: Ease.OutBounce, cycles: -1, cycleMode: CycleMode.Yoyo);
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
