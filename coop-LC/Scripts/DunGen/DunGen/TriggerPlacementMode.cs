using UnityEngine;

namespace DunGen
{
	public enum TriggerPlacementMode
	{
		[InspectorName("None")]
		None = 0,
		[InspectorName("3D")]
		ThreeDimensional = 1,
		[InspectorName("2D")]
		TwoDimensional = 2
	}
}
