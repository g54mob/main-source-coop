using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PaintCore
{
	[Serializable]
	[MovedFrom(true, "PaintIn3D", "PaintIn3D", "P3dModifyTexturePressure")]
	public class CwModifyTexturePressure : CwModifier
	{
		public static string Group = "Texture";

		public static string Title = "Pressure";

		[SerializeField]
		private Texture texture;

		[SerializeField]
		private float pressureMin = 0.5f;

		[SerializeField]
		private float pressureMax = 1f;

		public Texture Texture
		{
			get
			{
				return texture;
			}
			set
			{
				texture = value;
			}
		}

		public float PressureMin
		{
			get
			{
				return pressureMin;
			}
			set
			{
				pressureMin = value;
			}
		}

		public float PressureMax
		{
			get
			{
				return pressureMax;
			}
			set
			{
				pressureMax = value;
			}
		}

		protected override void OnModifyTexture(ref Texture currentTexture, float pressure)
		{
			if (pressure >= pressureMin && pressure <= pressureMax)
			{
				currentTexture = texture;
			}
		}
	}
}
