using UnityEngine;

namespace Features.JacuzziBeachInteractableModule.Scripts.VisualMusic
{
	public class OrbitVisualMusicBehaviour : VisualMusicBehaviour
	{
		[SerializeField]
		private Vector3 _rotationAxis = Vector3.up;

		[SerializeField]
		private float _degreesPerSecond = 90f;

		protected override void OnEffectStarted()
		{
		}

		protected override void OnEffectTick(float deltaTime)
		{
			LightsRoot.Rotate(_rotationAxis.normalized, _degreesPerSecond * base.Blend * deltaTime, Space.Self);
		}

		protected override void OnEffectStopped()
		{
		}
	}
}
