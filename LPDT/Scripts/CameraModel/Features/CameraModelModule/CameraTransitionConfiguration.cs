using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.CameraModelModule
{
	[CreateAssetMenu(fileName = "CameraTransitionConfiguration_Default", menuName = "Configurations/CamerModelModule/CameraTransitionConfiguration")]
	public class CameraTransitionConfiguration : ScriptableObject
	{
		[Serializable]
		public class CameraTransitionData
		{
			[field: SerializeField]
			public CameraTransitionKey CameraTransitionKey { get; set; }

			[field: SerializeField]
			public CameraTransitionPreset CameraTransitionPreset { get; set; }
		}

		[SerializeField]
		private List<CameraTransitionData> _cameraTransitions;

		private readonly Dictionary<CameraTransitionKey, CameraTransitionPreset> _cameraTransitionsMap = new Dictionary<CameraTransitionKey, CameraTransitionPreset>();

		private bool _isMapBuilt;

		[field: SerializeField]
		public CameraSkinUpdateTiming CameraSkinUpdateTiming { get; private set; } = CameraSkinUpdateTiming.TransitionStart;

		public IReadOnlyDictionary<CameraTransitionKey, CameraTransitionPreset> CameraTransitionsMap
		{
			get
			{
				EnsureMapBuilt();
				return _cameraTransitionsMap;
			}
		}

		private void OnEnable()
		{
			BuildTransitionsMap();
		}

		private void EnsureMapBuilt()
		{
			if (!_isMapBuilt)
			{
				BuildTransitionsMap();
			}
		}

		private void BuildTransitionsMap()
		{
			_cameraTransitionsMap.Clear();
			if (_cameraTransitions == null)
			{
				_isMapBuilt = true;
				return;
			}
			foreach (CameraTransitionData cameraTransition in _cameraTransitions)
			{
				_cameraTransitionsMap[cameraTransition.CameraTransitionKey] = cameraTransition.CameraTransitionPreset;
			}
			_isMapBuilt = true;
		}
	}
}
