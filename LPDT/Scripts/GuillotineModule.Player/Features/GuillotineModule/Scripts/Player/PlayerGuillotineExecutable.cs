using System;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.DamageableTrackModule.Scripts;
using Features.GrabModule.Scripts;
using Features.InputModule.Scripts.Generated;
using Features.RagdollModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Fusion;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using UnityEngine;
using Zenject;

namespace Features.GuillotineModule.Scripts.Player
{
	[NetworkBehaviourWeaved(1)]
	public class PlayerGuillotineExecutable : NetworkBehaviour, IGuillotineExecutable
	{
		[SerializeField]
		private PlayerDamageable _playerDamageable;

		[SerializeField]
		private PlayerRagdollEntity _playerRagdollEntity;

		[SerializeField]
		private EntityStatEntityNetworkedBase _playerStatEntity;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private EventReference _executionSound;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private bool _isPreparedForExecution;

		private IInputService _inputService;

		private IAudioService _audioService;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ShouldQuitExecution", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _ShouldQuitExecution;

		[field: SerializeField]
		public bool IsRuntime { get; private set; }

		public bool CanBeExecuted => _playerStatEntity.GetStat(EntityStatType.Health).FullValue > 0f;

		public NetworkObject NetworkObject => base.Object;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe bool ShouldQuitExecution
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerGuillotineExecutable.ShouldQuitExecution. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing PlayerGuillotineExecutable.ShouldQuitExecution. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Inject]
		public void InjectDependencies(IInputService inputService, IAudioService audioService)
		{
			_inputService = inputService;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			base.Spawned();
			InputVector2Actions movement = _inputService.Movement;
			movement.VectorChangedPerformed = (Action<Vector2>)Delegate.Combine(movement.VectorChangedPerformed, new Action<Vector2>(OnPlayerMoved));
			_simplePointGrabable.LocalOnGrab += OnSomePlayerPlayerStateChanged;
			_playerStatEntity.GetStat(EntityStatType.Health).OnReachedMinValue += OnPlayerKilled;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			InputVector2Actions movement = _inputService.Movement;
			movement.VectorChangedPerformed = (Action<Vector2>)Delegate.Remove(movement.VectorChangedPerformed, new Action<Vector2>(OnPlayerMoved));
			_simplePointGrabable.LocalOnGrab -= OnSomePlayerPlayerStateChanged;
			_playerStatEntity.GetStat(EntityStatType.Health).OnReachedMinValue -= OnPlayerKilled;
		}

		public void PrepareForExecution(Transform poseBlueprint)
		{
			_isPreparedForExecution = true;
			_playerRagdollEntity.SetSimulateHandsRagdoll(simulate: false);
			_playerRagdollEntity.AddSimulationReason(RagdollSimulationReasonEnum.Guillotine, poseBlueprint);
		}

		public void Execute()
		{
			_playerDamageable.Damage(new DamageData
			{
				Damage = float.PositiveInfinity,
				Source = DamageDataSourceExtensions.ForEnvironment(DamageType.Guillotine)
			});
			ShouldQuitExecution = true;
		}

		public void QuitExecution()
		{
			ShouldQuitExecution = false;
			_isPreparedForExecution = false;
			_playerRagdollEntity.SetSimulateHandsRagdoll(simulate: true);
			_playerRagdollEntity.RemoveSimulationReason(RagdollSimulationReasonEnum.Guillotine);
		}

		public void PlayExecutionSound()
		{
			_audioService.PlayOneShotAttached(_executionSound, _soundSourceBehaviour);
		}

		private void OnPlayerMoved(Vector2 movementVector)
		{
			if (!Mathf.Approximately(movementVector.magnitude, 0f) && !ShouldQuitExecution && _isPreparedForExecution)
			{
				ShouldQuitExecution = true;
			}
		}

		private void OnSomePlayerPlayerStateChanged(int i)
		{
			if (base.Object.HasStateAuthority && !IsRuntime && _isPreparedForExecution)
			{
				ShouldQuitExecution = true;
			}
		}

		private void OnPlayerKilled()
		{
			if (_isPreparedForExecution && base.HasStateAuthority)
			{
				ShouldQuitExecution = true;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			ShouldQuitExecution = _ShouldQuitExecution;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_ShouldQuitExecution = ShouldQuitExecution;
		}
	}
}
