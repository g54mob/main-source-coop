using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace PaintCore
{
	[Serializable]
	[MovedFrom(true, "PaintIn3D", "PaintIn3D", "P3dModifyPositionRandom")]
	public class CwModifyPositionRandom : CwModifier
	{
		public static string Group = "Position";

		public static string Title = "Random";

		[SerializeField]
		private float radius = 1f;

		public float Radius
		{
			get
			{
				return radius;
			}
			set
			{
				radius = value;
			}
		}

		protected override void OnModifyPosition(ref Vector3 position, float pressure)
		{
			position += UnityEngine.Random.insideUnitSphere * radius;
		}
	}
}
