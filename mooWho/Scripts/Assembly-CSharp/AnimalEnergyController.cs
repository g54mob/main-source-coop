using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class AnimalEnergyController : NetworkBehaviour
{
	[Header("Enerji Süreleri")]
	[Tooltip("Dolu bardan (1) boşa (0) inmek için gereken kesintisiz koşu süresi (sn)")]
	public float runDurationFull = 5f;

	[Tooltip("Boş bardan (0) doluya (1) çıkmak için gereken kesintisiz yeme süresi (sn) — sol tık")]
	public float refillDurationFull = 10f;

	[Header("Referanslar")]
	public PlayerRoleData roleData;

	private float _energy = 1f;

	private bool _isGameScene;

	private PlayerController _pc;

	private AnimalEatController _eatController;

	private Health _health;

	public bool CanRun
	{
		get
		{
			if (_isGameScene)
			{
				return _energy > 0f;
			}
			return true;
		}
	}

	private void Awake()
	{
		if (roleData == null)
		{
			roleData = GetComponent<PlayerRoleData>();
		}
		_pc = GetComponent<PlayerController>();
		_eatController = GetComponent<AnimalEatController>();
		_health = GetComponent<Health>();
		_isGameScene = MyNetworkManager.Singleton != null && MyNetworkManager.Singleton.IsInGameScene;
	}

	private void Update()
	{
		if (!base.isLocalPlayer)
		{
			return;
		}
		bool flag = roleData != null && roleData.Role == PlayerRole.Animal;
		PlayerHUD.Instance?.SetEnergyBarVisible(flag);
		if (!flag)
		{
			return;
		}
		if (!_isGameScene)
		{
			_energy = 1f;
			PlayerHUD.Instance?.SetEnergyFill(1f, pulseInsufficient: false);
			return;
		}
		if (_health != null && _health.IsDead)
		{
			PlayerHUD.Instance?.SetEnergyFill(_energy, pulseInsufficient: false);
			return;
		}
		bool num = _eatController != null && _eatController.IsEating;
		bool flag2 = _pc != null && _pc.IsRunning;
		if (num)
		{
			_energy = Mathf.Min(1f, _energy + Time.deltaTime / Mathf.Max(refillDurationFull, 0.01f));
		}
		else if (flag2)
		{
			_energy = Mathf.Max(0f, _energy - Time.deltaTime / Mathf.Max(runDurationFull, 0.01f));
		}
		bool pulseInsufficient = _pc != null && _pc.WantsToRunWithoutEnergy;
		PlayerHUD.Instance?.SetEnergyFill(_energy, pulseInsufficient);
	}

	[Server]
	public void ServerRefillFull()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalEnergyController::ServerRefillFull()' called when server was not active");
		}
		else if (base.connectionToClient != null)
		{
			TargetSetEnergyFull(base.connectionToClient);
		}
	}

	[TargetRpc]
	private void TargetSetEnergyFull(NetworkConnectionToClient target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		SendTargetRPCInternal(target, "System.Void AnimalEnergyController::TargetSetEnergyFull(Mirror.NetworkConnectionToClient)", 1369198726, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	[Server]
	public void ServerResetForNewRound()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimalEnergyController::ServerResetForNewRound()' called when server was not active");
		}
		else if (base.connectionToClient != null)
		{
			TargetSetEnergyFull(base.connectionToClient);
		}
	}

	public override bool Weaved()
	{
		return true;
	}

	protected void UserCode_TargetSetEnergyFull__NetworkConnectionToClient(NetworkConnectionToClient target)
	{
		_energy = 1f;
	}

	protected static void InvokeUserCode_TargetSetEnergyFull__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSetEnergyFull called on server.");
		}
		else
		{
			((AnimalEnergyController)obj).UserCode_TargetSetEnergyFull__NetworkConnectionToClient(null);
		}
	}

	static AnimalEnergyController()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(AnimalEnergyController), "System.Void AnimalEnergyController::TargetSetEnergyFull(Mirror.NetworkConnectionToClient)", InvokeUserCode_TargetSetEnergyFull__NetworkConnectionToClient);
	}
}
