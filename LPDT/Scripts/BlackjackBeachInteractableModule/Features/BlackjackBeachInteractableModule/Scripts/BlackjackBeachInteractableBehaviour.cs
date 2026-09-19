using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.BlackjackBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(57)]
	public class BlackjackBeachInteractableBehaviour : NetworkBehaviour
	{
		private const int CARD_COUNT = 54;

		private const float DEFAULT_PLAYFIELD_LOCAL_Y = 0.02f;

		[SerializeField]
		private Transform _playfieldTransform;

		[SerializeField]
		private Transform _cardSpawnPoint;

		[SerializeField]
		private Transform _cardFaceUpRotationReference;

		[SerializeField]
		private BlackjackCardZone _cardMoveZone;

		[Tooltip("Return-to-deck drop zone (put cards back here). Not the draw-pile occupancy.")]
		[SerializeField]
		private BlackjackCardZone _deckZone;

		[Tooltip("Draw-pile occupancy over spawn. Leaving this unlocks the next bell draw.")]
		[SerializeField]
		private BlackjackDrawOccupancyZone _drawOccupancyZone;

		[SerializeField]
		private List<BlackjackDeckBellInteractable> _deckBells = new List<BlackjackDeckBellInteractable>();

		[Tooltip("Bottom-first stack visuals: SM_CardBody_BJ (1), then (2), then (3).")]
		[SerializeField]
		private List<BlackjackDeckVisualCard> _deckVisualCards = new List<BlackjackDeckVisualCard>();

		private BlackjackConfiguration _configuration;

		private BlackjackCardId _pendingSpawnedCardId;

		private bool _drawInFlight;

		private readonly List<SimplePointGrabable> _fallbackBellGrabables = new List<SimplePointGrabable>();

		private readonly List<BlackjackCard> _activeCards = new List<BlackjackCard>();

		private readonly HashSet<NetworkId> _cardsReturningToDeck = new HashSet<NetworkId>();

		private int _lastSyncedDeckCount = -1;

		[WeaverGenerated]
		[DefaultForProperty("IsDeckInitialized", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _IsDeckInitialized;

		[WeaverGenerated]
		[DefaultForProperty("DeckCount", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _DeckCount;

		[WeaverGenerated]
		[DefaultForProperty("ActiveDeckCardId", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _ActiveDeckCardId;

		[WeaverGenerated]
		[DefaultForProperty("Deck", 3, 54)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private byte[] _Deck;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe NetworkBool IsDeckInitialized
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackBeachInteractableBehaviour.IsDeckInitialized. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackBeachInteractableBehaviour.IsDeckInitialized. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe int DeckCount
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackBeachInteractableBehaviour.DeckCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[1];
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackBeachInteractableBehaviour.DeckCount. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[1] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(2, 1)]
		private unsafe NetworkId ActiveDeckCardId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackBeachInteractableBehaviour.ActiveDeckCardId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)(Ptr + 2);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackBeachInteractableBehaviour.ActiveDeckCardId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 2) = value;
			}
		}

		[Networked]
		[Capacity(54)]
		[NetworkedWeaved(3, 54)]
		[NetworkedWeavedArray(54, 1, typeof(ElementReaderWriterUnmanaged<byte, MetaConstant1>))]
		private unsafe NetworkArray<byte> Deck
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BlackjackBeachInteractableBehaviour.Deck. Networked properties can only be accessed when Spawned() has been called.");
				}
				return new NetworkArray<byte>((byte*)Ptr + 12, 54, ElementReaderWriterUnmanaged<byte, MetaConstant1>.GetInstance());
			}
		}

		public Transform PlayfieldTransform => _playfieldTransform;

		public float CardMoveSpeed => _configuration.CardMoveSpeed;

		public float CardYawTurnSpeedDegrees => _configuration.CardYawTurnSpeedDegrees;

		public float RevealDuration => _configuration.RevealDuration;

		public float RevealJumpHeight => _configuration.RevealJumpHeight;

		public float ReturnDuration => _configuration.ReturnDuration;

		public Material CardDissolveMaterial => _configuration.CardDissolveMaterial;

		private float CardSpawnHeightOffset => _configuration.CardSpawnHeightOffset;

		private float CardRevealFlipAngleDegrees => _configuration.CardRevealFlipAngleDegrees;

		[Inject]
		public void InjectDependencies(BlackjackConfiguration configuration)
		{
			_configuration = configuration;
		}

		public override void Spawned()
		{
			base.Spawned();
			InitializeBellReferences();
			ResolveDeckVisualCards();
			SyncDeckVisuals(animate: false);
			if (base.HasStateAuthority && !IsDeckInitialized)
			{
				InitializeDeck();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			UnwireFallbackBellGrabables();
			_activeCards.Clear();
			_cardsReturningToDeck.Clear();
			base.Despawned(runner, hasState);
		}

		public override void Render()
		{
			SyncDeckVisuals(animate: true);
		}

		public override void FixedUpdateNetwork()
		{
			if (base.HasStateAuthority)
			{
				ReconcileActiveDeckCard();
			}
		}

		public void RegisterActiveCard(BlackjackCard card)
		{
			if (!(card == null) && !_activeCards.Contains(card))
			{
				_activeCards.Add(card);
			}
		}

		public void UnregisterActiveCard(BlackjackCard card)
		{
			if (!(card == null))
			{
				if (card.Object != null && card.Object.IsValid)
				{
					_cardsReturningToDeck.Remove(card.Object.Id);
				}
				_activeCards.Remove(card);
			}
		}

		public void RequestDrawCard()
		{
			if (!(base.Object == null) && base.Object.IsValid)
			{
				if (base.HasStateAuthority)
				{
					TryDrawCard();
				}
				else
				{
					RequestDrawCardRpc();
				}
			}
		}

		public void ClampCardXZ(float localX, float localZ, out float clampedX, out float clampedZ)
		{
			_cardMoveZone.ClampPlayfieldLocal(PlayfieldTransform, localX, localZ, out clampedX, out clampedZ);
		}

		public float GetPlayfieldLocalY(BlackjackCard self, float localX, float localZ)
		{
			float num = 0.02f;
			if (self == null || self.Object == null || !self.Object.IsValid)
			{
				return num;
			}
			float num2 = Mathf.Max(0.001f, _configuration.CardStackHeightStep);
			float num3 = Mathf.Max(0.01f, _configuration.CardOverlapRadius);
			float num4 = num3 * num3;
			uint raw = self.Object.Id.Raw;
			int num5 = 0;
			for (int num6 = _activeCards.Count - 1; num6 >= 0; num6--)
			{
				BlackjackCard blackjackCard = _activeCards[num6];
				if (blackjackCard == null)
				{
					_activeCards.RemoveAt(num6);
				}
				else if (!(blackjackCard == self) && !(blackjackCard.Object == null) && blackjackCard.Object.IsValid && !blackjackCard.IsReturning && (bool)blackjackCard.IsFaceUp)
				{
					Vector3 vector = PlayfieldTransform.InverseTransformPoint(blackjackCard.transform.position);
					float num7 = vector.x - localX;
					float num8 = vector.z - localZ;
					if (!(num7 * num7 + num8 * num8 > num4) && blackjackCard.Object.Id.Raw < raw)
					{
						num5++;
					}
				}
			}
			return num + (float)num5 * num2;
		}

		public bool IsInsideDeckZone(Vector3 worldPosition)
		{
			return _deckZone.ContainsWorldPoint(worldPosition);
		}

		public bool IsInsideDrawOccupancyZone(Vector3 worldPosition)
		{
			return _drawOccupancyZone.ContainsWorldPoint(worldPosition);
		}

		public Quaternion GetCardFaceDownRotation()
		{
			return _cardSpawnPoint.rotation;
		}

		public Quaternion GetCardFaceUpRotation()
		{
			return GetCardFaceDownRotation() * Quaternion.Euler(0f, 0f, 0f - CardRevealFlipAngleDegrees);
		}

		public Quaternion GetCardFaceUpRotationWithYawOffset(float yawOffsetDegrees)
		{
			return Quaternion.AngleAxis(yawOffsetDegrees, PlayfieldTransform.up) * GetCardFaceUpRotation();
		}

		public float GetFaceUpYawOffset(Quaternion worldRotation)
		{
			Quaternion cardFaceUpRotation = GetCardFaceUpRotation();
			Vector3 up = PlayfieldTransform.up;
			Vector3 vector = Vector3.ProjectOnPlane(worldRotation * Vector3.forward, up);
			Vector3 vector2 = Vector3.ProjectOnPlane(cardFaceUpRotation * Vector3.forward, up);
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.ProjectOnPlane(worldRotation * Vector3.up, up);
			}
			if (vector2.sqrMagnitude < 0.0001f)
			{
				vector2 = Vector3.ProjectOnPlane(cardFaceUpRotation * Vector3.up, up);
			}
			if (vector.sqrMagnitude < 0.0001f || vector2.sqrMagnitude < 0.0001f)
			{
				return 0f;
			}
			return Vector3.SignedAngle(vector2.normalized, vector.normalized, up);
		}

		public float GetFaceUpYawOffsetForTableSide(Vector3 worldPullerPosition)
		{
			Transform playfieldTransform = PlayfieldTransform;
			Vector3 vector = playfieldTransform.InverseTransformPoint(worldPullerPosition);
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				return 0f;
			}
			Vector3 direction = ((Mathf.Abs(vector.x) >= Mathf.Abs(vector.z)) ? new Vector3(Mathf.Sign(vector.x), 0f, 0f) : new Vector3(0f, 0f, Mathf.Sign(vector.z)));
			Vector3 worldDirection = playfieldTransform.TransformDirection(direction);
			return GetFaceUpYawOffsetForDirection(worldDirection);
		}

		public float GetFaceUpYawOffsetForDirection(Vector3 worldDirection)
		{
			Quaternion cardFaceUpRotation = GetCardFaceUpRotation();
			Vector3 up = PlayfieldTransform.up;
			Vector3 vector = Vector3.ProjectOnPlane(worldDirection, up);
			Vector3 vector2 = Vector3.ProjectOnPlane(cardFaceUpRotation * Vector3.forward, up);
			if (vector2.sqrMagnitude < 0.0001f)
			{
				vector2 = Vector3.ProjectOnPlane(cardFaceUpRotation * Vector3.up, up);
			}
			if (vector.sqrMagnitude < 0.0001f || vector2.sqrMagnitude < 0.0001f)
			{
				return 0f;
			}
			return Vector3.SignedAngle(vector2.normalized, vector.normalized, up);
		}

		public Quaternion GetCardRevealRotation(float normalizedTime)
		{
			return GetCardFaceDownRotation() * Quaternion.Euler(0f, 0f, Mathf.LerpAngle(0f, 0f - CardRevealFlipAngleDegrees, normalizedTime));
		}

		public Material GetCardMaterial(BlackjackCardId cardId)
		{
			if (cardId == BlackjackCardId.None)
			{
				return _configuration.CardBackMaterial;
			}
			return _configuration.GetMaterial(cardId);
		}

		public void NotifyCardLeftDrawOccupancy(NetworkId cardId)
		{
			if (base.HasStateAuthority)
			{
				ApplyCardLeftDrawOccupancy(cardId);
			}
			else
			{
				NotifyCardLeftDrawOccupancyRpc(cardId);
			}
		}

		public void RequestReturnCardToDeck(NetworkId cardId, BlackjackCardId cardValue)
		{
			if (base.HasStateAuthority)
			{
				TryReturnCardToDeck(cardId, cardValue);
			}
			else
			{
				RequestReturnCardToDeckRpc(cardId, cardValue);
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 3228091372u)]
		private void RequestDrawCardRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3228091372u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BlackjackBeachInteractableModule.Scripts.BlackjackBeachInteractableBehaviour::RequestDrawCardRpc()", invokeInfo, PlayerRef.None);
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
			TryDrawCard();
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 4199250939u)]
		private void NotifyCardLeftDrawOccupancyRpc([RpcPayload(4)] NetworkId cardId)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(4199250939u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BlackjackBeachInteractableModule.Scripts.BlackjackBeachInteractableBehaviour::NotifyCardLeftDrawOccupancyRpc(Fusion.NetworkId)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(cardId, 4);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			ApplyCardLeftDrawOccupancy(cardId);
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 1618672116u)]
		private void RequestReturnCardToDeckRpc([RpcPayload(4)] NetworkId cardId, [RpcPayload(1)] BlackjackCardId cardValue)
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				int bytePayloadSize = Fusion.RpcDataWriter.GetBytePayloadSize(4);
				bytePayloadSize += Fusion.RpcDataWriter.GetBytePayloadSize(1);
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(1618672116u, bytePayloadSize, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var writer);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BlackjackBeachInteractableModule.Scripts.BlackjackBeachInteractableBehaviour::RequestReturnCardToDeckRpc(Fusion.NetworkId,Features.BlackjackBeachInteractableModule.Scripts.BlackjackCardId)", invokeInfo, PlayerRef.None);
					}
					if (invokeInfo.SendMessageResult == RpcSendMessageResult.Sent)
					{
						writer.Write(cardId, 4);
						writer.Write(cardValue, 1);
						rpcBuilder.Send();
					}
				}
				NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromLocalCall((object)this, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), invokeInfo, RpcHostMode.SourceIsServer, PlayerRef.Invalid));
				if (invokeInfo.LocalInvokeResult != RpcLocalInvokeResult.Invoked)
				{
					return;
				}
			}
			TryReturnCardToDeck(cardId, cardValue);
		}

		private void TryDrawCard()
		{
			if (!base.HasStateAuthority || _drawInFlight)
			{
				return;
			}
			PruneActiveDeckCardIfMissing();
			ReconcileActiveDeckCard();
			if (ActiveDeckCardId.IsValid || DeckCount <= 0 || _configuration.CardPrefab == NetworkPrefabRef.Empty || !TryResolveDrawPose(DeckCount, out var spawnPosition, out var spawnRotation))
			{
				return;
			}
			_drawInFlight = true;
			BlackjackCardId cardId = (_pendingSpawnedCardId = DrawTopCard());
			NetworkObject networkObject = base.Runner.Spawn(_configuration.CardPrefab, spawnPosition, spawnRotation, base.Object.StateAuthority, InitializeSpawnedCard);
			if (networkObject == null)
			{
				AddCardBackToDeck(cardId);
				_drawInFlight = false;
				return;
			}
			ActiveDeckCardId = networkObject.Id;
			_drawInFlight = false;
			if (networkObject.TryGetComponent<BlackjackCard>(out var component))
			{
				component.InitializeNetwork(this, cardId);
				component.PlaceAt(spawnPosition, spawnRotation);
				component.PlayReveal();
			}
		}

		private void InitializeSpawnedCard(NetworkRunner runner, NetworkObject networkObject)
		{
			if (networkObject.TryGetComponent<BlackjackCard>(out var component))
			{
				component.InitializeNetwork(this, _pendingSpawnedCardId);
			}
		}

		private void TryReturnCardToDeck(NetworkId cardId, BlackjackCardId cardValue)
		{
			if (!base.HasStateAuthority || !cardId.IsValid || cardValue == BlackjackCardId.None || !base.Runner.TryFindObject(cardId, out var networkObject) || networkObject == null || !networkObject.TryGetComponent<BlackjackCard>(out var component) || (bool)component.IsReturning)
			{
				return;
			}
			if (!_cardsReturningToDeck.Contains(cardId))
			{
				if (DeckCount >= 54)
				{
					return;
				}
				_cardsReturningToDeck.Add(cardId);
				AddCardBackToDeck(cardValue);
				if (ActiveDeckCardId == cardId)
				{
					ActiveDeckCardId = default(NetworkId);
				}
			}
			component.RequestBeginReturn();
		}

		private void ApplyCardLeftDrawOccupancy(NetworkId cardId)
		{
			if (base.HasStateAuthority && cardId.IsValid && ActiveDeckCardId == cardId)
			{
				ActiveDeckCardId = default(NetworkId);
			}
		}

		private void ReconcileActiveDeckCard()
		{
			if (ActiveDeckCardId.IsValid)
			{
				BlackjackCard component;
				if (!base.Runner.TryFindObject(ActiveDeckCardId, out var networkObject) || networkObject == null)
				{
					ActiveDeckCardId = default(NetworkId);
				}
				else if (networkObject.TryGetComponent<BlackjackCard>(out component) && (bool)component.HasLeftDrawOccupancy)
				{
					ActiveDeckCardId = default(NetworkId);
				}
			}
		}

		private BlackjackCardId DrawTopCard()
		{
			int num = DeckCount - 1;
			byte result = Deck[num];
			Deck.Set(num, 0);
			DeckCount = num;
			return (BlackjackCardId)result;
		}

		private void AddCardBackToDeck(BlackjackCardId cardId)
		{
			int deckCount = DeckCount;
			Deck.Set(deckCount, (byte)cardId);
			DeckCount++;
			if (DeckCount > 1)
			{
				int index = UnityEngine.Random.Range(0, DeckCount);
				byte value = Deck[index];
				Deck.Set(index, Deck[deckCount]);
				Deck.Set(deckCount, value);
			}
		}

		private void InitializeDeck()
		{
			for (int i = 0; i < 54; i++)
			{
				Deck.Set(i, (byte)(i + 1));
			}
			DeckCount = 54;
			ShuffleDeck();
			IsDeckInitialized = true;
			ActiveDeckCardId = default(NetworkId);
			SyncDeckVisuals(animate: false);
		}

		private void ShuffleDeck()
		{
			for (int num = DeckCount - 1; num > 0; num--)
			{
				int index = UnityEngine.Random.Range(0, num + 1);
				byte value = Deck[num];
				Deck.Set(num, Deck[index]);
				Deck.Set(index, value);
			}
		}

		private void PruneActiveDeckCardIfMissing()
		{
			if (ActiveDeckCardId.IsValid && (!base.Runner.TryFindObject(ActiveDeckCardId, out var networkObject) || networkObject == null))
			{
				ActiveDeckCardId = default(NetworkId);
			}
		}

		private bool TryResolveDrawPose(int deckCountBeforeDraw, out Vector3 spawnPosition, out Quaternion spawnRotation)
		{
			spawnPosition = default(Vector3);
			spawnRotation = GetCardFaceDownRotation();
			BlackjackDeckVisualCard visualCardForDeckCount = GetVisualCardForDeckCount(deckCountBeforeDraw);
			if (visualCardForDeckCount != null)
			{
				spawnPosition = visualCardForDeckCount.SpawnTransform.position + Vector3.up * CardSpawnHeightOffset;
				return true;
			}
			spawnPosition = _cardSpawnPoint.position + Vector3.up * CardSpawnHeightOffset;
			return true;
		}

		private BlackjackDeckVisualCard GetVisualCardForDeckCount(int deckCount)
		{
			ResolveDeckVisualCards();
			if (_deckVisualCards.Count == 0 || deckCount <= 0 || deckCount > _deckVisualCards.Count)
			{
				return null;
			}
			return _deckVisualCards[deckCount - 1];
		}

		private void SyncDeckVisuals(bool animate)
		{
			ResolveDeckVisualCards();
			if (_deckVisualCards.Count == 0)
			{
				return;
			}
			int deckCount = DeckCount;
			if (_lastSyncedDeckCount == deckCount)
			{
				return;
			}
			float deckVisualDissolveDuration = _configuration.DeckVisualDissolveDuration;
			Material cardDissolveMaterial = CardDissolveMaterial;
			for (int i = 0; i < _deckVisualCards.Count; i++)
			{
				BlackjackDeckVisualCard blackjackDeckVisualCard = _deckVisualCards[i];
				if (!(blackjackDeckVisualCard == null))
				{
					bool flag = deckCount > i;
					if (animate && (bool)IsDeckInitialized)
					{
						blackjackDeckVisualCard.SetVisibleAnimated(flag, cardDissolveMaterial, deckVisualDissolveDuration);
					}
					else
					{
						blackjackDeckVisualCard.SetVisibleInstant(flag);
					}
				}
			}
			_lastSyncedDeckCount = deckCount;
		}

		private void ResolveDeckVisualCards()
		{
			for (int num = _deckVisualCards.Count - 1; num >= 0; num--)
			{
				if (_deckVisualCards[num] == null)
				{
					_deckVisualCards.RemoveAt(num);
				}
			}
			if (_deckVisualCards.Count > 0)
			{
				return;
			}
			BlackjackDeckVisualCard[] componentsInChildren = GetComponentsInChildren<BlackjackDeckVisualCard>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null)
				{
					_deckVisualCards.Add(componentsInChildren[i]);
				}
			}
			_deckVisualCards.Sort(CompareDeckVisualByNameAscending);
		}

		private int CompareDeckVisualByNameAscending(BlackjackDeckVisualCard left, BlackjackDeckVisualCard right)
		{
			string strA = ((left != null) ? left.name : string.Empty);
			string strB = ((right != null) ? right.name : string.Empty);
			return string.CompareOrdinal(strA, strB);
		}

		private void InitializeBellReferences()
		{
			for (int num = _deckBells.Count - 1; num >= 0; num--)
			{
				if (_deckBells[num] == null)
				{
					_deckBells.RemoveAt(num);
				}
			}
			BlackjackDeckBellInteractable[] componentsInChildren = GetComponentsInChildren<BlackjackDeckBellInteractable>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!(componentsInChildren[i] == null) && !_deckBells.Contains(componentsInChildren[i]))
				{
					_deckBells.Add(componentsInChildren[i]);
				}
			}
			for (int j = 0; j < _deckBells.Count; j++)
			{
				_deckBells[j].BindTable(this);
			}
			WireFallbackBellGrabables();
		}

		private void WireFallbackBellGrabables()
		{
			UnwireFallbackBellGrabables();
			SimplePointGrabable[] componentsInChildren = GetComponentsInChildren<SimplePointGrabable>(includeInactive: true);
			foreach (SimplePointGrabable simplePointGrabable in componentsInChildren)
			{
				if (!(simplePointGrabable == null) && !(simplePointGrabable.GetComponent<BlackjackDeckBellInteractable>() != null) && simplePointGrabable.name.StartsWith("BellGrabbable"))
				{
					simplePointGrabable.LocalOnGrab += OnFallbackBellGrabbed;
					_fallbackBellGrabables.Add(simplePointGrabable);
				}
			}
		}

		private void UnwireFallbackBellGrabables()
		{
			for (int i = 0; i < _fallbackBellGrabables.Count; i++)
			{
				if (_fallbackBellGrabables[i] != null)
				{
					_fallbackBellGrabables[i].LocalOnGrab -= OnFallbackBellGrabbed;
				}
			}
			_fallbackBellGrabables.Clear();
		}

		private void OnFallbackBellGrabbed(int playerId)
		{
			if (!(base.Runner == null) && base.Runner.LocalPlayer.PlayerId == playerId)
			{
				RequestDrawCard();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			IsDeckInitialized = _IsDeckInitialized;
			DeckCount = _DeckCount;
			ActiveDeckCardId = _ActiveDeckCardId;
			NetworkBehaviourUtils.InitializeNetworkArray(Deck, _Deck, "Deck");
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_IsDeckInitialized = IsDeckInitialized;
			_DeckCount = DeckCount;
			_ActiveDeckCardId = ActiveDeckCardId;
			NetworkBehaviourUtils.CopyFromNetworkArray(Deck, ref _Deck);
		}

		[NetworkRpcWeavedInvoker(3228091372u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestDrawCardRpc_0040Invoker3228091372([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BlackjackBeachInteractableBehaviour)context.TargetBehaviour).RequestDrawCardRpc();
		}

		[NetworkRpcWeavedInvoker(4199250939u)]
		[Preserve]
		[WeaverGenerated]
		protected static void NotifyCardLeftDrawOccupancyRpc_0040Invoker4199250939([In] ref RpcInvokeContext context)
		{
			context.PayloadReader.Read(out NetworkId value, 4);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BlackjackBeachInteractableBehaviour)context.TargetBehaviour).NotifyCardLeftDrawOccupancyRpc(value);
		}

		[NetworkRpcWeavedInvoker(1618672116u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestReturnCardToDeckRpc_0040Invoker1618672116([In] ref RpcInvokeContext context)
		{
			RpcDataReader payloadReader = context.PayloadReader;
			payloadReader.Read(out NetworkId value, 4);
			payloadReader.Read(out BlackjackCardId value2, 1);
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BlackjackBeachInteractableBehaviour)context.TargetBehaviour).RequestReturnCardToDeckRpc(value, value2);
		}
	}
}
