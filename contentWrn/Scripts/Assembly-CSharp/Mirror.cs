using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[DefaultExecutionOrder(4000)]
[ExecuteInEditMode]
public class Mirror : MonoBehaviour
{
	[Serializable]
	public class Info
	{
		public Camera cam;

		public RTHandle rtMain;

		public RTHandle rtAlt;
	}

	public bool shouldRender = true;

	public Shader shader;

	private GameObject mrObj;

	private GameObject camObj;

	private Camera linkedCam;

	private MeshRenderer mr;

	[Min(0.001f)]
	public float renderScale = 1f;

	public Vector2Int renderSize = Vector2Int.zero;

	private float oldRenderScale = 1f;

	private Vector2Int oldRenderSize = Vector2Int.zero;

	private Material mat;

	private List<Info> infos = new List<Info>();

	private static Plane[] frustrumPlanes = new Plane[6];

	public GameObject thisGameObject => mrObj;

	private void Awake()
	{
		mr = GetComponent<MeshRenderer>();
	}

	private void OnEnable()
	{
		if (mrObj == null)
		{
			mrObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
		}
		mrObj.name = base.name + "_Quad";
		mrObj.transform.SetParent(base.transform, worldPositionStays: false);
		mrObj.hideFlags = HideFlags.HideAndDontSave;
		mr = mrObj.GetComponent<MeshRenderer>();
		if (camObj == null)
		{
			camObj = new GameObject(base.name + "_Camera", typeof(Camera));
		}
		camObj.transform.SetParent(base.transform, worldPositionStays: false);
		camObj.hideFlags = HideFlags.HideAndDontSave;
		linkedCam = camObj.GetComponent<Camera>();
		linkedCam.enabled = false;
		if (mat == null)
		{
			mat = new Material(shader);
		}
		mr.sharedMaterial = mat;
		RTHandles.Initialize(Screen.width, Screen.height);
		oldRenderScale = renderScale;
		oldRenderSize = renderSize;
		RenderPipelineManager.beginCameraRendering += OnCameraRender;
	}

	private static bool IsVisible(Camera camera, Bounds bounds)
	{
		GeometryUtility.CalculateFrustumPlanes(camera, frustrumPlanes);
		return GeometryUtility.TestPlanesAABB(frustrumPlanes, bounds);
	}

	private RTHandle AllocMain()
	{
		if (renderSize.x <= 0 || renderSize.y <= 0)
		{
			return RTHandles.Alloc(Vector2.one * renderScale, 1, DepthBits.None, GraphicsFormat.R8G8B8A8_SRGB, FilterMode.Point, TextureWrapMode.Repeat, TextureDimension.Tex2D, enableRandomWrite: false, useMipMap: false, autoGenerateMips: true, isShadowMap: false, 1, 0f, MSAASamples.None, bindTextureMS: false, useDynamicScale: false, useDynamicScaleExplicit: false, RenderTextureMemoryless.None, VRTextureUsage.None, base.name + "_Main");
		}
		return RTHandles.Alloc(renderSize.x, renderSize.y, 1, DepthBits.None, GraphicsFormat.R8G8B8A8_SRGB, FilterMode.Point, TextureWrapMode.Repeat, TextureDimension.Tex2D, enableRandomWrite: false, useMipMap: false, autoGenerateMips: true, isShadowMap: false, 1, 0f, MSAASamples.None, bindTextureMS: false, useDynamicScale: false, useDynamicScaleExplicit: false, RenderTextureMemoryless.None, VRTextureUsage.None, base.name + "_Main");
	}

	private RTHandle AllocAlt()
	{
		if (renderSize.x <= 0 || renderSize.y <= 0)
		{
			return RTHandles.Alloc(Vector2.one * renderScale, 1, DepthBits.None, GraphicsFormat.R8G8B8A8_SRGB, FilterMode.Point, TextureWrapMode.Repeat, TextureDimension.Tex2D, enableRandomWrite: false, useMipMap: false, autoGenerateMips: true, isShadowMap: false, 1, 0f, MSAASamples.None, bindTextureMS: false, useDynamicScale: false, useDynamicScaleExplicit: false, RenderTextureMemoryless.None, VRTextureUsage.None, base.name + "_Alt");
		}
		return RTHandles.Alloc(renderSize.x, renderSize.y, 1, DepthBits.None, GraphicsFormat.R8G8B8A8_SRGB, FilterMode.Point, TextureWrapMode.Repeat, TextureDimension.Tex2D, enableRandomWrite: false, useMipMap: false, autoGenerateMips: true, isShadowMap: false, 1, 0f, MSAASamples.None, bindTextureMS: false, useDynamicScale: false, useDynamicScaleExplicit: false, RenderTextureMemoryless.None, VRTextureUsage.None, base.name + "_Alt");
	}

	private void OnCameraRender(ScriptableRenderContext arg1, Camera arg2)
	{
		if (IsVisible(arg2, mr.bounds))
		{
			OnWillRenderObjectWCam(arg2, render: true);
		}
	}

	private void LateUpdate()
	{
		if (oldRenderScale != renderScale || oldRenderSize != renderSize)
		{
			OnDisable();
			OnEnable();
		}
		Camera[] allCameras = Camera.allCameras;
		foreach (Camera camera in allCameras)
		{
			if (IsVisible(camera, mr.bounds))
			{
				OnWillRenderObjectWCam(camera, render: false);
			}
		}
	}

	private void OnDisable()
	{
		RenderPipelineManager.beginCameraRendering -= OnCameraRender;
		foreach (Info info in infos)
		{
			RTHandles.Release(info.rtMain);
			RTHandles.Release(info.rtAlt);
		}
		infos.Clear();
	}

	private void OnDestroy()
	{
		UnityEngine.Object.DestroyImmediate(mrObj);
		UnityEngine.Object.DestroyImmediate(camObj);
	}

	private void OnWillRenderObjectWCam(Camera current, bool render)
	{
		if (current == linkedCam)
		{
			return;
		}
		int num = infos.FindIndex((Info x) => x.cam == current);
		if (num == -1)
		{
			infos.Add(new Info
			{
				cam = current,
				rtMain = AllocMain(),
				rtAlt = AllocAlt()
			});
			num = infos.Count - 1;
		}
		RTHandle rtMain = infos[num].rtMain;
		RTHandle rtAlt = infos[num].rtAlt;
		if ((bool)linkedCam && (bool)thisGameObject)
		{
			if (current.stereoEnabled)
			{
				Vector3 position = current.transform.position;
				Quaternion rotation = current.transform.rotation;
				Vector3 position2 = current.transform.position;
				Quaternion rotation2 = current.transform.rotation;
				Matrix4x4 stereoProjectionMatrix = current.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
				current.GetStereoViewMatrix(Camera.StereoscopicEye.Right);
				Matrix4x4 stereoProjectionMatrix2 = current.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
				current.GetStereoViewMatrix(Camera.StereoscopicEye.Left);
				RenderCam(current, position, rotation, stereoProjectionMatrix, rtMain, "_MainTex", render);
				RenderCam(current, position2, rotation2, stereoProjectionMatrix2, rtAlt, "_AltTex", render);
			}
			else
			{
				Vector3 position3 = current.transform.position;
				Quaternion rotation3 = current.transform.rotation;
				Matrix4x4 projectionMatrix = current.projectionMatrix;
				RenderCam(current, position3, rotation3, projectionMatrix, rtMain, "_MainTex", render);
			}
		}
	}

	public void RenderCam(Camera current, Vector3 pos, Quaternion rot, Matrix4x4 mat, RTHandle rt3, string texId, bool render)
	{
		if (rt3 == null)
		{
			return;
		}
		if (!render)
		{
			linkedCam.stereoTargetEye = StereoTargetEyeMask.None;
			_ = linkedCam.projectionMatrix;
			_ = linkedCam.nearClipPlane;
			_ = linkedCam.transform.position;
			_ = linkedCam.transform.rotation;
			linkedCam.fieldOfView = current.fieldOfView;
			linkedCam.nearClipPlane = current.nearClipPlane;
			linkedCam.farClipPlane = current.farClipPlane;
			linkedCam.projectionMatrix = mat;
			linkedCam.transform.localPosition = Vector3.Reflect(thisGameObject.transform.InverseTransformPoint(pos), Vector3.forward);
			linkedCam.transform.localRotation = Quaternion.LookRotation(Vector3.Reflect(thisGameObject.transform.InverseTransformDirection(rot * Vector3.forward), Vector3.forward), Vector3.Reflect(thisGameObject.transform.InverseTransformDirection(rot * Vector3.up), Vector3.forward));
			Transform transform = thisGameObject.transform;
			int num = Math.Sign(Vector3.Dot(transform.forward, transform.position - linkedCam.transform.position));
			Vector3 lhs = linkedCam.worldToCameraMatrix.MultiplyPoint(transform.position);
			Vector3 rhs = linkedCam.worldToCameraMatrix.MultiplyVector(transform.forward) * num;
			float w = 0f - Vector3.Dot(lhs, rhs);
			Vector4 clipPlane = new Vector4(rhs.x, rhs.y, rhs.z, w);
			Matrix4x4 projectionMatrix = linkedCam.CalculateObliqueMatrix(clipPlane);
			linkedCam.projectionMatrix = projectionMatrix;
			linkedCam.forceIntoRenderTexture = true;
			RenderTexture targetTexture = linkedCam.targetTexture;
			linkedCam.targetTexture = rt3;
			if (shouldRender)
			{
				RenderPipeline.SubmitRenderRequest(linkedCam, new UniversalRenderPipeline.SingleCameraRequest
				{
					destination = rt3
				});
			}
			linkedCam.targetTexture = targetTexture;
		}
		this.mat.SetTexture(texId, rt3);
	}
}
