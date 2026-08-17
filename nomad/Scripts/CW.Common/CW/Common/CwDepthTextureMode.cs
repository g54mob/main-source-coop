using System;
using UnityEngine;

namespace CW.Common
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("CW/Common/CW Depth Texture Mode")]
	public class CwDepthTextureMode : MonoBehaviour
	{
		[SerializeField]
		private DepthTextureMode depthMode;

		[NonSerialized]
		private Camera cachedCamera;

		public DepthTextureMode DepthMode
		{
			get
			{
				return depthMode;
			}
			set
			{
				depthMode = value;
				UpdateDepthMode();
			}
		}

		public void UpdateDepthMode()
		{
			if (cachedCamera == null)
			{
				cachedCamera = GetComponent<Camera>();
			}
			cachedCamera.depthTextureMode = depthMode;
		}

		protected virtual void Update()
		{
			UpdateDepthMode();
		}
	}
}
