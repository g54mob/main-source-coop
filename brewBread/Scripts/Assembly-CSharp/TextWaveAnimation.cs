using System;
using TMPro;
using UnityEngine;

[Serializable]
public class TextWaveAnimation : TextAnimationBase
{
	[SerializeField]
	private float speed;

	[SerializeField]
	private float amplitude;

	[SerializeField]
	private float frequency;

	public override void AnimationUpdate(TMP_Text text, string animatedText)
	{
		base.AnimationUpdate(text, animatedText);
		TMP_TextInfo textInfo = text.textInfo;
		text.ForceMeshUpdate();
		for (int i = text.text.IndexOf(animatedText); i < text.text.IndexOf(animatedText) + animatedText.Length; i++)
		{
			TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[i];
			if (tMP_CharacterInfo.isVisible)
			{
				Vector3[] vertices = textInfo.meshInfo[tMP_CharacterInfo.materialReferenceIndex].vertices;
				for (int j = 0; j < 4; j++)
				{
					Vector3 vector = vertices[tMP_CharacterInfo.vertexIndex + j];
					vertices[tMP_CharacterInfo.vertexIndex + j] = vector + new Vector3(0f, Mathf.Sin(Time.time * speed + vector.x * frequency) * amplitude, 0f);
				}
			}
		}
		for (int k = 0; k < textInfo.meshInfo.Length; k++)
		{
			TMP_MeshInfo tMP_MeshInfo = textInfo.meshInfo[k];
			tMP_MeshInfo.mesh.vertices = tMP_MeshInfo.vertices;
			text.UpdateGeometry(tMP_MeshInfo.mesh, k);
		}
	}
}
