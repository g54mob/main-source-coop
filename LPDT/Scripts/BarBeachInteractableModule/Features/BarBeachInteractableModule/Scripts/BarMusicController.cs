using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using Features.VoiceOcclusionModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.BarBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(3)]
	public class BarMusicController : NetworkBehaviour
	{
		[SerializeField]
		private SoundSourceBehaviour _soundSource;

		[SerializeField]
		private ParticleSystem[] _playingParticleRoots;

		[SerializeField]
		private BarMusicButtonInteractReactor _musicButtonReactor;

		private BarBeachInteractableConfiguration _configuration;

		private IAudioService _audioService;

		private PlayerMovableModel _playerMovableModel;

		private VoiceOcclusionConfiguration _voiceOcclusionConfiguration;

		private EventInstance _musicInstance;

		private bool _hasMusicInstance;

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[16];

		private bool _lastRenderedPlaying;

		private int _lastRenderedTrackIndex = -1;

		private bool _arePlayingParticlesActive;

		[WeaverGenerated]
		[DefaultForProperty("IsMusicPlaying", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsMusicPlaying;

		[WeaverGenerated]
		[DefaultForProperty("TrackIndex", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _TrackIndex;

		[WeaverGenerated]
		[DefaultForProperty("HasInitializedMusic", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasInitializedMusic;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkBool IsMusicPlaying
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarMusicController.IsMusicPlaying. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarMusicController.IsMusicPlaying. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe int TrackIndex
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarMusicController.TrackIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarMusicController.TrackIndex. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe NetworkBool HasInitializedMusic
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarMusicController.HasInitializedMusic. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarMusicController.HasInitializedMusic. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = value;
			}
		}

		public bool IsPlaying => IsMusicPlaying;

		public event Action<bool> MusicPlayingChanged;

		[Inject]
		private void InjectDependencies(BarBeachInteractableConfiguration configuration, IAudioService audioService, PlayerMovableModel playerMovableModel, VoiceOcclusionConfiguration voiceOcclusionConfiguration)
		{
			_configuration = configuration;
			_audioService = audioService;
			_playerMovableModel = playerMovableModel;
			_voiceOcclusionConfiguration = voiceOcclusionConfiguration;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (base.HasStateAuthority && !HasInitializedMusic)
			{
				HasInitializedMusic = true;
				SetPlaying(isPlaying: true);
			}
			if (base.HasStateAuthority)
			{
				_musicButtonReactor?.SyncFromMusicController();
			}
			_lastRenderedPlaying = IsMusicPlaying;
			_lastRenderedTrackIndex = TrackIndex;
			if ((bool)IsMusicPlaying)
			{
				StartLocalPlayback(TrackIndex);
			}
			else
			{
				SetPlayingParticlesActive(isActive: false);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			StopAndReleaseLocalPlayback();
			SetPlayingParticlesActive(isActive: false);
			base.Despawned(runner, hasState);
		}

		public override void Render()
		{
			bool flag = IsMusicPlaying;
			int trackIndex = TrackIndex;
			if (flag)
			{
				if (!_hasMusicInstance || !_lastRenderedPlaying || _lastRenderedTrackIndex != trackIndex)
				{
					StartLocalPlayback(trackIndex);
				}
				else
				{
					UpdateLocalPlayback3D();
				}
				SetPlayingParticlesActive(isActive: true);
			}
			else
			{
				if (_hasMusicInstance)
				{
					StopAndReleaseLocalPlayback();
				}
				SetPlayingParticlesActive(isActive: false);
			}
			if (_lastRenderedPlaying != flag)
			{
				this.MusicPlayingChanged?.Invoke(flag);
			}
			_lastRenderedPlaying = flag;
			_lastRenderedTrackIndex = trackIndex;
		}

		public void SetPlaying(bool isPlaying)
		{
			if (!(base.Object == null) && base.Object.IsValid)
			{
				if (!base.Object.HasStateAuthority)
				{
					RequestSetPlayingRpc(isPlaying);
				}
				else
				{
					ApplyPlaying(isPlaying);
				}
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 668842168u)]
		private void RequestSetPlayingRpc([RpcPayload(4)] bool isPlaying)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(isPlaying);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(668842168u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BarBeachInteractableModule.Scripts.BarMusicController::RequestSetPlayingRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(isPlaying);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplyPlaying(isPlaying);
		}

		private void ApplyPlaying(bool isPlaying)
		{
			if (!base.Object.HasStateAuthority)
			{
				return;
			}
			if (isPlaying)
			{
				if (!IsMusicPlaying && TryPickRandomTrackIndex(out var trackIndex))
				{
					TrackIndex = trackIndex;
					IsMusicPlaying = true;
					StartLocalPlayback(trackIndex);
					SetPlayingParticlesActive(isActive: true);
					_lastRenderedPlaying = true;
					_lastRenderedTrackIndex = trackIndex;
					this.MusicPlayingChanged?.Invoke(obj: true);
				}
			}
			else if ((bool)IsMusicPlaying)
			{
				IsMusicPlaying = false;
				StopAndReleaseLocalPlayback();
				SetPlayingParticlesActive(isActive: false);
				_lastRenderedPlaying = false;
				this.MusicPlayingChanged?.Invoke(obj: false);
			}
		}

		public void ResyncButtonAfterAuthorityChanged()
		{
			if (base.HasStateAuthority)
			{
				_musicButtonReactor?.SyncFromMusicController();
			}
		}

		private bool TryPickRandomTrackIndex(out int trackIndex)
		{
			trackIndex = -1;
			if (_configuration == null || _configuration.MusicTracks == null)
			{
				return false;
			}
			int count = _configuration.MusicTracks.Count;
			if (count <= 0)
			{
				return false;
			}
			int num = Mathf.Min(count, 8);
			for (int i = 0; i < num; i++)
			{
				int num2 = UnityEngine.Random.Range(0, count);
				if (!_configuration.MusicTracks[num2].IsNull)
				{
					trackIndex = num2;
					return true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				if (!_configuration.MusicTracks[j].IsNull)
				{
					trackIndex = j;
					return true;
				}
			}
			return false;
		}

		private void StartLocalPlayback(int trackIndex)
		{
			StopAndReleaseLocalPlayback();
			if (!(_configuration == null) && _configuration.MusicTracks != null && trackIndex >= 0 && trackIndex < _configuration.MusicTracks.Count)
			{
				EventReference eventReference = _configuration.MusicTracks[trackIndex];
				if (!eventReference.IsNull && !(_soundSource == null) && _audioService != null)
				{
					_musicInstance = _audioService.CreateInstance(eventReference);
					_hasMusicInstance = true;
					SetSoundImmediately(_musicInstance);
					_audioService.StartInstanceWith3DAttributes(_musicInstance, _soundSource);
				}
			}
		}

		private void UpdateLocalPlayback3D()
		{
			if (_hasMusicInstance && !(_soundSource == null) && !(_soundSource.SoundSourceTransform == null))
			{
				_musicInstance.set3DAttributes(_soundSource.SoundSourceTransform.To3DAttributes());
				ProcessSoundOcclusion(_musicInstance);
			}
		}

		private void StopAndReleaseLocalPlayback()
		{
			if (_hasMusicInstance)
			{
				if (_audioService != null)
				{
					_audioService.StopInstance(_musicInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					_audioService.ReleaseInstance(_musicInstance);
				}
				_musicInstance = default(EventInstance);
				_hasMusicInstance = false;
			}
		}

		private void SetPlayingParticlesActive(bool isActive)
		{
			if (_playingParticleRoots == null || _playingParticleRoots.Length == 0 || isActive == _arePlayingParticlesActive)
			{
				return;
			}
			_arePlayingParticlesActive = isActive;
			for (int i = 0; i < _playingParticleRoots.Length; i++)
			{
				ParticleSystem particleSystem = _playingParticleRoots[i];
				if (!(particleSystem == null))
				{
					if (isActive)
					{
						particleSystem.Play(withChildren: true);
					}
					else
					{
						particleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
					}
				}
			}
		}

		private void ProcessSoundOcclusion(EventInstance eventInstance)
		{
			if (eventInstance.isValid() && !(_voiceOcclusionConfiguration == null) && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 vector = _soundSource.SoundSourceTransform.position - position;
				float magnitude = vector.magnitude;
				int num = (int)_voiceOcclusionConfiguration.OcclusionLayerMask | 1;
				eventInstance.getParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, out var value);
				if (IsOccluded(position, vector, magnitude, num))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(vector, _voiceOcclusionConfiguration.MaxDistance);
					float b = Mathf.Lerp(1f, _voiceOcclusionConfiguration.MinLowPassValue, normalizedOcclusionDistance);
					float value2 = Mathf.Lerp(value, b, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value2);
				}
				else
				{
					float value3 = Mathf.Lerp(value, 1f, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value3);
				}
			}
		}

		private void SetSoundImmediately(EventInstance eventInstance)
		{
			if (!(_voiceOcclusionConfiguration == null) && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 vector = _soundSource.SoundSourceTransform.position - position;
				float magnitude = vector.magnitude;
				int num = (int)_voiceOcclusionConfiguration.OcclusionLayerMask | 1;
				if (IsOccluded(position, vector, magnitude, num))
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(vector, _voiceOcclusionConfiguration.MaxDistance);
					float value = Mathf.Lerp(1f, _voiceOcclusionConfiguration.MinLowPassValue, normalizedOcclusionDistance);
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, value);
				}
				else
				{
					eventInstance.setParameterByName(_voiceOcclusionConfiguration.LowPassParameterName, 1f);
				}
			}
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier;
			return Mathf.Sqrt(magnitude * magnitude + num * num);
		}

		private float GetNormalizedOcclusionDistance(Vector3 offset, float maxDistance)
		{
			float weightedOcclusionDistance = GetWeightedOcclusionDistance(offset);
			float num = Mathf.Clamp01(weightedOcclusionDistance / Mathf.Max(0.01f, maxDistance));
			if (weightedOcclusionDistance <= Mathf.Epsilon)
			{
				return num;
			}
			float t = Mathf.Abs(offset.y) * _voiceOcclusionConfiguration.VerticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _voiceOcclusionConfiguration.VerticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private bool IsOccluded(Vector3 listenerPosition, Vector3 direction, float distance, LayerMask occlusionMask)
		{
			if (distance <= Mathf.Epsilon)
			{
				return false;
			}
			int num = Physics.RaycastNonAlloc(listenerPosition, direction.normalized, _occlusionHits, distance, occlusionMask, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				if (!IsBeachInteractableHit(_occlusionHits[i].collider))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsBeachInteractableHit(Collider hitCollider)
		{
			Transform parent = hitCollider.transform;
			while (parent != null)
			{
				if (parent.CompareTag("BeachInteractable"))
				{
					return true;
				}
				parent = parent.parent;
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsMusicPlaying = _IsMusicPlaying;
			TrackIndex = _TrackIndex;
			HasInitializedMusic = _HasInitializedMusic;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsMusicPlaying = IsMusicPlaying;
			_TrackIndex = TrackIndex;
			_HasInitializedMusic = HasInitializedMusic;
		}

		[NetworkRpcWeavedInvoker(668842168u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestSetPlayingRpc_0040Invoker668842168([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BarMusicController)context.TargetBehaviour).RequestSetPlayingRpc(value);
		}
	}
}
