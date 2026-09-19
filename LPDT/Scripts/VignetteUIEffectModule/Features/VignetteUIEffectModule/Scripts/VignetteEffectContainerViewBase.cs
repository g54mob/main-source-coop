using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.VignetteUIEffectModule.Scripts
{
	public abstract class VignetteEffectContainerViewBase : ViewBehaviour
	{
		[SerializeField]
		private Transform _effectsRoot;

		public Transform GetEffectsRoot()
		{
			return _effectsRoot;
		}
	}
}
