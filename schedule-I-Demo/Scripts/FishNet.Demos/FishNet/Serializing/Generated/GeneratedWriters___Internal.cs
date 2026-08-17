using System.Runtime.InteropServices;
using FishNet.Example.Authenticating;
using FishNet.Example.CustomSyncObject;
using FishNet.Example.Prediction.Rigidbodies;
using UnityEngine;

namespace FishNet.Serializing.Generated
{
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedWriters___Internal
	{
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeOnce()
		{
			GenericWriter<HostPasswordBroadcast>.Write = Write___FishNet_002EExample_002EAuthenticating_002EHostPasswordBroadcastFishNet_002ESerializing_002EGenerated;
			GenericWriter<PasswordBroadcast>.Write = Write___FishNet_002EExample_002EAuthenticating_002EPasswordBroadcastFishNet_002ESerializing_002EGenerated;
			GenericWriter<ResponseBroadcast>.Write = Write___FishNet_002EExample_002EAuthenticating_002EResponseBroadcastFishNet_002ESerializing_002EGenerated;
			GenericWriter<RigidbodyPrediction.MoveData>.Write = Write___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveDataFishNet_002ESerializing_002EGenerated;
			GenericWriter<RigidbodyPrediction.MoveData[]>.Write = Write___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveData_005B_005DFishNet_002ESerializing_002EGenerated;
			GenericWriter<RigidbodyPrediction.ReconcileData>.Write = Write___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FReconcileDataFishNet_002ESerializing_002EGenerated;
			GenericWriter<Structy>.Write = Write___FishNet_002EExample_002ECustomSyncObject_002EStructyFishNet_002ESerializing_002EGenerated;
		}

		public static void Write___FishNet_002EExample_002EAuthenticating_002EHostPasswordBroadcastFishNet_002ESerializing_002EGenerated(this Writer writer, HostPasswordBroadcast value)
		{
			writer.WriteString(value.Password);
		}

		public static void Write___FishNet_002EExample_002EAuthenticating_002EPasswordBroadcastFishNet_002ESerializing_002EGenerated(this Writer writer, PasswordBroadcast value)
		{
			writer.WriteString(value.Password);
		}

		public static void Write___FishNet_002EExample_002EAuthenticating_002EResponseBroadcastFishNet_002ESerializing_002EGenerated(this Writer writer, ResponseBroadcast value)
		{
			writer.WriteBoolean(value.Passed);
		}

		public static void Write___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveDataFishNet_002ESerializing_002EGenerated(this Writer writer, RigidbodyPrediction.MoveData value)
		{
			writer.WriteBoolean(value.Jump);
			writer.WriteSingle(value.Horizontal);
			writer.WriteSingle(value.Vertical);
		}

		public static void Write___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveData_005B_005DFishNet_002ESerializing_002EGenerated(this Writer writer, RigidbodyPrediction.MoveData[] value)
		{
			writer.WriteArray(value);
		}

		public static void Write___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FReconcileDataFishNet_002ESerializing_002EGenerated(this Writer writer, RigidbodyPrediction.ReconcileData value)
		{
			writer.WriteVector3(value.Position);
			writer.WriteQuaternion(value.Rotation);
			writer.WriteVector3(value.Velocity);
			writer.WriteVector3(value.AngularVelocity);
		}

		public static void Write___FishNet_002EExample_002ECustomSyncObject_002EStructyFishNet_002ESerializing_002EGenerated(this Writer writer, Structy value)
		{
			writer.WriteString(value.Name);
			writer.WriteUInt16(value.Age);
		}
	}
}
