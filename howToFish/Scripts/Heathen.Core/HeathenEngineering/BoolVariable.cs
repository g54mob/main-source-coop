using UnityEngine;

namespace HeathenEngineering
{
	[CreateAssetMenu(menuName = "System Core/Variables/Serializable/Values/Bool")]
	public class BoolVariable : DataVariable<bool>
	{
		public void ToggleValue()
		{
			base.Value = !base.Value;
		}
	}
}
