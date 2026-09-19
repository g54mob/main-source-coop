using System.Collections.Generic;
using UnityEngine;

namespace Features.AnimationModule.Scripts
{
	public class CompositeAnimator : MonoBehaviour
	{
		[SerializeField]
		private List<Animator> _animators;

		[SerializeField]
		private Animator _referenceAnimator;

		public void SetTrigger(int id)
		{
			_animators.ForEach(delegate(Animator animator)
			{
				animator.SetTrigger(id);
			});
		}

		public void SetBool(int id, bool value)
		{
			_animators.ForEach(delegate(Animator animator)
			{
				animator.SetBool(id, value);
			});
		}

		public void SetFloat(int id, float value)
		{
			_animators.ForEach(delegate(Animator animator)
			{
				animator.SetFloat(id, value);
			});
		}

		public void SetFloat(string paramName, float value)
		{
			_animators.ForEach(delegate(Animator animator)
			{
				animator.SetFloat(paramName, value);
			});
		}

		public void SetInteger(int id, int value)
		{
			_animators.ForEach(delegate(Animator animator)
			{
				animator.SetInteger(id, value);
			});
		}

		public void Play(int stateNameHash, int layer, float normalizedTime)
		{
			_animators.ForEach(delegate(Animator animator)
			{
				animator.Play(stateNameHash, layer, normalizedTime);
			});
		}

		public bool GetBool(int id)
		{
			return _referenceAnimator.GetBool(id);
		}

		public void SetLayerWeight(int layer, float value)
		{
			_animators.ForEach(delegate(Animator animator)
			{
				animator.SetLayerWeight(layer, value);
			});
		}

		public void SetLayerWeight(string layerName, float value)
		{
			_animators.ForEach(delegate(Animator animator)
			{
				int layerIndex = animator.GetLayerIndex(layerName);
				animator.SetLayerWeight(layerIndex, value);
			});
		}

		public int GetLayerIndex(string layerName)
		{
			return _referenceAnimator.GetLayerIndex(layerName);
		}

		public AnimatorStateInfo GetCurrentAnimatorStateInfo(string layerName)
		{
			int layerIndex = _referenceAnimator.GetLayerIndex(layerName);
			return _referenceAnimator.GetCurrentAnimatorStateInfo(layerIndex);
		}

		public AnimatorStateInfo GetNextAnimatorStateInfo(string layerName)
		{
			int layerIndex = _referenceAnimator.GetLayerIndex(layerName);
			return _referenceAnimator.GetNextAnimatorStateInfo(layerIndex);
		}

		public void ResetTrigger(int id)
		{
			_animators.ForEach(delegate(Animator animator)
			{
				animator.ResetTrigger(id);
			});
		}
	}
}
