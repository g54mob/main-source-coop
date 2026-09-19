using System;
using Features.GrabModule.Scripts;
using Features.InteractModule.Scripts;
using Features.LineArmModule.Scripts;
using Features.VoiceSpeakersModule.Scripts.Data;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.RuporModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class RuporInteractable : InteractableBase, IToggleableInteractable
	{
		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private ParticleSystem _particleSystem;

		private RuporModel _ruporModel;

		private bool _isInitialized;

		private LineArmsModel _lineArmsModel;

		private float _minDistanceScrollBeforeInteract;

		private PlayerVoiceActivityModel _playerVoiceActivityModel;

		[WeaverGenerated]
		[DefaultForProperty("IsInInteraction", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsInInteraction;

		private bool _isPendingInteraction;

		private bool _lastToggleState;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe bool IsInInteraction
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RuporInteractable.IsInInteraction. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RuporInteractable.IsInInteraction. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public bool IsToggledOn => IsInInteraction;

		public event Action OnToggleChanged;

		[Inject]
		private void InjectDependencies(RuporModel ruporModel, LineArmsModel lineArmsModel, PlayerVoiceActivityModel playerVoiceActivityModel)
		{
			_ruporModel = ruporModel;
			_lineArmsModel = lineArmsModel;
			_playerVoiceActivityModel = playerVoiceActivityModel;
		}

		public override void Interact()
		{
			if (IsInteractable)
			{
				if (base.Object.HasStateAuthority)
				{
					ExecuteInteractionLogic();
					return;
				}
				_isPendingInteraction = true;
				base.Object.RequestStateAuthority();
			}
		}

		public override void StateAuthorityChanged()
		{
			ClearLocalPendingInteractionUnlessAuthority(ref _isPendingInteraction);
			base.StateAuthorityChanged();
			if (base.HasStateAuthority && _isPendingInteraction)
			{
				_isPendingInteraction = false;
				ExecuteInteractionLogic();
			}
		}

		private void ExecuteInteractionLogic()
		{
			IsInInteraction = !IsInInteraction;
			ProcessValues();
		}

		private void ProcessValues()
		{
			if (_isInitialized)
			{
				if (IsInInteraction)
				{
					_minDistanceScrollBeforeInteract = _lineArmsModel.GetLineArmForPlayer(base.Object.StateAuthority.PlayerId).MinScrollDistanceValue;
					_lineArmsModel.GetLineArmForPlayer(base.Object.StateAuthority.PlayerId).MinScrollDistanceValue = 0.8f;
					_lineArmsModel.GetLineArmForPlayer(base.Object.StateAuthority.PlayerId).MouseScrollValue = new Vector2(0f, -100f);
					_lineArmsModel.GetLineArmForPlayer(base.Object.StateAuthority.PlayerId).DisableScroll = true;
					_lineArmsModel.GetLineArmForPlayer(base.Object.StateAuthority.PlayerId).HandleJointScroll(forced: true);
				}
				else
				{
					_lineArmsModel.GetLineArmForPlayer(base.Object.StateAuthority.PlayerId).DisableScroll = false;
					_lineArmsModel.GetLineArmForPlayer(base.Object.StateAuthority.PlayerId).MinScrollDistanceValue = _minDistanceScrollBeforeInteract;
					_minDistanceScrollBeforeInteract = 0f;
				}
				UpdateParticleVisual();
			}
		}

		public override void Spawned()
		{
			_ruporModel.RuporsData.Add(_simplePointGrabable, new RuporData(IsInInteraction));
			_isInitialized = true;
			_playerVoiceActivityModel.OnPlayerSpeakingChanged += OnPlayerSpeakingChanged;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_playerVoiceActivityModel.OnPlayerSpeakingChanged -= OnPlayerSpeakingChanged;
			if (_ruporModel.RuporsData.ContainsKey(_simplePointGrabable))
			{
				_ruporModel.RuporsData.Remove(_simplePointGrabable);
			}
			UpdateParticleVisual(forceStop: true);
		}

		public void Update()
		{
			if (_isInitialized)
			{
				_ruporModel.RuporsData[_simplePointGrabable].IsInInteraction = IsInInteraction;
				UpdateParticleVisual();
				if (_lastToggleState != IsInInteraction)
				{
					_lastToggleState = IsInInteraction;
					this.OnToggleChanged?.Invoke();
				}
			}
		}

		public override void OnInteractEnd()
		{
			if (base.HasStateAuthority && IsInInteraction)
			{
				IsInInteraction = false;
				ProcessValues();
			}
		}

		private void OnPlayerSpeakingChanged(int playerId, bool isSpeaking)
		{
			if (IsInInteraction && _simplePointGrabable.GrabbedByPlayers.Count != 0 && playerId == _simplePointGrabable.GrabbedByPlayers[0])
			{
				UpdateParticleVisual();
			}
		}

		private void UpdateParticleVisual(bool forceStop = false)
		{
			if (_particleSystem == null)
			{
				return;
			}
			if (!forceStop && IsInInteraction && IsInteractingPlayerSpeaking())
			{
				if (!_particleSystem.isPlaying)
				{
					_particleSystem.Play();
				}
			}
			else if (_particleSystem.isPlaying)
			{
				_particleSystem.Stop();
			}
		}

		private bool IsInteractingPlayerSpeaking()
		{
			if (_simplePointGrabable.GrabbedByPlayers.Count == 0)
			{
				return false;
			}
			return _playerVoiceActivityModel.IsPlayerSpeaking(_simplePointGrabable.GrabbedByPlayers[0]);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			IsInInteraction = _IsInInteraction;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_IsInInteraction = IsInInteraction;
		}
	}
}
