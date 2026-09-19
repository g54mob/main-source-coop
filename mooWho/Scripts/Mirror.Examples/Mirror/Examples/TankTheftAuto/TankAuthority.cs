using System;
using System.Runtime.InteropServices;
using Mirror.Examples.Common;
using Mirror.Examples.Common.Controllers.Tank;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Examples.TankTheftAuto
{
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	public class TankAuthority : NetworkBehaviour
	{
		[Header("Components")]
		public GameObject triggerUI;

		public TankTurretBase tankTurret;

		public GameObject tankTrigger;

		[SyncVar(hook = "OnIsControlledChanged")]
		public bool isControlled;

		public Action<bool, bool> _Mirror_SyncVarHookDelegate_isControlled;

		public bool NetworkisControlled
		{
			get
			{
				return isControlled;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref isControlled, 1uL, _Mirror_SyncVarHookDelegate_isControlled);
			}
		}

		private void OnIsControlledChanged(bool _, bool newValue)
		{
			tankTrigger.SetActive(!newValue);
		}

		protected override void OnValidate()
		{
			if (!Application.isPlaying)
			{
				base.OnValidate();
				Reset();
			}
		}

		private void Reset()
		{
			if (triggerUI == null)
			{
				triggerUI = base.transform.Find("TriggerUI").gameObject;
			}
			if (tankTrigger == null)
			{
				tankTrigger = base.transform.Find("TankTrigger").gameObject;
			}
			if (tankTurret == null)
			{
				tankTurret = GetComponent<TankTurretBase>();
			}
			triggerUI.SetActive(value: false);
		}

		[ClientCallback]
		private void Update()
		{
			if (NetworkClient.active)
			{
				if (triggerUI.activeSelf && Input.GetKeyDown(KeyCode.C))
				{
					CmdTakeControl();
				}
				if (base.isOwned && Input.GetKeyDown(KeyCode.X))
				{
					CmdReleaseControl();
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (base.isClient && other.gameObject.CompareTag("Player") && other.TryGetComponent<NetworkIdentity>(out var component) && component == NetworkClient.localPlayer)
			{
				triggerUI.SetActive(value: true);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (base.isClient && other.gameObject.CompareTag("Player") && other.TryGetComponent<NetworkIdentity>(out var component) && component == NetworkClient.localPlayer)
			{
				triggerUI.SetActive(value: false);
			}
		}

		[Command(requiresAuthority = false)]
		private void CmdTakeControl(NetworkConnectionToClient conn = null)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Mirror.Examples.TankTheftAuto.TankAuthority::CmdTakeControl(Mirror.NetworkConnectionToClient)", 2112344843, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		[Command]
		private void CmdReleaseControl()
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			SendCommandInternal("System.Void Mirror.Examples.TankTheftAuto.TankAuthority::CmdReleaseControl()", -2052273272, writer, 0);
			NetworkWriterPool.Return(writer);
		}

		public override void OnStartAuthority()
		{
			if (triggerUI.TryGetComponent<TextMesh>(out var component))
			{
				component.text = "Press 'X' to release control";
			}
		}

		public override void OnStopAuthority()
		{
			if (triggerUI.TryGetComponent<TextMesh>(out var component))
			{
				component.text = "Press 'C' to take control";
			}
		}

		public override void OnStartClient()
		{
			tankTrigger.SetActive(!isControlled);
		}

		public override void OnStopClient()
		{
			triggerUI.SetActive(value: false);
			tankTrigger.SetActive(value: true);
		}

		public TankAuthority()
		{
			_Mirror_SyncVarHookDelegate_isControlled = OnIsControlledChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_CmdTakeControl__NetworkConnectionToClient(NetworkConnectionToClient conn)
		{
			if (base.connectionToClient != null)
			{
				Debug.LogWarning("Someone else is already controlling this tank");
				return;
			}
			conn.authenticationData = conn.identity.gameObject;
			if (conn.identity.TryGetComponent<RandomColor>(out var component))
			{
				tankTurret.NetworkplayerColor = component.color;
			}
			NetworkisControlled = true;
			NetworkServer.ReplacePlayerForConnection(conn, base.gameObject, ReplacePlayerOptions.Unspawn);
		}

		protected static void InvokeUserCode_CmdTakeControl__NetworkConnectionToClient(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdTakeControl called on client.");
			}
			else
			{
				((TankAuthority)obj).UserCode_CmdTakeControl__NetworkConnectionToClient(senderConnection);
			}
		}

		protected void UserCode_CmdReleaseControl()
		{
			if (base.connectionToClient.authenticationData is GameObject gameObject)
			{
				Vector3 position = base.transform.position + base.transform.right * 3f + Vector3.up;
				gameObject.transform.SetPositionAndRotation(position, base.transform.rotation);
				NetworkisControlled = false;
				tankTurret.NetworkplayerColor = Color.black;
				base.connectionToClient.authenticationData = null;
				NetworkServer.ReplacePlayerForConnection(base.connectionToClient, gameObject, ReplacePlayerOptions.KeepActive);
			}
		}

		protected static void InvokeUserCode_CmdReleaseControl(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command CmdReleaseControl called on client.");
			}
			else
			{
				((TankAuthority)obj).UserCode_CmdReleaseControl();
			}
		}

		static TankAuthority()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(TankAuthority), "System.Void Mirror.Examples.TankTheftAuto.TankAuthority::CmdTakeControl(Mirror.NetworkConnectionToClient)", InvokeUserCode_CmdTakeControl__NetworkConnectionToClient, requiresAuthority: false);
			RemoteProcedureCalls.RegisterCommand(typeof(TankAuthority), "System.Void Mirror.Examples.TankTheftAuto.TankAuthority::CmdReleaseControl()", InvokeUserCode_CmdReleaseControl, requiresAuthority: true);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(isControlled);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteBool(isControlled);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref isControlled, _Mirror_SyncVarHookDelegate_isControlled, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref isControlled, _Mirror_SyncVarHookDelegate_isControlled, reader.ReadBool());
			}
		}
	}
}
