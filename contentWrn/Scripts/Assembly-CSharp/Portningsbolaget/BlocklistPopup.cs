using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Portningsbolaget
{
	public class BlocklistPopup : MonoBehaviour
	{
		private static BlocklistPopup s_instance;

		public BlocklistTable m_table;

		public GameObject m_overlay;

		public GameObject m_scrollView;

		public Button m_toggleButton;

		public Button m_closeButton;

		public InputActionReference m_toggle;

		public InputActionReference m_close;

		public InputActionReference m_tabLeft;

		public InputActionReference m_tabRight;

		public static BlocklistPopup Instance => s_instance;

		public bool IsVisible => m_scrollView.activeSelf;

		private void Awake()
		{
			base.gameObject.SetActive(value: false);
			m_toggleButton.gameObject.SetActive(value: false);
		}

		private void Start()
		{
			Show(visible: false);
		}

		private void OnDisable()
		{
			Show(visible: false);
		}

		private void Update()
		{
			if (m_toggle.action.WasPressedThisFrame())
			{
				Toggle();
			}
			else if (m_close.action.WasPressedThisFrame())
			{
				StartCoroutine(ShowDelayed(visible: false));
			}
			else if (m_tabLeft.action.WasPerformedThisFrame() || m_tabRight.action.WasPerformedThisFrame())
			{
				Show(visible: false);
			}
		}

		private IEnumerator ShowDelayed(bool visible)
		{
			yield return null;
			Show(visible);
		}

		public void Toggle()
		{
			Show(!IsVisible);
		}

		public void Close()
		{
			Show(visible: false);
		}

		private void Show(bool visible)
		{
			if (IsVisible != visible)
			{
				Debug.Log((visible ? "Showing" : "Hiding") + " Blocklist");
				m_overlay.SetActive(visible);
				m_scrollView.SetActive(visible);
				if (visible)
				{
					m_table.SelectFirst();
				}
			}
		}
	}
}
