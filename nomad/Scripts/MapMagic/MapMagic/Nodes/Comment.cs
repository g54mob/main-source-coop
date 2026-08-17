using System;
using UnityEngine;

namespace MapMagic.Nodes
{
	[Serializable]
	public class Comment : Auxiliary
	{
		public Comment()
		{
			name = "Comment";
			color = new Color(0.99609375f, 91f / 128f, 9f / 32f, 1f);
		}
	}
}
