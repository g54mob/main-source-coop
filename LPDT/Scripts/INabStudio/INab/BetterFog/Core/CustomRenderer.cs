using System;
using UnityEngine;

namespace INab.BetterFog.Core
{
	[Serializable]
	public class CustomRenderer
	{
		public bool render = true;

		public bool alwaysRender = true;

		public Renderer renderer;

		public bool drawAllSubmeshes;

		public Material material;
	}
}
