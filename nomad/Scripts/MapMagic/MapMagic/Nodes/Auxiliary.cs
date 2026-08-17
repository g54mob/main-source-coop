using System;
using UnityEngine;

namespace MapMagic.Nodes
{
	[Serializable]
	public class Auxiliary
	{
		public string name = "Group";

		public string comment = "Drag in generators to group them";

		public Color color = new Color(0.625f, 0.625f, 0.625f, 1f);

		public Vector2 guiPos;

		public Vector2 guiSize = new Vector2(100f, 100f);
	}
}
