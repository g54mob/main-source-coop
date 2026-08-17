using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace NomadDrive.Features.Vehicle.UI
{
	public class VehicleDashboardLight : MonoBehaviour
	{
		[SerializeField]
		private DashboardDisplayMode displayMode;

		[SerializeField]
		private float transitionDuration = 0.25f;

		[SerializeField]
		private Image image;

		[SerializeField]
		private Color uiOnColor = Color.green;

		[SerializeField]
		private Color uiOffColor = Color.gray;

		[SerializeField]
		private Renderer meshRenderer;

		[SerializeField]
		private int materialIndex;

		[SerializeField]
		private float emissionOnValue;

		[SerializeField]
		private float emissionOffValue = 1f;

		private bool _isOn;

		private Material _emissionMaterial;

		private static readonly int EmissiveExposureWeightId = Shader.PropertyToID("_EmissiveExposureWeight");

		public bool IsOn => _isOn;

		private void Awake()
		{
			uiOnColor.a = 1f;
			uiOffColor.a = 1f;
			if (displayMode == DashboardDisplayMode.UI)
			{
				if (image == null)
				{
					image = GetComponent<Image>();
				}
			}
			else if (meshRenderer != null)
			{
				_emissionMaterial = meshRenderer.materials[materialIndex];
			}
		}

		public void On()
		{
			if (_isOn)
			{
				return;
			}
			_isOn = true;
			if (displayMode == DashboardDisplayMode.UI)
			{
				if (image != null)
				{
					Tween.Color(image, uiOnColor, transitionDuration);
				}
			}
			else
			{
				AnimateEmission(emissionOnValue);
			}
		}

		public void Off()
		{
			if (!_isOn)
			{
				return;
			}
			_isOn = false;
			if (displayMode == DashboardDisplayMode.UI)
			{
				if (image != null)
				{
					Tween.Color(image, uiOffColor, transitionDuration);
				}
			}
			else
			{
				AnimateEmission(emissionOffValue);
			}
		}

		private void AnimateEmission(float targetValue)
		{
			if (!(_emissionMaterial == null))
			{
				float startValue = _emissionMaterial.GetFloat(EmissiveExposureWeightId);
				Tween.Custom(_emissionMaterial, startValue, targetValue, transitionDuration, delegate(Material m, float val)
				{
					m.SetFloat(EmissiveExposureWeightId, val);
				});
			}
		}
	}
}
