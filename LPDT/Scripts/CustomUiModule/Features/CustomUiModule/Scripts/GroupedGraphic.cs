using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CustomUiModule.Scripts
{
	public class GroupedGraphic : Graphic
	{
		[SerializeField]
		private List<Graphic> _graphics = new List<Graphic>();

		public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, bool useRGB)
		{
			foreach (Graphic graphic in _graphics)
			{
				graphic.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, useRGB);
			}
		}

		public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			foreach (Graphic graphic in _graphics)
			{
				graphic.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			}
		}

		public override void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			foreach (Graphic graphic in _graphics)
			{
				graphic.CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			}
		}

		public override bool Raycast(Vector2 sp, Camera eventCamera)
		{
			foreach (Graphic graphic in _graphics)
			{
				if (graphic.Raycast(sp, eventCamera))
				{
					return true;
				}
			}
			return false;
		}
	}
}
