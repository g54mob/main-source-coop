using Features.LiquidModule.Scripts;
using UnityEngine;

namespace Features.RumModule.Scripts
{
	public class RumVisualController : MonoBehaviour
	{
		private const string FILL_AMOUNT = "_FillAmount";

		[SerializeField]
		private Renderer _liquidRenderer;

		[SerializeField]
		private Liquid _liquid;

		[SerializeField]
		private float _defaultFillAmount = 0.5f;

		private Material _instanceMaterial;

		private void OnEnable()
		{
			_instanceMaterial = _liquidRenderer.material;
			ResetProgress();
		}

		public void SetProgress(float progress)
		{
			float num = Mathf.Lerp(_defaultFillAmount, 0.74f, progress);
			_instanceMaterial.SetFloat("_FillAmount", num);
			_liquid.FillAmount = num;
		}

		public void ResetProgress()
		{
			_instanceMaterial.SetFloat("_FillAmount", _defaultFillAmount);
			_liquid.FillAmount = _defaultFillAmount;
		}

		public void DisableRenderer(bool disable)
		{
			_liquidRenderer.enabled = disable;
		}
	}
}
