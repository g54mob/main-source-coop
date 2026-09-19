using System;
using Features.NetworkedModelCodegen.Scripts;

namespace Features.GamePhasesModule.Scripts.Data
{
	[NetworkedModel(ModelScope.Level, ModelOwnership.Shared)]
	public class GamePhasesModel : NetworkedModelBase
	{
		private float _currentPhaseTime;

		public float QuotaOnStartPhase;

		public Networked<int> NetworkedCurrentPhaseCount { get; } = new Networked<int>();

		public Networked<bool> NetworkedIsActive { get; } = new Networked<bool>();

		public float CurrentPhaseTime
		{
			get
			{
				return _currentPhaseTime;
			}
			set
			{
				_currentPhaseTime = value;
				this.OnCurrentPhaseTimeChanged?.Invoke(value);
			}
		}

		public bool InTransition { get; set; }

		public bool IsSomePlayerMovedFromSpawn { get; set; }

		public float TimeMultiplier { get; set; } = 1f;

		public GamePhasesData GamePhasesData { get; set; }

		public int CurrentPhaseCount => NetworkedCurrentPhaseCount.Value;

		public bool IsActive => NetworkedIsActive.Value;

		public event Action OnGamePhaseActivated;

		public event Action BeforeGamePhaseLoopActivated;

		public event Action BeforeFearAllEnemies;

		public event Action OnSomePlayerMovedFromSpawn;

		public event Action<float> OnCurrentPhaseTimeChanged;

		public void SetCurrentPhaseCount(int value)
		{
			NetworkedCurrentPhaseCount.Value = value;
		}

		public void SetIsActive(bool value)
		{
			NetworkedIsActive.Value = value;
		}

		public void InvokeOnGamePhaseActivated()
		{
			this.OnGamePhaseActivated?.Invoke();
		}

		public void InvokeBeforeGamePhaseLoopActivated()
		{
			this.BeforeGamePhaseLoopActivated?.Invoke();
		}

		public void InvokeBeforeFearAllEnemies()
		{
			this.BeforeFearAllEnemies?.Invoke();
		}

		public void InvokeSomePlayerMovedFromSpawn()
		{
			this.OnSomePlayerMovedFromSpawn?.Invoke();
		}
	}
}
