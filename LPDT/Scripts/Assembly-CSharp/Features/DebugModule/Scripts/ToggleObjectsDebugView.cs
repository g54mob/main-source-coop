using UnityEngine;
using UnityEngine.UI;

namespace Features.DebugModule.Scripts
{
	public class ToggleObjectsDebugView : ToggleObjectsDebugViewBase
	{
		[SerializeField]
		private Toggle _toggleAdditionalLights;

		[SerializeField]
		private Toggle _toggleObiRopes;

		[SerializeField]
		private Toggle _toggleGeometry;

		[SerializeField]
		private Toggle _togglePostProcessing;

		[SerializeField]
		private Toggle _toggleLatchGrab;

		protected override void OnEnable()
		{
			base.OnEnable();
			_toggleAdditionalLights.onValueChanged.AddListener(OnToggleAdditionalLightsChanged);
			_toggleObiRopes.onValueChanged.AddListener(OnToggleObiRopesChanged);
			_toggleGeometry.onValueChanged.AddListener(OnToggleGeometryChanged);
			_togglePostProcessing.onValueChanged.AddListener(OnTogglePostProcessingChanged);
			_toggleLatchGrab.onValueChanged.AddListener(OnToggleLatchGrabChanged);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_toggleAdditionalLights.onValueChanged.RemoveListener(OnToggleAdditionalLightsChanged);
			_toggleObiRopes.onValueChanged.RemoveListener(OnToggleObiRopesChanged);
			_toggleGeometry.onValueChanged.RemoveListener(OnToggleGeometryChanged);
			_togglePostProcessing.onValueChanged.RemoveListener(OnTogglePostProcessingChanged);
			_toggleLatchGrab.onValueChanged.RemoveListener(OnToggleLatchGrabChanged);
		}

		private void OnToggleAdditionalLightsChanged(bool value)
		{
			OnToggleAdditionalLights?.Invoke(value);
		}

		private void OnToggleObiRopesChanged(bool value)
		{
			OnToggleObiRopes?.Invoke(value);
		}

		private void OnToggleGeometryChanged(bool value)
		{
			OnToggleGeometry?.Invoke(value);
		}

		private void OnTogglePostProcessingChanged(bool value)
		{
			OnTogglePostProcessing?.Invoke(value);
		}

		private void OnToggleLatchGrabChanged(bool value)
		{
			OnToggleLatchGrab?.Invoke(value);
		}

		public override void RefreshLatchGrabToggle(bool isEnabled)
		{
			if (_toggleLatchGrab != null)
			{
				_toggleLatchGrab.isOn = isEnabled;
			}
		}
	}
}
