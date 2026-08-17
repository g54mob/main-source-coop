using UnityEngine;

namespace CorgiGodRays
{
	[CreateAssetMenu(fileName = "GodRaysRenderData", menuName = "GodRaysRenderData")]
	public class GodRaysRenderData : ScriptableObject
	{
		public Material Grabpass;

		public Material DepthGrabpass;

		public Material ScreenSpaceGodRays;

		public Material ApplyGodRays;

		public Material BilateralBlur;

		public ComputeShader TemporalReprojectionShader;

		public bool StripUnusedShadersFromBuilds;
	}
}
