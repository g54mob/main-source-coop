using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace solidocean
{
	public class UIAlwaysSelect : MonoBehaviour
	{
		private EventSystem currentEventSystem;

		private GameObject currentlySelected;

		private void Awake()
		{
		}

		private void Start()
		{
			currentEventSystem = EventSystem.current;
			currentlySelected = currentEventSystem.currentSelectedGameObject;
		}

		private void Update()
		{
			if (currentEventSystem.currentSelectedGameObject != null && currentlySelected != currentEventSystem.currentSelectedGameObject)
			{
				currentlySelected = currentEventSystem.currentSelectedGameObject;
			}
			if (currentEventSystem.currentSelectedGameObject == null)
			{
				if (currentlySelected != null)
				{
					currentlySelected.GetComponent<Selectable>().Select();
					return;
				}
				currentlySelected = currentEventSystem.firstSelectedGameObject;
				currentlySelected.GetComponent<Selectable>().Select();
			}
		}
	}
}
