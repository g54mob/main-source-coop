using UnityEngine;
using UnityEngine.InputSystem;
using Zorro.UI;

namespace Zorro.ControllerSupport
{
	public class TABS_Navigation : MonoBehaviour
	{
		public InputActionReference RightAction;

		public InputActionReference LeftAction;

		private ITABS m_tabs;

		private void Start()
		{
			m_tabs = GetComponent<ITABS>();
		}

		private void Update()
		{
			if (RightAction.action.WasPressedThisFrame())
			{
				m_tabs.SelectNext();
			}
			else if (LeftAction.action.WasPressedThisFrame())
			{
				m_tabs.SelectPrevious();
			}
		}
	}
}
