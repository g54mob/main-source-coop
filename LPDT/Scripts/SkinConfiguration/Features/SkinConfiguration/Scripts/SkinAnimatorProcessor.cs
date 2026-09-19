using System.Collections.Generic;
using UnityEngine;

namespace Features.SkinConfiguration.Scripts
{
	public class SkinAnimatorProcessor : MonoBehaviour
	{
		private readonly List<bool> _skinnedMeshRenderersStates = new List<bool>();

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private List<SkinnedMeshRenderer> _skinnedMeshRenderers;

		[SerializeField]
		private CustomizationAnimationReactor _customizationAnimationReactor;

		private bool _lastAnimatorState;

		private void Awake()
		{
			_lastAnimatorState = _animator.enabled;
			for (int i = 0; i < _skinnedMeshRenderers.Count; i++)
			{
				SkinnedMeshRenderer skinnedMeshRenderer = _skinnedMeshRenderers[i];
				_skinnedMeshRenderersStates.Add(skinnedMeshRenderer.enabled);
			}
		}

		private void Update()
		{
			if (_animator.enabled != _lastAnimatorState)
			{
				ProcessAnimatorEnabledChanged(_animator.enabled);
				_lastAnimatorState = _animator.enabled;
			}
		}

		private void ProcessAnimatorEnabledChanged(bool currentState)
		{
			if (!currentState)
			{
				for (int i = 0; i < _skinnedMeshRenderers.Count; i++)
				{
					_skinnedMeshRenderers[i].enabled = _skinnedMeshRenderersStates[i];
				}
				_customizationAnimationReactor.ResetCustomizationAnimationState();
			}
		}
	}
}
