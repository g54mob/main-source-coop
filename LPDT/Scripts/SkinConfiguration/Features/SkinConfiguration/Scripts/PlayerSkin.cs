using System.Collections.Generic;
using UnityEngine;

namespace Features.SkinConfiguration.Scripts
{
	public class PlayerSkin : MonoBehaviour
	{
		[SerializeField]
		private List<SkinnedMeshRenderer> _crouchMeshRenderers;

		private CustomizationAnimationReactor _reactor;

		public void Initialize(CustomizationAnimationReactor reactor)
		{
			_reactor = reactor;
			AddListeners();
		}

		private void OnDestroy()
		{
			RemoveListeners();
		}

		private void AddListeners()
		{
			_reactor.OnCrouchInEnd += HideCrouchClothPart;
			_reactor.OnCrouchOutStart += ShowCrouchPart;
			_reactor.OnCrouchLoopStart += HideCrouchClothPart;
			_reactor.OnCustomizationReset += CustomizationReset;
		}

		private void RemoveListeners()
		{
			if (_reactor != null)
			{
				_reactor.OnCrouchInEnd -= HideCrouchClothPart;
				_reactor.OnCrouchOutStart -= ShowCrouchPart;
				_reactor.OnCrouchLoopStart -= HideCrouchClothPart;
				_reactor.OnCustomizationReset -= CustomizationReset;
			}
		}

		private void HideCrouchClothPart()
		{
			foreach (SkinnedMeshRenderer crouchMeshRenderer in _crouchMeshRenderers)
			{
				crouchMeshRenderer.enabled = false;
			}
		}

		private void ShowCrouchPart()
		{
			foreach (SkinnedMeshRenderer crouchMeshRenderer in _crouchMeshRenderers)
			{
				crouchMeshRenderer.enabled = true;
			}
		}

		private void CustomizationReset()
		{
			ShowCrouchPart();
		}
	}
}
