using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Sound : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler
{
	public SFX_Instance hoverSound;

	public SFX_Instance clickSound;

	public void OnHover()
	{
		if (hoverSound != null)
		{
			hoverSound.Play();
		}
	}

	public void ButtonClicked()
	{
		if (clickSound != null)
		{
			clickSound.Play();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		OnHover();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		ButtonClicked();
	}
}
