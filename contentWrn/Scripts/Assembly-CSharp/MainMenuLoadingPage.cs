using TMPro;

public class MainMenuLoadingPage : MainMenuPage
{
	public TextMeshProUGUI TitleText;

	public void SetText(string text)
	{
		TitleText.text = text;
	}
}
