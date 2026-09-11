using TMPro;
using UnityEngine;
using Zorro.Core;

public class InteractionUI : MonoBehaviour
{
	public CanvasGroup m_canvasGroup;

	public TextMeshProUGUI m_text;

	public TextMeshProUGUI m_interactText;

	private Interactable m_currentInteractable;

	private InteractKeybindSetting m_interactKeybindSetting;

	public void SetData(Interactable interactable)
	{
		if (m_currentInteractable != interactable || m_currentInteractable == null)
		{
			m_canvasGroup.alpha = ((interactable != null) ? 1 : 0);
			m_text.text = ((interactable != null) ? interactable.hoverText : "");
			m_currentInteractable = interactable;
			base.gameObject.SetActive(interactable != null);
		}
	}

	private void Start()
	{
		m_interactKeybindSetting = GameHandler.Instance.SettingsHandler.GetSetting<InteractKeybindSetting>();
	}

	private void Update()
	{
		m_interactText.text = SingletonAsset<ControllerGlyphs>.Instance.GetGlyphText(m_interactKeybindSetting, ControllerGlyphs.GlyphType.Interact);
	}
}
