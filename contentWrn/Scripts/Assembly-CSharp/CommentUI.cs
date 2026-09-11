using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class CommentUI : MonoBehaviour
{
	public TextMeshProUGUI m_faceText;

	public Graphic m_faceColor;

	public TextMeshProUGUI m_text;

	public TextMeshProUGUI m_likes;

	private CanvasGroup m_canvasGroup;

	private Vector2 m_originaAnchorPos;

	private float m_movedDown;

	private float m_alpha;

	private int targetLikes;

	private static Dictionary<LocaleIdentifier, StringTable> s_tables;

	private void Awake()
	{
		if (s_tables != null)
		{
			return;
		}
		StringTable[] array = Resources.LoadAll<StringTable>("Comments");
		if (array.Length == 0)
		{
			Debug.LogError("Could not load Comments localization");
			return;
		}
		s_tables = new Dictionary<LocaleIdentifier, StringTable>();
		foreach (StringTable stringTable in array)
		{
			s_tables.Add(stringTable.LocaleIdentifier, stringTable);
		}
	}

	public void Setup(Comment comment)
	{
		m_text.text = GetLocalizedText(comment);
		targetLikes = comment.Likes;
		m_likes.text = BigNumbers.ViewsToString(targetLikes);
		string face = FaceDatabase.GetFace(comment.Face);
		Color color = FaceDatabase.GetColor(comment.FaceColor);
		m_faceText.text = face;
		m_faceColor.color = color;
		m_originaAnchorPos = GetComponent<RectTransform>().anchoredPosition;
		m_canvasGroup = GetComponent<CanvasGroup>();
		m_canvasGroup.alpha = 0f;
	}

	private string GetLocalizedText(Comment comment)
	{
		StringTable value = null;
		Locale selectedLocale = LocalizationSettings.SelectedLocale;
		if (!s_tables.TryGetValue(selectedLocale.Identifier, out value))
		{
			selectedLocale = LocalizationSettings.ProjectLocale;
			s_tables.TryGetValue(selectedLocale.Identifier, out value);
		}
		if (value != null)
		{
			StringTableEntry entry = value.GetEntry(comment.Text);
			if (entry != null && !string.IsNullOrEmpty(entry.Value))
			{
				if (!comment.Detailed)
				{
					return entry.Value;
				}
				return entry.Value.Replace("<playername>", comment.Player);
			}
		}
		Debug.LogError("Could not localize comment: " + comment.Text);
		return "LOCALIZATION ERROR";
	}

	public void Move(float delta)
	{
		m_movedDown += delta;
	}

	private void Update()
	{
		m_alpha = Mathf.Lerp(m_alpha, 1f, Time.deltaTime * 10f);
		RectTransform component = GetComponent<RectTransform>();
		component.anchoredPosition = Vector2.Lerp(b: m_originaAnchorPos + new Vector2(0f, m_movedDown), a: component.anchoredPosition, t: Time.deltaTime * 10f);
		m_canvasGroup.alpha = m_alpha;
	}
}
