using UnityEngine;
using UnityEngine.Rendering;

namespace Enviro
{
	public class EnviroVolumetricCloudRenderer
	{
		public Camera camera;

		public Material raymarchMat;

		public Material reprojectMat;

		public Material depthMat;

		public Material blendAndLightingMat;

		public Material shadowMat;

		public RenderTexture[] fullBuffer;

		public int fullBufferIndex;

		public RenderTexture undersampleBuffer;

		public RenderTexture downsampledDepth;

		public Matrix4x4 prevV;

		public Matrix4x4 prevVRight;

		public int frame;

		public bool firstFrame = true;

		public RTHandle[] fullBufferHandles;

		public RTHandle undersampleBufferHandle;

		public RTHandle downsampledDepthHandle;
	}
}
