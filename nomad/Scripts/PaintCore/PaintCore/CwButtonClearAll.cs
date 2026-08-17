using UnityEngine;
using UnityEngine.EventSystems;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwButtonClearAll")]
	[AddComponentMenu("CW/Paint Core/CW Button Clear All")]
	public class CwButtonClearAll : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		[SerializeField]
		private bool clearStates = true;

		public bool ClearStates
		{
			get
			{
				return clearStates;
			}
			set
			{
				clearStates = value;
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			ClearAll();
		}

		[ContextMenu("Clear All")]
		public void ClearAll()
		{
			foreach (CwPaintableTexture instance in CwPaintableTexture.Instances)
			{
				instance.Clear();
				if (clearStates)
				{
					instance.ClearStates();
				}
			}
		}
	}
}
