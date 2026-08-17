using System;
using Febucci.Attributes;
using UnityEngine;

namespace Febucci.UI.Core
{
	[Serializable]
	public class AppearanceDefaultValues
	{
		[Serializable]
		public class Defaults
		{
			[PositiveValue]
			public float sizeDuration = 0.3f;

			[MinValue(0f)]
			public float sizeAmplitude = 2f;

			[PositiveValue]
			public float fadeDuration = 0.3f;

			[PositiveValue]
			public float verticalExpandDuration = 0.3f;

			public bool verticalFromBottom;

			[PositiveValue]
			public float horizontalExpandDuration = 0.3f;

			[SerializeField]
			internal HorizontalExpandAppearance.ExpType horizontalExpandStart;

			[PositiveValue]
			public float diagonalExpandDuration = 0.3f;

			public bool diagonalFromBttmLeft;

			[NotZero]
			public Vector2 offsetDir = Vector2.one;

			[PositiveValue]
			public float offsetDuration = 0.3f;

			[NotZero]
			public float offsetAmplitude = 1f;

			[PositiveValue]
			public float rotationDuration = 0.3f;

			public float rotationStartAngle = 180f;

			[PositiveValue]
			public float randomDirDuration = 0.3f;

			[NotZero]
			public float randomDirAmplitude = 1f;
		}

		private const float defDuration = 0.3f;

		[SerializeField]
		[Header("Default Appearances")]
		public Defaults defaults = new Defaults();

		[SerializeField]
		[Header("Preset Effects")]
		internal PresetAppearanceValues[] presets = new PresetAppearanceValues[0];
	}
}
