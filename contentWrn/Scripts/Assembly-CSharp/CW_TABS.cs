using UnityEngine;
using UnityEngine.InputSystem;
using Zorro.UI;

public class CW_TABS : TABS<CW_TAB>
{
	public InputActionReference tabLeft;

	public InputActionReference tabRight;

	public GameObject keyboardMouseTab;

	private void Awake()
	{
	}

	public override void OnSelected(CW_TAB button)
	{
	}

	private void Update()
	{
		if (tabLeft.action.WasPerformedThisFrame())
		{
			SelectPrevious();
		}
		else if (tabRight.action.WasPerformedThisFrame())
		{
			SelectNext();
		}
	}
}
