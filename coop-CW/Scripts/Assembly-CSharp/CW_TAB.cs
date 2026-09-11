using UnityEngine;
using Zorro.UI;
using Zorro.UI.Effects;

public class CW_TAB : TAB_Button
{
	public FadeInEffect FadeInEffect;

	public SFX_Instance hoverSound;

	public SFX_Instance clickSound;

	public override void OnHover()
	{
		base.OnHover();
		if (hoverSound != null)
		{
			hoverSound.Play();
		}
	}

	public override void ButtonClicked()
	{
		base.ButtonClicked();
		if (clickSound != null)
		{
			clickSound.Play();
		}
	}

	private void Update()
	{
		Color b = (base.Selected ? Color.black : Color.white);
		text.color = Color.Lerp(text.color, b, Time.unscaledDeltaTime * 7f);
		FadeInEffect.Time = Mathf.Lerp(FadeInEffect.Time, base.Selected ? 1f : 0f, Time.unscaledDeltaTime * 13f);
	}
}
