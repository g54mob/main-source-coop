using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;
using Zorro.ControllerSupport;
using Zorro.Core;

[Serializable]
public class ItemKeyTooltip : IHaveUIData
{
	[FormerlySerializedAs("m_key")]
	public string m_text;

	public List<ControllerGlyphs.GlyphType> GlpyhType;

	private IMKbPromptProvider promptProvider;

	private InputScheme m_currentInputType = InputScheme.Unknown;

	private string m_info;

	public ItemKeyTooltip(string text, IMKbPromptProvider promptProvider, List<ControllerGlyphs.GlyphType> glpyhType)
	{
		m_text = text.Replace("[{key}]", "{key}");
		this.promptProvider = promptProvider;
		GlpyhType = glpyhType;
	}

	public string GetString()
	{
		if (promptProvider == null)
		{
			return m_text;
		}
		if (GlpyhType != null)
		{
			if (string.IsNullOrEmpty(m_info) || m_currentInputType != InputHandler.GetCurrentUsedInputScheme())
			{
				m_currentInputType = InputHandler.GetCurrentUsedInputScheme();
				m_info = m_text;
				if (m_currentInputType == InputScheme.Gamepad)
				{
					m_info = m_info.Replace("{key}", string.Join(" ", GlpyhType.Select((ControllerGlyphs.GlyphType type) => SingletonAsset<ControllerGlyphs>.Instance.GetGlyphText(promptProvider, type))));
				}
				else
				{
					m_info = m_info.Replace("{key}", SingletonAsset<ControllerGlyphs>.Instance.GetGlyphText(promptProvider, GlpyhType.First()));
				}
			}
			return m_info;
		}
		return m_text;
	}

	public void UpdateLocale()
	{
	}
}
