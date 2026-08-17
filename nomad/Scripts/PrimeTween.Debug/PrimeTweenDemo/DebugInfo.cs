using UnityEngine;
using UnityEngine.UI;

namespace PrimeTweenDemo
{
	public class DebugInfo : MonoBehaviour
	{
		[SerializeField]
		private MeasureMemoryAllocations measureMemoryAllocations;

		[SerializeField]
		private Text tweensCountText;

		[SerializeField]
		private Text gcAllocText;
	}
}
