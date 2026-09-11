using TMPro;
using UnityEngine;

namespace EasyTextEffects.Effects
{
	[CreateAssetMenu(fileName = "Move", menuName = "Easy Text Effects/2. Move", order = 2)]
	public class Effect_Move : TextEffectInstance
	{
		[Space(10f)]
		[Header("Move")]
		public Vector2 startOffset = -Vector2.up * 10f;

		public Vector2 endOffset;

		public override void ApplyEffect(TMP_TextInfo _textInfo, int _charIndex, int _startVertex = 0, int _endVertex = 3)
		{
			if (CheckCanApplyEffect(_charIndex))
			{
				TMP_CharacterInfo tMP_CharacterInfo = _textInfo.characterInfo[_charIndex];
				int materialReferenceIndex = tMP_CharacterInfo.materialReferenceIndex;
				Vector3[] vertices = _textInfo.meshInfo[materialReferenceIndex].vertices;
				for (int i = _startVertex; i <= _endVertex; i++)
				{
					int num = tMP_CharacterInfo.vertexIndex + i;
					Vector2 vector = Interpolate(startOffset, endOffset, _charIndex);
					vertices[num] += new Vector3(vector.x, vector.y, 0f);
				}
			}
		}
	}
}
