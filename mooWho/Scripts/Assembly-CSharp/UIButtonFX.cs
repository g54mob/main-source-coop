using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonFX : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	private MenuManager _menu;

	private float _hoverScale = 1.06f;

	private float _clickScale = 0.94f;

	private float _speed = 12f;

	private Vector3 _baseScale;

	private Vector3 _targetScale;

	private Button _button;

	private bool _pointerInside;

	private bool Interactable
	{
		get
		{
			if (!(_button == null))
			{
				return _button.interactable;
			}
			return true;
		}
	}

	public void Configure(MenuManager menu, float hoverScale, float clickScale, float speed)
	{
		_menu = menu;
		_hoverScale = hoverScale;
		_clickScale = clickScale;
		_speed = speed;
	}

	private void Awake()
	{
		_button = GetComponent<Button>();
		_baseScale = base.transform.localScale;
		_targetScale = _baseScale;
	}

	private void OnEnable()
	{
		base.transform.localScale = _baseScale;
		_targetScale = _baseScale;
		_pointerInside = false;
	}

	private void Update()
	{
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, _targetScale, Time.unscaledDeltaTime * _speed);
	}

	public void OnPointerEnter(PointerEventData e)
	{
		if (Interactable)
		{
			_pointerInside = true;
			_targetScale = _baseScale * _hoverScale;
			_menu?.PlayHoverSound();
		}
	}

	public void OnPointerExit(PointerEventData e)
	{
		_pointerInside = false;
		_targetScale = _baseScale;
	}

	public void OnPointerDown(PointerEventData e)
	{
		if (Interactable)
		{
			_targetScale = _baseScale * _clickScale;
			_menu?.PlayClickSound();
		}
	}

	public void OnPointerUp(PointerEventData e)
	{
		if (Interactable)
		{
			_targetScale = (_pointerInside ? (_baseScale * _hoverScale) : _baseScale);
		}
	}
}
