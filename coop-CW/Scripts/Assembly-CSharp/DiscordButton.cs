using UnityEngine;
using UnityEngine.UI;

public class DiscordButton : MonoBehaviour
{
	public string m_discordLink;

	private void Awake()
	{
	}

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
		Debug.Log("Opening Discord Link: " + m_discordLink);
		Application.OpenURL(m_discordLink);
	}
}
