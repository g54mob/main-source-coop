using TMPro;
using UnityEngine;
using Zorro.Core;

public abstract class DivingBellState : StateMachineState
{
	public static Color GREEN => new Color(0.15f, 1f, 0.15f);

	public static Color RED => new Color(1f, 0.15f, 0.15f);

	public abstract void SetStatusText(TextMeshProUGUI statusText);
}
