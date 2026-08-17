using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace StylizedWater2
{
	[ExecuteInEditMode]
	[AddComponentMenu("Stylized Water 2/Planar Reflection Renderer")]
	[HelpURL("https://staggart.xyz/unity/stylized-water-2/sws-2-docs/?section=planar-reflections")]
	public class PlanarReflectionRenderer : MonoBehaviour
	{
		public static List<PlanarReflectionRenderer> Instances = new List<PlanarReflectionRenderer>();

		public Dictionary<Camera, Camera> reflectionCameras = new Dictionary<Camera, Camera>();

		[Tooltip("If enabled, the reflection plane will be based on this transform's up vector (green arrow).\n\nOtherwise the world's upwards direction is assumed")]
		public bool rotatable;

		[Tooltip("Set the layers that should be rendered into the reflection. The \"Water\" layer is always excluded")]
		public LayerMask cullingMask = -1;

		[Tooltip("The renderer used by the reflection camera. It's recommend to create a separate renderer, so any custom render features aren't executed for the reflection")]
		public int rendererIndex = -1;

		[Min(0f)]
		public float offset = 0.05f;

		[Tooltip("When disabled, the skybox reflection comes from a Reflection Probe. This has the benefit of being omni-directional rather than flat/planar. Enabled this to render the skybox into the planar reflection anyway")]
		public bool includeSkybox;

		[Tooltip("Render Unity's default fog in the reflection. Note that this doesn't strictly work correctly on large triangles, as it is incompatible with oblique camera projections.")]
		public bool enableFog;

		public bool renderShadows;

		[Tooltip("Objects beyond this range aren't rendered into the reflection. Note that this may causes popping for large/tall objects.")]
		public float renderRange = 500f;

		[Range(0.25f, 1f)]
		[Tooltip("A multiplier for the rendering resolution, based on the current screen resolution. The render scale, as configured in the pipeline settings is multiplied over this.")]
		public float renderScale = 0.75f;

		[Range(0f, 4f)]
		[Tooltip("Do not render LOD objects lower than this value. Example: With a value of 1, LOD0 for LOD Groups will not be used")]
		public int maximumLODLevel;

		[SerializeField]
		public List<WaterObject> waterObjects = new List<WaterObject>();

		[Tooltip("If enabled, the center of the rendering bounds (that wraps around the water objects) moves with the Transform position\n\nYou must however ensure you are only moving on the XZ axis")]
		public bool moveWithTransform;

		[HideInInspector]
		public Bounds bounds;

		private float m_renderScale = 1f;

		private float m_renderRange;

		private static readonly int _PlanarReflectionsEnabledID = Shader.PropertyToID("_PlanarReflectionsEnabled");

		private static readonly int _PlanarReflectionID = Shader.PropertyToID("_PlanarReflection");

		[NonSerialized]
		public bool isRendering;

		private Camera m_reflectionCamera;

		private static UniversalAdditionalCameraData m_cameraData;

		private static readonly Plane[] frustrumPlanes = new Plane[6];

		private static Vector4 reflectionPlane;

		private static Matrix4x4 reflectionBase;

		private static Vector3 oldCamPos;

		private static Matrix4x4 worldToCamera;

		private static Matrix4x4 viewMatrix;

		private static Matrix4x4 projectionMatrix;

		private static Vector4 clipPlane;

		private static readonly float[] layerCullDistances = new float[32];

		public static bool AllowReflections { get; private set; } = true;

		private void Reset()
		{
			base.gameObject.name = "Planar Reflection Renderer";
		}

		private void OnEnable()
		{
			InitializeValues();
			Instances.Add(this);
			EnableReflections();
		}

		private void OnDisable()
		{
			Instances.Remove(this);
			DisableReflections();
		}

		public void InitializeValues()
		{
			m_renderScale = renderScale;
			m_renderRange = renderRange;
		}

		public void ApplyToAllWaterInstances()
		{
			waterObjects = new List<WaterObject>(WaterObject.Instances);
			RecalculateBounds();
			EnableMaterialReflectionSampling();
		}

		public static void SetQuality(bool enableReflections, float renderScale = -1f, float renderRange = -1f, int maxLodLevel = -1)
		{
			AllowReflections = enableReflections;
			foreach (PlanarReflectionRenderer instance in Instances)
			{
				if (renderScale > 0f)
				{
					instance.renderScale = renderScale;
				}
				if (renderRange > 0f)
				{
					instance.renderRange = renderRange;
				}
				if (maxLodLevel >= 0)
				{
					instance.maximumLODLevel = maxLodLevel;
				}
				instance.InitializeValues();
				if (enableReflections)
				{
					instance.EnableReflections();
				}
				if (!enableReflections)
				{
					instance.DisableReflections();
				}
			}
		}

		public void EnableReflections()
		{
			if (AllowReflections && !PipelineUtilities.VREnabled())
			{
				RenderPipelineManager.beginCameraRendering += OnWillRenderCamera;
				ToggleMaterialReflectionSampling(state: true);
			}
		}

		public void DisableReflections()
		{
			RenderPipelineManager.beginCameraRendering -= OnWillRenderCamera;
			ToggleMaterialReflectionSampling(state: false);
			foreach (KeyValuePair<Camera, Camera> reflectionCamera in reflectionCameras)
			{
				if (!(reflectionCamera.Value == null) && (bool)reflectionCamera.Value)
				{
					RenderTexture.ReleaseTemporary(reflectionCamera.Value.targetTexture);
					UnityEngine.Object.DestroyImmediate(reflectionCamera.Value.gameObject);
				}
			}
			reflectionCameras.Clear();
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = ((bounds.size.y > 0.01f) ? Color.yellow : Color.white);
			Gizmos.DrawWireCube(bounds.center, bounds.size);
		}

		public Bounds CalculateBounds()
		{
			Bounds result = new Bounds(Vector3.zero, Vector3.zero);
			if (waterObjects == null)
			{
				return result;
			}
			if (waterObjects.Count == 0)
			{
				return result;
			}
			Vector3 vector = Vector3.one * float.PositiveInfinity;
			Vector3 vector2 = Vector3.one * float.NegativeInfinity;
			for (int i = 0; i < waterObjects.Count; i++)
			{
				if ((bool)waterObjects[i])
				{
					vector = Vector3.Min(waterObjects[i].meshRenderer.bounds.min, vector);
					vector2 = Vector3.Max(waterObjects[i].meshRenderer.bounds.max, vector2);
				}
			}
			result.SetMinMax(vector, vector2);
			result.size = new Vector3(result.size.x, 0f, result.size.z);
			return result;
		}

		public void RecalculateBounds()
		{
			bounds = CalculateBounds();
		}

		public static bool InvalidContext(Camera camera)
		{
			if (camera.cameraType != CameraType.SceneView)
			{
				if (camera.cameraType != CameraType.Reflection && camera.cameraType != CameraType.Preview)
				{
					return camera.hideFlags != HideFlags.None;
				}
				return true;
			}
			return false;
		}

		private void OnWillRenderCamera(ScriptableRenderContext context, Camera camera)
		{
			if (InvalidContext(camera))
			{
				isRendering = false;
				return;
			}
			isRendering = WaterObjectsVisible(camera);
			if (!isRendering)
			{
				return;
			}
			if (moveWithTransform)
			{
				bounds.center = base.transform.position;
			}
			m_cameraData = camera.GetComponent<UniversalAdditionalCameraData>();
			if ((bool)m_cameraData && m_cameraData.renderType == CameraRenderType.Overlay)
			{
				return;
			}
			reflectionCameras.TryGetValue(camera, out m_reflectionCamera);
			if (m_reflectionCamera == null)
			{
				CreateReflectionCamera(camera);
			}
			if ((bool)m_reflectionCamera)
			{
				if (Math.Abs(renderScale - m_renderScale) > 0.02f)
				{
					RenderTexture.ReleaseTemporary(m_reflectionCamera.targetTexture);
					CreateRenderTexture(m_reflectionCamera, camera);
					m_renderScale = renderScale;
				}
				UpdateWaterProperties(m_reflectionCamera);
				UpdateCameraProperties(camera, m_reflectionCamera);
				UpdatePerspective(camera, m_reflectionCamera);
				bool flag = RenderSettings.fog && !enableFog;
				if (flag)
				{
					RenderSettings.fog = false;
				}
				int num = QualitySettings.maximumLODLevel;
				QualitySettings.maximumLODLevel = maximumLODLevel;
				GL.invertCulling = true;
				UniversalRenderPipeline.RenderSingleCamera(context, m_reflectionCamera);
				if (flag)
				{
					RenderSettings.fog = true;
				}
				QualitySettings.maximumLODLevel = num;
				GL.invertCulling = false;
			}
		}

		private float GetRenderScale()
		{
			return Mathf.Clamp(renderScale * UniversalRenderPipeline.asset.renderScale, 0.25f, 1f);
		}

		public void SetRendererIndex(int index)
		{
			index = PipelineUtilities.ValidateRenderer(index);
			foreach (KeyValuePair<Camera, Camera> reflectionCamera in reflectionCameras)
			{
				if (!(reflectionCamera.Value == null))
				{
					m_cameraData = reflectionCamera.Value.GetComponent<UniversalAdditionalCameraData>();
					m_cameraData.SetRenderer(index);
				}
			}
		}

		public void ToggleShadows(bool state)
		{
			foreach (KeyValuePair<Camera, Camera> reflectionCamera in reflectionCameras)
			{
				if (!(reflectionCamera.Value == null))
				{
					m_cameraData = reflectionCamera.Value.GetComponent<UniversalAdditionalCameraData>();
					m_cameraData.renderShadows = state;
				}
			}
		}

		public void AddWaterObject(WaterObject waterObject)
		{
			ToggleMaterialReflectionSampling(waterObject, state: true);
			waterObjects.Add(waterObject);
			RecalculateBounds();
		}

		public void RemoveWaterObject(WaterObject waterObject)
		{
			ToggleMaterialReflectionSampling(waterObject, state: false);
			waterObjects.Remove(waterObject);
			RecalculateBounds();
		}

		public void EnableMaterialReflectionSampling()
		{
			ToggleMaterialReflectionSampling(AllowReflections);
		}

		public void ToggleMaterialReflectionSampling(bool state)
		{
			if (waterObjects == null)
			{
				return;
			}
			for (int i = 0; i < waterObjects.Count; i++)
			{
				if (!(waterObjects[i] == null))
				{
					ToggleMaterialReflectionSampling(waterObjects[i], state);
				}
			}
		}

		private void ToggleMaterialReflectionSampling(WaterObject waterObject, bool state)
		{
			waterObject.props.SetFloat(_PlanarReflectionsEnabledID, state ? 1f : 0f);
			waterObject.ApplyInstancedProperties();
		}

		private void CreateReflectionCamera(Camera source)
		{
			Camera camera = new GameObject(source.name + " Planar Reflection")
			{
				hideFlags = (HideFlags.DontSave | HideFlags.HideInHierarchy)
			}.AddComponent<Camera>();
			camera.hideFlags = HideFlags.DontSave;
			camera.CopyFrom(source);
			camera.cullingMask = -17 & (int)cullingMask;
			camera.cameraType = CameraType.Game;
			camera.depth = source.depth - 1f;
			camera.rect = new Rect(0f, 0f, 1f, 1f);
			camera.enabled = false;
			camera.clearFlags = (includeSkybox ? CameraClearFlags.Skybox : CameraClearFlags.Depth);
			camera.backgroundColor = Color.clear;
			camera.useOcclusionCulling = false;
			UniversalAdditionalCameraData universalAdditionalCameraData = camera.gameObject.AddComponent<UniversalAdditionalCameraData>();
			universalAdditionalCameraData.requiresDepthTexture = false;
			universalAdditionalCameraData.requiresColorTexture = false;
			universalAdditionalCameraData.renderShadows = renderShadows;
			rendererIndex = PipelineUtilities.ValidateRenderer(rendererIndex);
			universalAdditionalCameraData.SetRenderer(rendererIndex);
			CreateRenderTexture(camera, source);
			reflectionCameras[source] = camera;
		}

		private void CreateRenderTexture(Camera targetCamera, Camera source)
		{
			RenderTextureFormat colorFormat = ((UniversalRenderPipeline.asset.supportsHDR && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.DefaultHDR)) ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default);
			float num = GetRenderScale();
			RenderTextureDescriptor desc = new RenderTextureDescriptor((int)((float)source.scaledPixelWidth * num), (int)((float)source.scaledPixelHeight * num), colorFormat);
			desc.depthBufferBits = 16;
			targetCamera.targetTexture = RenderTexture.GetTemporary(desc);
			targetCamera.targetTexture.name = $"{source.name}_Reflection {desc.width}x{desc.height}";
		}

		public bool WaterObjectsVisible(Camera targetCamera)
		{
			GeometryUtility.CalculateFrustumPlanes(targetCamera.projectionMatrix * targetCamera.worldToCameraMatrix, frustrumPlanes);
			return GeometryUtility.TestPlanesAABB(frustrumPlanes, bounds);
		}

		private void UpdateWaterProperties(Camera cam)
		{
			for (int i = 0; i < waterObjects.Count; i++)
			{
				if (!(waterObjects[i] == null))
				{
					waterObjects[i].props.SetTexture(_PlanarReflectionID, cam.targetTexture);
					waterObjects[i].ApplyInstancedProperties();
				}
			}
		}

		private void UpdateCameraProperties(Camera source, Camera reflectionCam)
		{
			reflectionCam.fieldOfView = source.fieldOfView;
			reflectionCam.orthographic = source.orthographic;
			reflectionCam.orthographicSize = source.orthographicSize;
			reflectionCam.useOcclusionCulling = source.useOcclusionCulling;
		}

		private void UpdatePerspective(Camera source, Camera reflectionCam)
		{
			if (!source || !reflectionCam)
			{
				return;
			}
			Vector3 vector = (rotatable ? base.transform.up : Vector3.up);
			Vector3 vector2 = bounds.center + vector * offset;
			float w = 0f - Vector3.Dot(vector, vector2);
			reflectionPlane = new Vector4(vector.x, vector.y, vector.z, w);
			reflectionBase = Matrix4x4.identity;
			reflectionBase *= Matrix4x4.Scale(new Vector3(1f, -1f, 1f));
			CalculateReflectionMatrix(ref reflectionBase, reflectionPlane);
			oldCamPos = source.transform.position - new Vector3(0f, vector2.y * 2f, 0f);
			reflectionCam.transform.forward = Vector3.Scale(source.transform.forward, new Vector3(1f, -1f, 1f));
			worldToCamera = source.worldToCameraMatrix;
			viewMatrix = worldToCamera * reflectionBase;
			oldCamPos.y = 0f - oldCamPos.y;
			reflectionCam.transform.position = oldCamPos;
			clipPlane = CameraSpacePlane(reflectionCam.worldToCameraMatrix, vector2 - vector * 0.1f, vector, 1f);
			projectionMatrix = source.CalculateObliqueMatrix(clipPlane);
			reflectionCam.cullingMask = -17 & (int)cullingMask;
			m_reflectionCamera.clearFlags = (includeSkybox ? CameraClearFlags.Skybox : CameraClearFlags.Depth);
			if (m_renderRange != renderRange)
			{
				m_renderRange = renderRange;
				for (int i = 0; i < layerCullDistances.Length; i++)
				{
					layerCullDistances[i] = renderRange;
				}
			}
			reflectionCam.layerCullDistances = layerCullDistances;
			reflectionCam.layerCullSpherical = true;
			reflectionCam.projectionMatrix = projectionMatrix;
			reflectionCam.worldToCameraMatrix = viewMatrix;
		}

		private void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
		{
			reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
			reflectionMat.m01 = -2f * plane[0] * plane[1];
			reflectionMat.m02 = -2f * plane[0] * plane[2];
			reflectionMat.m03 = -2f * plane[3] * plane[0];
			reflectionMat.m10 = -2f * plane[1] * plane[0];
			reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
			reflectionMat.m12 = -2f * plane[1] * plane[2];
			reflectionMat.m13 = -2f * plane[3] * plane[1];
			reflectionMat.m20 = -2f * plane[2] * plane[0];
			reflectionMat.m21 = -2f * plane[2] * plane[1];
			reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
			reflectionMat.m23 = -2f * plane[3] * plane[2];
			reflectionMat.m30 = 0f;
			reflectionMat.m31 = 0f;
			reflectionMat.m32 = 0f;
			reflectionMat.m33 = 1f;
		}

		private Vector4 CameraSpacePlane(Matrix4x4 worldToCameraMatrix, Vector3 pos, Vector3 normal, float sideSign)
		{
			Vector3 point = pos + normal * offset;
			Vector3 lhs = worldToCameraMatrix.MultiplyPoint(point);
			Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
			return new Vector4(rhs.x, rhs.y, rhs.z, 0f - Vector3.Dot(lhs, rhs));
		}

		public RenderTexture TryGetReflectionTexture(Camera targetCamera)
		{
			if ((bool)targetCamera)
			{
				reflectionCameras.TryGetValue(targetCamera, out m_reflectionCamera);
				if ((bool)m_reflectionCamera)
				{
					return m_reflectionCamera.targetTexture;
				}
			}
			return null;
		}
	}
}
