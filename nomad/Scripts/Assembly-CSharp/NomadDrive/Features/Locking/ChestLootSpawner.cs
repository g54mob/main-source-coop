using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.EvilSave;
using Mirror;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.ObjectPlacement;
using NomadDrive.Features.SaveSystem;
using NomadDrive.Features.WorldGeneration.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NomadDrive.Features.Locking
{
	public class ChestLootSpawner : NetworkBehaviour, INetworkSaveable
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CSpawnLootAsync_003Ed__10 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskVoidMethodBuilder _003C_003Et__builder;

			public ChestLootSpawner _003C_003E4__this;

			private WeightedRandomSelector<ChestLootEntry> _003Cselector_003E5__2;

			private System.Random _003Crandom_003E5__3;

			private int _003Ci_003E5__4;

			private ChestLootEntry _003Centry_003E5__5;

			private GameObject _003Cinstance_003E5__6;

			private UniTask<GameObject>.Awaiter _003C_003Eu__1;

			private UniTask.Awaiter _003C_003Eu__2;

			private void MoveNext()
			{
				int num = _003C_003E1__state;
				ChestLootSpawner chestLootSpawner = _003C_003E4__this;
				try
				{
					UniTask<GameObject>.Awaiter awaiter;
					if (num == 0)
					{
						awaiter = _003C_003Eu__1;
						_003C_003Eu__1 = default(UniTask<GameObject>.Awaiter);
						num = (_003C_003E1__state = -1);
						goto IL_019a;
					}
					UniTask.Awaiter awaiter2;
					if (num == 1)
					{
						awaiter2 = _003C_003Eu__2;
						_003C_003Eu__2 = default(UniTask.Awaiter);
						num = (_003C_003E1__state = -1);
						goto IL_02a4;
					}
					if (!chestLootSpawner._hasSpawned)
					{
						chestLootSpawner.Network_hasSpawned = true;
						if (!(chestLootSpawner.registry == null) && chestLootSpawner.registry.HasEntries)
						{
							if (chestLootSpawner.snappingPlane == null)
							{
								EvilLogger.LogError("[ChestLootSpawner] " + chestLootSpawner.name + ": snappingPlane missing, cannot spawn", "SpawnLootAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Locking\\Scripts\\ChestLootSpawner.cs", 84);
							}
							else
							{
								chestLootSpawner.snappingPlane.Enable();
								ChestLootEntry[] array = chestLootSpawner.registry.Entries.Where((ChestLootEntry e) => e != null && e.prefab != null && e.weight > 0f).ToArray();
								if (array.Length != 0)
								{
									_003Cselector_003E5__2 = new WeightedRandomSelector<ChestLootEntry>(array, (ChestLootEntry e) => e.weight);
									_003Crandom_003E5__3 = new System.Random();
									_003Ci_003E5__4 = 0;
									goto IL_02f5;
								}
							}
						}
					}
					goto end_IL_000e;
					IL_02f5:
					if (_003Ci_003E5__4 < chestLootSpawner.spawnCount)
					{
						_003Centry_003E5__5 = _003Cselector_003E5__2.Select(_003Crandom_003E5__3);
						if (_003Centry_003E5__5 != null)
						{
							awaiter = LoadPrefabAsync(_003Centry_003E5__5.prefab).GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								num = (_003C_003E1__state = 0);
								_003C_003Eu__1 = awaiter;
								_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter, ref this);
								return;
							}
							goto IL_019a;
						}
						goto IL_02e3;
					}
					goto end_IL_000e;
					IL_02e3:
					_003Ci_003E5__4++;
					goto IL_02f5;
					IL_019a:
					GameObject result = awaiter.GetResult();
					if (!(result == null))
					{
						Vector3 position = chestLootSpawner.snappingPlane.transform.position + chestLootSpawner.snappingPlane.transform.up * 0.5f;
						Quaternion rotation = Quaternion.LookRotation(chestLootSpawner.snappingPlane.transform.forward, chestLootSpawner.snappingPlane.transform.up);
						_003Cinstance_003E5__6 = UnityEngine.Object.Instantiate(result, position, rotation);
						NetworkServer.Spawn(_003Cinstance_003E5__6);
						PersistentObject.ServerEnsure(_003Cinstance_003E5__6, _003Centry_003E5__5.prefab.AssetGUID);
						awaiter2 = UniTask.Delay(50, ignoreTimeScale: false, PlayerLoopTiming.Update, chestLootSpawner.GetCancellationTokenOnDestroy()).GetAwaiter();
						if (!awaiter2.IsCompleted)
						{
							num = (_003C_003E1__state = 1);
							_003C_003Eu__2 = awaiter2;
							_003C_003Et__builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);
							return;
						}
						goto IL_02a4;
					}
					goto IL_02e3;
					IL_02a4:
					awaiter2.GetResult();
					if (!(_003Cinstance_003E5__6 == null) && _003Cinstance_003E5__6.TryGetComponent<HeldItem>(out var component))
					{
						chestLootSpawner.snappingPlane.ServerSnap(component);
						_003Centry_003E5__5 = null;
						_003Cinstance_003E5__6 = null;
					}
					goto IL_02e3;
					end_IL_000e:;
				}
				catch (Exception exception)
				{
					_003C_003E1__state = -2;
					_003Cselector_003E5__2 = null;
					_003Crandom_003E5__3 = null;
					_003C_003Et__builder.SetException(exception);
					return;
				}
				_003C_003E1__state = -2;
				_003Cselector_003E5__2 = null;
				_003Crandom_003E5__3 = null;
				_003C_003Et__builder.SetResult();
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_003C_003Et__builder.SetStateMachine(stateMachine);
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[Header("References")]
		[SerializeField]
		private ChestLootRegistry registry;

		[SerializeField]
		private SnappingPlane snappingPlane;

		[SerializeField]
		private InteractableHinge lidHinge;

		[Header("Spawn Settings")]
		[Min(0f)]
		[SerializeField]
		private int spawnCount = 1;

		[SyncVar]
		private bool _hasSpawned;

		public bool HasSpawned => _hasSpawned;

		public string ContributorKey => "chestloot";

		public bool Network_hasSpawned
		{
			get
			{
				return _hasSpawned;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _hasSpawned, 1uL, null);
			}
		}

		protected void Start()
		{
			if (!(lidHinge == null))
			{
				lidHinge.OnOpened.AddListener(HandleLidOpened);
			}
		}

		private void OnDestroy()
		{
			if (lidHinge != null)
			{
				lidHinge.OnOpened.RemoveListener(HandleLidOpened);
			}
		}

		private void HandleLidOpened()
		{
			if (base.isServer && !_hasSpawned)
			{
				SpawnLootAsync().Forget();
			}
		}

		[AsyncStateMachine(typeof(_003CSpawnLootAsync_003Ed__10))]
		[Server]
		private UniTaskVoid SpawnLootAsync()
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'Cysharp.Threading.Tasks.UniTaskVoid NomadDrive.Features.Locking.ChestLootSpawner::SpawnLootAsync()' called when server was not active");
				return default(UniTaskVoid);
			}
			_003CSpawnLootAsync_003Ed__10 stateMachine = default(_003CSpawnLootAsync_003Ed__10);
			stateMachine._003C_003Et__builder = AsyncUniTaskVoidMethodBuilder.Create();
			stateMachine._003C_003E4__this = this;
			stateMachine._003C_003E1__state = -1;
			stateMachine._003C_003Et__builder.Start(ref stateMachine);
			return stateMachine._003C_003Et__builder.Task;
		}

		private static async UniTask<GameObject> LoadPrefabAsync(AssetReferenceGameObject reference)
		{
			try
			{
				GameObject obj = await Addressables.LoadAssetAsync<GameObject>(reference).Task;
				if (obj == null)
				{
					EvilLogger.LogError("[ChestLootSpawner] Failed to load prefab from AssetReference " + reference.AssetGUID, "LoadPrefabAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Locking\\Scripts\\ChestLootSpawner.cs", 144);
				}
				return obj;
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[ChestLootSpawner] Exception loading prefab " + reference.AssetGUID + ": " + ex.Message, "LoadPrefabAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Locking\\Scripts\\ChestLootSpawner.cs", 150);
				return null;
			}
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(_hasSpawned);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			bool network_hasSpawned = reader.ReadBool();
			if (NetworkServer.active)
			{
				Network_hasSpawned = network_hasSpawned;
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		public override bool Weaved()
		{
			return true;
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(_hasSpawned);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteBool(_hasSpawned);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _hasSpawned, null, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _hasSpawned, null, reader.ReadBool());
			}
		}
	}
}
