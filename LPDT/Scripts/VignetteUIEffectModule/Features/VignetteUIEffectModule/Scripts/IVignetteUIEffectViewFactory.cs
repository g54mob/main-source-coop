using UnityEngine;

namespace Features.VignetteUIEffectModule.Scripts
{
	public interface IVignetteUIEffectViewFactory
	{
		IVignetteUIEffectVisual CreateLayer(VignetteUIEffectType effectType, Transform root);
	}
}
