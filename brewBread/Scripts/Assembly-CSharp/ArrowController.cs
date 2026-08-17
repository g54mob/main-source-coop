using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowController : MonoBehaviour
{
	public EventSystem events;

	public int childCount;

	private GameObject currentSelected;

	private void Start()
	{
		currentSelected = events.firstSelectedGameObject;
		base.transform.position = currentSelected.transform.GetChild(childCount).transform.position;
	}

	private void Update()
	{
		if (events.currentSelectedGameObject != currentSelected && events.currentSelectedGameObject != null)
		{
			currentSelected = events.currentSelectedGameObject;
			base.transform.position = currentSelected.transform.GetChild(childCount).transform.position;
		}
	}
}
