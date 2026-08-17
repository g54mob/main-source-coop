using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace TheVisualEngine
{
	[Serializable]
	public class TVEElementBufferData
	{
		[HideInInspector]
		public string name = "";

		[HideInInspector]
		public bool isInitialized;

		public TVEBool renderMode = TVEBool.On;

		[Tooltip("The name used for the global shader parameters.")]
		public string renderName = "Custom";

		[Space(10f)]
		[Tooltip("Sets the render texture format.")]
		public TVETextureRange textureType = TVETextureRange.HDRHalf;

		public TVEBool textureArray = TVEBool.On;

		[Tooltip("Sets render texture background color.")]
		public Color textureColor = Color.black;

		[Space(10f)]
		[Tooltip("When enabled, the elements are rendered in realtime.")]
		public bool isRendering = true;

		[NonSerialized]
		public int renderDataID;

		[NonSerialized]
		public int bufferSize = -1;

		[NonSerialized]
		public float[] bufferUsage;

		[NonSerialized]
		public RenderTexture renderTexBase;

		[NonSerialized]
		public RenderTexture renderTexNear;

		[NonSerialized]
		public CommandBuffer[] commandBuffers;

		[HideInInspector]
		public string texBaseName;

		[HideInInspector]
		public string texNearName;

		[HideInInspector]
		public string texParams;

		[HideInInspector]
		public string texLayers;
	}
}
