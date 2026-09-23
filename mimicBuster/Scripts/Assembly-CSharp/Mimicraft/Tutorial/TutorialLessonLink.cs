using UnityEngine;
using UnityEngine.EventSystems;

namespace Mimicraft.Tutorial
{
	public class TutorialLessonLink : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Right && !(TutorialDirector.Instance == null))
			{
				TutorialLessonMenu.Toggle();
			}
		}
	}
}
