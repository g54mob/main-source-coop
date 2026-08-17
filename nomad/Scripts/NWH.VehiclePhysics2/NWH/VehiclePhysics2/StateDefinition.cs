using System;

namespace NWH.VehiclePhysics2
{
	[Serializable]
	public class StateDefinition
	{
		public string fullName;

		public bool isEnabled;

		public bool initialized;

		public int lodIndex = -1;

		public StateDefinition()
		{
		}

		public StateDefinition(string fullName, bool isEnabled, int lod)
		{
			this.fullName = fullName;
			this.isEnabled = isEnabled;
			lodIndex = lod;
		}
	}
}
