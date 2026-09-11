using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zorro.ControllerSupport;
using Zorro.Core;
using Zorro.Settings;

public class HotbarSlotUI : MonoBehaviour
{
	public Image m_icon;

	public Sprite m_unkownIcon;

	public int hotbarIndex;

	public TextMeshProUGUI m_keybind;

	private InputScheme m_previousScheme;

	private CanvasGroup canvasGroup;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		InputSchemeChanged(InputHandler.GetCurrentUsedInputScheme());
	}

	public void SetData(InventorySlot slot, bool selected)
	{
		canvasGroup.alpha = (selected ? 1f : 0.4f);
		bool flag = slot.ItemInSlot.item != null;
		m_icon.enabled = flag;
		if (flag)
		{
			bool flag2 = slot.ItemInSlot.item.icon != null;
			m_icon.sprite = (flag2 ? slot.ItemInSlot.item.icon : m_unkownIcon);
		}
	}

	private void Update()
	{
		InputScheme currentUsedInputScheme = InputHandler.GetCurrentUsedInputScheme();
		if (m_previousScheme != currentUsedInputScheme)
		{
			m_previousScheme = currentUsedInputScheme;
			InputSchemeChanged(currentUsedInputScheme);
		}
	}

	private void InputSchemeChanged(InputScheme scheme)
	{
		if (scheme == InputScheme.Gamepad)
		{
			int glyphIndex = SingletonAsset<ControllerGlyphs>.Instance.GetGlyphIndex(GetGlyphType());
			m_keybind.text = ControllerGlyphs.GetSprite(glyphIndex);
			return;
		}
		string text = GetKeycodeSetting().Keycode().ToString();
		for (int i = 1; i <= 3; i++)
		{
			if (text.Contains(i.ToString()))
			{
				text = i.ToString();
			}
		}
		m_keybind.text = text;
		ControllerGlyphs.GlyphType GetGlyphType()
		{
			return hotbarIndex switch
			{
				0 => ControllerGlyphs.GlyphType.SelectItem1, 
				1 => ControllerGlyphs.GlyphType.SelectItem2, 
				2 => ControllerGlyphs.GlyphType.SelectItem3, 
				_ => ControllerGlyphs.GlyphType.Interact, 
			};
		}
		KeyCodeSetting GetKeycodeSetting()
		{
			return hotbarIndex switch
			{
				0 => GameHandler.Instance.SettingsHandler.GetSetting<SelectItem1KeybindSetting>(), 
				1 => GameHandler.Instance.SettingsHandler.GetSetting<SelectItem2KeybindSetting>(), 
				2 => GameHandler.Instance.SettingsHandler.GetSetting<SelectItem3KeybindSetting>(), 
				_ => null, 
			};
		}
	}
}
