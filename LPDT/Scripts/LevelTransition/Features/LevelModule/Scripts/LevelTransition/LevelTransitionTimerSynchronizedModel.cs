using System;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.SynchronizedModelsModule.Scripts.DataStreamingSynchronizer;
using UnityEngine;

namespace Features.LevelModule.Scripts.LevelTransition
{
	[Serializable]
	public class LevelTransitionTimerSynchronizedModel : DataStreamSynchronizableBase<LevelTransitionTimerSynchronizedModel>, ISessionCleanup
	{
		[SerializeField]
		private bool _isTimerRunning;

		[field: SerializeField]
		public float CurrentTimer { get; set; }

		[field: SerializeField]
		public float MaxTimer { get; private set; }

		public bool IsTimerRunning
		{
			get
			{
				return _isTimerRunning;
			}
			set
			{
				bool num = _isTimerRunning != value;
				_isTimerRunning = value;
				if (num)
				{
					this.OnTimerRunningChanged?.Invoke(_isTimerRunning);
				}
			}
		}

		public override bool IsNeedToSynchronizeOnSpawn => true;

		public event Action<float, float> OnTimerChanged;

		public event Action<bool> OnTimerRunningChanged;

		public void Initialize(float currentTimer, float maxTimer)
		{
			MaxTimer = maxTimer;
			CurrentTimer = currentTimer;
			IsTimerRunning = false;
			this.OnTimerChanged?.Invoke(CurrentTimer, MaxTimer);
			Synchronize();
		}

		protected override void SetNewValues(LevelTransitionTimerSynchronizedModel model)
		{
			CurrentTimer = model.CurrentTimer;
			MaxTimer = model.MaxTimer;
			IsTimerRunning = model.IsTimerRunning;
			this.OnTimerChanged?.Invoke(CurrentTimer, MaxTimer);
		}

		public void Cleanup()
		{
			CurrentTimer = 0f;
			MaxTimer = 0f;
		}
	}
}
