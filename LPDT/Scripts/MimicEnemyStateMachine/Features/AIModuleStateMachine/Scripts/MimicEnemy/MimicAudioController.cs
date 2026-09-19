using System;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.CameraModelModule;
using Features.Movement.Scripts;
using Features.VoiceSpeakersModule.Scripts.MimicVoice;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy
{
	[NetworkBehaviourWeaved(0)]
	public class MimicAudioController : NetworkBehaviour
	{
		private const string INTENSITY_PARAMETER_NAME = "MimicSoundDecreaseIntensity";

		private const int OCCLUSION_HITS_BUFFER_SIZE = 1;

		[SerializeField]
		private string _voiceOcclusionLowPassParameterName = "VoiceOcclusionLowPass";

		[SerializeField]
		private EventReference _snapshotReference;

		[SerializeField]
		private EventReference _biteReference;

		[SerializeField]
		private EventReference _mimicVoiceEventReference;

		[SerializeField]
		private LayerMask _occlusionLayerMask;

		[SerializeField]
		private float _occlusionMaxDistance;

		[SerializeField]
		private float _verticalDistanceMultiplier = 3f;

		[SerializeField]
		private float _verticalFalloffExponent = 2f;

		[SerializeField]
		private float _lowPassMinValue;

		[SerializeField]
		private bool _mimicVoiceEnabled = true;

		private PlayerMovableModel _playerMovableModel;

		private MimicEnemyContext _mimicContext;

		private IMimicVoicePlaybackService _mimicVoicePlaybackService;

		private MimicVoiceCaptureConfiguration _mimicVoiceCaptureConfiguration;

		private EventInstance _snapshotInstance;

		private bool _isPlaying;

		private float _mimicVoiceCooldown;

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[1];

		private IAudioService _audioService;

		private CameraModel _cameraModel;

		[Inject]
		private void InjectDependencies(PlayerMovableModel playerMovableModel, MimicEnemyContext mimicContext, IMimicVoicePlaybackService mimicVoicePlaybackService, MimicVoiceCaptureConfiguration mimicVoiceCaptureConfiguration, IAudioService audioService, CameraModel cameraModel)
		{
			_playerMovableModel = playerMovableModel;
			_mimicContext = mimicContext;
			_mimicVoicePlaybackService = mimicVoicePlaybackService;
			_mimicVoiceCaptureConfiguration = mimicVoiceCaptureConfiguration;
			_audioService = audioService;
			_cameraModel = cameraModel;
		}

		private void Start()
		{
			_snapshotInstance = RuntimeManager.CreateInstance(_snapshotReference);
			ResetMimicVoiceCooldown();
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority && _mimicVoiceEnabled && !(_mimicContext == null) && _mimicVoicePlaybackService != null && _mimicContext.ReplicateTarget > 0)
			{
				_mimicVoiceCooldown -= base.Runner.DeltaTime;
				if (!(_mimicVoiceCooldown > 0f))
				{
					int seed = UnityEngine.Random.Range(1, int.MaxValue);
					PlayMimicVoiceRpc(_mimicContext.ReplicateTarget, seed);
					ResetMimicVoiceCooldown();
				}
			}
		}

		public void ActivateSnapshot()
		{
			if (!_snapshotInstance.isValid())
			{
				_snapshotInstance = _audioService.CreateInstance(_snapshotReference);
			}
			if (!_isPlaying)
			{
				_isPlaying = true;
				_audioService.StartInstanceWith3DAttributes(_snapshotInstance, _mimicContext.SoundSourceBehaviour);
			}
		}

		public void PlaySoundBite()
		{
			EventInstance eventInstance = _audioService.CreateInstance(_biteReference);
			ProcessSoundOcclusion(eventInstance);
			_audioService.StartInstanceWith3DAttributes(eventInstance, _mimicContext.SoundSourceBehaviour);
			_audioService.ReleaseInstance(eventInstance);
		}

		private float GetWeightedOcclusionDistance(Vector3 offset)
		{
			float magnitude = new Vector2(offset.x, offset.z).magnitude;
			float num = Mathf.Abs(offset.y) * _verticalDistanceMultiplier;
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
			float t = Mathf.Abs(offset.y) * _verticalDistanceMultiplier / weightedOcclusionDistance;
			float p = Mathf.Lerp(1f, 1f / Mathf.Max(0.01f, _verticalFalloffExponent), t);
			return Mathf.Pow(num, p);
		}

		private void ProcessSoundOcclusion(EventInstance eventInstance)
		{
			if (!(_playerMovableModel.LocalMovable == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 offset = base.transform.position - position;
				float magnitude = offset.magnitude;
				eventInstance.getParameterByName(_voiceOcclusionLowPassParameterName, out var value);
				if (Physics.RaycastNonAlloc(position, offset.normalized, _occlusionHits, magnitude, _occlusionLayerMask) > 0)
				{
					float normalizedOcclusionDistance = GetNormalizedOcclusionDistance(offset, _occlusionMaxDistance);
					float b = Mathf.Lerp(1f, _lowPassMinValue, normalizedOcclusionDistance);
					float value2 = Mathf.Lerp(value, b, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, value2);
				}
				else
				{
					float value3 = Mathf.Lerp(value, 1f, Time.deltaTime * 5f);
					eventInstance.setParameterByName(_voiceOcclusionLowPassParameterName, value3);
				}
			}
		}

		public void SetParameterValue(float value)
		{
			if (_isPlaying)
			{
				_snapshotInstance.setParameterByName("MimicSoundDecreaseIntensity", value);
			}
		}

		public void StopSnapshot()
		{
			_isPlaying = false;
			_audioService.StopInstance(_snapshotInstance, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

		[ContextMenu("Mimic Voice/Force Play Replicate Target")]
		private void ForcePlayReplicateTarget()
		{
			if (!(_mimicContext == null) && _mimicContext.ReplicateTarget > 0 && _mimicVoicePlaybackService != null)
			{
				int seed = UnityEngine.Random.Range(1, int.MaxValue);
				if (base.HasStateAuthority && base.Runner != null)
				{
					PlayMimicVoiceRpc(_mimicContext.ReplicateTarget, seed);
				}
				else
				{
					PlayLocalMimicVoice(_mimicContext.ReplicateTarget, seed);
				}
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 4149911657u)]
		private void PlayMimicVoiceRpc([RpcPayload(4)] int playerId, [RpcPayload(4)] int seed)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4149911657u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AIModuleStateMachine.Scripts.MimicEnemy.MimicAudioController::PlayMimicVoiceRpc(System.Int32,System.Int32)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(playerId, 4);
						writer.Write(seed, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			PlayLocalMimicVoice(playerId, seed);
		}

		private void PlayLocalMimicVoice(int playerId, int seed)
		{
			if (_mimicVoicePlaybackService != null)
			{
				_mimicVoicePlaybackService.PlayRandomForPlayer(playerId, base.transform, ResolveListener, seed, _mimicVoiceEventReference);
			}
		}

		private Transform ResolveListener()
		{
			if (!_cameraModel.Cameras.TryGetValue(_cameraModel.ActiveCameraType, out var value) || !(value != null))
			{
				return null;
			}
			return value.transform;
		}

		private void ResetMimicVoiceCooldown()
		{
			if (_mimicVoiceCaptureConfiguration == null)
			{
				_mimicVoiceCooldown = 10f;
				return;
			}
			float num = Mathf.Max(0.1f, _mimicVoiceCaptureConfiguration.MinPlaybackIntervalSeconds);
			float maxInclusive = Mathf.Max(num, _mimicVoiceCaptureConfiguration.MaxPlaybackIntervalSeconds);
			_mimicVoiceCooldown = UnityEngine.Random.Range(num, maxInclusive);
		}

		private void OnDestroy()
		{
			NetworkBehaviourUtils.InternalOnDestroy(this);
			_audioService.ReleaseInstance(_snapshotInstance);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(4149911657u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayMimicVoiceRpc_0040Invoker4149911657([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out int value, 4);
			payloadReader.Read(out int value2, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((MimicAudioController)context.TargetBehaviour).PlayMimicVoiceRpc(value, value2);
		}
	}
}
