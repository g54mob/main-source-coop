using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Cysharp.Threading.Tasks;
using Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy;
using Features.GrabModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Features.BarBeachInteractableModule.Scripts
{
	[NetworkBehaviourWeaved(4)]
	public class BarBeachInteractableBehaviour : NetworkBehaviour, IStateAuthorityChanged, IPublicFacingInterface
	{
		[SerializeField]
		private BarServePath _servePath;

		[SerializeField]
		private BarBottleSpawner _bottleSpawner;

		[SerializeField]
		private SimplePointGrabable _bell;

		[SerializeField]
		private NetworkObject _bartenderPrefab;

		[SerializeField]
		private Vector3 _bartenderLocalPosition = new Vector3(0f, 0f, -1.2f);

		[SerializeField]
		private Vector3 _bartenderLocalEulerAngles = new Vector3(0f, 180f, 0f);

		[SerializeField]
		private Transform _bartenderCounterLookTarget;

		[SerializeField]
		private BarMusicController _musicController;

		[SerializeField]
		private TMP_Text _serveStockCounter;

		private BarBeachInteractableConfiguration _configuration;

		private MultiplayerModel _multiplayerModel;

		private readonly System.Random _random = new System.Random();

		private readonly Dictionary<int, BarBottleServeDriver> _slotBottles = new Dictionary<int, BarBottleServeDriver>();

		private bool _serveInFlight;

		private bool _bartenderSpawnInFlight;

		private BartenderEnemy _bartender;

		private int _lastRenderedServeStock = int.MinValue;

		[WeaverGenerated]
		[DefaultForProperty("ServeCooldown", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private TickTimer _ServeCooldown;

		[WeaverGenerated]
		[DefaultForProperty("BartenderNetworkId", 1, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkId _BartenderNetworkId;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ServeStock", 2, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private int _ServeStock;

		[WeaverGenerated]
		[DefaultForProperty("HasInitializedServeStock", 3, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private NetworkBool _HasInitializedServeStock;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe TickTimer ServeCooldown
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBeachInteractableBehaviour.ServeCooldown. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(TickTimer*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBeachInteractableBehaviour.ServeCooldown. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(TickTimer*)((byte*)Ptr + 0) = value;
			}
		}

		[Networked]
		[NetworkedWeaved(1, 1)]
		private unsafe NetworkId BartenderNetworkId
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBeachInteractableBehaviour.BartenderNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkId*)(Ptr + 1);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBeachInteractableBehaviour.BartenderNetworkId. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkId*)(Ptr + 1) = value;
			}
		}

		[Networked]
		[OnChangedRender("OnServeStockRender")]
		[NetworkedWeaved(2, 1)]
		public unsafe int ServeStock
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBeachInteractableBehaviour.ServeStock. Networked properties can only be accessed when Spawned() has been called.");
				}
				return Ptr[2];
			}
			private set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBeachInteractableBehaviour.ServeStock. Networked properties can only be accessed when Spawned() has been called.");
				}
				Ptr[2] = value;
			}
		}

		[Networked]
		[NetworkedWeaved(3, 1)]
		private unsafe NetworkBool HasInitializedServeStock
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBeachInteractableBehaviour.HasInitializedServeStock. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(NetworkBool*)(Ptr + 3);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing BarBeachInteractableBehaviour.HasInitializedServeStock. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)(Ptr + 3) = value;
			}
		}

		[Inject]
		private void InjectDependencies(BarBeachInteractableConfiguration configuration, MultiplayerModel multiplayerModel)
		{
			_configuration = configuration;
			_multiplayerModel = multiplayerModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			if (_bell != null)
			{
				_bell.LocalOnGrab += OnBellGrabbed;
			}
			if (_musicController == null)
			{
				_musicController = GetComponent<BarMusicController>();
			}
			if (_musicController != null)
			{
				_musicController.MusicPlayingChanged += OnMusicPlayingChanged;
			}
			if (base.Object.HasStateAuthority && _configuration != null && !HasInitializedServeStock)
			{
				HasInitializedServeStock = true;
				ServeStock = Mathf.Max(0, _configuration.InitialServeStock);
			}
			RebuildSlotMapFromBottles();
			RefreshServeStockCounter();
			TryResolveBartender();
			SyncBartenderStockState();
			SyncBartenderMusicState();
			if (base.Object.HasStateAuthority)
			{
				EnsureBartenderSpawnedAsync().Forget();
				_musicController?.ResyncButtonAfterAuthorityChanged();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_bell != null)
			{
				_bell.LocalOnGrab -= OnBellGrabbed;
			}
			if (_musicController != null)
			{
				_musicController.MusicPlayingChanged -= OnMusicPlayingChanged;
			}
			if (hasState && base.Object.HasStateAuthority)
			{
				DespawnBartender(runner);
			}
			_slotBottles.Clear();
			_bartender = null;
			base.Despawned(runner, hasState);
		}

		public void StateAuthorityChanged()
		{
			_serveInFlight = false;
			RebuildSlotMapFromBottles();
			TryResolveBartender();
			ConfigureResolvedBartender();
			SyncBartenderStockState();
			SyncBartenderMusicState();
			_musicController?.ResyncButtonAfterAuthorityChanged();
			RefreshServeStockCounter();
			if (base.Object.HasStateAuthority)
			{
				EnsureBartenderSpawnedAsync().Forget();
			}
		}

		public bool TryGetBartender(out BartenderEnemy bartender)
		{
			TryResolveBartender();
			bartender = _bartender;
			return (UnityEngine.Object)(object)bartender != null;
		}

		public bool TryRestockFromPayment()
		{
			if (!base.Object.HasStateAuthority || _configuration == null)
			{
				return false;
			}
			ServeStock = Mathf.Max(0, _configuration.RestockAmount);
			Debug.Log($"[BarServe] restock → ServeStock={ServeStock}");
			SyncBartenderStockState();
			return true;
		}

		public void TryRequestServeFromBell()
		{
			NetworkRunner networkRunner = _multiplayerModel?.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				Debug.Log("[BarServe] skip: runner null/not running");
				return;
			}
			Debug.Log($"[BarServe] bell grab → request. sa={base.Object.HasStateAuthority} master={networkRunner.IsSharedModeMasterClient} " + $"inFlight={_serveInFlight} cooldownBlocked={IsServeCooldownBlocked()} stock={ServeStock} " + $"remain={ServeCooldown.RemainingTime(base.Runner)} slots={FormatSlotDebug()}");
			if (base.Object.HasStateAuthority)
			{
				RequestServe();
			}
			else
			{
				RequestServeRpc();
			}
		}

		public void RequestServe()
		{
			NetworkRunner networkRunner = _multiplayerModel?.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || !networkRunner.IsSharedModeMasterClient)
			{
				Debug.Log($"[BarServe] RequestServe skip: masterGate runnerOk={networkRunner != null && networkRunner.IsRunning} master={networkRunner != null && networkRunner.IsSharedModeMasterClient}");
			}
			else if (!base.Object.HasStateAuthority || _serveInFlight)
			{
				Debug.Log($"[BarServe] RequestServe skip: sa={base.Object.HasStateAuthority} inFlight={_serveInFlight}");
			}
			else
			{
				RequestServeAsync().Forget();
			}
		}

		private async UniTaskVoid RequestServeAsync()
		{
			NetworkRunner networkRunner = _multiplayerModel?.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsSharedModeMasterClient || !base.Object.HasStateAuthority)
			{
				Debug.Log("[BarServe] async skip: not master SA");
				return;
			}
			if (_serveInFlight)
			{
				Debug.Log("[BarServe] async skip: already inFlight");
				return;
			}
			if (IsServeCooldownBlocked())
			{
				Debug.Log($"[BarServe] async skip: cooldown remain={ServeCooldown.RemainingTime(base.Runner)}");
				return;
			}
			if (_bottleSpawner == null || _servePath == null || _configuration == null)
			{
				Debug.Log($"[BarServe] async skip: refs spawner={_bottleSpawner != null} path={_servePath != null} cfg={_configuration != null}");
				return;
			}
			if (!TryGetFreeServeSlot(out var serveSlot))
			{
				Debug.Log("[BarServe] async skip: no free slot. " + FormatSlotDebug());
				return;
			}
			if (ServeStock <= 0)
			{
				Debug.Log("[BarServe] async skip: ServeStock empty (need coin restock)");
				SyncBartenderStockState();
				return;
			}
			if (!TryPickBottlePrefab(out var prefab))
			{
				Debug.Log("[BarServe] async skip: no bottle prefab in weights");
				return;
			}
			_serveInFlight = true;
			try
			{
				Vector3 spawnPosition = ((_servePath.StartPoint != null) ? _servePath.StartPoint.position : base.transform.position);
				Quaternion spawnRotation = ((_servePath.StartPoint != null) ? _servePath.StartPoint.rotation : base.transform.rotation);
				Debug.Log($"[BarServe] spawning '{prefab.name}' → slot {serveSlot} stock={ServeStock}");
				BarBottleServeDriver barBottleServeDriver = await _bottleSpawner.SpawnBottleAsync(prefab, spawnPosition, spawnRotation);
				if (barBottleServeDriver == null || !base.Object.HasStateAuthority)
				{
					Debug.Log($"[BarServe] spawn failed/lost SA. driverNull={barBottleServeDriver == null} sa={base.Object.HasStateAuthority}");
					return;
				}
				ServeCooldown = TickTimer.CreateFromSeconds(base.Runner, _configuration.ServeCooldownSeconds);
				_slotBottles[serveSlot] = barBottleServeDriver;
				barBottleServeDriver.Configure(_configuration, _servePath, OnBottleSlotFreed);
				barBottleServeDriver.StartSlide(serveSlot);
				ServeStock = Mathf.Max(0, ServeStock - 1);
				SyncBartenderStockState();
				Debug.Log($"[BarServe] slide started slot={serveSlot} stock={ServeStock} {FormatSlotDebug()}");
			}
			finally
			{
				_serveInFlight = false;
			}
		}

		private void OnBellGrabbed(int playerId)
		{
			TryRequestServeFromBell();
			NotifyBartenderBellGrabbed();
		}

		private void NotifyBartenderBellGrabbed()
		{
			TryResolveBartender();
			if ((UnityEngine.Object)(object)_bartender != null)
			{
				_bartender.NotifyBellGrabbed();
			}
			else if (!base.Object.HasStateAuthority)
			{
				NotifyBartenderBellGrabbedRpc();
			}
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 567574437u)]
		private void NotifyBartenderBellGrabbedRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(567574437u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BarBeachInteractableModule.Scripts.BarBeachInteractableBehaviour::NotifyBartenderBellGrabbedRpc()", invokeInfo, PlayerRef.None);
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
			TryResolveBartender();
			_bartender?.NotifyBellGrabbed();
		}

		[Rpc(RpcSources.All, RpcTargets.StateAuthority, Key = 3954113927u)]
		private void RequestServeRpc()
		{
			if (!NetworkBehaviourUtils.CheckInvokeRpc((NetworkBehaviour)this))
			{
				RpcInvokeInfo invokeInfo;
				using (Fusion.RpcBuilder rpcBuilder = base.Runner.CreateRpcBuilder(3954113927u, 0, (NetworkBehaviour)this, PlayerRef.Invalid))
				{
					invokeInfo = rpcBuilder.Prepare(out var _);
					if (NetworkBehaviourUtils.ShouldNotifyRpcError(invokeInfo))
					{
						NetworkBehaviourUtils.NotifyRpcError((ILogSource)this, "System.Void Features.BarBeachInteractableModule.Scripts.BarBeachInteractableBehaviour::RequestServeRpc()", invokeInfo, PlayerRef.None);
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
			RequestServe();
		}

		private async UniTaskVoid EnsureBartenderSpawnedAsync()
		{
			if (!base.Object.HasStateAuthority || _bartenderPrefab == null || _bartenderSpawnInFlight)
			{
				return;
			}
			if (TryResolveBartender())
			{
				ConfigureResolvedBartender();
				return;
			}
			NetworkRunner networkRunner = _multiplayerModel?.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || !networkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			_bartenderSpawnInFlight = true;
			try
			{
				if (TryResolveBartender())
				{
					ConfigureResolvedBartender();
					return;
				}
				Vector3 value = base.transform.TransformPoint(_bartenderLocalPosition);
				Quaternion value2 = base.transform.rotation * Quaternion.Euler(_bartenderLocalEulerAngles);
				NetworkObject networkObject = await networkRunner.SpawnAsync(_bartenderPrefab, value, value2, networkRunner.LocalPlayer);
				if (!(networkObject == null) && base.Object.HasStateAuthority)
				{
					value = base.transform.TransformPoint(_bartenderLocalPosition);
					value2 = base.transform.rotation * Quaternion.Euler(_bartenderLocalEulerAngles);
					networkObject.transform.SetPositionAndRotation(value, value2);
					BartenderNetworkId = networkObject.Id;
					_bartender = networkObject.GetComponent<BartenderEnemy>();
					ConfigureResolvedBartender();
				}
			}
			finally
			{
				_bartenderSpawnInFlight = false;
			}
		}

		private void ConfigureResolvedBartender()
		{
			if (!((UnityEngine.Object)(object)_bartender == null))
			{
				Transform transform = _bartenderCounterLookTarget;
				if (transform == null && _servePath != null)
				{
					transform = ((_servePath.MidPoint != null) ? _servePath.MidPoint : _servePath.StartPoint);
				}
				_bartender.SetCounterLookTarget(transform);
				SyncBartenderStockState();
				SyncBartenderMusicState();
			}
		}

		private void SyncBartenderStockState()
		{
			if (TryResolveBartender() && !((UnityEngine.Object)(object)_bartender == null))
			{
				_bartender.SetHasServeStock(ServeStock > 0);
			}
		}

		private void SyncBartenderMusicState()
		{
			if (TryResolveBartender() && !((UnityEngine.Object)(object)_bartender == null))
			{
				bool musicPlaying = _musicController != null && _musicController.IsPlaying;
				_bartender.SetMusicPlaying(musicPlaying);
			}
		}

		private void OnMusicPlayingChanged(bool isPlaying)
		{
			if (TryResolveBartender() && !((UnityEngine.Object)(object)_bartender == null))
			{
				_bartender.SetMusicPlaying(isPlaying);
			}
		}

		private bool TryResolveBartender()
		{
			if ((UnityEngine.Object)(object)_bartender != null && ((SimulationBehaviour)(object)_bartender).Object != null && ((SimulationBehaviour)(object)_bartender).Object.IsValid)
			{
				return true;
			}
			_bartender = null;
			if (BartenderNetworkId.IsValid && base.Runner != null && base.Runner.TryFindObject(BartenderNetworkId, out var networkObject) && networkObject != null && networkObject.TryGetComponent<BartenderEnemy>(out var component))
			{
				_bartender = component;
				return true;
			}
			return false;
		}

		private void DespawnBartender(NetworkRunner runner)
		{
			if (!(runner == null) && BartenderNetworkId.IsValid)
			{
				if (runner.TryFindObject(BartenderNetworkId, out var networkObject) && networkObject != null)
				{
					runner.Despawn(networkObject);
				}
				BartenderNetworkId = default(NetworkId);
				_bartender = null;
			}
		}

		private bool IsServeCooldownBlocked()
		{
			return !ServeCooldown.ExpiredOrNotRunning(base.Runner);
		}

		private void OnBottleSlotFreed(BarBottleServeDriver driver)
		{
			if (driver == null)
			{
				return;
			}
			List<int> list = null;
			foreach (KeyValuePair<int, BarBottleServeDriver> slotBottle in _slotBottles)
			{
				if (!(slotBottle.Value != driver))
				{
					if (list == null)
					{
						list = new List<int>();
					}
					list.Add(slotBottle.Key);
				}
			}
			if (list == null)
			{
				Debug.Log("[BarServe] slotFreed callback but driver not in map. " + FormatSlotDebug());
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				_slotBottles.Remove(list[i]);
			}
			Debug.Log("[BarServe] slot freed keys=[" + string.Join(",", list) + "] " + FormatSlotDebug());
		}

		private void RebuildSlotMapFromBottles()
		{
			_slotBottles.Clear();
			if (base.Runner == null)
			{
				return;
			}
			BarBottleServeDriver[] array = UnityEngine.Object.FindObjectsByType<BarBottleServeDriver>(FindObjectsSortMode.None);
			foreach (BarBottleServeDriver barBottleServeDriver in array)
			{
				if (barBottleServeDriver == null || barBottleServeDriver.Object == null || !barBottleServeDriver.Object.IsValid || !barBottleServeDriver.OccupiesServeSlot)
				{
					continue;
				}
				int serveSlotIndex = barBottleServeDriver.ServeSlotIndex;
				if (serveSlotIndex >= 0)
				{
					_slotBottles[serveSlotIndex] = barBottleServeDriver;
					if (_configuration != null && _servePath != null)
					{
						barBottleServeDriver.Configure(_configuration, _servePath, OnBottleSlotFreed);
					}
					barBottleServeDriver.EnsureSlideOrCounterStateAfterAuthorityChanged();
				}
			}
		}

		private void OnServeStockRender()
		{
			RefreshServeStockCounter();
		}

		private void RefreshServeStockCounter()
		{
			if (!((UnityEngine.Object)(object)_serveStockCounter == null) && _lastRenderedServeStock != ServeStock)
			{
				_lastRenderedServeStock = ServeStock;
				_serveStockCounter.text = ServeStock.ToString();
			}
		}

		private string FormatSlotDebug()
		{
			if (_servePath == null)
			{
				return "path=null";
			}
			int num = ((_configuration != null) ? Mathf.Clamp(_configuration.MaxBottlesOnCounter, 1, _servePath.ServePointCount) : _servePath.ServePointCount);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("max=").Append(num).Append(" mapCount=")
				.Append(_slotBottles.Count);
			for (int i = 0; i < num; i++)
			{
				stringBuilder.Append(" |").Append(i).Append('=');
				if (!_slotBottles.TryGetValue(i, out var value) || value == null)
				{
					stringBuilder.Append("empty");
					continue;
				}
				bool value2 = value.Object != null && value.Object.IsValid;
				stringBuilder.Append("valid=").Append(value2).Append(" occupy=")
					.Append(value.OccupiesServeSlot)
					.Append(" slide=")
					.Append(value.IsSliding)
					.Append(" counter=")
					.Append(value.IsOnCounter)
					.Append(" slotIdx=")
					.Append(value.ServeSlotIndex);
			}
			return stringBuilder.ToString();
		}

		private bool TryGetFreeServeSlot(out int slot)
		{
			slot = -1;
			if (_servePath == null)
			{
				return false;
			}
			int num = ((_configuration != null) ? Mathf.Clamp(_configuration.MaxBottlesOnCounter, 1, _servePath.ServePointCount) : _servePath.ServePointCount);
			for (int i = 0; i < num; i++)
			{
				if (!IsServeSlotOccupied(i))
				{
					slot = i;
					return true;
				}
			}
			return false;
		}

		private bool IsServeSlotOccupied(int slot)
		{
			if (_slotBottles.TryGetValue(slot, out var value) && value != null)
			{
				if (value.Object != null && value.Object.IsValid && value.OccupiesServeSlot)
				{
					return true;
				}
				_slotBottles.Remove(slot);
			}
			return false;
		}

		private bool TryPickBottlePrefab(out NetworkObject prefab)
		{
			prefab = null;
			IReadOnlyList<BarBottlePrefabWeight> readOnlyList = _configuration?.BottlePrefabWeights;
			if (readOnlyList == null || readOnlyList.Count == 0)
			{
				return false;
			}
			float num = 0f;
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				BarBottlePrefabWeight barBottlePrefabWeight = readOnlyList[i];
				if (barBottlePrefabWeight != null && !(barBottlePrefabWeight.Prefab == null) && !(barBottlePrefabWeight.Weight <= 0f))
				{
					num += barBottlePrefabWeight.Weight;
				}
			}
			if (num <= 0f)
			{
				return false;
			}
			float num2 = (float)_random.NextDouble() * num;
			float num3 = 0f;
			for (int j = 0; j < readOnlyList.Count; j++)
			{
				BarBottlePrefabWeight barBottlePrefabWeight2 = readOnlyList[j];
				if (barBottlePrefabWeight2 != null && !(barBottlePrefabWeight2.Prefab == null) && !(barBottlePrefabWeight2.Weight <= 0f))
				{
					num3 += barBottlePrefabWeight2.Weight;
					if (!(num2 > num3))
					{
						prefab = barBottlePrefabWeight2.Prefab;
						return true;
					}
				}
			}
			for (int num4 = readOnlyList.Count - 1; num4 >= 0; num4--)
			{
				BarBottlePrefabWeight barBottlePrefabWeight3 = readOnlyList[num4];
				if (!(barBottlePrefabWeight3?.Prefab == null))
				{
					prefab = barBottlePrefabWeight3.Prefab;
					return true;
				}
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			ServeCooldown = _ServeCooldown;
			BartenderNetworkId = _BartenderNetworkId;
			ServeStock = _ServeStock;
			HasInitializedServeStock = _HasInitializedServeStock;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_ServeCooldown = ServeCooldown;
			_BartenderNetworkId = BartenderNetworkId;
			_ServeStock = ServeStock;
			_HasInitializedServeStock = HasInitializedServeStock;
		}

		[NetworkRpcWeavedInvoker(567574437u)]
		[Preserve]
		[WeaverGenerated]
		protected static void NotifyBartenderBellGrabbedRpc_0040Invoker567574437([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BarBeachInteractableBehaviour)context.TargetBehaviour).NotifyBartenderBellGrabbedRpc();
		}

		[NetworkRpcWeavedInvoker(3954113927u)]
		[Preserve]
		[WeaverGenerated]
		protected static void RequestServeRpc_0040Invoker3954113927([In] ref RpcInvokeContext context)
		{
			NetworkRunner.DebugRpcEvent?.Invoke(NetworkRunnerDebugRpcEvent.FromRemoteCall(in context, MethodBase.GetMethodFromHandle((RuntimeMethodHandle)/*OpCode not supported: LdMemberToken*/), RpcHostMode.SourceIsServer));
			NetworkBehaviourUtils.InvokeRpc = true;
			((BarBeachInteractableBehaviour)context.TargetBehaviour).RequestServeRpc();
		}
	}
}
