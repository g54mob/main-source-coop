using EasyTextEffects.Editor.EditorDocumentation;
using EasyTextEffects.Editor.MyBoxCopy.Attributes;
using TMPro;
using UnityEngine;

namespace EasyTextEffects.Effects
{
	[CreateAssetMenu(fileName = "Color", menuName = "Easy Text Effects/1. Color", order = 1)]
	public class Effect_Color : TextEffectInstance
	{
		public enum ColorType
		{
			Gradient = 0,
			BetweenTwoColors = 1,
			OnlyAlpha = 2,
			ColorToOriginal = 3,
			OriginalToColor = 4
		}

		public enum GradientOrientation
		{
			Horizontal = 0,
			HorizontalPerCharacter = 1,
			Vertical = 2
		}

		[Space(10f)]
		[Header("Color")]
		[FoldBox("Color", new string[] { "Gradient: Applies a gradient horizontally across the text.", "Between Two Colors: Animates between two colors.", "Only Alpha: Animates only the alpha (transparency) of the text.", "Color To Original: Animates from a start color to the original color of the text.", "Original To Color: Animates from the original color of the text to an end color." }, new FoldBoxAttribute.ContentType[] { FoldBoxAttribute.ContentType.Text }, true)]
		public ColorType colorType;

		[ConditionalField("colorType", false, new object[]
		{
			ColorType.BetweenTwoColors,
			ColorType.ColorToOriginal
		})]
		public Color startColor = Color.white;

		[ConditionalField("colorType", false, new object[]
		{
			ColorType.BetweenTwoColors,
			ColorType.OriginalToColor
		})]
		public Color endColor = Color.white;

		[ConditionalField("colorType", false, new object[] { ColorType.Gradient })]
		public Gradient gradient;

		[ConditionalField("colorType", false, new object[] { ColorType.Gradient })]
		public GradientOrientation orientation;

		[ConditionalField("orientation", false, new object[]
		{
			GradientOrientation.HorizontalPerCharacter,
			GradientOrientation.Vertical
		})]
		public int stride = 10;

		[ConditionalField("colorType", false, new object[] { ColorType.OnlyAlpha })]
		[Range(0f, 1f)]
		public float startAlpha;

		[ConditionalField("colorType", false, new object[] { ColorType.OnlyAlpha })]
		[Range(0f, 1f)]
		public float endAlpha = 1f;

		public override void ApplyEffect(TMP_TextInfo _textInfo, int _charIndex, int _startVertex = 0, int _endVertex = 3)
		{
			if (!CheckCanApplyEffect(_charIndex))
			{
				return;
			}
			TMP_CharacterInfo tMP_CharacterInfo = _textInfo.characterInfo[_charIndex];
			int materialReferenceIndex = tMP_CharacterInfo.materialReferenceIndex;
			for (int i = _startVertex; i <= _endVertex; i++)
			{
				int num = tMP_CharacterInfo.vertexIndex + i;
				Color color = _textInfo.meshInfo[materialReferenceIndex].colors32[num];
				if (colorType == ColorType.Gradient)
				{
					if (orientation == GradientOrientation.Horizontal)
					{
						float time = Interpolate(0f, 1f, _charIndex);
						color = gradient.Evaluate(time);
					}
					if (orientation == GradientOrientation.HorizontalPerCharacter)
					{
						float num2 = Interpolate(0f, 1f, _charIndex);
						float num3 = Interpolate(0f, 1f, _charIndex + stride);
						float time2 = ((i == 0 || i == 1) ? num2 : num3);
						color = gradient.Evaluate(time2);
					}
					if (orientation == GradientOrientation.Vertical)
					{
						float num4 = Interpolate(0f, 1f, _charIndex);
						float num5 = Interpolate(0f, 1f, _charIndex + stride);
						float time3 = ((i == 1 || i == 2) ? num4 : num5);
						color = gradient.Evaluate(time3);
					}
				}
				else if (colorType == ColorType.BetweenTwoColors)
				{
					color = Interpolate(startColor, endColor, _charIndex);
				}
				else if (colorType == ColorType.OnlyAlpha)
				{
					color.a = Interpolate(startAlpha, endAlpha, _charIndex);
				}
				else if (colorType == ColorType.ColorToOriginal)
				{
					color = Interpolate(startColor, color, _charIndex);
				}
				else if (colorType == ColorType.OriginalToColor)
				{
					color = Interpolate(color, endColor, _charIndex);
				}
				_textInfo.meshInfo[materialReferenceIndex].colors32[num] = color;
			}
		}
	}
}
