using System.Collections.Generic;
using UnityEngine;

namespace INab.BetterFog.Core
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[ImageEffectAllowedInSceneView]
	public class BetterFogRenderers : MonoBehaviour
	{
		public List<CustomRenderer> depthRenderers = new List<CustomRenderer>();

		public List<CustomRenderer> fogOffsetRenderers = new List<CustomRenderer>();
	}
}
