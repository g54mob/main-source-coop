using UnityEngine;

namespace Features.SkinConfiguration.Scripts
{
	public class CustomizationAnimationStateBehavior : StateMachineBehaviour
	{
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			CustomizationAnimationReactor component = animator.GetComponent<CustomizationAnimationReactor>();
			if (stateInfo.IsName("Crounch_Squash_Enter"))
			{
				component.InvokeOnCrouchInStart();
			}
			if (stateInfo.IsName("Hide_Squash_Loop"))
			{
				component.InvokeOnCrouchLoopStart();
			}
			if (stateInfo.IsName("Crounch_Squash_Exit"))
			{
				component.InvokeOnCrouchOutStart();
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			CustomizationAnimationReactor component = animator.GetComponent<CustomizationAnimationReactor>();
			if (stateInfo.IsName("Crounch_Squash_Enter"))
			{
				component.InvokeOnCrouchInEnd();
			}
			if (stateInfo.IsName("Hide_Squash_Loop"))
			{
				component.InvokeOnCrouchLoopEnd();
			}
			if (stateInfo.IsName("Crounch_Squash_Exit"))
			{
				component.InvokeOnCrouchOutEnd();
			}
		}
	}
}
