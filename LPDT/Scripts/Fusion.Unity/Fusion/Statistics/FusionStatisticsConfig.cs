using System.Collections.Generic;
using UnityEngine;

namespace Fusion.Statistics
{
	[CreateAssetMenu(menuName = "Fusion/Config/Statistics Config", fileName = "FusionStatisticsConfig")]
	public class FusionStatisticsConfig : ScriptableObject
	{
		public enum Side
		{
			Right = 0,
			Left = 1
		}

		[Range(0f, 1f)]
		public float BackgroundOpacity = 0.9f;

		public int PageRefreshRate = 30;

		[SerializeField]
		public Gradient DefaultGradient = new Gradient
		{
			colorKeys = new GradientColorKey[2]
			{
				new GradientColorKey(new Color(0.2f, 1f, 0.2f), 0f),
				new GradientColorKey(new Color(0.2f, 0.2f, 1f), 1f)
			}
		};

		[SerializeField]
		public Gradient ThresholdGradient = new Gradient
		{
			colorKeys = new GradientColorKey[2]
			{
				new GradientColorKey(new Color(1f, 1f, 0.2f), 0f),
				new GradientColorKey(new Color(1f, 0.2f, 0.2f), 1f)
			}
		};

		public bool RenderZeroAsTransparent = true;

		public bool DontDisplayZeroOnLastValue = true;

		public List<FusionStatisticsPage> StatisticsPages;
	}
}
