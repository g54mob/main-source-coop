using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHoldRelay : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	public Action OnDown;

	public Action OnUp;

	public void OnPointerDown(PointerEventData eventData)
	{
		OnDown?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		OnUp?.Invoke();
	}
}
