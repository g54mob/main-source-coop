using System;
using TMPro;
using UnityEngine;

[Serializable]
public class TextShakeAnimation : TextAnimationBase
{
	[SerializeField]
	private float shakeAmount;

	public override void AnimationUpdate(TMP_Text text, string animatedText)
	{
		base.AnimationUpdate(text, animatedText);
		text.ForceMeshUpdate();
		TMP_TextInfo textInfo = text.textInfo;
		for (int i = text.text.IndexOf(animatedText); i < text.text.IndexOf(animatedText) + animatedText.Length; i++)
		{
			TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[i];
			if (tMP_CharacterInfo.isVisible)
			{
				Vector3[] vertices = textInfo.meshInfo[tMP_CharacterInfo.materialReferenceIndex].vertices;
				for (int j = 0; j < 4; j++)
				{
					Vector3 vector = vertices[tMP_CharacterInfo.vertexIndex + j];
					vertices[tMP_CharacterInfo.vertexIndex + j] = vector + UnityEngine.Random.insideUnitSphere * shakeAmount;
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
