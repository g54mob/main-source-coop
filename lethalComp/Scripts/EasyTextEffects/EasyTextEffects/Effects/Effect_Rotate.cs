using TMPro;
using UnityEngine;

namespace EasyTextEffects.Effects
{
	[CreateAssetMenu(fileName = "Rotate", menuName = "Easy Text Effects/3. Rotate", order = 3)]
	public class Effect_Rotate : TextEffectInstance
	{
		[Space(10f)]
		[Header("Rotate")]
		public Vector2 centerOffset = Vector2.zero;

		[Range(-360f, 360f)]
		public float startAngle = 90f;

		[Range(-360f, 360f)]
		public float endAngle;

		public override void ApplyEffect(TMP_TextInfo _textInfo, int _charIndex, int _startVertex = 0, int _endVertex = 3)
		{
			if (CheckCanApplyEffect(_charIndex))
			{
				TMP_CharacterInfo charInfo = _textInfo.characterInfo[_charIndex];
				int materialReferenceIndex = charInfo.materialReferenceIndex;
				Vector3[] vertices = _textInfo.meshInfo[materialReferenceIndex].vertices;
				float z = Interpolate(startAngle, endAngle, _charIndex);
				Vector3 vector = CharCenter(charInfo, vertices) + new Vector3(centerOffset.x, centerOffset.y, 0f);
				for (int i = _startVertex; i <= _endVertex; i++)
				{
					int num = charInfo.vertexIndex + i;
					Vector3 vector2 = vertices[num] - vector;
					vertices[num] = vector + Quaternion.Euler(0f, 0f, z) * vector2;
				}
			}
		}
	}
}
