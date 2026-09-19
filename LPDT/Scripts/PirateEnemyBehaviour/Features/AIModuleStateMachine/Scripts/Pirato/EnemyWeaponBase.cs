using Features.AnimationModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.Pirato
{
	[NetworkBehaviourWeaved(0)]
	public abstract class EnemyWeaponBase : NetworkBehaviour
	{
		public abstract AnimationType DefaultAnimationType { get; }

		public abstract AnimationType BottomAnimationType { get; }

		public abstract bool IsWithAiming { get; }

		public abstract float SphereCastRadius { get; }

		public abstract Transform SphereCastPosition { get; }

		public abstract void EnableWeaponVisual(bool enable);

		public abstract void Attack(Vector3 position);

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
