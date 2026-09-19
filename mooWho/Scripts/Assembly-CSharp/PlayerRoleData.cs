using System;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class PlayerRoleData : NetworkBehaviour
{
	[SyncVar(hook = "OnRoleChanged")]
	public PlayerRole Role = PlayerRole.Animal;

	[SyncVar(hook = "OnAnimalChanged")]
	public AnimalType AssignedAnimal;

	[SyncVar(hook = "OnRolesLockedChanged")]
	public bool RolesLocked;

	[SyncVar(hook = "OnRoleVersionChanged")]
	private int _roleVersion;

	[SyncVar]
	private AnimalType _lobbyPreviewAnimal;

	private PlayerModelController _modelController;

	public Action<PlayerRole, PlayerRole> _Mirror_SyncVarHookDelegate_Role;

	public Action<AnimalType, AnimalType> _Mirror_SyncVarHookDelegate_AssignedAnimal;

	public Action<bool, bool> _Mirror_SyncVarHookDelegate_RolesLocked;

	public Action<int, int> _Mirror_SyncVarHookDelegate__roleVersion;

	public AnimalType LobbyPreviewAnimal => _lobbyPreviewAnimal;

	public PlayerRole NetworkRole
	{
		get
		{
			return Role;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref Role, 1uL, _Mirror_SyncVarHookDelegate_Role);
		}
	}

	public AnimalType NetworkAssignedAnimal
	{
		get
		{
			return AssignedAnimal;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref AssignedAnimal, 2uL, _Mirror_SyncVarHookDelegate_AssignedAnimal);
		}
	}

	public bool NetworkRolesLocked
	{
		get
		{
			return RolesLocked;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref RolesLocked, 4uL, _Mirror_SyncVarHookDelegate_RolesLocked);
		}
	}

	public int Network_roleVersion
	{
		get
		{
			return _roleVersion;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _roleVersion, 8uL, _Mirror_SyncVarHookDelegate__roleVersion);
		}
	}

	public AnimalType Network_lobbyPreviewAnimal
	{
		get
		{
			return _lobbyPreviewAnimal;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _lobbyPreviewAnimal, 16uL, null);
		}
	}

	public static event Action<PlayerRole> LocalRoleChanged;

	private void Awake()
	{
		_modelController = GetComponent<PlayerModelController>();
	}

	public override void OnStartServer()
	{
		base.OnStartServer();
		Network_lobbyPreviewAnimal = ((_modelController != null) ? _modelController.GetRandomAnimal() : AnimalType.None);
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		if (RolesLocked)
		{
			PlayerHUD.Instance?.ApplyRole(Role);
			ApplyCameraMode(Role);
		}
	}

	private void OnRoleChanged(PlayerRole _, PlayerRole newRole)
	{
		ApplyVisuals();
		RefreshAllOutlines();
	}

	private void RefreshAllOutlines()
	{
		PlayerOutline[] array = UnityEngine.Object.FindObjectsOfType<PlayerOutline>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].RefreshOutlineVisibility();
		}
	}

	private void ApplyCameraMode(PlayerRole role)
	{
		NetworkedCameraController component = GetComponent<NetworkedCameraController>();
		if (!(component == null))
		{
			component.SetMode((role == PlayerRole.Hunter) ? NetworkedCameraController.CameraMode.FirstPerson : NetworkedCameraController.CameraMode.ThirdPerson);
		}
	}

	private void OnAnimalChanged(AnimalType _, AnimalType newAnimal)
	{
		ApplyVisuals();
	}

	private void OnRolesLockedChanged(bool _, bool locked)
	{
		ApplyVisuals();
		RefreshAllOutlines();
	}

	private void OnRoleVersionChanged(int _, int newVersion)
	{
		if (base.isLocalPlayer)
		{
			ApplyRoleUILocal(Role, AssignedAnimal);
			PlayerRoleData.LocalRoleChanged?.Invoke(Role);
			if (MyNetworkManager.isMulitplayer)
			{
				AchievementManager.Instance?.Unlock("ACH_EARLY_BIRD");
			}
		}
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		ApplyVisuals();
	}

	public void ApplyVisuals()
	{
		if (_modelController == null)
		{
			_modelController = GetComponent<PlayerModelController>();
		}
		if (!(_modelController == null))
		{
			if (RolesLocked)
			{
				_modelController.ApplyModel(Role, AssignedAnimal);
				return;
			}
			AnimalType animal = ((_lobbyPreviewAnimal != AnimalType.None) ? _lobbyPreviewAnimal : _modelController.GetFirstAnimal());
			_modelController.ApplyModel(PlayerRole.Animal, animal);
		}
	}

	[Server]
	public void ServerSetRole(PlayerRole role, AnimalType animal)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerRoleData::ServerSetRole(PlayerRole,AnimalType)' called when server was not active");
			return;
		}
		NetworkRole = role;
		NetworkAssignedAnimal = animal;
		NetworkRolesLocked = true;
		Network_roleVersion = _roleVersion + 1;
		if (base.connectionToClient != null)
		{
			TargetApplyRoleUI(base.connectionToClient, role, animal);
		}
	}

	[TargetRpc]
	private void TargetApplyRoleUI(NetworkConnectionToClient target, PlayerRole role, AnimalType animal)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		GeneratedNetworkCode._Write_PlayerRole(writer, role);
		GeneratedNetworkCode._Write_AnimalType(writer, animal);
		SendTargetRPCInternal(target, "System.Void PlayerRoleData::TargetApplyRoleUI(Mirror.NetworkConnectionToClient,PlayerRole,AnimalType)", -2124556405, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	private void ApplyRoleUILocal(PlayerRole role, AnimalType animal)
	{
		SafeInvoke(delegate
		{
			PlayerHUD.Instance?.HideLoading();
		});
		SafeInvoke(delegate
		{
			LoadingScreen.Instance?.Hide();
		});
		SafeInvoke(delegate
		{
			RoleAssignmentUI.Instance?.ShowRole(role, animal);
		});
		SafeInvoke(delegate
		{
			PlayerHUD.Instance?.ApplyRole(role);
		});
		SafeInvoke(delegate
		{
			ApplyCameraMode(role);
		});
		SafeInvoke(delegate
		{
			MyClient component = GetComponent<MyClient>();
			PlayerHUD.Instance?.UpdateProfile(role, animal, (component != null) ? component.playerInfo.username : "");
		});
	}

	private static void SafeInvoke(Action action)
	{
		try
		{
			action();
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	public PlayerRoleData()
	{
		_Mirror_SyncVarHookDelegate_Role = OnRoleChanged;
		_Mirror_SyncVarHookDelegate_AssignedAnimal = OnAnimalChanged;
		_Mirror_SyncVarHookDelegate_RolesLocked = OnRolesLockedChanged;
		_Mirror_SyncVarHookDelegate__roleVersion = OnRoleVersionChanged;
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_TargetApplyRoleUI__NetworkConnectionToClient__PlayerRole__AnimalType(NetworkConnectionToClient target, PlayerRole role, AnimalType animal)
	{
		ApplyRoleUILocal(role, animal);
	}

	protected static void InvokeUserCode_TargetApplyRoleUI__NetworkConnectionToClient__PlayerRole__AnimalType(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetApplyRoleUI called on server.");
		}
		else
		{
			((PlayerRoleData)obj).UserCode_TargetApplyRoleUI__NetworkConnectionToClient__PlayerRole__AnimalType(null, GeneratedNetworkCode._Read_PlayerRole(reader), GeneratedNetworkCode._Read_AnimalType(reader));
		}
	}

	static PlayerRoleData()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerRoleData), "System.Void PlayerRoleData::TargetApplyRoleUI(Mirror.NetworkConnectionToClient,PlayerRole,AnimalType)", InvokeUserCode_TargetApplyRoleUI__NetworkConnectionToClient__PlayerRole__AnimalType);
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			GeneratedNetworkCode._Write_PlayerRole(writer, Role);
			GeneratedNetworkCode._Write_AnimalType(writer, AssignedAnimal);
			writer.WriteBool(RolesLocked);
			writer.WriteVarInt(_roleVersion);
			GeneratedNetworkCode._Write_AnimalType(writer, _lobbyPreviewAnimal);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			GeneratedNetworkCode._Write_PlayerRole(writer, Role);
		}
		if ((syncVarDirtyBits & 2L) != 0L)
		{
			GeneratedNetworkCode._Write_AnimalType(writer, AssignedAnimal);
		}
		if ((syncVarDirtyBits & 4L) != 0L)
		{
			writer.WriteBool(RolesLocked);
		}
		if ((syncVarDirtyBits & 8L) != 0L)
		{
			writer.WriteVarInt(_roleVersion);
		}
		if ((syncVarDirtyBits & 0x10L) != 0L)
		{
			GeneratedNetworkCode._Write_AnimalType(writer, _lobbyPreviewAnimal);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref Role, _Mirror_SyncVarHookDelegate_Role, GeneratedNetworkCode._Read_PlayerRole(reader));
			GeneratedSyncVarDeserialize(ref AssignedAnimal, _Mirror_SyncVarHookDelegate_AssignedAnimal, GeneratedNetworkCode._Read_AnimalType(reader));
			GeneratedSyncVarDeserialize(ref RolesLocked, _Mirror_SyncVarHookDelegate_RolesLocked, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref _roleVersion, _Mirror_SyncVarHookDelegate__roleVersion, reader.ReadVarInt());
			GeneratedSyncVarDeserialize(ref _lobbyPreviewAnimal, null, GeneratedNetworkCode._Read_AnimalType(reader));
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref Role, _Mirror_SyncVarHookDelegate_Role, GeneratedNetworkCode._Read_PlayerRole(reader));
		}
		if ((num & 2L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref AssignedAnimal, _Mirror_SyncVarHookDelegate_AssignedAnimal, GeneratedNetworkCode._Read_AnimalType(reader));
		}
		if ((num & 4L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref RolesLocked, _Mirror_SyncVarHookDelegate_RolesLocked, reader.ReadBool());
		}
		if ((num & 8L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _roleVersion, _Mirror_SyncVarHookDelegate__roleVersion, reader.ReadVarInt());
		}
		if ((num & 0x10L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _lobbyPreviewAnimal, null, GeneratedNetworkCode._Read_AnimalType(reader));
		}
	}
}
