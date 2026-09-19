using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
	[SerializeField]
	private string key;

	private TMP_Text _text;

	public string Key
	{
		get
		{
			return key;
		}
		set
		{
			key = value;
			Refresh();
		}
	}

	private void Awake()
	{
		_text = GetComponent<TMP_Text>();
	}

	private void OnEnable()
	{
		Localization.OnLanguageChanged += Refresh;
		Refresh();
	}

	private void OnDisable()
	{
		Localization.OnLanguageChanged -= Refresh;
	}

	private void Refresh()
	{
		if (_text == null)
		{
			_text = GetComponent<TMP_Text>();
		}
		if (!(_text == null) && !string.IsNullOrEmpty(key))
		{
			_text.text = Localization.Get(key);
		}
	}
}
