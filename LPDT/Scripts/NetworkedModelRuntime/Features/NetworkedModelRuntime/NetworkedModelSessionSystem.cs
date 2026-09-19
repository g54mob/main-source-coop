using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.MultiplayerSessionServices.Scripts;
using Features.NetworkedModelCodegen.Scripts;
using Fusion;
using NetworkServices.NetworkEvents;
using UnityEngine;
using Zenject;

namespace Features.NetworkedModelRuntime
{
	public class NetworkedModelSessionSystem : IInitializable, IDisposable, ITickable, INetworkedModelScopeController
	{
		private class ShadowEntry
		{
			public NetworkedModelShadowDescriptor Descriptor;

			public INetworkedModelShadowBridge Bridge;

			public NetworkObject Prefab;

			public NetworkObject Spawned;

			public NetworkObject Bound;

			public bool IsBound;

			public bool IsSpawning;

			public float NextAuthorityRequestTime;

			public int SpawnGeneration;
		}

		private readonly INetworkedModelRegistry _networkedModelRegistry;

		private readonly List<NetworkedModelShadowDescriptor> _descriptors;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private readonly SessionCleanupEvent _sessionCleanupEvent;

		private const float SCOPE_ATTACH_TIMEOUT_SECONDS = 5f;

		private const float AUTHORITY_REQUEST_RETRY_SECONDS = 0.5f;

		private readonly List<ShadowEntry> _entries = new List<ShadowEntry>();

		private readonly HashSet<ModelScope> _globalScopes = new HashSet<ModelScope>();

		private readonly HashSet<ModelScope> _localScopes = new HashSet<ModelScope>();

		public NetworkedModelSessionSystem(INetworkedModelRegistry networkedModelRegistry, List<NetworkedModelShadowDescriptor> descriptors, MultiplayerModel multiplayerModel, NetworkRunnerEventBus networkRunnerEventBus, SessionCleanupEvent sessionCleanupEvent)
		{
			_networkedModelRegistry = networkedModelRegistry;
			_descriptors = descriptors;
			_multiplayerModel = multiplayerModel;
			_networkRunnerEventBus = networkRunnerEventBus;
			_sessionCleanupEvent = sessionCleanupEvent;
		}

		public void Initialize()
		{
			_networkRunnerEventBus.Subscribe<OnStartGamePhaseEvent>(OnSessionReady);
			_networkRunnerEventBus.Subscribe<OnShutdownEvent>(OnSessionEnded);
			_sessionCleanupEvent.OnSessionCleanup += TeardownAll;
			BuildEntries();
		}

		public void Dispose()
		{
			_networkRunnerEventBus.Unsubscribe<OnStartGamePhaseEvent>(OnSessionReady);
			_networkRunnerEventBus.Unsubscribe<OnShutdownEvent>(OnSessionEnded);
			_sessionCleanupEvent.OnSessionCleanup -= TeardownAll;
			TeardownAll();
		}

		public void Tick()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
			foreach (ShadowEntry entry in _entries)
			{
				if ((entry.Descriptor.Ownership & ModelOwnership.Individual) != ModelOwnership.None)
				{
					ReconcileIndividual(networkRunner, entry);
				}
				else
				{
					ReconcileShared(networkRunner, entry);
				}
			}
		}

		public async UniTask OpenGlobalScopeAsync(ModelScope scope)
		{
			_globalScopes.Add(scope);
			ReconcileScopeOnce(scope);
			await AwaitScopeAttachedAsync(scope, isGlobal: true);
		}

		public async UniTask OpenLocalScopeAsync(ModelScope scope)
		{
			_localScopes.Add(scope);
			ReconcileScopeOnce(scope);
			await AwaitScopeAttachedAsync(scope, isGlobal: false);
		}

		public void CloseLocalScope(ModelScope scope)
		{
			_localScopes.Remove(scope);
		}

		public void CloseGlobalScope(ModelScope scope)
		{
			_globalScopes.Remove(scope);
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning || !networkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			foreach (ShadowEntry entry in _entries)
			{
				if (entry.Descriptor.Scope == scope && (entry.Descriptor.Ownership & ModelOwnership.Individual) == 0)
				{
					NetworkObject networkObject = FindTarget(networkRunner, entry, wantsOwned: false);
					if (networkObject != null && networkObject.IsValid && networkObject.HasStateAuthority)
					{
						networkRunner.Despawn(networkObject);
					}
					InvalidateInFlightSpawn(entry);
					entry.Spawned = null;
				}
			}
		}

		private void ReconcileScopeOnce(ModelScope scope)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return;
			}
			foreach (ShadowEntry entry in _entries)
			{
				if (entry.Descriptor.Scope == scope)
				{
					if ((entry.Descriptor.Ownership & ModelOwnership.Individual) != ModelOwnership.None)
					{
						ReconcileIndividual(networkRunner, entry);
					}
					else
					{
						ReconcileShared(networkRunner, entry);
					}
				}
			}
		}

		private async UniTask AwaitScopeAttachedAsync(ModelScope scope, bool isGlobal)
		{
			float deadline = Time.realtimeSinceStartup + 5f;
			while (true)
			{
				if (isGlobal ? IsGlobalScopeFulfilled(scope) : IsLocalScopeFulfilled(scope))
				{
					return;
				}
				if (Time.realtimeSinceStartup > deadline)
				{
					break;
				}
				await UniTask.Yield();
			}
			throw new TimeoutException(DescribeUnfulfilled(scope, isGlobal));
		}

		private bool IsLocalScopeFulfilled(ModelScope scope)
		{
			foreach (ShadowEntry entry in _entries)
			{
				if (entry.Descriptor.Scope == scope && !entry.IsBound)
				{
					return false;
				}
			}
			return true;
		}

		private bool IsGlobalScopeFulfilled(ModelScope scope)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return false;
			}
			if (!networkRunner.IsSharedModeMasterClient)
			{
				return true;
			}
			foreach (ShadowEntry entry in _entries)
			{
				if (entry.Descriptor.Scope == scope && (entry.Descriptor.Ownership & ModelOwnership.Individual) == 0 && FindTarget(networkRunner, entry, wantsOwned: false) == null)
				{
					return false;
				}
			}
			return true;
		}

		private string DescribeUnfulfilled(ModelScope scope, bool isGlobal)
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			List<string> list = new List<string>();
			foreach (ShadowEntry entry in _entries)
			{
				if (entry.Descriptor.Scope != scope)
				{
					continue;
				}
				if (isGlobal)
				{
					if ((entry.Descriptor.Ownership & ModelOwnership.Individual) == 0 && FindTarget(networkRunner, entry, wantsOwned: false) == null)
					{
						list.Add(entry.Descriptor.ModelType.Name + " (room object never spawned)");
					}
				}
				else if (!entry.IsBound)
				{
					list.Add($"{entry.Descriptor.ModelType.Name} (spawned={entry.Spawned != null}, " + $"spawning={entry.IsSpawning}, targetFound={FindTarget(networkRunner, entry, wantsOwned: true) != null})");
				}
			}
			return string.Format("[NetworkedModel] {0} scope '{1}' did not attach within ", isGlobal ? "global" : "local", scope) + string.Format("{0:0.#}s — still unfulfilled: {1}", 5f, string.Join(", ", list));
		}

		private void ReconcileIndividual(NetworkRunner runner, ShadowEntry entry)
		{
			if (!_localScopes.Contains(entry.Descriptor.Scope))
			{
				if (entry.IsBound || entry.Spawned != null || entry.IsSpawning)
				{
					Teardown(entry, runner);
				}
				return;
			}
			if (entry.IsBound)
			{
				if (!IsBindingValid(entry))
				{
					Unbind(entry);
				}
				return;
			}
			NetworkObject networkObject = FindTarget(runner, entry, wantsOwned: true);
			if (networkObject == null)
			{
				if (entry.Spawned == null && !entry.IsSpawning)
				{
					BeginSpawn(runner, entry);
				}
			}
			else
			{
				Bind(entry, networkObject);
			}
		}

		private void ReconcileShared(NetworkRunner runner, ShadowEntry entry)
		{
			if (runner.IsSharedModeMasterClient && entry.Spawned == null && !entry.IsSpawning && _globalScopes.Contains(entry.Descriptor.Scope) && FindTarget(runner, entry, wantsOwned: false) == null)
			{
				BeginSpawn(runner, entry);
			}
			bool flag = _localScopes.Contains(entry.Descriptor.Scope);
			if (entry.IsBound && (!flag || !IsBindingValid(entry)))
			{
				Unbind(entry);
				return;
			}
			if (!entry.IsBound && flag)
			{
				NetworkObject networkObject = FindTarget(runner, entry, wantsOwned: false);
				if (networkObject != null)
				{
					Bind(entry, networkObject);
				}
			}
			ReclaimSharedAuthorityIfMaster(runner, entry);
		}

		private void ReclaimSharedAuthorityIfMaster(NetworkRunner runner, ShadowEntry entry)
		{
			if (!runner.IsSharedModeMasterClient || !entry.IsBound || !IsBindingValid(entry) || entry.Bound.HasStateAuthority)
			{
				entry.NextAuthorityRequestTime = 0f;
			}
			else if (!(Time.realtimeSinceStartup < entry.NextAuthorityRequestTime))
			{
				entry.Bound.RequestStateAuthority();
				entry.NextAuthorityRequestTime = Time.realtimeSinceStartup + 0.5f;
			}
		}

		private void BeginSpawn(NetworkRunner runner, ShadowEntry entry)
		{
			entry.IsSpawning = true;
			SpawnShadowAsync(runner, entry, entry.SpawnGeneration);
		}

		private async void SpawnShadowAsync(NetworkRunner runner, ShadowEntry entry, int generation)
		{
			NetworkObject spawned = null;
			try
			{
				spawned = await runner.SpawnAsync(entry.Prefab);
				bool flag = entry.SpawnGeneration == generation;
				if (flag)
				{
					entry.IsSpawning = false;
				}
				if (spawned != null && spawned.IsValid && flag && entry.Spawned == null && IsSpawnStillWanted(runner, entry) && FindExistingRival(runner, entry, spawned) == null)
				{
					entry.Spawned = spawned;
				}
				else
				{
					DespawnOrphan(runner, entry, spawned);
				}
			}
			catch (Exception ex)
			{
				if (entry.SpawnGeneration == generation)
				{
					entry.IsSpawning = false;
				}
				if (runner != null && runner.IsRunning)
				{
					Debug.LogWarning("[NetworkedModel] Async spawn of '" + entry.Descriptor.PrefabResource + "' failed: " + ex.Message);
				}
				DespawnOrphan(runner, entry, spawned);
			}
		}

		private bool IsSpawnStillWanted(NetworkRunner runner, ShadowEntry entry)
		{
			if (runner != _multiplayerModel.NetworkRunner || !runner.IsRunning)
			{
				return false;
			}
			if ((entry.Descriptor.Ownership & ModelOwnership.Individual) != ModelOwnership.None)
			{
				return _localScopes.Contains(entry.Descriptor.Scope);
			}
			if (runner.IsSharedModeMasterClient)
			{
				return _globalScopes.Contains(entry.Descriptor.Scope);
			}
			return false;
		}

		private NetworkObject FindExistingRival(NetworkRunner runner, ShadowEntry entry, NetworkObject mine)
		{
			if ((entry.Descriptor.Ownership & ModelOwnership.Individual) != ModelOwnership.None)
			{
				return null;
			}
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && !(allNetworkObject == mine) && allNetworkObject.GetComponent(entry.Descriptor.TransportType) != null)
				{
					return allNetworkObject;
				}
			}
			return null;
		}

		private static void DespawnOrphan(NetworkRunner runner, ShadowEntry entry, NetworkObject spawned)
		{
			if (!(spawned == null) && spawned.IsValid && !(runner == null) && runner.IsRunning && spawned.HasStateAuthority)
			{
				Debug.LogWarning("[NetworkedModel] Despawning orphaned '" + entry.Descriptor.PrefabResource + "' shadow — spawn completed after teardown / scope close / migration, or lost a duplicate race.");
				runner.Despawn(spawned);
			}
		}

		private void InvalidateInFlightSpawn(ShadowEntry entry)
		{
			entry.IsSpawning = false;
			entry.SpawnGeneration++;
		}

		private void OnSessionReady(OnStartGamePhaseEvent _)
		{
			OpenSessionScopeAsync().Forget();
		}

		private async UniTaskVoid OpenSessionScopeAsync()
		{
			await OpenGlobalScopeAsync(ModelScope.Session);
			await OpenLocalScopeAsync(ModelScope.Session);
		}

		private void OnSessionEnded(OnShutdownEvent _)
		{
			TeardownAll();
		}

		private void BuildEntries()
		{
			_entries.Clear();
			Dictionary<Type, NetworkedModelShadowDescriptor> dictionary = _descriptors.ToDictionary((NetworkedModelShadowDescriptor descriptor) => descriptor.ModelType);
			foreach (NetworkedModelBase model in _networkedModelRegistry.Models)
			{
				if (dictionary.TryGetValue(model.GetType(), out var value))
				{
					_entries.Add(new ShadowEntry
					{
						Descriptor = value,
						Bridge = value.CreateBridge(model),
						Prefab = Resources.Load<NetworkObject>(value.PrefabResource)
					});
				}
			}
		}

		private NetworkObject FindTarget(NetworkRunner runner, ShadowEntry entry, bool wantsOwned)
		{
			foreach (NetworkObject allNetworkObject in runner.GetAllNetworkObjects())
			{
				if (!(allNetworkObject == null) && allNetworkObject.IsValid && !(allNetworkObject.GetComponent(entry.Descriptor.TransportType) == null) && (!wantsOwned || allNetworkObject.HasStateAuthority))
				{
					return allNetworkObject;
				}
			}
			return null;
		}

		private void TeardownAll()
		{
			_globalScopes.Clear();
			_localScopes.Clear();
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			foreach (ShadowEntry entry in _entries)
			{
				Teardown(entry, networkRunner);
			}
		}

		private void Teardown(ShadowEntry entry, NetworkRunner runner)
		{
			Unbind(entry);
			if (runner != null && runner.IsRunning && entry.Spawned != null && entry.Spawned.IsValid)
			{
				runner.Despawn(entry.Spawned);
			}
			InvalidateInFlightSpawn(entry);
			entry.Spawned = null;
		}

		private void Bind(ShadowEntry entry, NetworkObject target)
		{
			entry.Bridge.BindObject(target);
			entry.Bound = target;
			entry.IsBound = true;
			entry.NextAuthorityRequestTime = 0f;
		}

		private void Unbind(ShadowEntry entry)
		{
			entry.Bridge.Unbind();
			entry.Bound = null;
			entry.IsBound = false;
			entry.NextAuthorityRequestTime = 0f;
		}

		private bool IsBindingValid(ShadowEntry entry)
		{
			if (entry.Bound != null)
			{
				return entry.Bound.IsValid;
			}
			return false;
		}
	}
}
