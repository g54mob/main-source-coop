using Fusion;
using UnityEngine;

namespace Features.AnimationModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public abstract class NetworkedAnimationControllerBase : NetworkBehaviour
	{
		public abstract void PlayAnimation(AnimationType animationType);

		public abstract void ResetAnimation(AnimationType animationType);

		public abstract void PlayAnimationLocal(AnimationType animationType);

		public abstract void ResetAnimationLocal(AnimationType animationType);

		public abstract void SetFloat(string parameter, float value);

		public abstract void SetFloat(int parameter, float value);

		public abstract float GetFloat(string parameter);

		public abstract void SetBool(string parameter, bool value);

		public abstract void SetBool(int parameter, bool value);

		public abstract AnimatorStateInfo GetCurrentAnimatorStateInfo(string layerName);

		public abstract AnimatorStateInfo GetNextAnimatorStateInfo(string layerName);

		public abstract bool IsInTransition(string attack);

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
