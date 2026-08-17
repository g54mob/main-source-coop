using System;
using TMPro;
using UnityEngine;

[Serializable]
public class TextColorAnimation : TextAnimationBase
{
	[SerializeField]
	private Color color;

	public override void AnimationUpdate(TMP_Text text, string animatedText)
	{
		base.AnimationUpdate(text, animatedText);
		TMP_TextInfo textInfo = text.textInfo;
		for (int i = text.text.IndexOf(animatedText); i < text.text.IndexOf(animatedText) + animatedText.Length; i++)
		{
			if (i < textInfo.characterInfo.Length)
			{
				TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[i];
				color.a = 0f;
				tMP_CharacterInfo.color = color;
				int materialReferenceIndex = tMP_CharacterInfo.materialReferenceIndex;
				int vertexIndex = tMP_CharacterInfo.vertexIndex;
				Color32[] colors = textInfo.meshInfo[materialReferenceIndex].colors32;
				if (colors != null)
				{
					colors[vertexIndex] = color;
					colors[vertexIndex + 1] = color;
					colors[vertexIndex + 2] = color;
					colors[vertexIndex + 3] = color;
				}
			}
		}
		text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
	}
}
