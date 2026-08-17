using System.Runtime.InteropServices;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror.Examples.CouchCoop
{
	public class MovingPlatform : NetworkBehaviour
	{
		public Transform endTarget;

		public float moveSpeed = 0.5f;

		[SyncVar]
		public bool moveObj = true;

		public bool moveStopsUponExit;

		public bool moveStartsUponCollision;

		private Vector3 startPosition;

		private Vector3 endPosition;

		public bool NetworkmoveObj
		{
			get
			{
				return moveObj;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref moveObj, 1uL, null);
			}
		}

		private void Awake()
		{
			startPosition = base.transform.position;
			endPosition = endTarget.position;
		}

		private void Update()
		{
			if (!moveObj)
			{
				return;
			}
			float maxDistanceDelta = moveSpeed * Time.deltaTime;
			base.transform.position = Vector3.MoveTowards(base.transform.position, endPosition, maxDistanceDelta);
			if (Vector3.Distance(base.transform.position, endPosition) < 0.001f)
			{
				endPosition = ((endPosition == startPosition) ? endTarget.position : startPosition);
				if (base.isServer)
				{
					RpcResyncPosition((byte)((endPosition == startPosition) ? 1 : 0));
				}
			}
		}

		[ClientRpc]
		private void RpcResyncPosition(byte _value)
		{
			NetworkWriterPooled writer = NetworkWriterPool.Get();
			NetworkWriterExtensions.WriteByte(writer, _value);
			SendRPCInternal("System.Void Mirror.Examples.CouchCoop.MovingPlatform::RpcResyncPosition(System.Byte)", 1743852064, writer, 0, includeOwner: true);
			NetworkWriterPool.Return(writer);
		}

		[ServerCallback]
		private void OnCollisionEnter(Collision collision)
		{
			if (NetworkServer.active && moveStartsUponCollision && collision.gameObject.tag == "Player")
			{
				NetworkmoveObj = true;
			}
		}

		[ServerCallback]
		private void OnCollisionExit(Collision collision)
		{
			if (NetworkServer.active && moveStopsUponExit && collision.gameObject.tag == "Player")
			{
				NetworkmoveObj = false;
			}
		}

		public override bool Weaved()
		{
			return true;
		}

		protected void UserCode_RpcResyncPosition__Byte(byte _value)
		{
			base.transform.position = ((_value == 1) ? endTarget.position : startPosition);
		}

		protected static void InvokeUserCode_RpcResyncPosition__Byte(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError("RPC RpcResyncPosition called on server.");
			}
			else
			{
				((MovingPlatform)obj).UserCode_RpcResyncPosition__Byte(NetworkReaderExtensions.ReadByte(reader));
			}
		}

		static MovingPlatform()
		{
			RemoteProcedureCalls.RegisterRpc(typeof(MovingPlatform), "System.Void Mirror.Examples.CouchCoop.MovingPlatform::RpcResyncPosition(System.Byte)", InvokeUserCode_RpcResyncPosition__Byte);
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WriteBool(moveObj);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 1L) != 0L)
			{
				writer.WriteBool(moveObj);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref moveObj, null, reader.ReadBool());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 1L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref moveObj, null, reader.ReadBool());
			}
		}
	}
}
