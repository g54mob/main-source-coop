using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroCameras
	{
		public Camera camera;

		public EnviroQuality quality;

		[Tooltip("Resets projection matrix of the camera. Might help with reflection cameras to render clouds.")]
		public bool resetMatrix;
	}
}
