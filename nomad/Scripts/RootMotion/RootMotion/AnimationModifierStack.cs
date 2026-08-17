using System;
using UnityEngine;

namespace RootMotion
{
	public class AnimationModifierStack : MonoBehaviour
	{
		public AnimationModifier[] modifiers = new AnimationModifier[0];

		private Animator animator;

		private Baker baker;

		private void Start()
		{
			animator = GetComponent<Animator>();
			baker = GetComponent<Baker>();
			Baker obj = baker;
			obj.OnStartClip = (Baker.BakerDelegate)Delegate.Combine(obj.OnStartClip, new Baker.BakerDelegate(OnBakerStartClip));
			Baker obj2 = baker;
			obj2.OnUpdateClip = (Baker.BakerDelegate)Delegate.Combine(obj2.OnUpdateClip, new Baker.BakerDelegate(OnBakerUpdateClip));
			AnimationModifier[] array = modifiers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnInitiate(baker, animator);
			}
		}

		private void OnBakerStartClip(AnimationClip clip, float normalizedTime)
		{
			AnimationModifier[] array = modifiers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnStartClip(clip);
			}
		}

		private void OnBakerUpdateClip(AnimationClip clip, float normalizedTime)
		{
			AnimationModifier[] array = modifiers;
			foreach (AnimationModifier animationModifier in array)
			{
				if (animationModifier.enabled)
				{
					animationModifier.OnBakerUpdate(normalizedTime);
				}
			}
		}

		private void LateUpdate()
		{
			if ((!animator.enabled && !baker.isBaking) || (baker.isBaking && baker.mode == Baker.Mode.AnimationClips) || animator.runtimeAnimatorController == null)
			{
				return;
			}
			float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
			AnimationModifier[] array = modifiers;
			foreach (AnimationModifier animationModifier in array)
			{
				if (animationModifier.enabled)
				{
					animationModifier.OnBakerUpdate(normalizedTime);
				}
			}
		}
	}
}
