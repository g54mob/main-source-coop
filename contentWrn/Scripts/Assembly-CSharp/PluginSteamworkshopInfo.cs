using TMPro;
using UnityEngine;

public class PluginSteamworkshopInfo : MonoBehaviour
{
	public TextMeshProUGUI m_text;

	public void Show(string message)
	{
		base.gameObject.SetActive(value: true);
		m_text.text = message;
	}
}
