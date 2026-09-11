using TMPro;
using UnityEngine;

namespace EasyTextEffects.Effects
{
	[CreateAssetMenu(fileName = "Scale", menuName = "Easy Text Effects/4. Scale", order = 4)]
	public class Effect_Scale : TextEffectInstance
	{
		[Space(10f)]
		[Header("Scale")]
		public float startScale;

		public float endScale = 1f;

		public override void ApplyEffect(TMP_TextInfo _textInfo, int _charIndex, int _startVertex = 0, int _endVertex = 3)
		{
			if (CheckCanApplyEffect(_charIndex))
			{
				TMP_CharacterInfo charInfo = _textInfo.characterInfo[_charIndex];
				int materialReferenceIndex = charInfo.materialReferenceIndex;
				Vector3[] vertices = _textInfo.meshInfo[materialReferenceIndex].vertices;
				Vector3 vector = CharCenter(charInfo, vertices);
				float num = Interpolate(startScale, endScale, _charIndex);
				for (int i = _startVertex; i <= _endVertex; i++)
				{
					int num2 = charInfo.vertexIndex + i;
					Vector3 vector2 = vertices[num2] - vector;
					vertices[num2] = vector + vector2 * num;
				}
			}
		}
	}
}
