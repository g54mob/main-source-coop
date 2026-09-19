using System;
using System.Collections.Generic;
using Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Data;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Data
{
	public class HeadManTargetsModel : MonoBehaviour
	{
		private readonly Dictionary<PlayerRef, TargetDetectData> _headManTargets = new Dictionary<PlayerRef, TargetDetectData>();

		private readonly List<PlayerRef> _headManTargetList = new List<PlayerRef>();

		private PlayerRef[] _prioritizedTargetArray = Array.Empty<PlayerRef>();

		private SessionAnalyticsModel _sessionAnalyticsModel;

		public IReadOnlyDictionary<PlayerRef, TargetDetectData> HeadManTargets => _headManTargets;

		public IReadOnlyList<PlayerRef> HeadManTargetsList => _headManTargetList;

		public IReadOnlyList<PlayerRef> PrioritizedTargets => _prioritizedTargetArray;

		public event Action<TargetDetectData> OnTargetRegistered;

		public event Action<PlayerRef> OnTargetRemoved;

		public event Action OnPrioritizeChanged;

		[Inject]
		private void InjectDependencies(SessionAnalyticsModel sessionAnalyticsModel)
		{
			_sessionAnalyticsModel = sessionAnalyticsModel;
		}

		public void Cleanup()
		{
			_headManTargets.Clear();
			_headManTargetList.Clear();
			_prioritizedTargetArray = Array.Empty<PlayerRef>();
		}

		public void RegisterTarget(TargetDetectData data)
		{
			if (!_headManTargets.ContainsKey(data.DetectedPlayer))
			{
				_headManTargets.Add(data.DetectedPlayer, data);
				_headManTargetList.Add(data.DetectedPlayer);
				this.OnTargetRegistered?.Invoke(data);
				_sessionAnalyticsModel.RegisterEnemyTarget(data.DetectedPlayer.PlayerId);
			}
		}

		public void RemoveTarget(PlayerRef target)
		{
			_headManTargets.Remove(target);
			_headManTargetList.Remove(target);
			this.OnTargetRemoved?.Invoke(target);
		}

		public void SetPrioritize(PlayerRef[] prioritizedTargetArray, int count)
		{
			if (count == 0)
			{
				if (_prioritizedTargetArray.Length != 0)
				{
					_prioritizedTargetArray = Array.Empty<PlayerRef>();
					this.OnPrioritizeChanged?.Invoke();
				}
				else
				{
					_prioritizedTargetArray = Array.Empty<PlayerRef>();
				}
			}
			else
			{
				_prioritizedTargetArray = new PlayerRef[count];
				Array.Copy(prioritizedTargetArray, _prioritizedTargetArray, count);
				this.OnPrioritizeChanged?.Invoke();
			}
		}
	}
}
