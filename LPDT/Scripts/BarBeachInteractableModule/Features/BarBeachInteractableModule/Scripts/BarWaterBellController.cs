using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using FMOD.Studio;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.GrabModule.Scripts;
using Features.HingeModule.Scripts;
using Features.Movement.Scripts;
using Features.RumModule.Scripts;
using Features.VoiceOcclusionModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;
using Zenject;

namespace Features.BarBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class BarWaterBellController : NetworkBehaviour
	{
		[SerializeField]
		[FormerlySerializedAs("_waterBell")]
		private SimplePointGrabable _leverGrabbable;

		[SerializeField]
		private ParticleSystem _waterSplashParticle;

		[SerializeField]
		private Transform _soakZone;

		[SerializeField]
		private Vector3 _soakZoneHalfExtents = new Vector3(0.7f, 1.2f, 0.7f);

		[SerializeField]
		[Min(0f)]
		private float _soakApplyDelaySeconds = 0.35f;

		[SerializeField]
		[Range(0f, 1f)]
		private float _wetnessPulse = 1f;

		[SerializeField]
		private float _activationRotationThreshold = 60f;

		[SerializeField]
		[Min(0f)]
		private float _deactivationDelay = 2f;

		[SerializeField]
		[Min(0.01f)]
		private float _leverReturnTime = 0.75f;

		[Header("Editor")]
		[SerializeField]
		private bool _drawSoakZoneAlways = true;

		[SerializeField]
		private Color _soakZoneFillColor = new Color(0.15f, 0.65f, 1f, 0.18f);

		[SerializeField]
		private Color _soakZoneWireColor = new Color(0.1f, 0.85f, 1f, 0.95f);

		private IDrunkSoberSplashService _drunkSoberSplashService;

		private PlayerMovableModel _playerMovableModel;

		private BarBeachInteractableConfiguration _configuration;

		private IAudioService _audioService;

		private VoiceOcclusionConfiguration _voiceOcclusionConfiguration;

		private HingeSetupHandler _hingeSetupHandler;

		private Coroutine _soakRoutine;

		private Coroutine _leverReturnRoutine;

		private readonly List<int> _playersInZoneBuffer = new List<int>(8);

		private Quaternion _initialLeverRotation;

		private bool _isActivated;

		private bool _isArmed = true;

		private bool _isInitialized;

		private float _nextSplashAllowedTime;

		private readonly RaycastHit[] _occlusionHits = new RaycastHit[16];

		[WeaverGenerated]
		[DefaultForProperty("SplashPulse", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _SplashPulse;

		private int _lastRenderedPulse = -1;

		[Networked]
		[OnChangedRender("OnSplashPulseRender")]
		[NetworkedWeaved(0, 1)]
		private unsafe int SplashPulse
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarWaterBellController.SplashPulse. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(int*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarWaterBellController.SplashPulse. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(int*)((byte*)Ptr + 0) = value;
			}
		}

		[Inject]
		private void InjectDependencies(IDrunkSoberSplashService drunkSoberSplashService, PlayerMovableModel playerMovableModel, BarBeachInteractableConfiguration configuration, IAudioService audioService, VoiceOcclusionConfiguration voiceOcclusionConfiguration)
		{
			_drunkSoberSplashService = drunkSoberSplashService;
			_playerMovableModel = playerMovableModel;
			_configuration = configuration;
			_audioService = audioService;
			_voiceOcclusionConfiguration = voiceOcclusionConfiguration;
		}

		public override void Spawned()
		{
			base.Spawned();
			_hingeSetupHandler = ((_leverGrabbable != null) ? _leverGrabbable.GetComponent<HingeSetupHandler>() : null);
			if (_hingeSetupHandler == null || _hingeSetupHandler.IsJointInitialized)
			{
				InitializeLeverActivation();
			}
			else
			{
				_hingeSetupHandler.OnJointInitialized += InitializeLeverActivation;
			}
			if (_waterSplashParticle != null)
			{
				ParticleSystem.MainModule main = _waterSplashParticle.main;
				main.playOnAwake = false;
				_waterSplashParticle.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
			_lastRenderedPulse = SplashPulse;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_hingeSetupHandler != null)
			{
				_hingeSetupHandler.OnJointInitialized -= InitializeLeverActivation;
			}
			if (_soakRoutine != null)
			{
				StopCoroutine(_soakRoutine);
				_soakRoutine = null;
			}
			StopAllCoroutines();
			_leverReturnRoutine = null;
			_isInitialized = false;
			base.Despawned(runner, hasState);
		}

		private void InitializeLeverActivation()
		{
			if (_hingeSetupHandler != null)
			{
				_hingeSetupHandler.OnJointInitialized -= InitializeLeverActivation;
			}
			if (!(_leverGrabbable == null) && !(_leverGrabbable.Rigidbody == null))
			{
				_initialLeverRotation = GetLeverRotationRelativeToBar();
				_isArmed = true;
				_isActivated = false;
				_isInitialized = true;
			}
		}

		private void Update()
		{
			if (_isInitialized && !_isActivated && !(_leverGrabbable == null) && !(_leverGrabbable.Rigidbody == null))
			{
				if (Quaternion.Angle(_initialLeverRotation, GetLeverRotationRelativeToBar()) < _activationRotationThreshold)
				{
					_isArmed = true;
				}
				else if (_isArmed)
				{
					_isArmed = false;
					_isActivated = true;
					TryPulseSplash();
					ResetLever(_deactivationDelay);
				}
			}
		}

		private void ResetLever(float deactivationDelay)
		{
			if (_leverReturnRoutine != null)
			{
				StopCoroutine(_leverReturnRoutine);
			}
			_leverReturnRoutine = StartCoroutine(LeverReturnRoutine(deactivationDelay, _leverReturnTime));
		}

		private IEnumerator LeverReturnRoutine(float deactivationDelay, float deactivationTime)
		{
			yield return new WaitForSeconds(deactivationDelay);
			if (_leverGrabbable == null || _leverGrabbable.GrabObject == null)
			{
				yield break;
			}
			_leverGrabbable.GrabObject.GrabbingPhysicsBlocked = true;
			float leverReturnTimer = 0f;
			Quaternion currentLeverRotation = _leverGrabbable.Rigidbody.rotation;
			Quaternion restWorldRotation = GetLeverWorldRotationFromRelative(_initialLeverRotation);
			while (leverReturnTimer < deactivationTime)
			{
				if (_leverGrabbable == null || _leverGrabbable.NetworkObject == null)
				{
					yield break;
				}
				float t = Mathf.Clamp01(leverReturnTimer / deactivationTime);
				Quaternion rotation = Quaternion.Lerp(currentLeverRotation, restWorldRotation, t);
				if (_leverGrabbable.NetworkObject.HasStateAuthority)
				{
					_leverGrabbable.Rigidbody.rotation = rotation;
				}
				leverReturnTimer += Time.deltaTime;
				yield return null;
			}
			if (_leverGrabbable != null && _leverGrabbable.NetworkObject != null && _leverGrabbable.NetworkObject.HasStateAuthority)
			{
				_leverGrabbable.Rigidbody.rotation = restWorldRotation;
				_leverGrabbable.Rigidbody.angularVelocity = Vector3.zero;
			}
			_isActivated = false;
			if (_leverGrabbable.GrabObject != null)
			{
				_leverGrabbable.GrabObject.GrabbingPhysicsBlocked = false;
			}
			_leverReturnRoutine = null;
		}

		private Quaternion GetLeverRotationRelativeToBar()
		{
			return Quaternion.Inverse(base.transform.rotation) * _leverGrabbable.Rigidbody.rotation;
		}

		private Quaternion GetLeverWorldRotationFromRelative(Quaternion relativeRotation)
		{
			return base.transform.rotation * relativeRotation;
		}

		private void TryPulseSplash()
		{
			if (base.Object.HasStateAuthority)
			{
				PulseSplashIfNeeded();
			}
			else
			{
				RequestSplashPulseRpc();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 1267235254u)]
		private void RequestSplashPulseRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1267235254u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BarBeachInteractableModule.Scripts.BarWaterBellController::RequestSplashPulseRpc()", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			PulseSplashIfNeeded();
		}

		private void PulseSplashIfNeeded()
		{
			if (!(Time.time < _nextSplashAllowedTime))
			{
				_nextSplashAllowedTime = Time.time + _deactivationDelay + _leverReturnTime;
				SplashPulse++;
				ApplySplashPulseRender();
			}
		}

		private void OnSplashPulseRender()
		{
			ApplySplashPulseRender();
		}

		private void ApplySplashPulseRender()
		{
			if (SplashPulse != _lastRenderedPulse)
			{
				_lastRenderedPulse = SplashPulse;
				PlaySplashVfx();
				PlaySplashSound();
				if (_soakRoutine != null)
				{
					StopCoroutine(_soakRoutine);
				}
				_soakRoutine = StartCoroutine(SoakAfterParticleRoutine());
			}
		}

		private void PlaySplashVfx()
		{
			if (!(_waterSplashParticle == null))
			{
				GameObject gameObject = _waterSplashParticle.gameObject;
				if (!gameObject.activeSelf)
				{
					gameObject.SetActive(value: true);
				}
				_waterSplashParticle.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
				_waterSplashParticle.Clear(withChildren: true);
				_waterSplashParticle.Play(withChildren: true);
			}
		}

		private void PlaySplashSound()
		{
			if (_audioService != null && !(_configuration == null) && !_configuration.WaterBellSplashEvent.IsNull)
			{
				Transform transform = ((_waterSplashParticle != null) ? _waterSplashParticle.transform : ((_soakZone != null) ? _soakZone : base.transform));
				PlayOneShot(_configuration.WaterBellSplashEvent, new GenericSoundSource(transform.position, transform.GetInstanceID()));
			}
		}

		private IEnumerator SoakAfterParticleRoutine()
		{
			float num = Mathf.Max(0f, _soakApplyDelaySeconds);
			if (num > 0f)
			{
				yield return new WaitForSeconds(num);
			}
			CollectPlayersInSoakZone(_playersInZoneBuffer);
			if (_playersInZoneBuffer.Count == 0)
			{
				_soakRoutine = null;
				yield break;
			}
			if (base.Object.HasStateAuthority)
			{
				_drunkSoberSplashService?.ClearDrunkennessForPlayers(_playersInZoneBuffer);
			}
			int num2 = ((base.Runner != null) ? base.Runner.LocalPlayer.PlayerId : (-1));
			if (num2 >= 0 && _playersInZoneBuffer.Contains(num2))
			{
				_drunkSoberSplashService?.ApplyLocalWetnessPulse(_wetnessPulse);
			}
			_soakRoutine = null;
		}

		private void CollectPlayersInSoakZone(List<int> results)
		{
			results.Clear();
			if (_playerMovableModel?.AllCharacterMovables == null)
			{
				return;
			}
			GetSoakZoneWorld(out var center, out var rotation);
			Matrix4x4 inverse = Matrix4x4.TRS(center, rotation, Vector3.one).inverse;
			foreach (KeyValuePair<PlayerRef, PlayerCharacterMovableBase> allCharacterMovable in _playerMovableModel.AllCharacterMovables)
			{
				PlayerCharacterMovableBase value = allCharacterMovable.Value;
				if (value == null)
				{
					continue;
				}
				CapsuleCollider bodyCollider = value.BodyCollider;
				if (!(bodyCollider == null) && bodyCollider.enabled && DoesCapsuleIntersectSoakObb(bodyCollider, inverse))
				{
					int playerId = allCharacterMovable.Key.PlayerId;
					if (!results.Contains(playerId))
					{
						results.Add(playerId);
					}
				}
			}
		}

		private bool DoesCapsuleIntersectSoakObb(CapsuleCollider capsule, Matrix4x4 worldToLocal)
		{
			Transform transform = capsule.transform;
			float num = capsule.radius * MaxAbsAxis(transform.lossyScale);
			float num2 = Mathf.Max(capsule.height * AbsAxis(transform.lossyScale, GetCapsuleDirectionAxis(capsule)), num * 2f);
			Vector3 vector = transform.TransformPoint(capsule.center);
			Vector3 normalized = transform.TransformDirection(GetCapsuleDirection(capsule)).normalized;
			float num3 = Mathf.Max(0f, num2 * 0.5f - num);
			Vector3 point = vector + normalized * num3;
			Vector3 point2 = vector - normalized * num3;
			Vector3 point3 = vector;
			if (!PointSphereHitsSoakObb(worldToLocal.MultiplyPoint3x4(point), num) && !PointSphereHitsSoakObb(worldToLocal.MultiplyPoint3x4(point2), num))
			{
				return PointSphereHitsSoakObb(worldToLocal.MultiplyPoint3x4(point3), num);
			}
			return true;
		}

		private bool PointSphereHitsSoakObb(Vector3 localPoint, float radius)
		{
			Vector3 vector = new Vector3(Mathf.Clamp(localPoint.x, 0f - _soakZoneHalfExtents.x, _soakZoneHalfExtents.x), Mathf.Clamp(localPoint.y, 0f - _soakZoneHalfExtents.y, _soakZoneHalfExtents.y), Mathf.Clamp(localPoint.z, 0f - _soakZoneHalfExtents.z, _soakZoneHalfExtents.z));
			return (localPoint - vector).sqrMagnitude <= radius * radius;
		}

		private static Vector3 GetCapsuleDirection(CapsuleCollider capsule)
		{
			return capsule.direction switch
			{
				0 => Vector3.right, 
				1 => Vector3.up, 
				_ => Vector3.forward, 
			};
		}

		private static int GetCapsuleDirectionAxis(CapsuleCollider capsule)
		{
			return capsule.direction;
		}

		private static float AbsAxis(Vector3 scale, int axis)
		{
			return Mathf.Abs(axis switch
			{
				1 => scale.y, 
				0 => scale.x, 
				_ => scale.z, 
			});
		}

		private static float MaxAbsAxis(Vector3 scale)
		{
			return Mathf.Max(Mathf.Abs(scale.x), Mathf.Max(Mathf.Abs(scale.y), Mathf.Abs(scale.z)));
		}

		private void GetSoakZoneWorld(out Vector3 center, out Quaternion rotation)
		{
			Transform transform = ((_soakZone != null) ? _soakZone : ((_waterSplashParticle != null) ? _waterSplashParticle.transform : base.transform));
			center = transform.position;
			rotation = transform.rotation;
			if (_soakZone == null && _waterSplashParticle != null)
			{
				center += Vector3.down * _soakZoneHalfExtents.y;
			}
		}

		private void PlayOneShot(EventReference reference, ISoundSource soundSource)
		{
			if (_audioService != null && soundSource != null && !reference.IsNull)
			{
				EventInstance eventInstance = _audioService.CreateInstance(reference);
				SetSoundImmediately(eventInstance, soundSource.SourcePosition);
				_audioService.StartInstanceWith3DAttributes(eventInstance, soundSource);
				_audioService.ReleaseInstance(eventInstance);
			}
		}

		private void SetSoundImmediately(EventInstance eventInstance, Vector3 emitterPosition)
		{
			if (!(_voiceOcclusionConfiguration == null) && !(_playerMovableModel.LocalMovable == null) && !(_playerMovableModel.LocalMovable.CameraPositionTransform == null))
			{
				Vector3 position = _playerMovableModel.LocalMovable.CameraPositionTransform.position;
				Vector3 vector = emitterPosition - position;
				float magnitude = vector.magnitude;
				if (IsOccluded(position, vector, magnitude, _voiceOcclusionConfiguration.OcclusionLayerMask))
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
			SplashPulse = _SplashPulse;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_SplashPulse = SplashPulse;
		}

		[NetworkRpcWeavedInvoker(1267235254u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestSplashPulseRpc_0040Invoker1267235254([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BarWaterBellController)context.TargetBehaviour).RequestSplashPulseRpc();
		}
	}
}
