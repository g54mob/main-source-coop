using System;
using TMPro;
using UnityEngine;

[Serializable]
public class TextAppearAnimation : TextAnimationBase
{
	[HideInInspector]
	public bool appear = true;

	private bool Auxappear = true;

	private byte alpha = byte.MaxValue;

	private int count;

	private int auxCount;

	private float timer;

	public override void AnimationUpdate(TMP_Text text, string animatedText)
	{
		base.AnimationUpdate(text, animatedText);
		float num = 34f;
		TMP_TextInfo textInfo = text.textInfo;
		for (int i = 0; i <= auxCount; i++)
		{
			if (i >= textInfo.characterInfo.Length)
			{
				continue;
			}
			TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[i];
			if (tMP_CharacterInfo.character != ' ')
			{
				if (i < count)
				{
					alpha = byte.MaxValue;
				}
				else
				{
					alpha = 0;
				}
				tMP_CharacterInfo.color.a = alpha;
				int materialReferenceIndex = tMP_CharacterInfo.materialReferenceIndex;
				int vertexIndex = tMP_CharacterInfo.vertexIndex;
				Color32[] colors = textInfo.meshInfo[materialReferenceIndex].colors32;
				if (colors != null)
				{
					colors[vertexIndex].a = alpha;
					colors[vertexIndex + 1].a = alpha;
					colors[vertexIndex + 2].a = alpha;
					colors[vertexIndex + 3].a = alpha;
				}
			}
		}
		if (count < text.text.Length && timer > num && appear)
		{
			count++;
			auxCount = count;
			timer = 0f;
		}
		if (count == text.text.Length)
		{
			auxCount = count;
		}
		if (count > 0 && timer > num && !appear)
		{
			count--;
			timer = 0f;
		}
		if (Auxappear != appear)
		{
			Auxappear = appear;
			timer = 0f;
		}
		timer += Time.deltaTime;
		text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
	}
}
