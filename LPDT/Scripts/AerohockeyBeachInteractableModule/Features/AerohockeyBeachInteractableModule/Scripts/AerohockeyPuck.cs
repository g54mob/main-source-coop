using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.CustomSynchronizersModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;

namespace Features.AerohockeyBeachInteractableModule.Scripts
{
	[DefaultExecutionOrder(50)]
	[NetworkBehaviourWeaved(0)]
	public class AerohockeyPuck : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		private const float MinIntoSurfaceSpeed = 0.05f;

		private const float PaddleHitCooldownSeconds = 0.12f;

		private const float PaddleContactGap = 0.02f;

		private const float AuthorityClaimRetrySeconds = 0.12f;

		private const float PendingHitMaxGap = 0.04f;

		private const float SoftResolveFraction = 1f;

		private const float SoftResolveSkin = 0.008f;

		private const float SoftResolveSeparateGain = 16f;

		[SerializeField]
		private AerohockeyBeachInteractableBehaviour _table;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private Collider _collider;

		[SerializeField]
		private PhysicsSynchronizer _physicsSynchronizer;

		[SerializeField]
		private AerohockeyPaddle _blackPaddle;

		[SerializeField]
		private AerohockeyPaddle _redPaddle;

		[SerializeField]
		private AerohockeyGoalTrigger _blackGoal;

		[SerializeField]
		private AerohockeyGoalTrigger _redGoal;

		private float _maxSpeed = 4f;

		private float _linearDamping = 1.1f;

		private float _angularDamping = 1.2f;

		private float _wallRestitution = 0.72f;

		private float _wallTangentRetain = 0.85f;

		private float _paddleHitVelocityScale = 0.85f;

		private float _playfieldLocalY;

		private Transform _playfield;

		private bool _goalLatch;

		private Vector3 _velocityBeforeSimulate;

		private float _blackPaddleHitCooldown;

		private float _redPaddleHitCooldown;

		private bool _pendingLocalHit;

		private AerohockeyPaddle _pendingHitPaddle;

		private Vector3 _pendingHitNormal;

		private Vector3 _pendingHitPaddleVelocity;

		private bool _pendingHitWasTouching;

		private bool _authorityClaimInFlight;

		private float _authorityClaimSentAt;

		public Rigidbody Rigidbody => _rigidbody;

		public Collider Collider => _collider;

		public PhysicsSynchronizer PhysicsSynchronizer => _physicsSynchronizer;

		private void ApplyAuthorityPhysicsMode()
		{
			bool flag = base.Object != null && base.Object.IsValid && base.HasStateAuthority && base.gameObject.activeSelf;
			_rigidbody.isKinematic = !flag;
			_rigidbody.detectCollisions = flag;
			if (!flag)
			{
				_rigidbody.linearVelocity = Vector3.zero;
				_rigidbody.angularVelocity = Vector3.zero;
			}
			EnsurePaddleCollisionsIgnored();
		}

		public void Bind(AerohockeyBeachInteractableBehaviour table, Transform playfield, float playfieldLocalY, float maxSpeed, float linearDamping, float angularDamping, float wallRestitution, float wallTangentRetain, float paddleHitVelocityScale)
		{
			_table = table;
			_playfield = playfield;
			_playfieldLocalY = playfieldLocalY;
			_maxSpeed = maxSpeed;
			_linearDamping = linearDamping;
			_angularDamping = angularDamping;
			_wallRestitution = Mathf.Clamp01(wallRestitution);
			_wallTangentRetain = Mathf.Clamp01(wallTangentRetain);
			_paddleHitVelocityScale = paddleHitVelocityScale;
			ApplyPhysicsSettings();
			EnsurePaddleCollisionsIgnored();
		}

		public override void Spawned()
		{
			base.Spawned();
			ApplyAuthorityPhysicsMode();
		}

		public void StateAuthorityChanged()
		{
			ApplyAuthorityPhysicsMode();
			if (base.HasStateAuthority)
			{
				_authorityClaimInFlight = false;
				return;
			}
			ClearPendingLocalHit();
			_authorityClaimInFlight = false;
		}

		public void HideForRespawn()
		{
			_goalLatch = true;
			ClearPendingLocalHit();
			_authorityClaimInFlight = false;
			_rigidbody.linearVelocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			_rigidbody.isKinematic = true;
			_rigidbody.detectCollisions = false;
			SetActiveRpc(active: false);
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 1169518487u)]
		public void HideForRespawnRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1169518487u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AerohockeyBeachInteractableModule.Scripts.AerohockeyPuck::HideForRespawnRpc()", invokeInfo, PlayerRef.None);
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
			HideForRespawn();
		}

		public void PlaceAt(Vector3 worldPosition)
		{
			SetActiveRpc(active: true);
			_goalLatch = false;
			_blackPaddleHitCooldown = 0f;
			_redPaddleHitCooldown = 0f;
			ClearPendingLocalHit();
			_authorityClaimInFlight = false;
			_playfieldLocalY = _playfield.InverseTransformPoint(worldPosition).y;
			_rigidbody.linearVelocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			if (_physicsSynchronizer != null && _physicsSynchronizer.Object != null && _physicsSynchronizer.Object.IsValid)
			{
				_physicsSynchronizer.Teleport(worldPosition, _rigidbody.rotation);
			}
			else
			{
				_rigidbody.position = worldPosition;
				base.transform.position = worldPosition;
			}
			ApplyAuthorityPhysicsMode();
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 4062753811u)]
		public void PlaceAtRpc([RpcPayload(12)] Vector3 worldPosition)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4062753811u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AerohockeyBeachInteractableModule.Scripts.AerohockeyPuck::PlaceAtRpc(UnityEngine.Vector3)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(worldPosition, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			PlaceAt(worldPosition);
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 501501461u)]
		private void SetActiveRpc([RpcPayload(4)] bool active)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int payloadSize = Fusion.RpcDataWriter.GetPayloadSize(active);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(501501461u, payloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.AerohockeyBeachInteractableModule.Scripts.AerohockeyPuck::SetActiveRpc(System.Boolean)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(active);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			if (base.gameObject.activeSelf != active)
			{
				base.gameObject.SetActive(active);
			}
		}

		public void SnapToPlayfieldHeight()
		{
			if (base.HasStateAuthority && base.gameObject.activeSelf)
			{
				Vector3 position = _playfield.InverseTransformPoint(_rigidbody.position);
				Vector3 direction = _playfield.InverseTransformDirection(_rigidbody.linearVelocity);
				if (Mathf.Abs(direction.y) > 0.0001f)
				{
					direction.y = 0f;
					_rigidbody.linearVelocity = _playfield.TransformDirection(direction);
				}
				if (!(Mathf.Abs(position.y - _playfieldLocalY) <= 0.002f))
				{
					position.y = _playfieldLocalY;
					Vector3 position2 = _playfield.TransformPoint(position);
					_rigidbody.position = position2;
					base.transform.position = position2;
				}
			}
		}

		public void ClampSpeed()
		{
			if (base.HasStateAuthority && base.gameObject.activeSelf && !_rigidbody.isKinematic)
			{
				Vector3 linearVelocity = _rigidbody.linearVelocity;
				float magnitude = linearVelocity.magnitude;
				if (magnitude > _maxSpeed && magnitude > 0.001f)
				{
					_rigidbody.linearVelocity = linearVelocity * (_maxSpeed / magnitude);
				}
			}
		}

		private void ApplyPhysicsSettings()
		{
			_rigidbody.useGravity = false;
			_rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
			_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			_rigidbody.constraints = (RigidbodyConstraints)84;
			_rigidbody.linearDamping = _linearDamping;
			_rigidbody.angularDamping = _angularDamping;
			_collider.isTrigger = false;
		}

		private void EnsurePaddleCollisionsIgnored()
		{
			IgnorePaddleColliders(_blackPaddle);
			IgnorePaddleColliders(_redPaddle);
		}

		private void IgnorePaddleColliders(AerohockeyPaddle paddle)
		{
			if (!(_collider == null) && !(paddle == null))
			{
				if (paddle.HitCollider != null)
				{
					Physics.IgnoreCollision(_collider, paddle.HitCollider, ignore: true);
				}
				if (paddle.AuthorityClaimCollider != null)
				{
					Physics.IgnoreCollision(_collider, paddle.AuthorityClaimCollider, ignore: true);
				}
			}
		}

		private void FixedUpdate()
		{
			if ((bool)base.Object && base.Object.IsValid && base.gameObject.activeSelf)
			{
				TryClaimPuckAuthority(_blackPaddle);
				TryClaimPuckAuthority(_redPaddle);
				if (base.HasStateAuthority && !_rigidbody.isKinematic)
				{
					EnsurePaddleCollisionsIgnored();
					TickCooldown(ref _blackPaddleHitCooldown);
					TickCooldown(ref _redPaddleHitCooldown);
					_velocityBeforeSimulate = _rigidbody.linearVelocity;
					SnapToPlayfieldHeight();
					TryScriptedPaddleHit(_blackPaddle, ref _blackPaddleHitCooldown);
					TryScriptedPaddleHit(_redPaddle, ref _redPaddleHitCooldown);
					TryConsumePendingLocalHit();
					SoftResolvePaddleOverlap(_blackPaddle);
					SoftResolvePaddleOverlap(_redPaddle);
					ClampSpeed();
				}
			}
		}

		private static void TickCooldown(ref float cooldown)
		{
			if (cooldown > 0f)
			{
				cooldown -= Time.fixedDeltaTime;
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			HandleCollision(collision);
		}

		private void HandleCollision(Collision collision)
		{
			if ((bool)base.Object && base.Object.IsValid && base.HasStateAuthority && base.gameObject.activeSelf && !TryGetHitPaddle(collision.collider, out var _) && !IsGoalCollider(collision.collider) && ApplyWallBounce(collision))
			{
				_table.NotifyPuckHitWall();
			}
		}

		private void TryScriptedPaddleHit(AerohockeyPaddle paddle, ref float cooldown)
		{
			if (!(paddle == null) && !(paddle.HitCollider == null) && !(cooldown > 0f) && TryGetPaddleSeparation(paddle.HitCollider, 0.02f, out var playfieldNormal, out var _))
			{
				Vector3 worldVelocity = paddle.WorldVelocity;
				if (!(Vector3.Dot(_velocityBeforeSimulate - worldVelocity, playfieldNormal) >= -0.05f))
				{
					ApplyPaddleImpulse(worldVelocity, playfieldNormal, ref cooldown);
				}
			}
		}

		private void SoftResolvePaddleOverlap(AerohockeyPaddle paddle)
		{
			if (!(paddle == null) && !(paddle.HitCollider == null) && TryGetPaddlePenetration(paddle.HitCollider, out var playfieldNormal, out var depth))
			{
				float depth2 = depth * 1f + 0.008f;
				DepenetrateFromPaddle(playfieldNormal, depth2);
				Vector3 worldVelocity = paddle.WorldVelocity;
				Vector3 linearVelocity = _rigidbody.linearVelocity;
				float num = 0f - Vector3.Dot(linearVelocity - worldVelocity, playfieldNormal);
				if (num > 0f)
				{
					linearVelocity += playfieldNormal * num;
				}
				float num2 = depth * 16f;
				float num3 = Vector3.Dot(linearVelocity - worldVelocity, playfieldNormal);
				if (num3 < num2)
				{
					linearVelocity += playfieldNormal * (num2 - num3);
				}
				SetPlayfieldPlanarVelocity(linearVelocity);
			}
		}

		private bool TryGetPaddlePenetration(Collider paddleCollider, out Vector3 playfieldNormal, out float depth)
		{
			playfieldNormal = Vector3.zero;
			depth = 0f;
			Rigidbody attachedRigidbody = paddleCollider.attachedRigidbody;
			Vector3 vector = ((attachedRigidbody != null) ? attachedRigidbody.position : paddleCollider.transform.position);
			Quaternion rotationB = ((attachedRigidbody != null) ? attachedRigidbody.rotation : paddleCollider.transform.rotation);
			if (!Physics.ComputePenetration(_collider, _rigidbody.position, _rigidbody.rotation, paddleCollider, vector, rotationB, out var direction, out var distance))
			{
				return false;
			}
			if (!TryGetPlayfieldNormal(direction, out playfieldNormal))
			{
				Vector3 vector2 = FlattenToPlayfield(vector - _rigidbody.position);
				if (vector2.sqrMagnitude < 0.0001f)
				{
					return false;
				}
				playfieldNormal = vector2.normalized;
			}
			depth = distance;
			return depth > 0.0001f;
		}

		private void TryClaimPuckAuthority(AerohockeyPaddle paddle)
		{
			if (base.HasStateAuthority || _goalLatch || _playfield == null)
			{
				return;
			}
			if (paddle == null || paddle.HitCollider == null || !paddle.IsLocalPlayerDriving)
			{
				CancelClaimIfPendingPaddle(paddle);
				return;
			}
			Collider paddleCollider = ((paddle.AuthorityClaimCollider != null) ? paddle.AuthorityClaimCollider : paddle.HitCollider);
			if (!TryGetPaddleSeparation(paddleCollider, 0.01f, out var _, out var separation))
			{
				CancelClaimIfPendingPaddle(paddle);
				return;
			}
			if (TryGetPaddleSeparation(paddle.HitCollider, 0.02f, out var playfieldNormal2, out separation))
			{
				_pendingHitWasTouching = true;
				_pendingHitNormal = playfieldNormal2;
				_pendingHitPaddleVelocity = paddle.WorldVelocity;
			}
			_pendingLocalHit = true;
			_pendingHitPaddle = paddle;
			if (!_authorityClaimInFlight || !(Time.time - _authorityClaimSentAt < 0.12f))
			{
				_authorityClaimInFlight = true;
				_authorityClaimSentAt = Time.time;
				base.Object.RequestStateAuthority();
			}
		}

		private void CancelClaimIfPendingPaddle(AerohockeyPaddle paddle)
		{
			if (!(_pendingHitPaddle != paddle))
			{
				ClearPendingLocalHit();
				_authorityClaimInFlight = false;
			}
		}

		private void ClearPendingLocalHit()
		{
			_pendingLocalHit = false;
			_pendingHitPaddle = null;
			_pendingHitNormal = Vector3.zero;
			_pendingHitPaddleVelocity = Vector3.zero;
			_pendingHitWasTouching = false;
		}

		private void TryConsumePendingLocalHit()
		{
			if (!_pendingLocalHit)
			{
				return;
			}
			AerohockeyPaddle pendingHitPaddle = _pendingHitPaddle;
			if (pendingHitPaddle == null || pendingHitPaddle.HitCollider == null)
			{
				ClearPendingLocalHit();
				return;
			}
			ref float reference = ref pendingHitPaddle.Side == AerohockeySide.Black ? ref _blackPaddleHitCooldown : ref _redPaddleHitCooldown;
			if (reference > 0f)
			{
				ClearPendingLocalHit();
				return;
			}
			Vector3 playfieldNormal;
			float separation;
			bool flag = TryGetPaddleSeparation(pendingHitPaddle.HitCollider, 0.04f, out playfieldNormal, out separation);
			if (flag || _pendingHitWasTouching)
			{
				Vector3 normal = (flag ? playfieldNormal : _pendingHitNormal);
				Vector3 paddleVelocity = ((pendingHitPaddle.WorldVelocity.sqrMagnitude > 0.0001f) ? pendingHitPaddle.WorldVelocity : _pendingHitPaddleVelocity);
				if (normal.sqrMagnitude > 0.0001f)
				{
					ApplyPaddleImpulse(paddleVelocity, normal, ref reference);
				}
				ClearPendingLocalHit();
			}
			else
			{
				Collider paddleCollider = ((pendingHitPaddle.AuthorityClaimCollider != null) ? pendingHitPaddle.AuthorityClaimCollider : pendingHitPaddle.HitCollider);
				if (!TryGetPaddleSeparation(paddleCollider, 0.01f, out var _, out separation))
				{
					ClearPendingLocalHit();
				}
			}
		}

		private void ApplyPaddleImpulse(Vector3 paddleVelocity, Vector3 normal, ref float cooldown)
		{
			Vector3 vector = ReflectWithLoss(_velocityBeforeSimulate - paddleVelocity, normal, _wallRestitution, _wallTangentRetain);
			SetPlayfieldPlanarVelocity((vector + paddleVelocity) * _paddleHitVelocityScale);
			_velocityBeforeSimulate = _rigidbody.linearVelocity;
			ClampSpeed();
			cooldown = 0.12f;
			_table.NotifyPuckHitPaddle();
		}

		private bool TryGetPaddleSeparation(Collider paddleCollider, float maxSeparation, out Vector3 playfieldNormal, out float separation)
		{
			playfieldNormal = Vector3.zero;
			separation = 0f;
			Transform transform = _collider.transform;
			Transform transform2 = paddleCollider.transform;
			if (Physics.ComputePenetration(_collider, transform.position, transform.rotation, paddleCollider, transform2.position, transform2.rotation, out var direction, out var distance))
			{
				if (!TryGetPlayfieldNormal(direction, out playfieldNormal))
				{
					return false;
				}
				separation = 0f - distance;
				return true;
			}
			Vector3 vector = paddleCollider.ClosestPoint(_rigidbody.position);
			Vector3 vector2 = FlattenToPlayfield(_rigidbody.position - vector);
			float sqrMagnitude = vector2.sqrMagnitude;
			if (sqrMagnitude < 0.0001f)
			{
				return false;
			}
			Bounds bounds = _collider.bounds;
			float num = Mathf.Max(bounds.extents.x, bounds.extents.z);
			float num2 = num + maxSeparation;
			if (sqrMagnitude > num2 * num2)
			{
				return false;
			}
			if (!TryGetPlayfieldNormal(vector2.normalized, out playfieldNormal))
			{
				return false;
			}
			separation = Mathf.Sqrt(sqrMagnitude) - num;
			return separation <= maxSeparation;
		}

		private void DepenetrateFromPaddle(Vector3 normal, float depth)
		{
			if (!(depth <= 0.0001f))
			{
				Vector3 vector = normal * (depth + 0.005f);
				Vector3 position = _rigidbody.position + vector;
				Vector3 position2 = _playfield.InverseTransformPoint(position);
				position2.y = _playfieldLocalY;
				position = _playfield.TransformPoint(position2);
				_rigidbody.position = position;
				base.transform.position = position;
			}
		}

		private Vector3 FlattenToPlayfield(Vector3 world)
		{
			Vector3 direction = _playfield.InverseTransformDirection(world);
			direction.y = 0f;
			return _playfield.TransformDirection(direction);
		}

		private bool ApplyWallBounce(Collision collision)
		{
			if (collision.contactCount == 0)
			{
				return false;
			}
			if (!TryGetPlayfieldNormal(collision.GetContact(0).normal, out var playfieldNormal))
			{
				return false;
			}
			Vector3 vector = _velocityBeforeSimulate;
			float num = Vector3.Dot(vector, playfieldNormal);
			if (num >= -0.05f)
			{
				vector = collision.relativeVelocity;
				num = Vector3.Dot(vector, playfieldNormal);
			}
			if (num >= -0.05f)
			{
				return false;
			}
			SetPlayfieldPlanarVelocity(ReflectWithLoss(vector, playfieldNormal, _wallRestitution, _wallTangentRetain));
			_velocityBeforeSimulate = _rigidbody.linearVelocity;
			ClampSpeed();
			return true;
		}

		private Vector3 ReflectWithLoss(Vector3 inbound, Vector3 normal, float restitution, float tangentRetain)
		{
			Vector3 vector = Vector3.Dot(inbound, normal) * normal;
			Vector3 vector2 = inbound - vector;
			return (0f - restitution) * vector + tangentRetain * vector2;
		}

		private void SetPlayfieldPlanarVelocity(Vector3 worldVelocity)
		{
			Vector3 direction = _playfield.InverseTransformDirection(worldVelocity);
			direction.y = 0f;
			_rigidbody.linearVelocity = _playfield.TransformDirection(direction);
		}

		private bool TryGetPlayfieldNormal(Vector3 worldNormal, out Vector3 playfieldNormal)
		{
			Vector3 vector = _playfield.InverseTransformDirection(worldNormal);
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				playfieldNormal = Vector3.zero;
				return false;
			}
			playfieldNormal = _playfield.TransformDirection(vector.normalized);
			return true;
		}

		private bool TryGetHitPaddle(Collider hit, out AerohockeyPaddle paddle)
		{
			if (hit == _blackPaddle.HitCollider)
			{
				paddle = _blackPaddle;
				return true;
			}
			if (hit == _redPaddle.HitCollider)
			{
				paddle = _redPaddle;
				return true;
			}
			paddle = null;
			return false;
		}

		private bool IsGoalCollider(Collider hit)
		{
			if (!(hit == _blackGoal.Trigger))
			{
				return hit == _redGoal.Trigger;
			}
			return true;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!_goalLatch && (bool)base.Object && base.Object.IsValid && base.HasStateAuthority && TryGetHitGoal(other, out var side))
			{
				_goalLatch = true;
				_table.NotifyGoalScored(side);
			}
		}

		private bool TryGetHitGoal(Collider hit, out AerohockeySide side)
		{
			if (hit == _blackGoal.Trigger)
			{
				side = _blackGoal.GoalSide;
				return true;
			}
			if (hit == _redGoal.Trigger)
			{
				side = _redGoal.GoalSide;
				return true;
			}
			side = AerohockeySide.Black;
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}

		[NetworkRpcWeavedInvoker(1169518487u)]
		[Preserve]
		[WeaverGenerated]
		protected static void HideForRespawnRpc_0040Invoker1169518487([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((AerohockeyPuck)context.TargetBehaviour).HideForRespawnRpc();
		}

		[NetworkRpcWeavedInvoker(4062753811u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlaceAtRpc_0040Invoker4062753811([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out Vector3 value, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((AerohockeyPuck)context.TargetBehaviour).PlaceAtRpc(value);
		}

		[NetworkRpcWeavedInvoker(501501461u)]
		[Preserve]
		[WeaverGenerated]
		protected static void SetActiveRpc_0040Invoker501501461([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out bool value);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((AerohockeyPuck)context.TargetBehaviour).SetActiveRpc(value);
		}
	}
}
