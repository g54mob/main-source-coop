using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Detection
{
	[NetworkBehaviourWeaved(0)]
	public class DetectedPlayersTimeSystem : MonoSystem
	{
		private IDetectionContext _detectionContext;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(IDetectionContext detectionContext)
		{
			_detectionContext = detectionContext;
		}

		public override void Enable()
		{
			_isEnabled = true;
			_detectionContext.OnDetectedPlayersChanged += OnDetectedPlayersChanged;
			OnDetectedPlayersChanged();
		}

		public override void Disable()
		{
			_isEnabled = false;
			_detectionContext.OnDetectedPlayersChanged -= OnDetectedPlayersChanged;
			Clear();
		}

		public override void Clear()
		{
			_detectionContext.ClearDetectedPlayersFirstSeenTime();
		}

		private void OnDetectedPlayersChanged()
		{
			if (!base.Initialized || !_isEnabled)
			{
				return;
			}
			List<PlayerDataHolder> detectedPlayers = _detectionContext.DetectedPlayers;
			foreach (PlayerDataHolder item in detectedPlayers)
			{
				if (!_detectionContext.DetectedPlayerFirstDetectedTime.ContainsKey(item))
				{
					_detectionContext.SetDetectedPlayerFirstSeenTime(item, Time.time);
				}
			}
			List<PlayerDataHolder> list = new List<PlayerDataHolder>();
			foreach (PlayerDataHolder key in _detectionContext.DetectedPlayerFirstDetectedTime.Keys)
			{
				if (!detectedPlayers.Contains(key))
				{
					list.Add(key);
				}
			}
			foreach (PlayerDataHolder item2 in list)
			{
				_detectionContext.RemoveDetectedPlayerFirstSeenTime(item2);
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
