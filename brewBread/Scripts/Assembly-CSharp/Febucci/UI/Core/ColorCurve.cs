using System;
using Febucci.Attributes;
using UnityEngine;

namespace Febucci.UI.Core
{
	[Serializable]
	internal class ColorCurve
	{
		[SerializeField]
		public bool enabled;

		[SerializeField]
		protected Gradient gradient;

		[SerializeField]
		[MinValue(0.1f)]
		protected float duration;

		[SerializeField]
		[Range(0f, 100f)]
		protected float charsTimeOffset;

		private bool isAppearance;

		public float GetDuration()
		{
			return duration;
		}

		public void Initialize(bool isAppearance)
		{
			this.isAppearance = isAppearance;
			if (duration < 0.1f)
			{
				duration = 0.1f;
			}
		}

		public Color32 GetColor(float time, int characterIndex)
		{
			if (isAppearance)
			{
				return gradient.Evaluate(Mathf.Clamp01(time / duration));
			}
			return gradient.Evaluate((time / duration % 1f + (float)characterIndex * (charsTimeOffset / 100f)) % 1f);
		}
	}
}
