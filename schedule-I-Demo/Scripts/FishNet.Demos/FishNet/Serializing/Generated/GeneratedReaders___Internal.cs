using System.Runtime.InteropServices;
using FishNet.Example.Authenticating;
using FishNet.Example.CustomSyncObject;
using FishNet.Example.Prediction.Rigidbodies;
using UnityEngine;

namespace FishNet.Serializing.Generated
{
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedReaders___Internal
	{
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeOnce()
		{
			GenericReader<HostPasswordBroadcast>.Read = Read___FishNet_002EExample_002EAuthenticating_002EHostPasswordBroadcastFishNet_002ESerializing_002EGenerateds;
			GenericReader<PasswordBroadcast>.Read = Read___FishNet_002EExample_002EAuthenticating_002EPasswordBroadcastFishNet_002ESerializing_002EGenerateds;
			GenericReader<ResponseBroadcast>.Read = Read___FishNet_002EExample_002EAuthenticating_002EResponseBroadcastFishNet_002ESerializing_002EGenerateds;
			GenericReader<RigidbodyPrediction.MoveData>.Read = Read___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveDataFishNet_002ESerializing_002EGenerateds;
			GenericReader<RigidbodyPrediction.MoveData[]>.Read = Read___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveData_005B_005DFishNet_002ESerializing_002EGenerateds;
			GenericReader<RigidbodyPrediction.ReconcileData>.Read = Read___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FReconcileDataFishNet_002ESerializing_002EGenerateds;
			GenericReader<Structy>.Read = Read___FishNet_002EExample_002ECustomSyncObject_002EStructyFishNet_002ESerializing_002EGenerateds;
		}

		public static HostPasswordBroadcast Read___FishNet_002EExample_002EAuthenticating_002EHostPasswordBroadcastFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return new HostPasswordBroadcast
			{
				Password = reader.ReadString()
			};
		}

		public static PasswordBroadcast Read___FishNet_002EExample_002EAuthenticating_002EPasswordBroadcastFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return new PasswordBroadcast
			{
				Password = reader.ReadString()
			};
		}

		public static ResponseBroadcast Read___FishNet_002EExample_002EAuthenticating_002EResponseBroadcastFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return new ResponseBroadcast
			{
				Passed = reader.ReadBoolean()
			};
		}

		public static RigidbodyPrediction.MoveData Read___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveDataFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return new RigidbodyPrediction.MoveData
			{
				Jump = reader.ReadBoolean(),
				Horizontal = reader.ReadSingle(),
				Vertical = reader.ReadSingle()
			};
		}

		public static RigidbodyPrediction.MoveData[] Read___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveData_005B_005DFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return reader.ReadArrayAllocated<RigidbodyPrediction.MoveData>();
		}

		public static RigidbodyPrediction.ReconcileData Read___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FReconcileDataFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return new RigidbodyPrediction.ReconcileData
			{
				Position = reader.ReadVector3(),
				Rotation = reader.ReadQuaternion(),
				Velocity = reader.ReadVector3(),
				AngularVelocity = reader.ReadVector3()
			};
		}

		public static Structy Read___FishNet_002EExample_002ECustomSyncObject_002EStructyFishNet_002ESerializing_002EGenerateds(Reader reader)
		{
			return new Structy
			{
				Name = reader.ReadString(),
				Age = reader.ReadUInt16()
			};
		}
	}
}
