using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

public class LobbyGameSettings : NetworkBehaviour
{
	[Header("Sınırlar")]
	public int minDurationMinutes = 3;

	public int maxDurationMinutes = 30;

	public int minHunterCount = 1;

	public int maxHunterCount = 7;

	public int minAmmo = 1;

	public int maxAmmoLimit = 10;

	public int minBuzzing;

	public int maxBuzzing = 120;

	public int buzzingStep = 5;

	public int minHunterPenalty;

	public int maxHunterPenalty = 120;

	public int hunterPenaltyStep = 5;

	public float minRecordLength = 1f;

	public float maxRecordLength = 5f;

	public float recordLengthStep = 0.5f;

	[SyncVar(hook = "OnDurationChanged")]
	public int durationMinutes = 10;

	[SyncVar(hook = "OnHunterCountChanged")]
	public int hunterCount = 1;

	[SyncVar(hook = "OnAmmoChanged")]
	public int hunterAmmo = 4;

	[SyncVar(hook = "OnBuzzingChanged")]
	public int buzzingInterval = 30;

	[SyncVar(hook = "OnAnimalSoundChanged")]
	public AnimalSoundLevel animalSound = AnimalSoundLevel.Medium;

	[SyncVar(hook = "OnNpcPopulationChanged")]
	public NpcPopulationLevel npcPopulation = NpcPopulationLevel.Medium;

	[SyncVar(hook = "OnMapChanged")]
	public MapType selectedMap;

	[SyncVar(hook = "OnHunterPenaltyChanged")]
	public int hunterPenaltySeconds = 30;

	[SyncVar(hook = "OnRecordLengthChanged")]
	public float maxRecordSeconds = 3f;

	public Action<int, int> _Mirror_SyncVarHookDelegate_durationMinutes;

	public Action<int, int> _Mirror_SyncVarHookDelegate_hunterCount;

	public Action<int, int> _Mirror_SyncVarHookDelegate_hunterAmmo;

	public Action<int, int> _Mirror_SyncVarHookDelegate_buzzingInterval;

	public Action<AnimalSoundLevel, AnimalSoundLevel> _Mirror_SyncVarHookDelegate_animalSound;

	public Action<NpcPopulationLevel, NpcPopulationLevel> _Mirror_SyncVarHookDelegate_npcPopulation;

	public Action<MapType, MapType> _Mirror_SyncVarHookDelegate_selectedMap;

	public Action<int, int> _Mirror_SyncVarHookDelegate_hunterPenaltySeconds;

	public Action<float, float> _Mirror_SyncVarHookDelegate_maxRecordSeconds;

	public static LobbyGameSettings Instance { get; private set; }

	public int NetworkdurationMinutes
	{
		get
		{
			return durationMinutes;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref durationMinutes, 1uL, _Mirror_SyncVarHookDelegate_durationMinutes);
		}
	}

	public int NetworkhunterCount
	{
		get
		{
			return hunterCount;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref hunterCount, 2uL, _Mirror_SyncVarHookDelegate_hunterCount);
		}
	}

	public int NetworkhunterAmmo
	{
		get
		{
			return hunterAmmo;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref hunterAmmo, 4uL, _Mirror_SyncVarHookDelegate_hunterAmmo);
		}
	}

	public int NetworkbuzzingInterval
	{
		get
		{
			return buzzingInterval;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref buzzingInterval, 8uL, _Mirror_SyncVarHookDelegate_buzzingInterval);
		}
	}

	public AnimalSoundLevel NetworkanimalSound
	{
		get
		{
			return animalSound;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref animalSound, 16uL, _Mirror_SyncVarHookDelegate_animalSound);
		}
	}

	public NpcPopulationLevel NetworknpcPopulation
	{
		get
		{
			return npcPopulation;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref npcPopulation, 32uL, _Mirror_SyncVarHookDelegate_npcPopulation);
		}
	}

	public MapType NetworkselectedMap
	{
		get
		{
			return selectedMap;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref selectedMap, 64uL, _Mirror_SyncVarHookDelegate_selectedMap);
		}
	}

	public int NetworkhunterPenaltySeconds
	{
		get
		{
			return hunterPenaltySeconds;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref hunterPenaltySeconds, 128uL, _Mirror_SyncVarHookDelegate_hunterPenaltySeconds);
		}
	}

	public float NetworkmaxRecordSeconds
	{
		get
		{
			return maxRecordSeconds;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref maxRecordSeconds, 256uL, _Mirror_SyncVarHookDelegate_maxRecordSeconds);
		}
	}

	private void OnDurationChanged(int _, int __)
	{
		LobbyGameSettingsUI.Instance?.RefreshValues();
	}

	private void OnHunterCountChanged(int _, int __)
	{
		LobbyGameSettingsUI.Instance?.RefreshValues();
	}

	private void OnAmmoChanged(int _, int __)
	{
		LobbyGameSettingsUI.Instance?.RefreshValues();
	}

	private void OnBuzzingChanged(int _, int __)
	{
		LobbyGameSettingsUI.Instance?.RefreshValues();
	}

	private void OnAnimalSoundChanged(AnimalSoundLevel _, AnimalSoundLevel __)
	{
		LobbyGameSettingsUI.Instance?.RefreshValues();
	}

	private void OnNpcPopulationChanged(NpcPopulationLevel _, NpcPopulationLevel __)
	{
		LobbyGameSettingsUI.Instance?.RefreshValues();
	}

	private void OnMapChanged(MapType _, MapType __)
	{
		LobbyGameSettingsUI.Instance?.RefreshValues();
	}

	private void OnHunterPenaltyChanged(int _, int __)
	{
		LobbyGameSettingsUI.Instance?.RefreshValues();
	}

	private void OnRecordLengthChanged(float _, float __)
	{
		LobbyGameSettingsUI.Instance?.RefreshValues();
	}

	private void Awake()
	{
		Instance = this;
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public override void OnStartServer()
	{
		MyNetworkManager singleton = MyNetworkManager.Singleton;
		if (!(singleton == null))
		{
			NetworkdurationMinutes = Mathf.Clamp(Mathf.RoundToInt(singleton.GameDurationSeconds / 60f), minDurationMinutes, maxDurationMinutes);
			NetworkhunterCount = Mathf.Clamp(singleton.HunterCount, minHunterCount, maxHunterCount);
			NetworkhunterAmmo = Mathf.Clamp(singleton.HunterAmmo, minAmmo, maxAmmoLimit);
			NetworkbuzzingInterval = Mathf.Clamp(singleton.BuzzingInterval, minBuzzing, maxBuzzing);
			NetworkanimalSound = singleton.AnimalSound;
			NetworknpcPopulation = singleton.NpcPopulation;
			NetworkselectedMap = singleton.SelectedMap;
			NetworkhunterPenaltySeconds = Mathf.Clamp(Mathf.RoundToInt(singleton.HunterPenaltySeconds), minHunterPenalty, maxHunterPenalty);
			NetworkmaxRecordSeconds = Mathf.Clamp(singleton.MaxRecordSeconds, minRecordLength, maxRecordLength);
		}
	}

	[Server]
	public void IncrementDuration()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::IncrementDuration()' called when server was not active");
		}
		else
		{
			SetDuration(durationMinutes + 1);
		}
	}

	[Server]
	public void DecrementDuration()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::DecrementDuration()' called when server was not active");
		}
		else
		{
			SetDuration(durationMinutes - 1);
		}
	}

	[Server]
	private void SetDuration(int minutes)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::SetDuration(System.Int32)' called when server was not active");
		}
		else
		{
			NetworkdurationMinutes = Mathf.Clamp(minutes, minDurationMinutes, maxDurationMinutes);
		}
	}

	[Server]
	public void IncrementHunterCount()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::IncrementHunterCount()' called when server was not active");
		}
		else
		{
			SetHunterCount(hunterCount + 1);
		}
	}

	[Server]
	public void DecrementHunterCount()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::DecrementHunterCount()' called when server was not active");
		}
		else
		{
			SetHunterCount(hunterCount - 1);
		}
	}

	[Server]
	private void SetHunterCount(int value)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::SetHunterCount(System.Int32)' called when server was not active");
		}
		else
		{
			NetworkhunterCount = Mathf.Clamp(value, minHunterCount, maxHunterCount);
		}
	}

	[Server]
	public void IncrementAmmo()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::IncrementAmmo()' called when server was not active");
		}
		else
		{
			SetAmmo(hunterAmmo + 1);
		}
	}

	[Server]
	public void DecrementAmmo()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::DecrementAmmo()' called when server was not active");
		}
		else
		{
			SetAmmo(hunterAmmo - 1);
		}
	}

	[Server]
	private void SetAmmo(int value)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::SetAmmo(System.Int32)' called when server was not active");
		}
		else
		{
			NetworkhunterAmmo = Mathf.Clamp(value, minAmmo, maxAmmoLimit);
		}
	}

	[Server]
	public void IncrementBuzzing()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::IncrementBuzzing()' called when server was not active");
		}
		else
		{
			SetBuzzing(buzzingInterval + buzzingStep);
		}
	}

	[Server]
	public void DecrementBuzzing()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::DecrementBuzzing()' called when server was not active");
		}
		else
		{
			SetBuzzing(buzzingInterval - buzzingStep);
		}
	}

	[Server]
	private void SetBuzzing(int value)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::SetBuzzing(System.Int32)' called when server was not active");
		}
		else
		{
			NetworkbuzzingInterval = Mathf.Clamp(value, minBuzzing, maxBuzzing);
		}
	}

	[Server]
	public void CycleAnimalSound(int delta)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::CycleAnimalSound(System.Int32)' called when server was not active");
		}
		else
		{
			NetworkanimalSound = CycleEnum(animalSound, delta);
		}
	}

	[Server]
	public void CycleNpcPopulation(int delta)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::CycleNpcPopulation(System.Int32)' called when server was not active");
		}
		else
		{
			NetworknpcPopulation = CycleEnum(npcPopulation, delta);
		}
	}

	[Server]
	public void CycleMap(int delta)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::CycleMap(System.Int32)' called when server was not active");
		}
		else
		{
			NetworkselectedMap = CycleEnum(selectedMap, delta);
		}
	}

	[Server]
	public void IncrementHunterPenalty()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::IncrementHunterPenalty()' called when server was not active");
		}
		else
		{
			SetHunterPenalty(hunterPenaltySeconds + hunterPenaltyStep);
		}
	}

	[Server]
	public void DecrementHunterPenalty()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::DecrementHunterPenalty()' called when server was not active");
		}
		else
		{
			SetHunterPenalty(hunterPenaltySeconds - hunterPenaltyStep);
		}
	}

	[Server]
	private void SetHunterPenalty(int value)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::SetHunterPenalty(System.Int32)' called when server was not active");
		}
		else
		{
			NetworkhunterPenaltySeconds = Mathf.Clamp(value, minHunterPenalty, maxHunterPenalty);
		}
	}

	[Server]
	public void IncrementRecordLength()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::IncrementRecordLength()' called when server was not active");
		}
		else
		{
			SetRecordLength(maxRecordSeconds + recordLengthStep);
		}
	}

	[Server]
	public void DecrementRecordLength()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::DecrementRecordLength()' called when server was not active");
		}
		else
		{
			SetRecordLength(maxRecordSeconds - recordLengthStep);
		}
	}

	[Server]
	private void SetRecordLength(float value)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::SetRecordLength(System.Single)' called when server was not active");
			return;
		}
		float value2 = Mathf.Round(value / recordLengthStep) * recordLengthStep;
		NetworkmaxRecordSeconds = Mathf.Clamp(value2, minRecordLength, maxRecordLength);
	}

	private static T CycleEnum<T>(T current, int delta) where T : struct, Enum
	{
		T[] array = (T[])Enum.GetValues(typeof(T));
		int num = Array.IndexOf(array, current);
		num = ((num + delta) % array.Length + array.Length) % array.Length;
		return array[num];
	}

	[Server]
	public void ResetToDefaults()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::ResetToDefaults()' called when server was not active");
			return;
		}
		SetDuration(10);
		SetHunterCount(1);
		SetAmmo(4);
		SetBuzzing(30);
		NetworkanimalSound = AnimalSoundLevel.Medium;
		NetworknpcPopulation = NpcPopulationLevel.Medium;
		NetworkselectedMap = MapType.Farm;
		SetHunterPenalty(30);
		SetRecordLength(3f);
	}

	[Server]
	public void CommitToNetworkManager()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void LobbyGameSettings::CommitToNetworkManager()' called when server was not active");
			return;
		}
		MyNetworkManager singleton = MyNetworkManager.Singleton;
		if (!(singleton == null))
		{
			singleton.GameDurationSeconds = (float)durationMinutes * 60f;
			singleton.HunterCount = hunterCount;
			singleton.HunterAmmo = hunterAmmo;
			singleton.BuzzingInterval = buzzingInterval;
			singleton.AnimalSound = animalSound;
			singleton.NpcPopulation = npcPopulation;
			singleton.SelectedMap = selectedMap;
			singleton.HunterPenaltySeconds = hunterPenaltySeconds;
			singleton.MaxRecordSeconds = maxRecordSeconds;
		}
	}

	public LobbyGameSettings()
	{
		_Mirror_SyncVarHookDelegate_durationMinutes = OnDurationChanged;
		_Mirror_SyncVarHookDelegate_hunterCount = OnHunterCountChanged;
		_Mirror_SyncVarHookDelegate_hunterAmmo = OnAmmoChanged;
		_Mirror_SyncVarHookDelegate_buzzingInterval = OnBuzzingChanged;
		_Mirror_SyncVarHookDelegate_animalSound = OnAnimalSoundChanged;
		_Mirror_SyncVarHookDelegate_npcPopulation = OnNpcPopulationChanged;
		_Mirror_SyncVarHookDelegate_selectedMap = OnMapChanged;
		_Mirror_SyncVarHookDelegate_hunterPenaltySeconds = OnHunterPenaltyChanged;
		_Mirror_SyncVarHookDelegate_maxRecordSeconds = OnRecordLengthChanged;
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
			writer.WriteVarInt(durationMinutes);
			writer.WriteVarInt(hunterCount);
			writer.WriteVarInt(hunterAmmo);
			writer.WriteVarInt(buzzingInterval);
			GeneratedNetworkCode._Write_AnimalSoundLevel(writer, animalSound);
			GeneratedNetworkCode._Write_NpcPopulationLevel(writer, npcPopulation);
			GeneratedNetworkCode._Write_MapType(writer, selectedMap);
			writer.WriteVarInt(hunterPenaltySeconds);
			writer.WriteFloat(maxRecordSeconds);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteVarInt(durationMinutes);
		}
		if ((syncVarDirtyBits & 2L) != 0L)
		{
			writer.WriteVarInt(hunterCount);
		}
		if ((syncVarDirtyBits & 4L) != 0L)
		{
			writer.WriteVarInt(hunterAmmo);
		}
		if ((syncVarDirtyBits & 8L) != 0L)
		{
			writer.WriteVarInt(buzzingInterval);
		}
		if ((syncVarDirtyBits & 0x10L) != 0L)
		{
			GeneratedNetworkCode._Write_AnimalSoundLevel(writer, animalSound);
		}
		if ((syncVarDirtyBits & 0x20L) != 0L)
		{
			GeneratedNetworkCode._Write_NpcPopulationLevel(writer, npcPopulation);
		}
		if ((syncVarDirtyBits & 0x40L) != 0L)
		{
			GeneratedNetworkCode._Write_MapType(writer, selectedMap);
		}
		if ((syncVarDirtyBits & 0x80L) != 0L)
		{
			writer.WriteVarInt(hunterPenaltySeconds);
		}
		if ((syncVarDirtyBits & 0x100L) != 0L)
		{
			writer.WriteFloat(maxRecordSeconds);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref durationMinutes, _Mirror_SyncVarHookDelegate_durationMinutes, reader.ReadVarInt());
			GeneratedSyncVarDeserialize(ref hunterCount, _Mirror_SyncVarHookDelegate_hunterCount, reader.ReadVarInt());
			GeneratedSyncVarDeserialize(ref hunterAmmo, _Mirror_SyncVarHookDelegate_hunterAmmo, reader.ReadVarInt());
			GeneratedSyncVarDeserialize(ref buzzingInterval, _Mirror_SyncVarHookDelegate_buzzingInterval, reader.ReadVarInt());
			GeneratedSyncVarDeserialize(ref animalSound, _Mirror_SyncVarHookDelegate_animalSound, GeneratedNetworkCode._Read_AnimalSoundLevel(reader));
			GeneratedSyncVarDeserialize(ref npcPopulation, _Mirror_SyncVarHookDelegate_npcPopulation, GeneratedNetworkCode._Read_NpcPopulationLevel(reader));
			GeneratedSyncVarDeserialize(ref selectedMap, _Mirror_SyncVarHookDelegate_selectedMap, GeneratedNetworkCode._Read_MapType(reader));
			GeneratedSyncVarDeserialize(ref hunterPenaltySeconds, _Mirror_SyncVarHookDelegate_hunterPenaltySeconds, reader.ReadVarInt());
			GeneratedSyncVarDeserialize(ref maxRecordSeconds, _Mirror_SyncVarHookDelegate_maxRecordSeconds, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref durationMinutes, _Mirror_SyncVarHookDelegate_durationMinutes, reader.ReadVarInt());
		}
		if ((num & 2L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref hunterCount, _Mirror_SyncVarHookDelegate_hunterCount, reader.ReadVarInt());
		}
		if ((num & 4L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref hunterAmmo, _Mirror_SyncVarHookDelegate_hunterAmmo, reader.ReadVarInt());
		}
		if ((num & 8L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref buzzingInterval, _Mirror_SyncVarHookDelegate_buzzingInterval, reader.ReadVarInt());
		}
		if ((num & 0x10L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref animalSound, _Mirror_SyncVarHookDelegate_animalSound, GeneratedNetworkCode._Read_AnimalSoundLevel(reader));
		}
		if ((num & 0x20L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref npcPopulation, _Mirror_SyncVarHookDelegate_npcPopulation, GeneratedNetworkCode._Read_NpcPopulationLevel(reader));
		}
		if ((num & 0x40L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref selectedMap, _Mirror_SyncVarHookDelegate_selectedMap, GeneratedNetworkCode._Read_MapType(reader));
		}
		if ((num & 0x80L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref hunterPenaltySeconds, _Mirror_SyncVarHookDelegate_hunterPenaltySeconds, reader.ReadVarInt());
		}
		if ((num & 0x100L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref maxRecordSeconds, _Mirror_SyncVarHookDelegate_maxRecordSeconds, reader.ReadFloat());
		}
	}
}
