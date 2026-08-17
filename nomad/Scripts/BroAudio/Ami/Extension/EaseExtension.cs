using System;
using UnityEngine;

namespace Ami.Extension
{
	public static class EaseExtension
	{
		public static float SetEase(this float value, Ease ease)
		{
			Mathf.Clamp01(value);
			return ease switch
			{
				Ease.Linear => value, 
				Ease.InQuad => Mathf.Pow(value, 2f), 
				Ease.InCubic => Mathf.Pow(value, 3f), 
				Ease.InQuart => Mathf.Pow(value, 4f), 
				Ease.InQuint => Mathf.Pow(value, 5f), 
				Ease.InSine => 1f - Mathf.Cos(value * (float)Math.PI / 2f), 
				Ease.InCirc => 1f - Mathf.Sqrt(1f - Mathf.Pow(value, 2f)), 
				Ease.OutQuad => 1f - Mathf.Pow(1f - value, 2f), 
				Ease.OutCubic => 1f - Mathf.Pow(1f - value, 3f), 
				Ease.OutQuart => 1f - Mathf.Pow(1f - value, 4f), 
				Ease.OutQuint => 1f - Mathf.Pow(1f - value, 5f), 
				Ease.OutSine => Mathf.Sin(value * (float)Math.PI / 2f), 
				Ease.OutCirc => Mathf.Sqrt(1f - Mathf.Pow(value - 1f, 2f)), 
				Ease.InOutQuad => (value < 0.5f) ? (2f * Mathf.Pow(value, 2f)) : (1f - Mathf.Pow(-2f * value + 2f, 2f) / 2f), 
				Ease.InOutCubic => (value < 0.5f) ? (2f * Mathf.Pow(value, 3f)) : (1f - Mathf.Pow(-2f * value + 2f, 3f) / 2f), 
				Ease.InOutQuart => (value < 0.5f) ? (2f * Mathf.Pow(value, 4f)) : (1f - Mathf.Pow(-2f * value + 2f, 4f) / 2f), 
				Ease.InOutQuint => (value < 0.5f) ? (2f * Mathf.Pow(value, 5f)) : (1f - Mathf.Pow(-2f * value + 2f, 5f) / 2f), 
				Ease.InOutSine => (0f - (Mathf.Cos((float)Math.PI * value) - 1f)) / 2f, 
				Ease.InOutCirc => ((double)value < 0.5) ? ((1f - Mathf.Sqrt(1f - Mathf.Pow(2f * value, 2f))) / 2f) : ((Mathf.Sqrt(1f - Mathf.Pow(-2f * value + 2f, 2f)) + 1f) / 2f), 
				_ => 0f, 
			};
		}
	}
}
