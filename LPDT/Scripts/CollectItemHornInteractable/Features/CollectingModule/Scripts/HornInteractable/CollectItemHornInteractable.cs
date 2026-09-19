using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts;
using Features.InteractModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.LineArmModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.CollectingModule.Scripts.HornInteractable
{
	[NetworkBehaviourWeaved(1)]
	public class CollectItemHornInteractable : InteractableBase, IToggleableInteractable
	{
		private const float ActiveMinScrollDistance = 0.8f;

		private const float VolumeFadeDuration = 0.15f;

		[SerializeField]
		private MonoItem _monoItem;

		[SerializeField]
		private SoundSourceBehaviour _soundSourceBehaviour;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		private IAudioService _audioService;

		private LineArmsModel _lineArmsModel;

		private CollectItemHornConfig _hornConfig;

		private EventInstance _eventInstance;

		private bool _hasEventInstance;

		private bool _isPendingInteraction;

		private bool _lastRenderedPlaying;

		private float _minDistanceScrollBeforeInteract;

		private bool _isInitialized;

		private readonly List<EventInstance> _fadingInstances = new List<EventInstance>();

		[WeaverGenerated]
		[DefaultForProperty("IsInInteraction", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsInInteraction;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkBool IsInInteraction
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CollectItemHornInteractable.IsInInteraction. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CollectItemHornInteractable.IsInInteraction. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = value;
			}
		}

		public bool IsToggledOn => IsInInteraction;

		public event Action OnToggleChanged;

		[Inject]
		private void InjectDependencies(IAudioService audioService, LineArmsModel lineArmsModel)
		{
			_audioService = audioService;
			_lineArmsModel = lineArmsModel;
		}

		private void Awake()
		{
			_hornConfig = ((_monoItem != null) ? (_monoItem.DefaultConfig as CollectItemHornConfig) : null);
		}

		public override void Spawned()
		{
			_isInitialized = true;
			_lastRenderedPlaying = IsInInteraction;
			if ((bool)IsInInteraction)
			{
				StartLocalPlayback();
				ProcessArmHoldPose();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_isInitialized && (bool)IsInInteraction)
			{
				RestoreArmHoldPose();
			}
			StopAndReleaseAllPlayback();
			_isInitialized = false;
		}

		public override void Interact()
		{
			if (IsInteractable)
			{
				if (base.HasStateAuthority)
				{
					ToggleInteraction();
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
				ToggleInteraction();
			}
		}

		public override void OnInteractEnd()
		{
			if (base.HasStateAuthority && (bool)IsInInteraction)
			{
				SetPlaying(isPlaying: false);
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && (bool)IsInInteraction && !IsHeldByActivePlayer())
			{
				SetPlaying(isPlaying: false);
			}
		}

		public override void Render()
		{
			bool flag = IsInInteraction;
			if (_lastRenderedPlaying != flag)
			{
				_lastRenderedPlaying = flag;
				this.OnToggleChanged?.Invoke();
			}
			if (flag)
			{
				if (!_hasEventInstance)
				{
					StartLocalPlayback();
				}
				else
				{
					UpdateLocalPlayback();
				}
			}
			else if (_hasEventInstance)
			{
				BeginFadeOutAndRelease();
			}
			else
			{
				UpdateFadingPlayback();
			}
		}

		private void ToggleInteraction()
		{
			SetPlaying(!IsInInteraction);
		}

		private void SetPlaying(bool isPlaying)
		{
			IsInInteraction = isPlaying;
			_lastRenderedPlaying = isPlaying;
			this.OnToggleChanged?.Invoke();
			ProcessArmHoldPose();
			if (isPlaying)
			{
				StartLocalPlayback();
			}
			else
			{
				BeginFadeOutAndRelease();
			}
		}

		private bool IsHeldByActivePlayer()
		{
			if (_simplePointGrabable == null || _simplePointGrabable.GrabbedByPlayers.Count == 0)
			{
				return false;
			}
			int num = _simplePointGrabable.GrabbedByPlayers[0];
			foreach (PlayerRef activePlayer in base.Runner.ActivePlayers)
			{
				if (activePlayer.PlayerId == num)
				{
					return true;
				}
			}
			return false;
		}

		private void ProcessArmHoldPose()
		{
			if (_isInitialized && _lineArmsModel.TryGetLineArmForPlayer(base.Object.StateAuthority.PlayerId, out var lineArm))
			{
				if ((bool)IsInInteraction)
				{
					_minDistanceScrollBeforeInteract = lineArm.MinScrollDistanceValue;
					lineArm.MinScrollDistanceValue = 0.8f;
					lineArm.MouseScrollValue = new Vector2(0f, -100f);
					lineArm.DisableScroll = true;
					lineArm.HandleJointScroll(forced: true);
				}
				else
				{
					RestoreArmHoldPose(lineArm);
				}
			}
		}

		private void RestoreArmHoldPose(LineArmControllerBase lineArm = null)
		{
			if (!(lineArm == null) || _lineArmsModel.TryGetLineArmForPlayer(base.Object.StateAuthority.PlayerId, out lineArm))
			{
				lineArm.DisableScroll = false;
				lineArm.MinScrollDistanceValue = _minDistanceScrollBeforeInteract;
				_minDistanceScrollBeforeInteract = 0f;
			}
		}

		private void StartLocalPlayback()
		{
			if (_hasEventInstance)
			{
				BeginFadeOutAndRelease();
			}
			if (!(_hornConfig == null) && !_hornConfig.HornSound.IsNull && !(_soundSourceBehaviour == null))
			{
				_eventInstance = _audioService.CreateInstance(_hornConfig.HornSound);
				_hasEventInstance = true;
				_audioService.StartInstanceWith3DAttributes(_eventInstance, _soundSourceBehaviour);
			}
		}

		private void UpdateLocalPlayback()
		{
			if (!(_soundSourceBehaviour == null))
			{
				if (_hasEventInstance)
				{
					_eventInstance.set3DAttributes(_soundSourceBehaviour.SoundSourceTransform.To3DAttributes());
				}
				UpdateFadingPlayback();
			}
		}

		private void UpdateFadingPlayback()
		{
			if (!(_soundSourceBehaviour == null))
			{
				ATTRIBUTES_3D attributes = _soundSourceBehaviour.SoundSourceTransform.To3DAttributes();
				for (int i = 0; i < _fadingInstances.Count; i++)
				{
					_fadingInstances[i].set3DAttributes(attributes);
				}
			}
		}

		private void BeginFadeOutAndRelease()
		{
			if (_hasEventInstance)
			{
				EventInstance eventInstance = _eventInstance;
				_eventInstance.clearHandle();
				_hasEventInstance = false;
				_fadingInstances.Add(eventInstance);
				StartCoroutine(FadeOutAndReleaseRoutine(eventInstance));
			}
		}

		private IEnumerator FadeOutAndReleaseRoutine(EventInstance instance)
		{
			instance.getVolume(out var startVolume);
			float timer = 0f;
			while (timer < 0.15f)
			{
				timer += Time.deltaTime;
				instance.setVolume(Mathf.Lerp(startVolume, 0f, timer / 0.15f));
				yield return null;
			}
			ReleaseInstance(instance);
			_fadingInstances.Remove(instance);
		}

		private void StopAndReleaseAllPlayback()
		{
			StopAllCoroutines();
			if (_hasEventInstance)
			{
				ReleaseInstance(_eventInstance);
				_eventInstance.clearHandle();
				_hasEventInstance = false;
			}
			for (int i = 0; i < _fadingInstances.Count; i++)
			{
				ReleaseInstance(_fadingInstances[i]);
			}
			_fadingInstances.Clear();
		}

		private void ReleaseInstance(EventInstance instance)
		{
			_audioService.StopInstance(instance, FMOD.Studio.STOP_MODE.IMMEDIATE);
			_audioService.ReleaseInstance(instance);
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
