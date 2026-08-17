using EvilCore.DynamicCasting;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking.Parenting;
using EvilCore.Particles;
using Mirror;
using Mirror.RemoteCalls;
using NomadDrive.Features.Tools;
using PrimeTween;
using UnityEngine;
using UnityEngine.Animations;
using VContainer;

namespace NomadDrive.Features.Digging
{
	public class Shovel : DirectHeldItem
	{
		[Header("Shovel Configuration")]
		[SerializeField]
		private ShovelConfig config;

		[Inject]
		private ICastingManager _castingManager;

		[Inject]
		private IParticlesManager _particlesManager;

		private float _lastDigTime;

		private NetworkedTransform _networkedTransform;

		private ParentConstraint _parentConstraint;

		private bool _constraintSuspended;

		private bool _snapEngaged;

		private bool _gripPaused;

		private bool _placementActive;

		private float _snapWeight;

		private float _digVertical;

		private float _digScoopWeight;

		private Vector3 _snapTargetPosition;

		private Quaternion _snapTargetRotation = Quaternion.identity;

		private Vector3 _snapTargetNormal = Vector3.up;

		private Tween _weightTween;

		private Sequence _digSeq;

		private Sequence _rotSeq;

		public override void OnEquip()
		{
			base.OnEquip();
			EnsureConstraintRefs();
		}

		public override void OnUnequip()
		{
			ForceUnsnap();
			base.OnUnequip();
		}

		public override void OnPlacementModeEnter()
		{
			base.OnPlacementModeEnter();
			_placementActive = true;
			ForceUnsnap();
		}

		public override void OnPlacementModeExit()
		{
			base.OnPlacementModeExit();
			_placementActive = false;
		}

		private void Update()
		{
			if (!base.isOwned)
			{
				return;
			}
			if (!base.IsEquipped || _placementActive)
			{
				SetSnapEngaged(on: false);
				return;
			}
			EnsureConstraintRefs();
			if (TryAimAtDiggableTerrain(out var point, out var normal))
			{
				SetSnapTarget(point, normal, GetFacingYawRotation() * Quaternion.Euler(config.digRotationEuler));
				SetSnapEngaged(on: true);
			}
			else
			{
				SetSnapEngaged(on: false);
			}
		}

		private void SetSnapEngaged(bool on)
		{
			if (_snapEngaged == on)
			{
				return;
			}
			_snapEngaged = on;
			_weightTween.Stop();
			SetGripPaused(on);
			if (on)
			{
				_weightTween = Tween.Custom(this, _snapWeight, 1f, config.digDownDuration, delegate(Shovel shovel, float w)
				{
					shovel._snapWeight = w;
				}, config.digDownEase);
				BeginSnapStream();
				return;
			}
			_digSeq.Stop();
			_rotSeq.Stop();
			_snapWeight = 0f;
			_digVertical = 0f;
			_digScoopWeight = 0f;
			RestoreConstraintNow();
			EndSnapStream();
		}

		public override void OnUseButtonDown()
		{
			base.OnUseButtonDown();
			if (!base.isOwned || !base.IsEquipped || _placementActive)
			{
				return;
			}
			if (config == null)
			{
				EvilLogger.LogError("[Shovel] ShovelConfig is not assigned.", "OnUseButtonDown", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Digging\\Scripts\\Shovel.cs", 148);
			}
			else
			{
				if (Time.time - _lastDigTime < config.cooldown)
				{
					return;
				}
				DiggingConfig instance = DiggingConfig.Instance;
				Vector3 point;
				Vector3 normal;
				if (instance == null)
				{
					EvilLogger.LogError("[Shovel] DiggingConfig not found (expected a Resources/DiggingConfig asset).", "OnUseButtonDown", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Digging\\Scripts\\Shovel.cs", 156);
				}
				else if (TryAimAtDiggableTerrain(out point, out normal))
				{
					_lastDigTime = Time.time;
					BuriedTreasure buriedTreasure = ProbeForTreasure(point, normal);
					bool num = buriedTreasure != null;
					bool flag = num && buriedTreasure.CanAdvance;
					bool firstStrike = flag && buriedTreasure.Stage == 0;
					bool flag2 = !num;
					float markYaw = ((flag2 && instance.randomizeDecalYaw) ? Random.Range(0f, 360f) : 0f);
					DoDigFeedback(point, normal, firstStrike, flag2, markYaw);
					PlayDigScoop();
					if (flag)
					{
						buriedTreasure.CmdAdvanceStage(point, normal);
					}
					CmdPerformDig(point, normal, firstStrike, flag2, markYaw);
				}
			}
		}

		private BuriedTreasure ProbeForTreasure(Vector3 surfacePoint, Vector3 surfaceNormal)
		{
			CastRequest castRequest = new CastRequest
			{
				Origin = null,
				Offset = surfacePoint,
				UseTransformForward = false,
				Direction = -surfaceNormal,
				Type = CastType.Capsule,
				Distance = config.detectDepth,
				LayerMask = config.treasureDetectionLayer,
				TriggerInteraction = QueryTriggerInteraction.Collide,
				Radius = config.detectRadius,
				CapsuleHeight = config.detectRadius * 2f,
				MaxHits = 8
			};
			CastRequest request = castRequest.RequireComponent<BuriedTreasure>(searchInParent: true);
			CastResult castResult = _castingManager.CastImmediate(request);
			if (castResult.DidHit && castResult.TryGetComponentInParent<BuriedTreasure>(out var component))
			{
				return component;
			}
			return null;
		}

		[Command]
		private void CmdPerformDig(Vector3 point, Vector3 normal, bool firstStrike, bool spawnMark, float markYaw)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(point);
			writer.WriteVector3(normal);
			writer.WriteBool(firstStrike);
			writer.WriteBool(spawnMark);
			writer.WriteFloat(markYaw);
			SendCommandInternal("System.Void NomadDrive.Features.Digging.Shovel::CmdPerformDig(UnityEngine.Vector3,UnityEngine.Vector3,System.Boolean,System.Boolean,System.Single)", -627687101, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc(includeOwner = false)]
		private void RpcPerformDig(Vector3 point, Vector3 normal, bool firstStrike, bool spawnMark, float markYaw)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVector3(point);
			writer.WriteVector3(normal);
			writer.WriteBool(firstStrike);
			writer.WriteBool(spawnMark);
			writer.WriteFloat(markYaw);
			SendRPCInternal("System.Void NomadDrive.Features.Digging.Shovel::RpcPerformDig(UnityEngine.Vector3,UnityEngine.Vector3,System.Boolean,System.Boolean,System.Single)", 1224620422, writer, 0, includeOwner: false);
			NetworkWriterPool.Return(writer);
		}

		private void DoDigFeedback(Vector3 point, Vector3 normal, bool firstStrike, bool spawnMark, float markYaw)
		{
			AudioManager?.PlayOneShot(firstStrike ? config.strikeSound : config.digSound, point);
			if (config.digParticle.IsValid)
			{
				_particlesManager?.PlayOneShot(config.digParticle, point, Quaternion.FromToRotation(Vector3.up, normal));
			}
			if (spawnMark)
			{
				SpawnDigMark(point, normal, markYaw);
			}
		}

		private void LateUpdate()
		{
			if (!base.isOwned || _parentConstraint == null || _parentConstraint.sourceCount == 0)
			{
				return;
			}
			if (!(_snapWeight > 0.0001f) || !base.IsEquipped || _placementActive)
			{
				if (_constraintSuspended)
				{
					_parentConstraint.constraintActive = true;
					_constraintSuspended = false;
				}
				return;
			}
			Transform sourceTransform = _parentConstraint.GetSource(0).sourceTransform;
			if (!(sourceTransform == null))
			{
				if (_parentConstraint.constraintActive)
				{
					_parentConstraint.constraintActive = false;
					_constraintSuspended = true;
				}
				Vector3 b = _snapTargetPosition + _snapTargetNormal * _digVertical;
				Quaternion b2 = _snapTargetRotation * Quaternion.Euler(config.digScoopRotationEuler * _digScoopWeight);
				base.transform.SetPositionAndRotation(Vector3.Lerp(sourceTransform.position, b, _snapWeight), Quaternion.Slerp(sourceTransform.rotation, b2, _snapWeight));
				StreamSnapPose(base.transform.position, base.transform.rotation);
			}
		}

		private void SetSnapTarget(Vector3 point, Vector3 normal, Quaternion rotation)
		{
			_snapTargetNormal = normal;
			_snapTargetRotation = rotation;
			_snapTargetPosition = point + normal * config.digSurfaceClearance + rotation * config.digLocalPositionOffset;
		}

		private Quaternion GetFacingYawRotation()
		{
			Transform transform = playerService?.CameraTransform;
			float y = ((transform != null) ? transform.eulerAngles.y : 0f);
			return Quaternion.Euler(0f, y, 0f);
		}

		private void PlayDigScoop()
		{
			_digSeq.Stop();
			_rotSeq.Stop();
			_digVertical = 0f;
			_digScoopWeight = 0f;
			Sequence digSeq = Sequence.Create().Chain(Tween.Custom(this, 0f, 0f - config.digPlungeDepth, config.digDownDuration, delegate(Shovel s, float v)
			{
				s._digVertical = v;
			}, config.digDownEase)).Chain(Tween.Custom(this, 0f - config.digPlungeDepth, config.digScoopLift, config.digScoopDuration, delegate(Shovel s, float v)
			{
				s._digVertical = v;
			}, config.digScoopEase))
				.Chain(Tween.Custom(this, config.digScoopLift, 0f, config.digUpDuration, delegate(Shovel s, float v)
				{
					s._digVertical = v;
				}, config.digUpEase));
			_digSeq = digSeq;
			Sequence rotSeq = Sequence.Create().Chain(Tween.Custom(this, 0f, 1f, config.digDownDuration + config.digScoopDuration, delegate(Shovel s, float v)
			{
				s._digScoopWeight = v;
			}, config.digScoopEase)).Chain(Tween.Custom(this, 1f, 0f, config.digUpDuration, delegate(Shovel s, float v)
			{
				s._digScoopWeight = v;
			}, config.digUpEase));
			_rotSeq = rotSeq;
		}

		private bool TryAimAtDiggableTerrain(out Vector3 point, out Vector3 normal)
		{
			point = default(Vector3);
			normal = Vector3.up;
			if (config == null || _castingManager == null || playerService?.CameraTransform == null)
			{
				return false;
			}
			DiggingConfig instance = DiggingConfig.Instance;
			if (instance == null)
			{
				return false;
			}
			Transform cameraTransform = playerService.CameraTransform;
			CastResult castResult = _castingManager.CastImmediate(CastRequest.Ray(cameraTransform, config.maxRayDistance, instance.terrainMask));
			if (!castResult.DidHit)
			{
				return false;
			}
			if (castResult.HitGameObject == null || !castResult.HitGameObject.TryGetComponent<Terrain>(out var _))
			{
				return false;
			}
			Vector3 position = cameraTransform.position;
			float num = Vector3.Distance(position, castResult.HitPoint) - 0.02f;
			if (num > 0f && Physics.Raycast(position, cameraTransform.forward, num, instance.digObstructionMask, QueryTriggerInteraction.Ignore))
			{
				return false;
			}
			point = castResult.HitPoint;
			normal = castResult.HitNormal;
			return true;
		}

		private void SetGripPaused(bool paused)
		{
			if (_gripPaused == paused)
			{
				return;
			}
			_gripPaused = paused;
			if (base.isOwned)
			{
				if (paused)
				{
					playerService?.EquipmentManager?.OnGripPauseRequested?.Invoke();
				}
				else
				{
					playerService?.EquipmentManager?.OnGripResumeRequested?.Invoke();
				}
			}
		}

		private void ForceUnsnap()
		{
			_weightTween.Stop();
			_digSeq.Stop();
			_rotSeq.Stop();
			_snapEngaged = false;
			_snapWeight = 0f;
			_digVertical = 0f;
			_digScoopWeight = 0f;
			SetGripPaused(paused: false);
			RestoreConstraintNow();
			EndSnapStream();
		}

		private void RestoreConstraintNow()
		{
			if (_constraintSuspended && _parentConstraint != null && _parentConstraint.sourceCount > 0)
			{
				_parentConstraint.constraintActive = true;
			}
			_constraintSuspended = false;
		}

		private void EnsureConstraintRefs()
		{
			if (!(_parentConstraint != null))
			{
				if (_networkedTransform == null)
				{
					_networkedTransform = GetComponent<NetworkedTransform>();
				}
				_parentConstraint = ((_networkedTransform != null) ? _networkedTransform.ParentConstraint : null);
			}
		}

		private void SpawnDigMark(Vector3 point, Vector3 normal, float markYaw)
		{
			DiggingConfig instance = DiggingConfig.Instance;
			if (!(instance == null) && !(instance.sandDecalPrefab == null))
			{
				Vector3 position = point + normal * instance.decalSurfaceOffset;
				Quaternion rotation = Quaternion.AngleAxis(markYaw, normal) * Quaternion.FromToRotation(Vector3.up, normal);
				GameObject gameObject = Object.Instantiate(instance.sandDecalPrefab, position, rotation);
				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					gameObject.transform.GetChild(i).gameObject.SetActive(i == 0);
				}
				if (instance.markLifetimeSeconds > 0f)
				{
					Object.Destroy(gameObject, instance.markLifetimeSeconds);
				}
			}
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdPerformDig__Vector3__Vector3__Boolean__Boolean__Single(Vector3 point, Vector3 normal, bool firstStrike, bool spawnMark, float markYaw)
		{
			RpcPerformDig(point, normal, firstStrike, spawnMark, markYaw);
		}

		protected static void InvokeUserCode_CmdPerformDig__Vector3__Vector3__Boolean__Boolean__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdPerformDig called on client.");
			}
			else
			{
				((Shovel)obj).UserCode_CmdPerformDig__Vector3__Vector3__Boolean__Boolean__Single(reader.ReadVector3(), reader.ReadVector3(), reader.ReadBool(), reader.ReadBool(), reader.ReadFloat());
			}
		}

		protected void UserCode_RpcPerformDig__Vector3__Vector3__Boolean__Boolean__Single(Vector3 point, Vector3 normal, bool firstStrike, bool spawnMark, float markYaw)
		{
			DoDigFeedback(point, normal, firstStrike, spawnMark, markYaw);
		}

		protected static void InvokeUserCode_RpcPerformDig__Vector3__Vector3__Boolean__Boolean__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcPerformDig called on server.");
			}
			else
			{
				((Shovel)obj).UserCode_RpcPerformDig__Vector3__Vector3__Boolean__Boolean__Single(reader.ReadVector3(), reader.ReadVector3(), reader.ReadBool(), reader.ReadBool(), reader.ReadFloat());
			}
		}

		static Shovel()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Shovel), "System.Void NomadDrive.Features.Digging.Shovel::CmdPerformDig(UnityEngine.Vector3,UnityEngine.Vector3,System.Boolean,System.Boolean,System.Single)", InvokeUserCode_CmdPerformDig__Vector3__Vector3__Boolean__Boolean__Single, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(Shovel), "System.Void NomadDrive.Features.Digging.Shovel::RpcPerformDig(UnityEngine.Vector3,UnityEngine.Vector3,System.Boolean,System.Boolean,System.Single)", InvokeUserCode_RpcPerformDig__Vector3__Vector3__Boolean__Boolean__Single);
		}
	}
}
