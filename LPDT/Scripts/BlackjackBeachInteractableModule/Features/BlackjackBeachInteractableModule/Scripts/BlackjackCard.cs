using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.CustomSynchronizersModule.Scripts;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.BlackjackBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(9)]
	public class BlackjackCard : NetworkBehaviour
	{
		private const float GRAB_COLLIDER_HEIGHT = 0.07f;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private Rigidbody _rigidbody;

		[SerializeField]
		private PhysicsSynchronizer _physicsSynchronizer;

		[SerializeField]
		private Renderer _cardRenderer;

		[SerializeField]
		private BoxCollider _grabCollider;

		[SerializeField]
		private BlackjackCardDissolveEffect _dissolveEffect;

		private BlackjackConfiguration _configuration;

		private BlackjackBeachInteractableBehaviour _table;

		private Coroutine _returnRoutine;

		private Coroutine _revealRoutine;

		private Material _appliedMaterial;

		private bool _isDissolving;

		private bool _returnDissolveStarted;

		private BlackjackCardId _localCardId;

		private float _grabYawOffsetDegrees;

		private float _targetSeatYawOffsetDegrees;

		private bool _hasGrabYaw;

		private bool _revealMaterialSwapped;

		private bool _isRevealingLocal;

		private Vector3 _placedRestWorldPosition;

		private bool _hasPlacedRestWorldPosition;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("TableNetworkId", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _TableNetworkId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("NetworkedCardId", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte _NetworkedCardId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsFaceUp", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsFaceUp;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("HasLeftDrawOccupancy", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasLeftDrawOccupancy;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsReturning", 4, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsReturning;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("IsRevealing", 5, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsRevealing;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("DrawRestPosition", 6, 3)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Vector3 _DrawRestPosition;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe NetworkId TableNetworkId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.TableNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.TableNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		public unsafe byte NetworkedCardId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.NetworkedCardId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ((byte*)Ptr)[4];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.NetworkedCardId. Networked properties can only be accessed when Spawned() has been called.");
				}
				((sbyte*)Ptr)[4] = (sbyte)value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		public unsafe NetworkBool IsFaceUp
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.IsFaceUp. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.IsFaceUp. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 2) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		public unsafe NetworkBool HasLeftDrawOccupancy
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.HasLeftDrawOccupancy. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.HasLeftDrawOccupancy. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(4, 1)]
		public unsafe NetworkBool IsReturning
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.IsReturning. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 4);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.IsReturning. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 4) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(5, 1)]
		public unsafe NetworkBool IsRevealing
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.IsRevealing. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 5);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.IsRevealing. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 5) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(6, 3)]
		public unsafe Vector3 DrawRestPosition
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.DrawRestPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Vector3*)(Ptr + 6);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackCard.DrawRestPosition. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Vector3*)(Ptr + 6) = value;
			}
		}

		public BlackjackCardId CardId
		{
			get
			{
				if (NetworkedCardId != 0)
				{
					return (BlackjackCardId)NetworkedCardId;
				}
				return _localCardId;
			}
		}

		[Inject]
		public void InjectDependencies(BlackjackConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void InitializeNetwork(BlackjackBeachInteractableBehaviour table, BlackjackCardId cardId)
		{
			_table = table;
			TableNetworkId = table.Object.Id;
			NetworkedCardId = (byte)cardId;
			_localCardId = cardId;
			IsFaceUp = false;
			HasLeftDrawOccupancy = false;
			IsReturning = false;
			IsRevealing = false;
		}

		public Vector3 GetDrawRestPosition()
		{
			if (_hasPlacedRestWorldPosition)
			{
				return _placedRestWorldPosition;
			}
			if (DrawRestPosition.sqrMagnitude > 0.0001f)
			{
				return DrawRestPosition;
			}
			return _rigidbody.position;
		}

		public Vector3 GetDrawLockSamplePosition()
		{
			return _rigidbody.position;
		}

		public void PlaceAt(Vector3 worldPosition, Quaternion worldRotation)
		{
			ConfigureCardRigidbody();
			_placedRestWorldPosition = worldPosition;
			_hasPlacedRestWorldPosition = true;
			if (base.HasStateAuthority || base.Object == null || !base.Object.IsValid)
			{
				DrawRestPosition = worldPosition;
			}
			TeleportPose(worldPosition, worldRotation);
		}

		public override void Spawned()
		{
			base.Spawned();
			if (NetworkedCardId != 0)
			{
				_localCardId = (BlackjackCardId)NetworkedCardId;
			}
			CacheTableFromNetwork();
			ConfigureGrabCollider();
			ConfigureCardRigidbody();
			ApplyCurrentMaterial();
			if (DrawRestPosition.sqrMagnitude > 0.0001f)
			{
				_placedRestWorldPosition = DrawRestPosition;
				_hasPlacedRestWorldPosition = true;
			}
			if (_table != null)
			{
				_table.RegisterActiveCard(this);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_table != null)
			{
				_table.UnregisterActiveCard(this);
			}
			StopCardCoroutine(ref _revealRoutine);
			StopCardCoroutine(ref _returnRoutine);
			base.Despawned(runner, hasState);
		}

		public override void Render()
		{
			if (NetworkedCardId != 0)
			{
				_localCardId = (BlackjackCardId)NetworkedCardId;
			}
			if ((bool)IsReturning)
			{
				TryStartReturnDissolveVisual();
			}
			else if (!_isDissolving)
			{
				ApplyCurrentMaterial();
			}
		}

		public void PlayReveal()
		{
			if (base.HasStateAuthority && !IsRevealing && !IsReturning && !_isRevealingLocal)
			{
				Vector3 startPosition = (DrawRestPosition = GetDrawRestPosition());
				IsRevealing = true;
				PlayRevealRpc(startPosition);
			}
		}

		[Rpc(RpcSources.StateAuthority, RpcTargets.All, Key = 611312437u)]
		private void PlayRevealRpc([RpcPayload(12)] Vector3 startPosition)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(12);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(611312437u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BlackjackBeachInteractableModule.Scripts.BlackjackCard::PlayRevealRpc(UnityEngine.Vector3)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(startPosition, 12);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			StopCardCoroutine(ref _revealRoutine);
			_revealRoutine = StartCoroutine(RevealRoutineLocal(startPosition));
		}

		public void RequestBeginReturn()
		{
			BeginReturnToDeckRpc();
		}

		public void BeginReturnToDeck()
		{
			if (base.HasStateAuthority && !IsReturning)
			{
				IsReturning = true;
				TryStartReturnDissolveVisual();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 617294052u)]
		private void BeginReturnToDeckRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(617294052u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BlackjackBeachInteractableModule.Scripts.BlackjackCard::BeginReturnToDeckRpc()", invokeInfo, PlayerRef.None);
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
			BeginReturnToDeck();
		}

		private void FixedUpdate()
		{
			ConfigureCardRigidbody();
			if (_simplePointGrabable.Initialized && !(_simplePointGrabable.Object == null) && _simplePointGrabable.Object.IsValid && _simplePointGrabable.Object.HasStateAuthority && !IsReturning && !IsRevealing && !_isRevealingLocal)
			{
				ProcessZoneState();
				ProcessGrabbedMovement();
			}
		}

		private void ProcessGrabbedMovement()
		{
			if (_table == null)
			{
				return;
			}
			NetworkRunner runner = _simplePointGrabable.Runner;
			if (runner == null)
			{
				return;
			}
			int playerId = runner.LocalPlayer.PlayerId;
			if (!_simplePointGrabable.GrabbedByPlayers.Contains(playerId))
			{
				_hasGrabYaw = false;
			}
			else
			{
				if (_simplePointGrabable.GrabObject == null || _simplePointGrabable.GrabObject.Grabbers.Count == 0)
				{
					return;
				}
				PhysGrabber physGrabber = _simplePointGrabable.GrabObject.Grabbers[0];
				if (!(physGrabber == null) && !(physGrabber.physGrabPointPullerPosition == null))
				{
					Transform playfieldTransform = _table.PlayfieldTransform;
					Vector3 position = physGrabber.physGrabPointPullerPosition.position;
					Vector3 vector = playfieldTransform.InverseTransformPoint(position);
					_table.ClampCardXZ(vector.x, vector.z, out var clampedX, out var clampedZ);
					Vector3 vector2 = playfieldTransform.InverseTransformPoint(base.transform.position);
					float fixedDeltaTime = Time.fixedDeltaTime;
					float maxDelta = _table.CardMoveSpeed * fixedDeltaTime;
					float num = Mathf.MoveTowards(vector2.x, clampedX, maxDelta);
					float num2 = Mathf.MoveTowards(vector2.z, clampedZ, maxDelta);
					if (!_hasGrabYaw)
					{
						_grabYawOffsetDegrees = _table.GetFaceUpYawOffset(base.transform.rotation);
						_targetSeatYawOffsetDegrees = _table.GetFaceUpYawOffsetForTableSide(physGrabber.transform.position);
						_hasGrabYaw = true;
					}
					_grabYawOffsetDegrees = Mathf.MoveTowardsAngle(_grabYawOffsetDegrees, _targetSeatYawOffsetDegrees, _table.CardYawTurnSpeedDegrees * fixedDeltaTime);
					Vector3 position2 = vector2;
					position2.x = num;
					position2.z = num2;
					position2.y = _table.GetPlayfieldLocalY(this, num, num2);
					Vector3 worldPosition = playfieldTransform.TransformPoint(position2);
					Quaternion cardFaceUpRotationWithYawOffset = _table.GetCardFaceUpRotationWithYawOffset(_grabYawOffsetDegrees);
					ApplyDrivenPose(worldPosition, cardFaceUpRotationWithYawOffset);
				}
			}
		}

		private void ProcessZoneState()
		{
			if (!(_table == null) && (bool)IsFaceUp)
			{
				ProcessDrawOccupancyZone();
				ProcessReturnDeckZone();
			}
		}

		private void ProcessDrawOccupancyZone()
		{
			Vector3 drawLockSamplePosition = GetDrawLockSamplePosition();
			if (!_table.IsInsideDrawOccupancyZone(drawLockSamplePosition) && !HasLeftDrawOccupancy)
			{
				HasLeftDrawOccupancy = true;
				_table.NotifyCardLeftDrawOccupancy(base.Object.Id);
			}
		}

		private void ProcessReturnDeckZone()
		{
			if (!IsReturning)
			{
				bool flag = _simplePointGrabable.GrabbedByPlayersCount > 0;
				bool flag2 = _table.IsInsideDeckZone(GetDrawLockSamplePosition());
				if (!((!HasLeftDrawOccupancy && _table.IsInsideDrawOccupancyZone(GetDrawLockSamplePosition())) || !flag2 || flag))
				{
					_table.RequestReturnCardToDeck(base.Object.Id, CardId);
				}
			}
		}

		private IEnumerator RevealRoutineLocal(Vector3 startPosition)
		{
			if (_table == null)
			{
				_isRevealingLocal = false;
				if (base.HasStateAuthority)
				{
					IsRevealing = false;
				}
				yield break;
			}
			_isRevealingLocal = true;
			_revealMaterialSwapped = false;
			_placedRestWorldPosition = startPosition;
			_hasPlacedRestWorldPosition = true;
			if (base.HasStateAuthority)
			{
				_simplePointGrabable.LocalGrabBlocked = true;
			}
			float duration = Mathf.Max(0.01f, _table.RevealDuration);
			Quaternion targetRotation = _table.GetCardFaceUpRotation();
			ApplyRevealVisual(startPosition, 0f);
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				if (!_revealMaterialSwapped && num >= 0.5f)
				{
					_revealMaterialSwapped = true;
					if (base.HasStateAuthority)
					{
						IsFaceUp = true;
					}
					ApplyCurrentMaterial();
				}
				ApplyRevealVisual(startPosition, num);
				yield return null;
			}
			if (base.HasStateAuthority)
			{
				IsFaceUp = true;
				IsRevealing = false;
				ApplyCurrentMaterial();
				TeleportPose(startPosition, targetRotation);
				_simplePointGrabable.LocalGrabBlocked = false;
			}
			else
			{
				ApplyRevealVisual(startPosition, 1f);
				base.transform.SetPositionAndRotation(startPosition, targetRotation);
			}
			_isRevealingLocal = false;
			_revealRoutine = null;
		}

		private void ApplyRevealVisual(Vector3 startPosition, float normalizedTime)
		{
			float revealJumpHeight = _table.RevealJumpHeight;
			Vector3 position = startPosition + Vector3.up * (Mathf.Sin(Mathf.Clamp01(normalizedTime) * MathF.PI) * revealJumpHeight);
			Quaternion cardRevealRotation = _table.GetCardRevealRotation(normalizedTime);
			base.transform.SetPositionAndRotation(position, cardRevealRotation);
			_rigidbody.isKinematic = true;
			_rigidbody.position = position;
			_rigidbody.rotation = cardRevealRotation;
			_rigidbody.linearVelocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
		}

		private void TryStartReturnDissolveVisual()
		{
			if (!_returnDissolveStarted && (bool)IsReturning)
			{
				_returnDissolveStarted = true;
				StopCardCoroutine(ref _returnRoutine);
				_returnRoutine = StartCoroutine(ReturnToDeckRoutine());
			}
		}

		private IEnumerator ReturnToDeckRoutine()
		{
			_simplePointGrabable.LocalGrabBlocked = true;
			float duration = Mathf.Max(0.01f, (_table != null) ? _table.ReturnDuration : _configuration.ReturnDuration);
			Material material = ((_table != null) ? _table.CardDissolveMaterial : _configuration.CardDissolveMaterial);
			_isDissolving = true;
			if (material != null)
			{
				yield return _dissolveEffect.PlayDissolveOut(duration, material);
			}
			else
			{
				Vector3 startScale = base.transform.localScale;
				float elapsed = 0f;
				while (elapsed < duration)
				{
					elapsed += Time.deltaTime;
					float t = Mathf.Clamp01(elapsed / duration);
					base.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
					yield return null;
				}
			}
			if (base.Runner != null && base.Object != null && base.Object.IsValid && base.HasStateAuthority)
			{
				base.Runner.Despawn(base.Object);
			}
			_returnRoutine = null;
		}

		private void CacheTableFromNetwork()
		{
			if (!(_table != null) && !(base.Runner == null) && TableNetworkId.IsValid && base.Runner.TryFindObject(TableNetworkId, out var networkObject))
			{
				networkObject.TryGetComponent<BlackjackBeachInteractableBehaviour>(out _table);
			}
		}

		private void ApplyCurrentMaterial()
		{
			if (!_isDissolving)
			{
				BlackjackCardId cardId = (IsFaceUp ? CardId : BlackjackCardId.None);
				Material material = ResolveMaterial(cardId);
				if (!(material == null) && !(material == _appliedMaterial))
				{
					ApplyMaterialToRenderer(material);
					_appliedMaterial = material;
				}
			}
		}

		private Material ResolveMaterial(BlackjackCardId cardId)
		{
			if (_table != null)
			{
				return _table.GetCardMaterial(cardId);
			}
			if (cardId == BlackjackCardId.None)
			{
				return _configuration.CardBackMaterial;
			}
			return _configuration.GetMaterial(cardId);
		}

		private void ApplyMaterialToRenderer(Material material)
		{
			Material[] sharedMaterials = _cardRenderer.sharedMaterials;
			if (sharedMaterials == null || sharedMaterials.Length == 0)
			{
				_cardRenderer.sharedMaterial = material;
				return;
			}
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				sharedMaterials[i] = material;
			}
			_cardRenderer.sharedMaterials = sharedMaterials;
		}

		private void ApplyDrivenPose(Vector3 worldPosition, Quaternion worldRotation)
		{
			_rigidbody.isKinematic = true;
			_rigidbody.MovePosition(worldPosition);
			_rigidbody.MoveRotation(worldRotation);
			_rigidbody.angularVelocity = Vector3.zero;
		}

		private void TeleportPose(Vector3 worldPosition, Quaternion worldRotation)
		{
			if (base.Object != null && base.Object.IsValid)
			{
				_physicsSynchronizer.Teleport(worldPosition, worldRotation);
				return;
			}
			_rigidbody.isKinematic = true;
			_rigidbody.position = worldPosition;
			_rigidbody.rotation = worldRotation;
			_rigidbody.linearVelocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
			base.transform.SetPositionAndRotation(worldPosition, worldRotation);
		}

		private void ConfigureGrabCollider()
		{
			Vector3 size = _grabCollider.size;
			Vector3 center = _grabCollider.center;
			if (size.y < 0.07f)
			{
				float num = 0.07f - size.y;
				size.y = 0.07f;
				center.y += num * 0.5f;
				_grabCollider.size = size;
				_grabCollider.center = center;
			}
		}

		private void ConfigureCardRigidbody()
		{
			_rigidbody.isKinematic = true;
			_rigidbody.useGravity = false;
			_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
			if (_simplePointGrabable.GrabObject != null)
			{
				_simplePointGrabable.GrabObject.GrabbingPhysicsBlocked = false;
			}
		}

		private void StopCardCoroutine(ref Coroutine coroutine)
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
				coroutine = null;
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			TableNetworkId = _TableNetworkId;
			NetworkedCardId = _NetworkedCardId;
			IsFaceUp = _IsFaceUp;
			HasLeftDrawOccupancy = _HasLeftDrawOccupancy;
			IsReturning = _IsReturning;
			IsRevealing = _IsRevealing;
			DrawRestPosition = _DrawRestPosition;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_TableNetworkId = TableNetworkId;
			_NetworkedCardId = NetworkedCardId;
			_IsFaceUp = IsFaceUp;
			_HasLeftDrawOccupancy = HasLeftDrawOccupancy;
			_IsReturning = IsReturning;
			_IsRevealing = IsRevealing;
			_DrawRestPosition = DrawRestPosition;
		}

		[NetworkRpcWeavedInvoker(611312437u)]
		[Preserve]
		[WeaverGenerated]
		protected static void PlayRevealRpc_0040Invoker611312437([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out Vector3 value, 12);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BlackjackCard)context.TargetBehaviour).PlayRevealRpc(value);
		}

		[NetworkRpcWeavedInvoker(617294052u)]
		[Preserve]
		[WeaverGenerated]
		protected static void BeginReturnToDeckRpc_0040Invoker617294052([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BlackjackCard)context.TargetBehaviour).BeginReturnToDeckRpc();
		}
	}
}
