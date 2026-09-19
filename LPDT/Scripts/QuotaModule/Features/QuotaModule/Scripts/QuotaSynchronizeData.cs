using System;

namespace Features.QuotaModule.Scripts
{
	[Serializable]
	public struct QuotaSynchronizeData
	{
		public float QuotaToChange;

		public QuotaOperation Operation;
	}
}
