using System;
using UnityEngine;

namespace LimitlessDev.RetroLookPro
{
	[Serializable]
	public class effectPreset
	{
		public string effectName;

		public int numberOfColors;

		public Color32[] palette;

		public bool changed;

		public Color32[] pixels;
	}
}
