using System;
using UnityEngine;
using Zorro.Core;

namespace Zorro.UI.Modal
{
	public class Modal : RetrievableResourceSingleton<Modal>
	{
		public Transform headerParent;

		public Transform bodyParent;

		private CanvasGroup m_canvasGroup;

		private Action m_onClose;

		public bool Visible => m_canvasGroup.blocksRaycasts;

		protected override void OnCreated()
		{
			base.OnCreated();
			m_canvasGroup = base.gameObject.GetComponentInChildren<CanvasGroup>();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public static void OpenModal(HeaderModalOption headerContent, ModalContentOption bodyContentOption, Action onClose = null)
		{
			Modal instance = RetrievableResourceSingleton<Modal>.Instance;
			instance.m_onClose = onClose;
			instance.Open(headerContent, bodyContentOption);
		}

		public static void CloseModal()
		{
			Modal instance = RetrievableResourceSingleton<Modal>.Instance;
			instance.m_canvasGroup.blocksRaycasts = false;
			instance.m_canvasGroup.interactable = false;
			instance.m_onClose?.Invoke();
		}

		private void Open(HeaderModalOption headerContent, ModalContentOption bodyContentOption)
		{
			if (!Visible)
			{
				m_canvasGroup.blocksRaycasts = true;
				m_canvasGroup.interactable = true;
			}
			headerParent.ClearChildren();
			bodyParent.ClearChildren();
			headerContent.Setup(headerParent);
			bodyContentOption.Setup(bodyParent);
		}

		private void Update()
		{
			float b = (Visible ? 1 : 0);
			m_canvasGroup.alpha = Mathf.Lerp(m_canvasGroup.alpha, b, Time.unscaledDeltaTime * 20f);
		}
	}
}
