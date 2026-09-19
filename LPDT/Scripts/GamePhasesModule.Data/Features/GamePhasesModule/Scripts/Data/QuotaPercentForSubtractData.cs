using System;
using UnityEngine;

namespace Features.GamePhasesModule.Scripts.Data
{
	[Serializable]
	public class QuotaPercentForSubtractData
	{
		[Range(0f, 100f)]
		public float QuotaPercentForSubtract;

		public float SubtractTime;
	}
}
