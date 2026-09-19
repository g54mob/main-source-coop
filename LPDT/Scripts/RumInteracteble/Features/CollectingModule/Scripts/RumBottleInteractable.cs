using System;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.CollectingModule.Scripts.New;
using Features.InteractModule.Scripts;
using Features.LineArmModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.CollectingModule.Scripts
{
	[NetworkBehaviourWeaved(2)]
	public class RumBottleInteractable : DrinkingInteractableBase
	{
		[SerializeField]
		private PhysicsItemUpVectorLimiter _physicsItemUpVectorLimiter;

		[SerializeField]
		private GameObject _crock;

		[SerializeField]
		private EventReference _interactEventReference;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		private LineArmsModel _lineArmsModel;

		private IAudioService _audioService;

		private float _minDistanceScrollBeforeInteract;

		private bool _isInitialized;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsInInteraction", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsInInteraction;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsConsumed", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsConsumed;

		private bool _isPendingInteraction;

		private bool _lastToggleState;

		private bool _lastConsumedState;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe bool IsInInteraction
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RumBottleInteractable.IsInInteraction. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RumBottleInteractable.IsInInteraction. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe override bool IsConsumed
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RumBottleInteractable.IsConsumed. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean(Ptr + 1);
			}
			protected set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing RumBottleInteractable.IsConsumed. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 1) = new NetworkBool(value);
			}
		}

		public override bool IsToggledOn => IsInInteraction;

		[Inject]
		private void InjectDependencies(LineArmsModel lineArmsModel, IAudioService audioService)
		{
			_lineArmsModel = lineArmsModel;
			_audioService = audioService;
		}

		public override void Spawned()
		{
			_isInitialized = true;
		}

		public override void Interact()
		{
			if (IsInteractable && !IsConsumed && _isInitialized)
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
			_physicsItemUpVectorLimiter.DisableCorrection = !_physicsItemUpVectorLimiter.DisableCorrection;
			ProcessValues();
		}

		private void Update()
		{
			if (!_isInitialized)
			{
				return;
			}
			if (_lastToggleState != IsInInteraction)
			{
				_lastToggleState = IsInInteraction;
				RaiseToggleChanged();
			}
			if (_lastConsumedState != IsConsumed)
			{
				_lastConsumedState = IsConsumed;
				if (IsConsumed)
				{
					RaiseConsumed();
				}
			}
			if (_crock != null)
			{
				if (_crock.activeInHierarchy && IsInInteraction && !IsConsumed)
				{
					_audioService.PlayOneShotAttached(_interactEventReference, _soundSourceBehaviour);
				}
				_crock.SetActive(!IsInInteraction && !IsConsumed);
			}
		}

		public void Consume()
		{
			if (base.HasStateAuthority && !IsConsumed)
			{
				IsConsumed = true;
				IsInteractable = false;
				OnInteractEnd();
			}
		}

		private void ProcessValues()
		{
			if (_isInitialized)
			{
				if (IsInInteraction)
				{
					_minDistanceScrollBeforeInteract = _lineArmsModel.GetLineArmForPlayer(base.Object.StateAuthority.PlayerId).MinScrollDistanceValue;
					_lineArmsModel.GetLineArmForPlayer(base.Object.StateAuthority.PlayerId).MinScrollDistanceValue = 0.6f;
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
			}
		}

		public override void OnInteractEnd()
		{
			if (_isInitialized && base.HasStateAuthority && IsInInteraction)
			{
				IsInInteraction = false;
				_physicsItemUpVectorLimiter.DisableCorrection = true;
				ProcessValues();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			IsInInteraction = _IsInInteraction;
			IsConsumed = _IsConsumed;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_IsInInteraction = IsInInteraction;
			_IsConsumed = IsConsumed;
		}
	}
}
