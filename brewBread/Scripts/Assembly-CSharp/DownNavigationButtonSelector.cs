using UnityEngine;
using UnityEngine.UI;

public class DownNavigationButtonSelector : MonoBehaviour
{
	[SerializeField]
	private Button _currentButton;

	[SerializeField]
	private Button[] _buttonsOptions;

	private void Update()
	{
		if (_currentButton == null)
		{
			return;
		}
		Navigation navigation = _currentButton.navigation;
		Button[] buttonsOptions = _buttonsOptions;
		foreach (Button button in buttonsOptions)
		{
			if (button.isActiveAndEnabled)
			{
				navigation.selectOnDown = button;
				break;
			}
		}
		_currentButton.navigation = navigation;
	}
}
