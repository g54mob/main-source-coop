using System.Runtime.InteropServices;
using Mirror.RemoteCalls;
using UnityEngine;
using UnityEngine.AI;

namespace Mirror.Examples.Tanks
{
	public class Tank : NetworkBehaviour
	{
		[Header("Components")]
		public NavMeshAgent agent;

		public Animator animator;

		public TextMesh healthBar;

		public Transform turret;

		[Header("Movement")]
		public float rotationSpeed = 100f;

		[Header("Firing")]
		public KeyCode shootKey = KeyCode.Space;

		public GameObject projectilePrefab;

		public Transform projectileMount;

		[Header("Stats")]
		[SyncVar]
		public int health = 5;

		public int Networkhealth
		{
			get
			{
				return health;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref health, 1uL, null);
			}
		}

		public override void OnStartClient()
		{
			base.name = string.Format("Player[{0}|{1}]", base.netId, base.isLocalPlayer ? "local" : "remote");
		}

		public override void OnStartServer()
		{
			base.name = $"Player[{base.netId}|server]";
		}

		private void Update()
		{
			healthBar.text = new string('-', health);
			if (Application.isFocused && base.isLocalPlayer)
			{
				float axis = Input.GetAxis("Horizontal");
				base.transform.Rotate(0f, axis * rotationSpeed * Time.deltaTime, 0f);
				float axis2 = Input.GetAxis("Vertical");
				Vector3 vector = base.transform.TransformDirection(Vector3.forward);
				agent.velocity = vector * Mathf.Max(axis2, 0f) * agent.speed;
				animator.SetBool("Moving", agent.velocity != Vector3.zero);
				if (Input.GetKeyDown(shootKey))
				{
					CmdFire();
				}
				RotateTurret();
			}
		}

		[Command]
		private void CmdFire()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Mirror.Examples.Tanks.Tank::CmdFire()", -277581242, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		[ClientRpc]
		private void RpcOnFire()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendRPCInternal("System.Void Mirror.Examples.Tanks.Tank::RpcOnFire()", -22609524, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		private void RotateTurret()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out var hitInfo, 100f))
			{
				Debug.DrawLine(ray.origin, hitInfo.point);
				Vector3 worldPosition = new Vector3(hitInfo.point.x, turret.transform.position.y, hitInfo.point.z);
				turret.transform.LookAt(worldPosition);
			}
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdFire()
		{
			NetworkServer.Spawn(Object.Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation));
			RpcOnFire();
		}

		protected static void InvokeUserCode_CmdFire(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdFire called on client.");
			}
			else
			{
				((Tank)obj).UserCode_CmdFire();
			}
		}

		protected void UserCode_RpcOnFire()
		{
			animator.SetTrigger("Shoot");
		}

		protected static void InvokeUserCode_RpcOnFire(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcOnFire called on server.");
			}
			else
			{
				((Tank)obj).UserCode_RpcOnFire();
			}
		}

		static Tank()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(Tank), "System.Void Mirror.Examples.Tanks.Tank::CmdFire()", InvokeUserCode_CmdFire, requiresAuthority: true);
			RemoteProcedureCalls.RegisterRpc(typeof(Tank), "System.Void Mirror.Examples.Tanks.Tank::RpcOnFire()", InvokeUserCode_RpcOnFire);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVarInt(health);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVarInt(health);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref health, null, reader.ReadVarInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref health, null, reader.ReadVarInt());
			}
		}
	}
}
