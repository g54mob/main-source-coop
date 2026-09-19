using System;
using System.Runtime.InteropServices;
using Dissonance.Audio.Playback;
using Dissonance.Integrations.MirrorIgnorance;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

public class Health : NetworkBehaviour
{
	[SyncVar(hook = "OnDeadChanged")]
	private bool _isDead;

	[Header("Ölüm Efekti")]
	[Tooltip("Oyuncu ölünce konumuna spawn edilecek particle prefab")]
	public GameObject deathParticlePrefab;

	public float particleLifetime = 5f;

	[SyncVar]
	private bool _wasShotThisRound;

	public Action<bool, bool> _Mirror_SyncVarHookDelegate__isDead;

	public bool IsDead => _isDead;

	public bool WasShotThisRound => _wasShotThisRound;

	public bool Network_isDead
	{
		get
		{
			return _isDead;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _isDead, 1uL, _Mirror_SyncVarHookDelegate__isDead);
		}
	}

	public bool Network_wasShotThisRound
	{
		get
		{
			return _wasShotThisRound;
		}
		[param: In]
		set
		{
			GeneratedSyncVarSetter(value, ref _wasShotThisRound, 2uL, null);
		}
	}

	[Server]
	public void ServerResetForNewRound()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void Health::ServerResetForNewRound()' called when server was not active");
		}
		else
		{
			Network_wasShotThisRound = false;
		}
	}

	[Server]
	public void ServerKill()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void Health::ServerKill()' called when server was not active");
		}
		else if (!_isDead)
		{
			Network_isDead = true;
			Network_wasShotThisRound = true;
			RpcSpawnDeathParticle(base.transform.position);
			TargetOnDeath(base.connectionToClient);
			PlayerRoleData component = GetComponent<PlayerRoleData>();
			if (component != null && component.Role == PlayerRole.Animal)
			{
				GameManager.Instance?.StopSoundsForAnimalType(component.AssignedAnimal);
			}
			GetComponent<AnimalEatController>()?.ServerForceStopEating();
		}
	}

	[Server]
	public void ServerRevive()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void Health::ServerRevive()' called when server was not active");
		}
		else if (_isDead)
		{
			Network_isDead = false;
			TargetOnRevive(base.connectionToClient);
		}
	}

	[ClientRpc]
	private void RpcSpawnDeathParticle(Vector3 pos)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(pos);
		SendRPCInternal("System.Void Health::RpcSpawnDeathParticle(UnityEngine.Vector3)", -1456283412, writer, 0, includeOwner: true);
		NetworkWriterPool.Return(writer);
	}

	[TargetRpc]
	private void TargetOnDeath(NetworkConnectionToClient target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendTargetRPCInternal(target, "System.Void Health::TargetOnDeath(Mirror.NetworkConnectionToClient)", -10903132, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[TargetRpc]
	private void TargetOnRevive(NetworkConnectionToClient target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendTargetRPCInternal(target, "System.Void Health::TargetOnRevive(Mirror.NetworkConnectionToClient)", 1537292331, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	private void SetLocalDeadVisuals()
	{
		PlayerModelController component = GetComponent<PlayerModelController>();
		if (component != null)
		{
			component.SetModelVisible(visible: false);
			component.enabled = false;
		}
		PlayerController component2 = GetComponent<PlayerController>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
		AnimalEatController component3 = GetComponent<AnimalEatController>();
		if (component3 != null)
		{
			component3.enabled = false;
		}
	}

	private void SetLocalAliveVisuals()
	{
		PlayerModelController component = GetComponent<PlayerModelController>();
		if (component != null)
		{
			component.enabled = true;
			component.SetModelVisible(visible: true);
		}
		PlayerController component2 = GetComponent<PlayerController>();
		if (component2 != null && !RoleAssignmentUI.IsShowing)
		{
			component2.enabled = true;
		}
		AnimalEatController component3 = GetComponent<AnimalEatController>();
		if (component3 != null && !RoleAssignmentUI.IsShowing)
		{
			component3.enabled = true;
		}
	}

	private void OnDeadChanged(bool _, bool dead)
	{
		MirrorIgnorancePlayer component = GetComponent<MirrorIgnorancePlayer>();
		if (component != null && !string.IsNullOrEmpty(component.PlayerId))
		{
			RemoteSpeakerDeathState.SetDead(component.PlayerId, dead);
		}
		PlayerModelController component2 = GetComponent<PlayerModelController>();
		if (component2 != null)
		{
			component2.SetModelVisible(!dead);
		}
		PlayerController component3 = GetComponent<PlayerController>();
		if (component3 != null)
		{
			if (dead)
			{
				component3.enabled = false;
			}
			else if (!RoleAssignmentUI.IsShowing)
			{
				component3.enabled = true;
			}
		}
		AnimalEatController component4 = GetComponent<AnimalEatController>();
		if (component4 != null)
		{
			if (dead)
			{
				component4.enabled = false;
			}
			else if (!RoleAssignmentUI.IsShowing)
			{
				component4.enabled = true;
			}
		}
		CharacterController component5 = GetComponent<CharacterController>();
		if (component5 != null)
		{
			component5.enabled = !dead;
		}
		PlayerListUI.Instance?.RefreshClient(GetComponent<MyClient>());
		if (base.isLocalPlayer && !dead)
		{
			SpectatorController.ExitSpectate();
		}
	}

	public Health()
	{
		_Mirror_SyncVarHookDelegate__isDead = OnDeadChanged;
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_RpcSpawnDeathParticle__Vector3(Vector3 pos)
	{
		if (!(deathParticlePrefab == null))
		{
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate(deathParticlePrefab, pos, Quaternion.identity), particleLifetime);
		}
	}

	protected static void InvokeUserCode_RpcSpawnDeathParticle__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSpawnDeathParticle called on server.");
		}
		else
		{
			((Health)obj).UserCode_RpcSpawnDeathParticle__Vector3(reader.ReadVector3());
		}
	}

	protected void UserCode_TargetOnDeath__NetworkConnectionToClient(NetworkConnectionToClient target)
	{
		SetLocalDeadVisuals();
		SpectatorController.EnterSpectator();
	}

	protected static void InvokeUserCode_TargetOnDeath__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetOnDeath called on server.");
		}
		else
		{
			((Health)obj).UserCode_TargetOnDeath__NetworkConnectionToClient(null);
		}
	}

	protected void UserCode_TargetOnRevive__NetworkConnectionToClient(NetworkConnectionToClient target)
	{
		SetLocalAliveVisuals();
		SpectatorController.ExitSpectate();
	}

	protected static void InvokeUserCode_TargetOnRevive__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetOnRevive called on server.");
		}
		else
		{
			((Health)obj).UserCode_TargetOnRevive__NetworkConnectionToClient(null);
		}
	}

	static Health()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(Health), "System.Void Health::RpcSpawnDeathParticle(UnityEngine.Vector3)", InvokeUserCode_RpcSpawnDeathParticle__Vector3);
		RemoteProcedureCalls.RegisterRpc(typeof(Health), "System.Void Health::TargetOnDeath(Mirror.NetworkConnectionToClient)", InvokeUserCode_TargetOnDeath__NetworkConnectionToClient);
		RemoteProcedureCalls.RegisterRpc(typeof(Health), "System.Void Health::TargetOnRevive(Mirror.NetworkConnectionToClient)", InvokeUserCode_TargetOnRevive__NetworkConnectionToClient);
	}

	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(_isDead);
			writer.WriteBool(_wasShotThisRound);
			return;
		}
		writer.WriteVarULong(syncVarDirtyBits);
		if ((syncVarDirtyBits & 1L) != 0L)
		{
			writer.WriteBool(_isDead);
		}
		if ((syncVarDirtyBits & 2L) != 0L)
		{
			writer.WriteBool(_wasShotThisRound);
		}
	}

	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			GeneratedSyncVarDeserialize(ref _isDead, _Mirror_SyncVarHookDelegate__isDead, reader.ReadBool());
			GeneratedSyncVarDeserialize(ref _wasShotThisRound, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadVarULong();
		if ((num & 1L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _isDead, _Mirror_SyncVarHookDelegate__isDead, reader.ReadBool());
		}
		if ((num & 2L) != 0L)
		{
			GeneratedSyncVarDeserialize(ref _wasShotThisRound, null, reader.ReadBool());
		}
	}
}
