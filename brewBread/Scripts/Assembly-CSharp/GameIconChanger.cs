using UnityEngine;
using UnityEngine.InputSystem;

public class GameIconChanger : MonoBehaviour
{
	[SerializeField]
	private TutorialIcon[] _icons;

	private void OnEnable()
	{
		TutorialIcon[] icons;
		if ((bool)Object.FindObjectOfType<Spawner>())
		{
			icons = _icons;
			for (int i = 0; i < icons.Length; i++)
			{
				TutorialIcon tutorialIcon = icons[i];
				if (tutorialIcon.player == 1)
				{
					if (tutorialIcon.sprite != null)
					{
						if (StaticInstance<DataBetweenScenes>.Instance.player1Input == Keyboard.current)
						{
							tutorialIcon.sprite.sprite = tutorialIcon.KeyboardSprite;
						}
						else
						{
							tutorialIcon.sprite.sprite = tutorialIcon.ControllerSprite;
						}
					}
					else if (StaticInstance<DataBetweenScenes>.Instance.player1Input == Keyboard.current)
					{
						tutorialIcon.image.sprite = tutorialIcon.KeyboardSprite;
					}
					else
					{
						tutorialIcon.image.sprite = tutorialIcon.ControllerSprite;
					}
				}
				else if (StaticInstance<DataBetweenScenes>.Instance.player2Input == Keyboard.current)
				{
					tutorialIcon.sprite.sprite = tutorialIcon.KeyboardSprite;
				}
				else
				{
					tutorialIcon.sprite.sprite = tutorialIcon.ControllerSprite;
				}
			}
			return;
		}
		DataBetweenScenes dataBetweenScenes = Object.FindObjectOfType<DataBetweenScenes>();
		icons = _icons;
		for (int i = 0; i < icons.Length; i++)
		{
			TutorialIcon tutorialIcon2 = icons[i];
			if (tutorialIcon2.player == 1)
			{
				if (dataBetweenScenes.player1Input == Keyboard.current)
				{
					tutorialIcon2.sprite.sprite = tutorialIcon2.KeyboardSprite;
				}
				else
				{
					tutorialIcon2.sprite.sprite = tutorialIcon2.ControllerSprite;
				}
			}
			else if (dataBetweenScenes.player2Input == Keyboard.current)
			{
				tutorialIcon2.sprite.sprite = tutorialIcon2.KeyboardSprite;
			}
			else
			{
				tutorialIcon2.sprite.sprite = tutorialIcon2.ControllerSprite;
			}
		}
	}
}
