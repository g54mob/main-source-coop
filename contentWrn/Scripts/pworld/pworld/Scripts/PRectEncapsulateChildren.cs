using UnityEngine;
using UnityEngine.UI;
using pworld.Scripts.Extensions;

namespace pworld.Scripts
{
	public class PRectEncapsulateChildren : MonoBehaviour, ILayoutSelfController, ILayoutController
	{
		public RectTransform parent;

		private RectTransform me;

		public RectTransform Me
		{
			get
			{
				if (me != null)
				{
					return me;
				}
				me = GetComponent<RectTransform>();
				return me;
			}
		}

		private void Awake()
		{
		}

		public void Encapuslate()
		{
			Me.EncapsulateChildrenOf(Me);
		}

		public void SetLayoutHorizontal()
		{
			Me.EncapsulateChildrenOf(Me);
		}

		public void SetLayoutVertical()
		{
			Me.EncapsulateChildrenOf(Me);
		}
	}
}
