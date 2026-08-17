using System.Runtime.InteropServices;
using FishNet.Example.Prediction.Rigidbodies;
using FishNet.Serializing.Helping;
using UnityEngine;

namespace FishNet.Serializing.Generated
{
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedComparers___Internal
	{
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeOnce()
		{
			GeneratedComparer<RigidbodyPrediction.MoveData>.IsDefault = IsDefault___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveData;
			GeneratedComparer<RigidbodyPrediction.MoveData>.Compare = Comparer___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveData;
			GeneratedComparer<float>.Compare = Comparer___System_002ESingle;
			GeneratedComparer<bool>.Compare = Comparer___System_002EBoolean;
		}

		public static bool Comparer___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveData(RigidbodyPrediction.MoveData value0, RigidbodyPrediction.MoveData value1)
		{
			if (value0.Jump == value1.Jump && value0.Horizontal == value1.Horizontal && value0.Vertical == value1.Vertical)
			{
				return true;
			}
			return false;
		}

		public static bool Comparer___System_002EBoolean(bool value0, bool value1)
		{
			return value0 == value1;
		}

		public static bool Comparer___System_002ESingle(float value0, float value1)
		{
			return value0 == value1;
		}

		public static bool IsDefault___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveData(RigidbodyPrediction.MoveData value0)
		{
			return Comparer___FishNet_002EExample_002EPrediction_002ERigidbodies_002ERigidbodyPrediction_002FMoveData(value0, default(RigidbodyPrediction.MoveData));
		}
	}
}
