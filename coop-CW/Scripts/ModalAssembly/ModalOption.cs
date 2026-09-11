using System;

public class ModalOption
{
	public string Text;

	public Action OnClick;

	public ModalOption(string text, Action onClick = null)
	{
		Text = text;
		OnClick = onClick;
	}
}
