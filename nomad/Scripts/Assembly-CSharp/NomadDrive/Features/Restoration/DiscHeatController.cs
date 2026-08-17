using UnityEngine;

namespace NomadDrive.Features.Restoration
{
	public class DiscHeatController : MonoBehaviour
	{
		[SerializeField]
		private DiscRotationAnimator discAnimator;

		[SerializeField]
		private Renderer discRenderer;

		[SerializeField]
		private int materialIndex;

		[SerializeField]
		[Range(0f, 5f)]
		private float heatUpSpeed = 2f;

		[SerializeField]
		[Range(0f, 5f)]
		private float coolDownSpeed = 1f;

		private static readonly int HeatAmountProperty = Shader.PropertyToID("_HeatAmount");

		private MaterialPropertyBlock _propertyBlock;

		private float _currentHeat;

		private void Update()
		{
			if (!(discAnimator == null) && !(discRenderer == null))
			{
				float normalizedSpeed = discAnimator.NormalizedSpeed;
				float num = ((normalizedSpeed > _currentHeat) ? heatUpSpeed : coolDownSpeed);
				_currentHeat = Mathf.MoveTowards(_currentHeat, normalizedSpeed, num * Time.deltaTime);
				if (_propertyBlock == null)
				{
					_propertyBlock = new MaterialPropertyBlock();
				}
				discRenderer.GetPropertyBlock(_propertyBlock, materialIndex);
				_propertyBlock.SetFloat(HeatAmountProperty, _currentHeat);
				discRenderer.SetPropertyBlock(_propertyBlock, materialIndex);
			}
		}
	}
}
