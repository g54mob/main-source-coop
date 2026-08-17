using System;

namespace FishNet.Object.Prediction
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class ReconcileAttribute : Attribute
	{
		[Obsolete("Use PredictionManager.RedundancyCount.")]
		public byte Resends = 3;
	}
}
