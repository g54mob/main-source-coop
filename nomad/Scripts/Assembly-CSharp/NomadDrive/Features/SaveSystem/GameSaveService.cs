using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Enviro;
using EvilCore;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.EvilSave;
using EvilCore.Networking;
using Mirror;
using NomadDrive.Features.FloatingOrigin;
using NomadDrive.Features.Furnitures;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;
using NomadDrive.Features.Player.Downed;
using NomadDrive.Features.Vehicle.Networking;
using NomadDrive.Features.WorldGeneration;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NomadDrive.Features.SaveSystem
{
	public sealed class GameSaveService : IGameSaveService
	{
		private const string ThumbnailFileName = "thumbnail.jpg";

		private const int ThumbnailWidth = 384;

		private const int ThumbnailHeight = 216;

		private const int ThumbnailJpgQuality = 70;

		private readonly IEvilSaveManager _evilSave;

		private readonly IPlayerService _playerService;

		private readonly IWorldSeedProvider _worldSeedProvider;

		private IWorldGenerator _worldGenerator;

		private bool _restoreArmed;

		private PlayerRecord _pendingHostPlayer;

		private readonly Dictionary<string, PlayerRecord> _remotePlayerRecords = new Dictionary<string, PlayerRecord>();

		private const float VehicleSettleProbeHeight = 50f;

		private const float VehicleSettleClearance = 0.1f;

		private const float VehicleSettleMaxWaitSeconds = 30f;

		private const float VehicleSettlePollSeconds = 0.25f;

		private const bool LogRestoreOverlaps = true;

		public bool IsRestoring { get; private set; }

		public static bool IsRestoringWorld { get; private set; }

		public bool IsLoadedWorld { get; private set; }

		public int LoadedSeed { get; private set; }

		public bool HasLoadedLootLedger { get; private set; }

		public IReadOnlyList<int> LoadedLootSeeds { get; private set; }

		public bool CanSaveNow => NetworkServer.active;

		public bool HasSaveForActiveSlot
		{
			get
			{
				if (!EvilSave.SlotExists(EvilSave.ActiveSlot))
				{
					return false;
				}
				EvilSave.LoadFromDisk(EvilSave.ActiveSlot);
				return EvilSave.LoadRaw("game.manifest") != null;
			}
		}

		public string ActiveSlotId => EvilSave.ActiveSlot;

		public event Action OnRestoreComplete;

		public event Action OnSaveStarted;

		public event Action OnSaveCompleted;

		public GameSaveService(IEvilSaveManager evilSave, IPlayerService playerService, IWorldSeedProvider worldSeedProvider)
		{
			_evilSave = evilSave;
			_playerService = playerService;
			_worldSeedProvider = worldSeedProvider;
			if (_evilSave == null)
			{
				return;
			}
			_evilSave.OnBeforeSave += HandleBeforeSave;
			if (_evilSave is EvilSaveManager evilSaveManager)
			{
				evilSaveManager.AutoSaveGate = () => NetworkServer.active;
			}
		}

		public SaveSlotInfo[] GetSaveSlots()
		{
			string[] slotIds = EvilSave.GetSlotIds();
			List<SaveSlotInfo> list = new List<SaveSlotInfo>(slotIds.Length);
			string[] array = slotIds;
			foreach (string slotId in array)
			{
				SaveSlotMetadata slotMetadata = EvilSave.GetSlotMetadata(slotId);
				if (slotMetadata != null)
				{
					string text = Path.Combine(EvilSave.GetSlotDirectory(slotId), "thumbnail.jpg");
					if (!File.Exists(text))
					{
						text = string.Empty;
					}
					list.Add(new SaveSlotInfo(slotId, slotMetadata.displayName, slotMetadata.lastSavedAt, slotMetadata.playtimeSeconds, text));
				}
			}
			list.Sort((SaveSlotInfo a, SaveSlotInfo b) => string.CompareOrdinal(b.LastSavedAtIso, a.LastSavedAtIso));
			return list.ToArray();
		}

		public void DeleteSaveSlot(string slotId)
		{
			if (!string.IsNullOrEmpty(slotId))
			{
				EvilSave.DeleteSlot(slotId);
			}
		}

		public void MarkNewGame(string worldName)
		{
			IsLoadedWorld = false;
			LoadedSeed = 0;
			HasLoadedLootLedger = false;
			LoadedLootSeeds = null;
			_restoreArmed = false;
			_remotePlayerRecords.Clear();
			string text = (EvilSave.ActiveSlot = $"slot_{Guid.NewGuid():N}");
			EvilSave.CreateSlot(text, string.IsNullOrEmpty(worldName) ? text : worldName);
		}

		public void SelectSlotForContinue(string slotId)
		{
			_remotePlayerRecords.Clear();
			EvilSave.ActiveSlot = slotId;
			EvilSave.LoadFromDisk(slotId);
			GameSaveManifest gameSaveManifest = GameSaveManifest.Deserialize(EvilSave.LoadRaw("game.manifest"));
			if (gameSaveManifest == null)
			{
				IsLoadedWorld = false;
				LoadedSeed = 0;
				HasLoadedLootLedger = false;
				LoadedLootSeeds = null;
			}
			else
			{
				LoadedSeed = gameSaveManifest.Seed;
				IsLoadedWorld = true;
				HasLoadedLootLedger = gameSaveManifest.FormatVersion >= 5;
				LoadedLootSeeds = (HasLoadedLootLedger ? gameSaveManifest.SpawnedLootSeeds : null);
				_restoreArmed = false;
			}
		}

		public void NotifyHostStarted()
		{
			if (NetworkServer.active && IsLoadedWorld && !_restoreArmed)
			{
				_restoreArmed = true;
				ArmRestoreAsync().Forget();
			}
		}

		public void RequestSave()
		{
			if (NetworkServer.active)
			{
				RequestSaveAsync().Forget();
			}
		}

		private async UniTaskVoid RequestSaveAsync()
		{
			this.OnSaveStarted?.Invoke();
			await UniTask.NextFrame();
			await UniTask.NextFrame();
			try
			{
				if (_evilSave != null)
				{
					await _evilSave.SaveGameAsync();
				}
			}
			finally
			{
				this.OnSaveCompleted?.Invoke();
			}
		}

		public void SaveNow()
		{
			if (NetworkServer.active)
			{
				_evilSave?.SaveGame();
			}
		}

		private async UniTaskVoid ArmRestoreAsync()
		{
			_worldGenerator = await ResolveWorldGeneratorAsync();
			if (_worldGenerator == null)
			{
				EvilLogger.LogError("[GameSaveService] Could not resolve WorldGenerator; restore aborted.", "ArmRestoreAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\SaveSystem\\GameSaveService.cs", 222);
			}
			else if (_worldGenerator.IsWorldFullyReady)
			{
				await RunRestoreAsync();
			}
			else
			{
				_worldGenerator.OnWorldFullyReady += HandleWorldFullyReady;
			}
		}

		private void HandleWorldFullyReady()
		{
			if (_worldGenerator != null)
			{
				_worldGenerator.OnWorldFullyReady -= HandleWorldFullyReady;
			}
			RunRestoreAsync().Forget();
		}

		private async UniTask RunRestoreAsync()
		{
			IsRestoring = true;
			IsRestoringWorld = true;
			try
			{
				_evilSave?.ApplyLoadedSaveables();
				GameSaveManifest manifest = GameSaveManifest.Deserialize(EvilSave.LoadRaw("game.manifest"));
				if (manifest != null)
				{
					LoadRemotePlayerRecords(manifest.RemotePlayers);
					ApplyTimeOfDay(manifest.TimeOfDay);
					if (manifest.HostPlayer != null)
					{
						FloatingOriginManager.Instance?.ApplyRestoreShift(manifest.HostPlayer.Position);
					}
					ApplyHostPlayerWhenReady(manifest.HostPlayer);
					await RestoreDynamicObjectsAsync(manifest.DynamicObjects);
					await ApplyHostPlayerLinksAsync(manifest.HostPlayer);
				}
			}
			finally
			{
				IsRestoring = false;
				IsRestoringWorld = false;
				this.OnRestoreComplete?.Invoke();
			}
		}

		private async UniTask RestoreDynamicObjectsAsync(List<ObjectRecord> records)
		{
			if (records == null || records.Count == 0)
			{
				return;
			}
			PersistentIdRegistry.Clear();
			SaveContext ctx = new SaveContext(DateTime.UtcNow);
			List<(PersistentObject obj, ObjectRecord record)> spawned = new List<(PersistentObject, ObjectRecord)>(records.Count);
			foreach (ObjectRecord record in records)
			{
				PersistentObject persistentObject = await SpawnFromRecordAsync(record);
				if (persistentObject != null)
				{
					spawned.Add((persistentObject, record));
				}
				await UniTask.Yield();
			}
			foreach (var (persistentObject2, objectRecord) in spawned)
			{
				if (persistentObject2 != null)
				{
					persistentObject2.RestoreSelf(objectRecord.Contributors, ctx);
				}
			}
			foreach (var (persistentObject3, objectRecord2) in spawned)
			{
				if (persistentObject3 != null)
				{
					persistentObject3.RestoreLinks(objectRecord2.Contributors, ctx);
				}
			}
			SettleRestoredVehiclesAsync(spawned).Forget();
		}

		private static async UniTask SettleRestoredVehiclesAsync(List<(PersistentObject obj, ObjectRecord record)> spawned)
		{
			int mask = LayerMask.GetMask("Terrain");
			List<UniTask> list = new List<UniTask>();
			foreach (var item2 in spawned)
			{
				PersistentObject item = item2.obj;
				if (!(item == null) && item.TryGetComponent<NetworkedNWHVehicle>(out var component))
				{
					list.Add(SettleVehicleAsync(component, mask));
				}
			}
			if (list.Count > 0)
			{
				await UniTask.WhenAll(list);
			}
		}

		private static async UniTask SettleVehicleAsync(NetworkedNWHVehicle vehicle, int terrainMask)
		{
			if (vehicle == null)
			{
				return;
			}
			Transform vehicleTransform = vehicle.transform;
			bool grounded = false;
			for (float waited = 0f; waited < 30f; waited += 0.25f)
			{
				if (vehicle == null)
				{
					return;
				}
				Vector3 position = vehicleTransform.position;
				if (Physics.Raycast(new Vector3(position.x, position.y + 50f, position.z), Vector3.down, out var _, 100f, terrainMask, QueryTriggerInteraction.Ignore))
				{
					grounded = true;
					break;
				}
				await UniTask.Delay(TimeSpan.FromSeconds(0.25));
			}
			if (vehicle == null)
			{
				return;
			}
			if (grounded)
			{
				Vector3 position2 = (vehicleTransform.position += Vector3.up * 0.1f);
				if (vehicle.TryGetComponent<Rigidbody>(out var component))
				{
					component.position = position2;
				}
				vehicle.ServerSetParkedFrozen(frozen: false);
				vehicle.ServerReassertPhysicsStateAfterRestore();
				DumpVehicleOverlaps(vehicle);
			}
			VehicleFreezeManager.Instance?.ServerRegister(vehicle);
		}

		private static void DumpVehicleOverlaps(NetworkedNWHVehicle vehicle)
		{
			if (vehicle == null || !vehicle.TryGetComponent<Rigidbody>(out var component))
			{
				return;
			}
			Collider[] componentsInChildren = vehicle.GetComponentsInChildren<Collider>();
			if (componentsInChildren.Length == 0)
			{
				return;
			}
			Bounds bounds = componentsInChildren[0].bounds;
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				bounds.Encapsulate(collider.bounds);
			}
			int layer = vehicle.gameObject.layer;
			Collider[] array2 = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity, -1, QueryTriggerInteraction.Ignore);
			int num = 0;
			array = array2;
			foreach (Collider collider2 in array)
			{
				if (!(collider2 == null) && !(collider2.attachedRigidbody == component) && !Physics.GetIgnoreLayerCollision(layer, collider2.gameObject.layer))
				{
					num++;
				}
			}
		}

		private async UniTask<PersistentObject> SpawnFromRecordAsync(ObjectRecord record)
		{
			if (record == null || string.IsNullOrEmpty(record.AddressableGuid))
			{
				return null;
			}
			if (!NetworkServer.active)
			{
				return null;
			}
			GameObject gameObject;
			try
			{
				gameObject = await Addressables.LoadAssetAsync<GameObject>(record.AddressableGuid).Task;
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[GameSaveService] Failed to load prefab " + record.AddressableGuid + ": " + ex.Message, "SpawnFromRecordAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\SaveSystem\\GameSaveService.cs", 446);
				return null;
			}
			if (gameObject == null)
			{
				return null;
			}
			FloatingOriginManager instance = FloatingOriginManager.Instance;
			Vector3 position = ((instance != null) ? instance.WorldToRender(record.Position) : record.Position);
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, position, record.Rotation);
			PersistentObject result = PersistentObject.ServerEnsure(gameObject2, record.AddressableGuid, record.Guid);
			if (!NetworkServer.active)
			{
				UnityEngine.Object.Destroy(gameObject2);
				return null;
			}
			NetworkServer.Spawn(gameObject2, NetworkServer.localConnection);
			if (gameObject2.TryGetComponent<NetworkedNWHVehicle>(out var component))
			{
				component.MarkRestoredForDiag();
				VehicleFreezeManager.Instance?.ServerUnregister(component.netId);
				component.ServerSetParkedFrozen(frozen: true);
				if (gameObject2.TryGetComponent<Rigidbody>(out var component2))
				{
					component2.isKinematic = true;
				}
			}
			if (instance != null && instance.WorldContentRoot != null && gameObject2.GetComponentInChildren<IFloatingOriginShiftable>(includeInactive: true) == null)
			{
				gameObject2.transform.SetParent(instance.WorldContentRoot, worldPositionStays: true);
			}
			return result;
		}

		private void ApplyTimeOfDay(float timeOfDay)
		{
			if (!(timeOfDay <= 0f))
			{
				EnviroManager instance = EnviroManager.instance;
				if (instance != null && instance.Time != null)
				{
					instance.Time.SetTimeOfDay(timeOfDay);
				}
			}
		}

		private void ApplyHostPlayerWhenReady(PlayerRecord record)
		{
			if (record == null)
			{
				return;
			}
			if (_playerService != null && _playerService.IsPlayerSpawned)
			{
				ApplyHostPlayer(record);
				return;
			}
			_pendingHostPlayer = record;
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered += HandlePlayerRegisteredForRestore;
			}
		}

		private void HandlePlayerRegisteredForRestore()
		{
			if (_playerService != null)
			{
				_playerService.OnPlayerRegistered -= HandlePlayerRegisteredForRestore;
			}
			if (_pendingHostPlayer != null)
			{
				ApplyHostPlayer(_pendingHostPlayer);
				_pendingHostPlayer = null;
			}
		}

		private void ApplyHostPlayer(PlayerRecord record)
		{
			NomadDrive.Features.Player.Player player = _playerService?.LocalPlayer;
			if (!(player == null))
			{
				FloatingOriginManager instance = FloatingOriginManager.Instance;
				Vector3 vector = ((instance != null) ? instance.WorldToRender(record.Position) : record.Position);
				player.SetPositionAndRotation(vector, record.EulerAngles);
				if (record.IsDowned && DownedPlayerRegistry.ServerHasOtherAlivePlayer(player.netId))
				{
					player.GetComponent<PlayerDeathController>()?.ServerForceDowned(vector, Quaternion.Euler(record.EulerAngles));
				}
				if (record.SurvivalBlob != null && record.SurvivalBlob.Length != 0 && _playerService.TryGetStatsManager(out var manager) && manager != null && manager.IsInitialized)
				{
					ApplySurvivalBlob(manager, record.SurvivalBlob);
				}
			}
		}

		private static void ApplySurvivalBlob(PlayerStatsManager stats, byte[] blob)
		{
			try
			{
				using MemoryStream stream = new MemoryStream(blob);
				using EvilReader evilReader = new EvilReader(stream);
				evilReader.ReadByte();
				float nutrition = evilReader.ReadFloat();
				float hydration = evilReader.ReadFloat();
				float energy = evilReader.ReadFloat();
				float health = evilReader.ReadFloat();
				float poison = evilReader.ReadFloat();
				stats.RestoreFromSave(nutrition, hydration, energy, health, poison);
			}
			catch (Exception)
			{
			}
		}

		private async UniTask ApplyHostPlayerLinksAsync(PlayerRecord record)
		{
			if (record == null)
			{
				return;
			}
			bool hasSeat = !string.IsNullOrEmpty(record.SeatGuid);
			bool hasEquip = !string.IsNullOrEmpty(record.EquippedItemGuid);
			if (!hasSeat && !hasEquip)
			{
				return;
			}
			for (int i = 0; i < 100; i++)
			{
				if (_playerService != null && _playerService.IsPlayerSpawned)
				{
					break;
				}
				await UniTask.Delay(100);
			}
			if (_playerService == null || !_playerService.IsPlayerSpawned)
			{
				return;
			}
			NomadDrive.Features.Player.Player player = _playerService.LocalPlayer;
			if (player == null)
			{
				return;
			}
			bool flag = hasSeat;
			if (flag)
			{
				flag = await TryReseatHostAsync(record.SeatGuid);
			}
			if (!flag && hasEquip && !player.IsPlayerSitting() && PersistentIdRegistry.TryResolve(record.EquippedItemGuid, out var persistentId) && !(persistentId == null))
			{
				HeldItem component = persistentId.GetComponent<HeldItem>();
				if (!(component == null) && _playerService.TryGetEquipmentManager(out var manager))
				{
					manager?.Equip(component);
				}
			}
		}

		private async UniTask<bool> TryReseatHostAsync(string seatGuid)
		{
			SittableSurface seat = null;
			for (int i = 0; i < 50; i++)
			{
				if (PersistentIdRegistry.TryResolve(seatGuid, out var persistentId) && persistentId != null)
				{
					SittableSurface componentInChildren = persistentId.GetComponentInChildren<SittableSurface>(includeInactive: true);
					if (componentInChildren != null && IsSeatReady(componentInChildren))
					{
						seat = componentInChildren;
						break;
					}
				}
				await UniTask.Delay(100);
			}
			if (seat == null)
			{
				return false;
			}
			NomadDrive.Features.Player.Player player = _playerService?.LocalPlayer;
			if (player == null)
			{
				return false;
			}
			if (player.IsPlayerSitting())
			{
				return true;
			}
			seat.SetOccupied(value: true);
			player.OnPlayerSit.Invoke(seat);
			return true;
		}

		private static bool IsSeatReady(SittableSurface seat)
		{
			Seat componentInParent = seat.GetComponentInParent<Seat>();
			if (componentInParent != null && componentInParent.IsAttached)
			{
				return seat.VehicleSeatSlot != null;
			}
			return true;
		}

		private async UniTask<IWorldGenerator> ResolveWorldGeneratorAsync()
		{
			for (int attempt = 0; attempt < 100; attempt++)
			{
				WorldGenerator worldGenerator = UnityEngine.Object.FindAnyObjectByType<WorldGenerator>();
				if (worldGenerator != null)
				{
					return worldGenerator;
				}
				await UniTask.Delay(100);
			}
			return null;
		}

		private void HandleBeforeSave()
		{
			if (NetworkServer.active)
			{
				CaptureManifest();
				CaptureThumbnail();
			}
		}

		private void CaptureManifest()
		{
			EnsureWorldGenerator();
			GameSaveManifest gameSaveManifest = new GameSaveManifest
			{
				Seed = ResolveCurrentSeed(),
				PlayerSpawnPoint = (_worldGenerator?.PlayerSpawnPoint ?? Vector3.zero),
				TimeOfDay = CaptureTimeOfDay(),
				HostPlayer = CaptureHostPlayer()
			};
			SaveContext ctx = new SaveContext(gameSaveManifest.SavedAtUtc);
			gameSaveManifest.DynamicObjects.AddRange(CaptureDynamicObjects(ctx));
			IReadOnlyCollection<int> readOnlyCollection = ((NetworkSingleton<WorldGenerator>.Instance != null) ? NetworkSingleton<WorldGenerator>.Instance.GetSpawnedLootSeeds() : null);
			if (readOnlyCollection != null)
			{
				gameSaveManifest.SpawnedLootSeeds.AddRange(readOnlyCollection);
			}
			RefreshRemoteDownedStates();
			gameSaveManifest.RemotePlayers.AddRange(_remotePlayerRecords.Values);
			EvilSave.SaveRaw("game.manifest", gameSaveManifest.Serialize());
		}

		private void EnsureWorldGenerator()
		{
			if (_worldGenerator == null)
			{
				_worldGenerator = UnityEngine.Object.FindAnyObjectByType<WorldGenerator>();
			}
		}

		private static float CaptureTimeOfDay()
		{
			EnviroManager instance = EnviroManager.instance;
			if (!(instance != null) || !(instance.Time != null))
			{
				return 0f;
			}
			return instance.Time.GetTimeOfDay();
		}

		private PlayerRecord CaptureHostPlayer()
		{
			if (_playerService == null || !_playerService.IsPlayerSpawned)
			{
				return null;
			}
			NomadDrive.Features.Player.Player localPlayer = _playerService.LocalPlayer;
			if (localPlayer == null)
			{
				return null;
			}
			PlayerRecord playerRecord = new PlayerRecord
			{
				Puid = (localPlayer.EosProductUserId ?? string.Empty),
				DisplayName = (localPlayer.DisplayName ?? string.Empty),
				Position = FloatingOriginManager.ToTrueWorld(localPlayer.transform.position),
				EulerAngles = localPlayer.transform.eulerAngles
			};
			if (_playerService.TryGetStatsManager(out var manager) && manager != null && manager.IsInitialized)
			{
				playerRecord.SurvivalBlob = CaptureSurvivalBlob(manager);
			}
			playerRecord.EquippedItemGuid = CaptureEquippedItemGuid();
			playerRecord.SeatGuid = CaptureSeatGuid(localPlayer);
			PlayerDeathController component = localPlayer.GetComponent<PlayerDeathController>();
			playerRecord.IsDowned = component != null && component.IsDowned;
			if (playerRecord.IsDowned && component.ChickenNetId != 0 && NetworkServer.spawned.TryGetValue(component.ChickenNetId, out var value) && value != null)
			{
				playerRecord.Position = FloatingOriginManager.ToTrueWorld(value.transform.position);
				playerRecord.EulerAngles = value.transform.eulerAngles;
			}
			return playerRecord;
		}

		private static string CaptureSeatGuid(NomadDrive.Features.Player.Player player)
		{
			if (player == null || !player.IsPlayerSitting())
			{
				return string.Empty;
			}
			if (!(player.GetPlayerSeat() is Component component))
			{
				return string.Empty;
			}
			PersistentId componentInParent = component.GetComponentInParent<PersistentId>();
			if (!(componentInParent != null) || !componentInParent.HasGuid)
			{
				return string.Empty;
			}
			return componentInParent.Guid;
		}

		private string CaptureEquippedItemGuid()
		{
			if (_playerService == null || !_playerService.TryGetEquipmentManager(out var manager) || manager == null || !manager.IsItemEquipped)
			{
				return string.Empty;
			}
			HeldItem equippedEntity = manager.EquippedEntity;
			PersistentId persistentId = ((equippedEntity != null) ? equippedEntity.GetComponent<PersistentId>() : null);
			if (!(persistentId != null) || !persistentId.HasGuid)
			{
				return string.Empty;
			}
			return persistentId.Guid;
		}

		public void UpdateRemotePlayerRecord(string puid, string displayName, byte[] survivalBlob, string equippedItemGuid)
		{
			if (!string.IsNullOrEmpty(puid))
			{
				PlayerRecord value;
				bool isDowned = _remotePlayerRecords.TryGetValue(puid, out value) && value != null && value.IsDowned;
				_remotePlayerRecords[puid] = new PlayerRecord
				{
					Puid = puid,
					DisplayName = (displayName ?? string.Empty),
					SurvivalBlob = (survivalBlob ?? Array.Empty<byte>()),
					EquippedItemGuid = (equippedItemGuid ?? string.Empty),
					IsDowned = isDowned,
					Position = (value?.Position ?? Vector3.zero),
					EulerAngles = (value?.EulerAngles ?? Vector3.zero)
				};
			}
		}

		public bool TryGetRemotePlayerRecord(string puid, out byte[] survivalBlob, out string equippedItemGuid, out bool isDowned, out Vector3 downedPosition, out Vector3 downedEulerAngles)
		{
			survivalBlob = null;
			equippedItemGuid = null;
			isDowned = false;
			downedPosition = Vector3.zero;
			downedEulerAngles = Vector3.zero;
			if (string.IsNullOrEmpty(puid) || !_remotePlayerRecords.TryGetValue(puid, out var value) || value == null)
			{
				return false;
			}
			survivalBlob = value.SurvivalBlob;
			equippedItemGuid = value.EquippedItemGuid;
			isDowned = value.IsDowned;
			downedPosition = value.Position;
			downedEulerAngles = value.EulerAngles;
			return true;
		}

		private void RefreshRemoteDownedStates()
		{
			if (!NetworkServer.active)
			{
				return;
			}
			string text = ((_playerService?.LocalPlayer != null) ? _playerService.LocalPlayer.EosProductUserId : null);
			PlayerDeathController[] array = UnityEngine.Object.FindObjectsByType<PlayerDeathController>(FindObjectsSortMode.None);
			foreach (PlayerDeathController playerDeathController in array)
			{
				if (playerDeathController == null)
				{
					continue;
				}
				NomadDrive.Features.Player.Player component = playerDeathController.GetComponent<NomadDrive.Features.Player.Player>();
				if (component == null)
				{
					continue;
				}
				string eosProductUserId = component.EosProductUserId;
				if (!string.IsNullOrEmpty(eosProductUserId) && (string.IsNullOrEmpty(text) || !(eosProductUserId == text)))
				{
					if (!_remotePlayerRecords.TryGetValue(eosProductUserId, out var value) || value == null)
					{
						value = new PlayerRecord
						{
							Puid = eosProductUserId,
							DisplayName = (component.DisplayName ?? string.Empty)
						};
						_remotePlayerRecords[eosProductUserId] = value;
					}
					value.IsDowned = playerDeathController.IsDowned;
					if (value.IsDowned && playerDeathController.ChickenNetId != 0 && NetworkServer.spawned.TryGetValue(playerDeathController.ChickenNetId, out var value2) && value2 != null)
					{
						value.Position = FloatingOriginManager.ToTrueWorld(value2.transform.position);
						value.EulerAngles = value2.transform.eulerAngles;
					}
				}
			}
		}

		private void LoadRemotePlayerRecords(List<PlayerRecord> records)
		{
			if (records == null)
			{
				return;
			}
			foreach (PlayerRecord record in records)
			{
				if (record != null && !string.IsNullOrEmpty(record.Puid))
				{
					_remotePlayerRecords[record.Puid] = record;
				}
			}
		}

		private static byte[] CaptureSurvivalBlob(PlayerStatsManager stats)
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (EvilWriter evilWriter = new EvilWriter(memoryStream))
			{
				evilWriter.Write((byte)1);
				evilWriter.Write(stats.Nutrition.CurrentValue);
				evilWriter.Write(stats.Hydration.CurrentValue);
				evilWriter.Write(stats.Energy.CurrentValue);
				evilWriter.Write(stats.Health.CurrentValue);
				evilWriter.Write(stats.Poison.CurrentValue);
			}
			return memoryStream.ToArray();
		}

		private List<ObjectRecord> CaptureDynamicObjects(ISaveContext ctx)
		{
			List<ObjectRecord> list = new List<ObjectRecord>();
			PersistentObject[] array = UnityEngine.Object.FindObjectsByType<PersistentObject>(FindObjectsSortMode.None);
			int num = 0;
			int num2 = 0;
			PersistentObject[] array2 = array;
			foreach (PersistentObject persistentObject in array2)
			{
				if (!(persistentObject == null))
				{
					PersistentId persistentId = persistentObject.PersistentId;
					if (persistentId == null || !persistentId.HasGuid)
					{
						num++;
						continue;
					}
					if (string.IsNullOrEmpty(persistentObject.AddressableGuid))
					{
						num2++;
						continue;
					}
					Transform transform = persistentObject.transform;
					list.Add(new ObjectRecord
					{
						Guid = persistentId.Guid,
						AddressableGuid = persistentObject.AddressableGuid,
						Position = FloatingOriginManager.ToTrueWorld(transform.position),
						Rotation = transform.rotation,
						ParentLink = SaveParentLink.None,
						Contributors = persistentObject.CaptureContributors(ctx)
					});
				}
			}
			return list;
		}

		private int ResolveCurrentSeed()
		{
			if (_worldSeedProvider != null && _worldSeedProvider.HasSeed)
			{
				return _worldSeedProvider.Seed;
			}
			if (_worldGenerator != null)
			{
				return _worldGenerator.Seed;
			}
			return 0;
		}

		private void CaptureThumbnail()
		{
			Camera camera = ResolveCamera();
			if (!(camera == null))
			{
				string filePath = Path.Combine(EvilSave.GetSlotDirectory(EvilSave.ActiveSlot), "thumbnail.jpg");
				SaveThumbnailCapture.TryCaptureToFile(camera, filePath, 384, 216, 70);
			}
		}

		private Camera ResolveCamera()
		{
			if (_playerService != null && _playerService.IsPlayerSpawned)
			{
				Transform cameraTransform = _playerService.CameraTransform;
				if (cameraTransform != null && cameraTransform.TryGetComponent<Camera>(out var component))
				{
					return component;
				}
			}
			return Camera.main;
		}
	}
}
