using TMPro;
using UnityEngine;

public class TextChangeSFX : MonoBehaviour
{
	public bool usePosition;

	public SFX_Instance textSound;

	private TextMeshProUGUI text;

	private TextMeshPro text2;

	private string prevText;

	private void Start()
	{
		if ((bool)GetComponent<TextMeshProUGUI>())
		{
			text = GetComponent<TextMeshProUGUI>();
		}
		if ((bool)GetComponent<TextMeshPro>())
		{
			text2 = GetComponent<TextMeshPro>();
		}
	}

	private void Update()
	{
		if ((bool)text2)
		{
			if (!usePosition && text2.text != prevText)
			{
				textSound.Play(Camera.main.transform.position);
			}
			if (usePosition && text2.text != prevText)
			{
				textSound.Play(base.transform.position);
			}
			prevText = text2.text;
		}
		if ((bool)text)
		{
			if (!usePosition && text.text != prevText)
			{
				textSound.Play(Camera.main.transform.position);
			}
			if (usePosition && text.text != prevText)
			{
				textSound.Play(base.transform.position);
			}
			prevText = text.text;
		}
	}
}
