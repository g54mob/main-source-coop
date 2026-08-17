using System;
using UnityEngine;

namespace NomadDrive.Features.LiquidTransferSystem.UI
{
	[Serializable]
	public class LiquidTransferUIInfoConfigs
	{
		[Header("Base Rect Transform Settings")]
		public float rectWidthForInfo = 600f;

		public float rectHeightForInfo = 140f;

		public float rectWidthForTransfer = 255f;

		public float rectHeightForTransfer = 140f;

		[Header("Transfer Info Sprites")]
		public Sprite defaultSuccessSprite;

		public Sprite defaultFailureSprite;

		[Header("Liquid Colors")]
		public Color waterColor = new Color(0.2f, 0.6f, 1f);

		public Color gasolineColor = new Color(1f, 0.8f, 0.2f);

		public Color oilColor = new Color(0.3f, 0.2f, 0.1f);

		public Color coffeeColor = new Color(0.4f, 0.2f, 0.1f);

		public Color milkColor = new Color(1f, 1f, 0.9f);

		public Color coolantColor = new Color(0.2f, 0.9f, 0.4f);

		public Color defaultColor = new Color(0.5f, 0.5f, 0.5f);
	}
}
