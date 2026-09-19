using System.Collections;
using UnityEngine;

public class LobbyAreaNotification : MonoBehaviour
{
	public float visibleY = 50f;

	public float hiddenY = -250f;

	public float slideSpeed = 8f;

	public float showDuration = 7f;

	private RectTransform _rect;

	private float _targetY;

	private Coroutine _hideRoutine;

	private void Awake()
	{
		_rect = GetComponent<RectTransform>();
		_targetY = hiddenY;
		if (_rect != null)
		{
			Vector2 anchoredPosition = _rect.anchoredPosition;
			anchoredPosition.y = hiddenY;
			_rect.anchoredPosition = anchoredPosition;
		}
	}

	private void Update()
	{
		if (!(_rect == null))
		{
			Vector2 anchoredPosition = _rect.anchoredPosition;
			anchoredPosition.y = Mathf.Lerp(anchoredPosition.y, _targetY, Time.deltaTime * slideSpeed);
			_rect.anchoredPosition = anchoredPosition;
		}
	}

	public void NotifyEnter()
	{
		_targetY = visibleY;
		if (_hideRoutine != null)
		{
			StopCoroutine(_hideRoutine);
		}
		_hideRoutine = StartCoroutine(HideAfterDelay());
	}

	public void NotifyExit()
	{
		if (_hideRoutine != null)
		{
			StopCoroutine(_hideRoutine);
			_hideRoutine = null;
		}
		_targetY = hiddenY;
	}

	private IEnumerator HideAfterDelay()
	{
		yield return new WaitForSeconds(showDuration);
		_targetY = hiddenY;
		_hideRoutine = null;
	}
}
