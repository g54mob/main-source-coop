using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.UI.Effects;

public class EscapeMenuButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
	[SerializeField]
	private FadeInEffect m_effect01;

	[SerializeField]
	private FadeInEffect m_effect02;

	public SFX_Instance hoverSound;

	public SFX_Instance clickSound;

	public Graphic[] m_graphics;

	private bool m_selected;

	private void Start()
	{
		Button component = GetComponent<Button>();
		if (component != null)
		{
			component.onClick.AddListener(ButtonClicked);
		}
	}

	private void ButtonClicked()
	{
		if (clickSound != null)
		{
			clickSound.Play();
		}
	}

	public void OnSelected()
	{
		m_selected = true;
		Graphic[] graphics = m_graphics;
		for (int i = 0; i < graphics.Length; i++)
		{
			graphics[i].color = SingletonAsset<UIColorDatabase>.Instance.BlackColor;
		}
		if (hoverSound != null)
		{
			hoverSound.Play();
		}
	}

	public void OnDeselect()
	{
		m_selected = false;
		Graphic[] graphics = m_graphics;
		for (int i = 0; i < graphics.Length; i++)
		{
			graphics[i].color = SingletonAsset<UIColorDatabase>.Instance.WhiteColor;
		}
	}

	private void OnDisable()
	{
		OnDeselect();
		m_effect01.Time = 0f;
		m_effect02.Time = 0f;
	}

	private void Update()
	{
		float b = (m_selected ? 1f : 0f);
		float num = (m_selected ? 1f : 10f);
		m_effect01.Time = Mathf.Lerp(m_effect01.Time, b, Time.unscaledDeltaTime * 15f * num);
		m_effect02.Time = Mathf.Lerp(m_effect02.Time, b, Time.unscaledDeltaTime * 8f * num);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		OnSelected();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		OnDeselect();
	}

	public void OnSelect(BaseEventData eventData)
	{
		if (InputHandler.GetCurrentUsedInputScheme() != InputScheme.KeyboardMouse)
		{
			OnSelected();
		}
	}

	public void OnDeselect(BaseEventData eventData)
	{
		Debug.Log("Deselecting " + base.gameObject.name, base.gameObject);
		OnDeselect();
	}
}
