using TMPro;
using UnityEngine;

public class UI_MetaCoin : MonoBehaviour
{
	private TextMeshProUGUI text;

	private int m_metaCoin = -1;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		if (m_metaCoin != RetrievableSingleton<MetaProgressionHandler>.Instance.MetaCoins)
		{
			m_metaCoin = RetrievableSingleton<MetaProgressionHandler>.Instance.MetaCoins;
			text.text = m_metaCoin + " MC";
		}
	}
}
