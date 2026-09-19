using UnityEngine;

namespace solidocean
{
	public class ItemController : MonoBehaviour
	{
		public enum itemTypes
		{
			Button = 0,
			Slider = 1,
			HorizontalSelector = 2,
			Toggle = 3
		}

		[Header("Item Type")]
		public itemTypes itemType;
	}
}
