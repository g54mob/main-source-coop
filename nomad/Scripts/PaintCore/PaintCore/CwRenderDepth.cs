using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaintCore
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintCore#CwRenderDepth")]
	[AddComponentMenu("CW/Paint Core/CW Render Depth")]
	public class CwRenderDepth : MonoBehaviour
	{
		[SerializeField]
		private Camera sourceCamera;

		[SerializeField]
		private Matrix4x4 sourceMatrix;

		[SerializeField]
		private int resizeAndDownscale;

		[SerializeField]
		private float bias = 1E-07f;

		[SerializeField]
		private bool multitap;

		[SerializeField]
		private bool readInStart = true;

		[SerializeField]
		private bool readInUpdate;

		private Shader cachedShader;

		[NonSerialized]
		private Camera depthCamera;

		[NonSerialized]
		private RenderTexture depthTexture;

		private static int _CwDepthMatrix = Shader.PropertyToID("_CwDepthMatrix");

		private static LinkedList<CwRenderDepth> instances = new LinkedList<CwRenderDepth>();

		private LinkedListNode<CwRenderDepth> instancesNode;

		public Camera SourceCamera
		{
			get
			{
				return sourceCamera;
			}
			set
			{
				sourceCamera = value;
			}
		}

		public Matrix4x4 SourceMatrix
		{
			get
			{
				return sourceMatrix;
			}
			set
			{
				sourceMatrix = value;
			}
		}

		public int ResizeAndDownscale
		{
			get
			{
				return resizeAndDownscale;
			}
			set
			{
				resizeAndDownscale = value;
			}
		}

		public float Bias
		{
			get
			{
				return bias;
			}
			set
			{
				bias = value;
			}
		}

		public bool Multitap
		{
			get
			{
				return multitap;
			}
			set
			{
				multitap = value;
			}
		}

		public bool ReadInStart
		{
			get
			{
				return readInStart;
			}
			set
			{
				readInStart = value;
			}
		}

		public bool ReadInUpdate
		{
			get
			{
				return readInUpdate;
			}
			set
			{
				readInUpdate = value;
			}
		}

		public static LinkedList<CwRenderDepth> Instances => instances;

		public RenderTexture DepthTexture => depthTexture;

		public int TapCount
		{
			get
			{
				if (!multitap)
				{
					return 0;
				}
				return 8;
			}
		}

		public static CwRenderDepth Find()
		{
			if (instances.Count <= 0)
			{
				return null;
			}
			return instances.First.Value;
		}

		[ContextMenu("Read Now")]
		public void ReadNow()
		{
			if (cachedShader == null)
			{
				cachedShader = Shader.Find("Hidden/PaintCore/CwRenderDepth");
			}
			if (depthCamera == null)
			{
				CreateDepthCamera();
			}
			if (depthTexture == null)
			{
				depthTexture = new RenderTexture(64, 64, 32, RenderTextureFormat.Depth);
			}
			if (!(sourceCamera != null))
			{
				return;
			}
			int num = sourceCamera.pixelWidth;
			int num2 = sourceCamera.pixelHeight;
			if (resizeAndDownscale > 0)
			{
				num /= resizeAndDownscale;
				num2 /= resizeAndDownscale;
			}
			if (depthTexture.width != num || depthTexture.height != num2)
			{
				if (depthTexture.IsCreated())
				{
					depthTexture.Release();
				}
				depthTexture.width = num;
				depthTexture.height = num2;
				depthTexture.Create();
			}
			sourceMatrix = Matrix4x4.Translate(new Vector3(0.5f, 0.5f, 0.5f)) * Matrix4x4.Scale(new Vector3(0.5f, 0.5f, 0.5f)) * sourceCamera.projectionMatrix * sourceCamera.worldToCameraMatrix;
			Shader.SetGlobalMatrix(_CwDepthMatrix, sourceMatrix);
			depthCamera.CopyFrom(sourceCamera);
			depthCamera.enabled = false;
			depthCamera.clearFlags = CameraClearFlags.Color;
			depthCamera.backgroundColor = Color.black;
			depthCamera.targetTexture = depthTexture;
			depthCamera.transform.position = sourceCamera.transform.position;
			depthCamera.transform.rotation = sourceCamera.transform.rotation;
			depthCamera.RenderWithShader(cachedShader, "RenderType");
		}

		protected virtual void Start()
		{
			if (readInStart)
			{
				ReadNow();
			}
		}

		protected virtual void Update()
		{
			if (readInUpdate)
			{
				ReadNow();
			}
		}

		protected virtual void OnEnable()
		{
			instancesNode = instances.AddLast(this);
		}

		protected virtual void OnDisable()
		{
			instances.Remove(instancesNode);
			instancesNode = null;
			ClearDepthCamera();
		}

		private void CreateDepthCamera()
		{
			ClearDepthCamera();
			GameObject gameObject = new GameObject("DepthCamera");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			depthCamera = gameObject.AddComponent<Camera>();
			depthCamera.enabled = false;
		}

		private void ClearDepthCamera()
		{
			if (depthCamera != null)
			{
				UnityEngine.Object.DestroyImmediate(depthCamera.gameObject);
				depthCamera = null;
			}
			if (depthTexture != null)
			{
				depthTexture.Release();
				UnityEngine.Object.DestroyImmediate(depthTexture);
				depthTexture = null;
			}
		}
	}
}
