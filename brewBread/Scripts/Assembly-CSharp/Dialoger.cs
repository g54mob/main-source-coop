using System;
using UnityEngine;

public class Dialoger : MonoBehaviour
{
	[Flags]
	public enum DialogState
	{
		None = 0,
		DialogEnabled = 1,
		InDialog = 2,
		InRange = 4,
		ContextualPopUp = 8
	}

	private DialogState _state;

	public DialogState CurrentState
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
		}
	}
}
