using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectPreviousButton : MonoBehaviour
{
	[SerializeField]
	private EventSystem _eventSystem;

	[SerializeField]
	private GameObject[] _previousButtons;

	private GameObject _previousButton;

	private void Start()
	{
		_previousButton = _previousButtons[0];
	}

	public void UpdatedLastSelected(BaseEventData eventData)
	{
		GameObject[] previousButtons = _previousButtons;
		foreach (GameObject gameObject in previousButtons)
		{
			if (gameObject == eventData.selectedObject)
			{
				_previousButton = gameObject;
				break;
			}
		}
	}

	public void SelectLastButton()
	{
		StartCoroutine(SelectLastButtonDelay());
	}

	private IEnumerator SelectLastButtonDelay()
	{
		yield return new WaitForEndOfFrame();
		_eventSystem.SetSelectedGameObject(_previousButton);
	}
}
