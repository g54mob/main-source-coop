using System;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

namespace NomadDrive.Sandbox.Test
{
	public class TestNetworkObject : NetworkBehaviour
	{
		public int TestInteger;

		[SyncVar(hook = "OnTestIntegerChanged")]
		private readonly int _testInteger;

		public Action<int, int> _Mirror_SyncVarHookDelegate__testInteger;

		public int Network_testInteger
		{
			get
			{
				return _testInteger;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _testInteger, 1uL, _Mirror_SyncVarHookDelegate__testInteger);
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			TestInteger = 10;
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			TestInteger = 20;
		}

		private void OnTestIntegerChanged(int oldValue, int newValue)
		{
		}

		[Command(requiresAuthority = false)]
		public void SetTestInteger(int value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			writer.WriteVarInt(value);
			SendCommandInternal("System.Void NomadDrive.Sandbox.Test.TestNetworkObject::SetTestInteger(System.Int32)", 155771437, writer, 0, requiresAuthority: false);
			NetworkWriterPool.Return(writer);
		}

		public TestNetworkObject()
		{
			_Mirror_SyncVarHookDelegate__testInteger = OnTestIntegerChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_SetTestInteger__Int32(int value)
		{
			TestInteger = value;
		}

		protected static void InvokeUserCode_SetTestInteger__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("Command SetTestInteger called on client.");
			}
			else
			{
				((TestNetworkObject)obj).UserCode_SetTestInteger__Int32(reader.ReadVarInt());
			}
		}

		static TestNetworkObject()
		{
			RemoteProcedureCalls.RegisterCommand(typeof(TestNetworkObject), "System.Void NomadDrive.Sandbox.Test.TestNetworkObject::SetTestInteger(System.Int32)", InvokeUserCode_SetTestInteger__Int32, requiresAuthority: false);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteVarInt(_testInteger);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteVarInt(_testInteger);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _testInteger, _Mirror_SyncVarHookDelegate__testInteger, reader.ReadVarInt());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _testInteger, _Mirror_SyncVarHookDelegate__testInteger, reader.ReadVarInt());
			}
		}
	}
}
