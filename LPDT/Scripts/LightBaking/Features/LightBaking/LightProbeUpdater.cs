using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.LightBaking
{
	public class LightProbeUpdater : MonoBehaviour
	{
		[SerializeField]
		private Vector3 _initialPosition;

		[SerializeField]
		private bool _autoUpdate = true;

		private LightProbes _lightProbes;

		private Vector3[] _originalPositions;

		private Vector3 _lastPosition;

		private bool _isReady;

		private void Reset()
		{
			_initialPosition = base.transform.position;
		}

		private void Start()
		{
			InitializeProbes();
			UpdateProbePositions();
		}

		private void Update()
		{
			if (_isReady && _autoUpdate && base.transform.position != _lastPosition)
			{
				UpdateProbePositions();
			}
		}

		private void InitializeProbes()
		{
			Scene scene = base.gameObject.scene;
			if (!scene.IsValid())
			{
				Debug.LogWarning("[LightProbeUpdater] Scene of this GameObject is not valid.");
				return;
			}
			_lightProbes = LightProbes.GetInstantiatedLightProbesForScene(scene);
			if (_lightProbes == null)
			{
				Debug.LogWarning("[LightProbeUpdater] No LightProbes found in scene '" + scene.name + "'.");
				return;
			}
			_originalPositions = _lightProbes.GetPositionsSelf();
			_lastPosition = _initialPosition;
			_isReady = true;
		}

		private void UpdateProbePositions()
		{
			Vector3[] array = new Vector3[_originalPositions.Length];
			for (int i = 0; i < _originalPositions.Length; i++)
			{
				array[i] = _originalPositions[i] + base.transform.position;
			}
			_lightProbes.SetPositionsSelf(array, checkForDuplicatePositions: true);
			_lastPosition = base.transform.position;
			LightProbes.TetrahedralizeAsync();
		}

		public void ForceUpdate()
		{
			if (_isReady)
			{
				UpdateProbePositions();
			}
		}

		public void ResetPositions()
		{
			if (_isReady)
			{
				_lightProbes.SetPositionsSelf(_originalPositions, checkForDuplicatePositions: true);
				LightProbes.TetrahedralizeAsync();
			}
		}
	}
}
