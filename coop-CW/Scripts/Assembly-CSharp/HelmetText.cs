using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class HelmetText : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_TextObject;

	private readonly float TIME_BETWEEN_CHARACTERS = 0.05f;

	private bool m_Writing;

	private Coroutine m_WriteCoroutine;

	public static HelmetText Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public void SetHelmetText(string text, float t)
	{
		if (text.Length > 100)
		{
			text = text.Substring(0, 100);
		}
		Debug.Log($"Showing Helmet Text: {text} for {t} seconds");
		if (m_Writing && m_WriteCoroutine != null)
		{
			StopCoroutine(m_WriteCoroutine);
			ClearText();
		}
		m_WriteCoroutine = StartCoroutine(InternalHelmetText(text, t));
	}

	private IEnumerator InternalHelmetText(string text, float time)
	{
		m_Writing = true;
		yield return new WaitForSecondsRealtime(1f);
		char[] array = text.ToArray();
		string written = string.Empty;
		char[] array2 = array;
		foreach (char c in array2)
		{
			written += c;
			m_TextObject.text = written;
			m_TextObject.SetAllDirty();
			yield return new WaitForSecondsRealtime(TIME_BETWEEN_CHARACTERS);
		}
		yield return new WaitForSecondsRealtime(time);
		ClearText();
	}

	private void ClearText()
	{
		m_TextObject.text = string.Empty;
		m_TextObject.SetAllDirty();
		m_Writing = false;
	}
}
