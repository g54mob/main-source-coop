using UnityEngine;
using UnityEngine.EventSystems;

namespace Zorro.ControllerSupport
{
	public class NavigationInfoHandler : RetrievableSingleton<NavigationInfoHandler>
	{
		private GameObject m_currentlySelectedGameObject;

		protected override void OnCreated()
		{
			base.OnCreated();
			Object.DontDestroyOnLoad(base.gameObject);
			Debug.Log("Initialized NavigationHandler");
		}

		public void RegisterPage()
		{
		}

		private void Update()
		{
			if (!(EventSystem.current == null) && EventSystem.current.currentSelectedGameObject != m_currentlySelectedGameObject)
			{
				m_currentlySelectedGameObject = EventSystem.current.currentSelectedGameObject;
				if (m_currentlySelectedGameObject != null)
				{
					Debug.Log("Selection changed to: " + m_currentlySelectedGameObject.name, m_currentlySelectedGameObject);
				}
				else
				{
					Debug.Log("Selection changed to: null");
				}
			}
		}
	}
}
