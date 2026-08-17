using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class RuntimePreviewGenerator
{
	private class CameraSetup
	{
		private Vector3 position;

		private Quaternion rotation;

		private Color backgroundColor;

		private bool orthographic;

		private float orthographicSize;

		private float nearClipPlane;

		private float farClipPlane;

		private float aspect;

		private int cullingMask;

		private CameraClearFlags clearFlags;

		private RenderTexture targetTexture;

		public void GetSetup(Camera camera)
		{
			position = camera.transform.position;
			rotation = camera.transform.rotation;
			backgroundColor = camera.backgroundColor;
			orthographic = camera.orthographic;
			orthographicSize = camera.orthographicSize;
			nearClipPlane = camera.nearClipPlane;
			farClipPlane = camera.farClipPlane;
			aspect = camera.aspect;
			cullingMask = camera.cullingMask;
			clearFlags = camera.clearFlags;
			targetTexture = camera.targetTexture;
		}

		public void ApplySetup(Camera camera)
		{
			camera.transform.position = position;
			camera.transform.rotation = rotation;
			camera.backgroundColor = backgroundColor;
			camera.orthographic = orthographic;
			camera.orthographicSize = orthographicSize;
			camera.aspect = aspect;
			camera.cullingMask = cullingMask;
			camera.clearFlags = clearFlags;
			if (nearClipPlane < camera.farClipPlane)
			{
				camera.nearClipPlane = nearClipPlane;
				camera.farClipPlane = farClipPlane;
			}
			else
			{
				camera.farClipPlane = farClipPlane;
				camera.nearClipPlane = nearClipPlane;
			}
			camera.targetTexture = targetTexture;
			targetTexture = null;
		}
	}

	private const int PREVIEW_LAYER = 20;

	private static Vector3 PREVIEW_POSITION = new Vector3(-250f, -250f, -250f);

	private static Camera renderCamera;

	private static readonly CameraSetup cameraSetup = new CameraSetup();

	private static readonly Vector3[] boundingBoxPoints = new Vector3[8];

	private static readonly Vector3[] localBoundsMinMax = new Vector3[2];

	private static readonly List<Renderer> renderersList = new List<Renderer>(64);

	private static readonly List<int> layersList = new List<int>(64);

	private static Camera m_internalCamera = null;

	public static Vector3 CamPos;

	public static Quaternion CamRot;

	private static Camera m_previewRenderCamera;

	private static Vector3 m_previewDirection = new Vector3(-0.57735f, -0.57735f, -0.57735f);

	private static float m_padding;

	private static Color m_backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);

	private static bool m_orthographicMode = false;

	private static bool m_useLocalBounds = false;

	private static float m_renderSupersampling = 1f;

	private static bool m_markTextureNonReadable = false;

	private static Camera InternalCamera
	{
		get
		{
			if (m_internalCamera == null)
			{
				m_internalCamera = new GameObject("ModelPreviewGeneratorCamera").AddComponent<Camera>();
				m_internalCamera.enabled = false;
				m_internalCamera.nearClipPlane = 0.01f;
				m_internalCamera.cullingMask = 1048576;
				m_internalCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_internalCamera;
		}
	}

	public static Camera PreviewRenderCamera
	{
		get
		{
			return m_previewRenderCamera;
		}
		set
		{
			m_previewRenderCamera = value;
		}
	}

	public static Vector3 PreviewDirection
	{
		get
		{
			return m_previewDirection;
		}
		set
		{
			m_previewDirection = value.normalized;
		}
	}

	public static float Padding
	{
		get
		{
			return m_padding;
		}
		set
		{
			m_padding = Mathf.Clamp(value, -0.25f, 0.25f);
		}
	}

	public static Color BackgroundColor
	{
		get
		{
			return m_backgroundColor;
		}
		set
		{
			m_backgroundColor = value;
		}
	}

	public static bool OrthographicMode
	{
		get
		{
			return m_orthographicMode;
		}
		set
		{
			m_orthographicMode = value;
		}
	}

	public static bool UseLocalBounds
	{
		get
		{
			return m_useLocalBounds;
		}
		set
		{
			m_useLocalBounds = value;
		}
	}

	public static float RenderSupersampling
	{
		get
		{
			return m_renderSupersampling;
		}
		set
		{
			m_renderSupersampling = Mathf.Max(value, 0.1f);
		}
	}

	public static bool MarkTextureNonReadable
	{
		get
		{
			return m_markTextureNonReadable;
		}
		set
		{
			m_markTextureNonReadable = value;
		}
	}

	public static Texture2D GenerateMaterialPreview(Material material, PrimitiveType previewPrimitive, int width = 64, int height = 64)
	{
		return GenerateMaterialPreviewInternal(material, previewPrimitive, null, null, width, height);
	}

	public static Texture2D GenerateMaterialPreviewWithShader(Material material, PrimitiveType previewPrimitive, Shader shader, string replacementTag, int width = 64, int height = 64)
	{
		return GenerateMaterialPreviewInternal(material, previewPrimitive, shader, replacementTag, width, height);
	}

	public static void GenerateMaterialPreviewAsync(Action<Texture2D> callback, Material material, PrimitiveType previewPrimitive, int width = 64, int height = 64)
	{
		GenerateMaterialPreviewInternal(material, previewPrimitive, null, null, width, height, callback);
	}

	public static void GenerateMaterialPreviewWithShaderAsync(Action<Texture2D> callback, Material material, PrimitiveType previewPrimitive, Shader shader, string replacementTag, int width = 64, int height = 64)
	{
		GenerateMaterialPreviewInternal(material, previewPrimitive, shader, replacementTag, width, height, callback);
	}

	private static Texture2D GenerateMaterialPreviewInternal(Material material, PrimitiveType previewPrimitive, Shader shader, string replacementTag, int width, int height, Action<Texture2D> asyncCallback = null)
	{
		GameObject gameObject = GameObject.CreatePrimitive(previewPrimitive);
		gameObject.gameObject.hideFlags = HideFlags.HideAndDontSave;
		gameObject.GetComponent<Renderer>().sharedMaterial = material;
		try
		{
			return GenerateModelPreviewInternal(gameObject.transform, shader, replacementTag, width, height, shouldCloneModel: false, shouldIgnoreParticleSystems: true, asyncCallback);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		finally
		{
			UnityEngine.Object.DestroyImmediate(gameObject);
		}
		return null;
	}

	public static Texture2D GenerateModelPreview(Transform model, int width = 64, int height = 64, bool shouldCloneModel = false, bool shouldIgnoreParticleSystems = true)
	{
		return GenerateModelPreviewInternal(model, null, null, width, height, shouldCloneModel, shouldIgnoreParticleSystems);
	}

	public static Texture2D GenerateModelPreviewWithShader(Transform model, Shader shader, string replacementTag, int width = 64, int height = 64, bool shouldCloneModel = false, bool shouldIgnoreParticleSystems = true)
	{
		return GenerateModelPreviewInternal(model, shader, replacementTag, width, height, shouldCloneModel, shouldIgnoreParticleSystems);
	}

	public static void GenerateModelPreviewAsync(Action<Texture2D> callback, Transform model, int width = 64, int height = 64, bool shouldCloneModel = false, bool shouldIgnoreParticleSystems = true)
	{
		GenerateModelPreviewInternal(model, null, null, width, height, shouldCloneModel, shouldIgnoreParticleSystems, callback);
	}

	public static void GenerateModelPreviewWithShaderAsync(Action<Texture2D> callback, Transform model, Shader shader, string replacementTag, int width = 64, int height = 64, bool shouldCloneModel = false, bool shouldIgnoreParticleSystems = true)
	{
		GenerateModelPreviewInternal(model, shader, replacementTag, width, height, shouldCloneModel, shouldIgnoreParticleSystems, callback);
	}

	private static Texture2D GenerateModelPreviewInternal(Transform model, Shader shader, string replacementTag, int width, int height, bool shouldCloneModel, bool shouldIgnoreParticleSystems, Action<Texture2D> asyncCallback = null)
	{
		if (!model)
		{
			if (asyncCallback != null)
			{
				asyncCallback(null);
			}
			return null;
		}
		Texture2D result = null;
		Texture2D texture2D = null;
		if (!model.gameObject.scene.IsValid() || !model.gameObject.scene.isLoaded)
		{
			shouldCloneModel = true;
		}
		Transform transform;
		if (shouldCloneModel)
		{
			transform = UnityEngine.Object.Instantiate(model, null, worldPositionStays: false);
			transform.gameObject.hideFlags = HideFlags.HideAndDontSave;
		}
		else
		{
			transform = model;
			layersList.Clear();
			GetLayerRecursively(transform);
		}
		bool flag = IsStatic(model);
		bool activeSelf = transform.gameObject.activeSelf;
		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;
		bool flag2 = false;
		try
		{
			SetupCamera();
			if (!activeSelf)
			{
				transform.gameObject.SetActive(value: true);
			}
			Quaternion quaternion = Quaternion.LookRotation(transform.rotation * m_previewDirection, transform.up);
			Bounds bounds = default(Bounds);
			if (!CalculateBounds(transform, shouldIgnoreParticleSystems, quaternion, out bounds))
			{
				if (asyncCallback != null)
				{
					asyncCallback(null);
				}
				return null;
			}
			renderCamera.aspect = (float)width / (float)height;
			renderCamera.transform.rotation = quaternion;
			renderCamera.transform.position = CamPos;
			renderCamera.transform.rotation = CamRot;
			renderCamera.farClipPlane = (renderCamera.transform.position - bounds.center).magnitude + (m_useLocalBounds ? (bounds.extents.z * 1.01f) : bounds.size.magnitude);
			RenderTexture active = RenderTexture.active;
			RenderTexture renderTexture = null;
			try
			{
				int num = Mathf.RoundToInt((float)width * m_renderSupersampling);
				int num2 = Mathf.RoundToInt((float)height * m_renderSupersampling);
				renderTexture = RenderTexture.GetTemporary(num, num2, 16);
				RenderTexture.active = renderTexture;
				if (m_backgroundColor.a < 1f)
				{
					GL.Clear(clearDepth: true, clearColor: true, m_backgroundColor);
				}
				renderCamera.targetTexture = renderTexture;
				if (!shader)
				{
					renderCamera.Render();
				}
				else
				{
					renderCamera.RenderWithShader(shader, replacementTag ?? string.Empty);
				}
				renderCamera.targetTexture = null;
				if (num != width || num2 != height)
				{
					RenderTexture renderTexture2 = null;
					try
					{
						renderTexture2 = (RenderTexture.active = RenderTexture.GetTemporary(width, height, 16));
						if (m_backgroundColor.a < 1f)
						{
							GL.Clear(clearDepth: true, clearColor: true, m_backgroundColor);
						}
						Graphics.Blit(renderTexture, renderTexture2);
					}
					finally
					{
						if ((bool)renderTexture2)
						{
							RenderTexture.ReleaseTemporary(renderTexture);
							renderTexture = renderTexture2;
						}
					}
				}
				if (asyncCallback != null)
				{
					AsyncGPUReadback.Request(renderTexture, 0, (m_backgroundColor.a < 1f) ? TextureFormat.RGBA32 : TextureFormat.RGB24, delegate(AsyncGPUReadbackRequest asyncResult)
					{
						try
						{
							result = new Texture2D(width, height, (m_backgroundColor.a < 1f) ? TextureFormat.RGBA32 : TextureFormat.RGB24, mipChain: false);
							if (!asyncResult.hasError)
							{
								result.LoadRawTextureData(asyncResult.GetData<byte>());
							}
							else
							{
								Debug.LogWarning("Async thumbnail request failed, falling back to conventional method");
								RenderTexture active2 = RenderTexture.active;
								try
								{
									RenderTexture.active = renderTexture;
									result.ReadPixels(new Rect(0f, 0f, width, height), 0, 0, recalculateMipMaps: false);
								}
								finally
								{
									RenderTexture.active = active2;
								}
							}
							result.Apply(updateMipmaps: false, m_markTextureNonReadable);
							asyncCallback(result);
						}
						finally
						{
							if ((bool)renderTexture)
							{
								RenderTexture.ReleaseTemporary(renderTexture);
							}
						}
					});
					flag2 = true;
				}
				else
				{
					result = new Texture2D(width, height, (m_backgroundColor.a < 1f) ? TextureFormat.RGBA32 : TextureFormat.RGB24, mipChain: false);
					result.ReadPixels(new Rect(0f, 0f, width, height), 0, 0, recalculateMipMaps: false);
					result.Apply(updateMipmaps: false, m_markTextureNonReadable);
					byte[] rawTextureData = result.GetRawTextureData();
					texture2D = new Texture2D(width, height, result.format, mipChain: false);
					texture2D.LoadRawTextureData(rawTextureData);
					RenderTexture.ReleaseTemporary(renderTexture);
				}
			}
			finally
			{
				RenderTexture.active = active;
				if ((bool)renderTexture && !flag2)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		finally
		{
			if (shouldCloneModel)
			{
				UnityEngine.Object.DestroyImmediate(transform.gameObject);
			}
			else
			{
				if (!activeSelf)
				{
					transform.gameObject.SetActive(value: false);
				}
				if (!flag)
				{
					transform.position = position;
					transform.rotation = rotation;
				}
				int index = 0;
				SetLayerRecursively(transform, ref index);
			}
			if (renderCamera == m_previewRenderCamera)
			{
				cameraSetup.ApplySetup(renderCamera);
			}
		}
		if (!flag2 && asyncCallback != null)
		{
			asyncCallback(null);
		}
		return texture2D;
	}

	public static bool CalculateBounds(Transform target, bool shouldIgnoreParticleSystems, Quaternion cameraRotation, out Bounds bounds)
	{
		renderersList.Clear();
		target.GetComponentsInChildren(renderersList);
		Quaternion quaternion = Quaternion.Inverse(cameraRotation);
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		bounds = default(Bounds);
		bool flag = false;
		for (int i = 0; i < renderersList.Count; i++)
		{
			if (!renderersList[i].enabled || (shouldIgnoreParticleSystems && renderersList[i] is ParticleSystemRenderer))
			{
				continue;
			}
			if (m_useLocalBounds)
			{
				Bounds localBounds = renderersList[i].localBounds;
				Transform transform = renderersList[i].transform;
				localBoundsMinMax[0] = localBounds.min;
				localBoundsMinMax[1] = localBounds.max;
				for (int j = 0; j < 2; j++)
				{
					for (int k = 0; k < 2; k++)
					{
						for (int l = 0; l < 2; l++)
						{
							Vector3 rhs = quaternion * transform.TransformPoint(new Vector3(localBoundsMinMax[j].x, localBoundsMinMax[k].y, localBoundsMinMax[l].z));
							vector = Vector3.Min(vector, rhs);
							vector2 = Vector3.Max(vector2, rhs);
						}
					}
				}
				flag = true;
			}
			else if (!flag)
			{
				bounds = renderersList[i].bounds;
				flag = true;
			}
			else
			{
				bounds.Encapsulate(renderersList[i].bounds);
			}
		}
		if (m_useLocalBounds && flag)
		{
			bounds = new Bounds(cameraRotation * ((vector + vector2) * 0.5f), vector2 - vector);
		}
		return flag;
	}

	public static void CalculateCameraPosition(Camera camera, Bounds bounds, float padding = 0f)
	{
		Transform transform = camera.transform;
		Vector3 forward = transform.forward;
		float aspect = camera.aspect;
		if (padding != 0f)
		{
			bounds.size *= 1f + padding * 2f;
		}
		Vector3 center = bounds.center;
		Vector3 extents = bounds.extents;
		Vector3 vector = 2f * extents;
		if (m_useLocalBounds)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(center, camera.transform.rotation, Vector3.one);
			Vector3 point = extents;
			boundingBoxPoints[0] = matrix4x.MultiplyPoint3x4(point);
			point.x -= vector.x;
			boundingBoxPoints[1] = matrix4x.MultiplyPoint3x4(point);
			point.y -= vector.y;
			boundingBoxPoints[2] = matrix4x.MultiplyPoint3x4(point);
			point.x += vector.x;
			boundingBoxPoints[3] = matrix4x.MultiplyPoint3x4(point);
			point.z -= vector.z;
			boundingBoxPoints[4] = matrix4x.MultiplyPoint3x4(point);
			point.x -= vector.x;
			boundingBoxPoints[5] = matrix4x.MultiplyPoint3x4(point);
			point.y += vector.y;
			boundingBoxPoints[6] = matrix4x.MultiplyPoint3x4(point);
			point.x += vector.x;
			boundingBoxPoints[7] = matrix4x.MultiplyPoint3x4(point);
		}
		else
		{
			Vector3 vector2 = center + extents;
			boundingBoxPoints[0] = vector2;
			vector2.x -= vector.x;
			boundingBoxPoints[1] = vector2;
			vector2.y -= vector.y;
			boundingBoxPoints[2] = vector2;
			vector2.x += vector.x;
			boundingBoxPoints[3] = vector2;
			vector2.z -= vector.z;
			boundingBoxPoints[4] = vector2;
			vector2.x -= vector.x;
			boundingBoxPoints[5] = vector2;
			vector2.y += vector.y;
			boundingBoxPoints[6] = vector2;
			vector2.x += vector.x;
			boundingBoxPoints[7] = vector2;
		}
		if (camera.orthographic)
		{
			transform.position = center;
			float num = float.PositiveInfinity;
			float num2 = float.PositiveInfinity;
			float num3 = float.NegativeInfinity;
			float num4 = float.NegativeInfinity;
			for (int i = 0; i < boundingBoxPoints.Length; i++)
			{
				Vector3 vector3 = transform.InverseTransformPoint(boundingBoxPoints[i]);
				if (vector3.x < num)
				{
					num = vector3.x;
				}
				if (vector3.x > num3)
				{
					num3 = vector3.x;
				}
				if (vector3.y < num2)
				{
					num2 = vector3.y;
				}
				if (vector3.y > num4)
				{
					num4 = vector3.y;
				}
			}
			float num5 = extents.magnitude + 1f;
			camera.orthographicSize = Mathf.Max(num4 - num2, (num3 - num) / aspect) * 0.5f;
			transform.position = center - forward * num5;
			return;
		}
		Vector3 up = transform.up;
		Vector3 right = transform.right;
		float num6 = camera.fieldOfView * 0.5f;
		float num7 = Mathf.Atan(Mathf.Tan(num6 * (MathF.PI / 180f)) * aspect) * 57.29578f;
		Vector3 vector4 = Quaternion.AngleAxis(90f + num6, -right) * forward;
		Vector3 vector5 = Quaternion.AngleAxis(90f + num6, right) * forward;
		Vector3 vector6 = Quaternion.AngleAxis(90f + num7, up) * forward;
		Vector3 vector7 = Quaternion.AngleAxis(90f + num7, -up) * forward;
		int num8 = -1;
		int num9 = -1;
		int num10 = -1;
		int num11 = -1;
		for (int j = 0; j < boundingBoxPoints.Length; j++)
		{
			if (num8 < 0 && IsOutermostPointInDirection(j, vector7))
			{
				num8 = j;
			}
			if (num9 < 0 && IsOutermostPointInDirection(j, vector6))
			{
				num9 = j;
			}
			if (num10 < 0 && IsOutermostPointInDirection(j, vector4))
			{
				num10 = j;
			}
			if (num11 < 0 && IsOutermostPointInDirection(j, vector5))
			{
				num11 = j;
			}
		}
		Ray planesIntersection = GetPlanesIntersection(new Plane(vector7, boundingBoxPoints[num8]), new Plane(vector6, boundingBoxPoints[num9]));
		Ray planesIntersection2 = GetPlanesIntersection(new Plane(vector4, boundingBoxPoints[num10]), new Plane(vector5, boundingBoxPoints[num11]));
		FindClosestPointsOnTwoLines(planesIntersection, planesIntersection2, out var closestPointLine, out var closestPointLine2);
		transform.position = ((Vector3.Dot(closestPointLine - closestPointLine2, forward) < 0f) ? closestPointLine : closestPointLine2);
	}

	private static bool IsOutermostPointInDirection(int pointIndex, Vector3 direction)
	{
		Vector3 vector = boundingBoxPoints[pointIndex];
		for (int i = 0; i < boundingBoxPoints.Length; i++)
		{
			if (i != pointIndex && Vector3.Dot(direction, boundingBoxPoints[i] - vector) > 0f)
			{
				return false;
			}
		}
		return true;
	}

	private static Ray GetPlanesIntersection(Plane p1, Plane p2)
	{
		Vector3 vector = Vector3.Cross(p1.normal, p2.normal);
		float sqrMagnitude = vector.sqrMagnitude;
		return new Ray((Vector3.Cross(vector, p2.normal) * p1.distance + Vector3.Cross(p1.normal, vector) * p2.distance) / sqrMagnitude, vector);
	}

	private static void FindClosestPointsOnTwoLines(Ray line1, Ray line2, out Vector3 closestPointLine1, out Vector3 closestPointLine2)
	{
		Vector3 direction = line1.direction;
		Vector3 direction2 = line2.direction;
		float num = Vector3.Dot(direction, direction);
		float num2 = Vector3.Dot(direction, direction2);
		float num3 = Vector3.Dot(direction2, direction2);
		float num4 = num * num3 - num2 * num2;
		Vector3 rhs = line1.origin - line2.origin;
		float num5 = Vector3.Dot(direction, rhs);
		float num6 = Vector3.Dot(direction2, rhs);
		float num7 = (num2 * num6 - num5 * num3) / num4;
		float num8 = (num * num6 - num5 * num2) / num4;
		closestPointLine1 = line1.origin + direction * num7;
		closestPointLine2 = line2.origin + direction2 * num8;
	}

	private static void SetupCamera()
	{
		if ((bool)m_previewRenderCamera)
		{
			cameraSetup.GetSetup(m_previewRenderCamera);
			renderCamera = m_previewRenderCamera;
			renderCamera.nearClipPlane = 0.01f;
			renderCamera.cullingMask = 1048576;
		}
		else
		{
			renderCamera = InternalCamera;
		}
		renderCamera.backgroundColor = m_backgroundColor;
		renderCamera.orthographic = m_orthographicMode;
		renderCamera.clearFlags = ((m_backgroundColor.a < 1f) ? CameraClearFlags.Depth : CameraClearFlags.Color);
	}

	private static bool IsStatic(Transform obj)
	{
		if (obj.gameObject.isStatic)
		{
			return true;
		}
		for (int i = 0; i < obj.childCount; i++)
		{
			if (IsStatic(obj.GetChild(i)))
			{
				return true;
			}
		}
		return false;
	}

	private static void SetLayerRecursively(Transform obj)
	{
		obj.gameObject.layer = 20;
		for (int i = 0; i < obj.childCount; i++)
		{
			SetLayerRecursively(obj.GetChild(i));
		}
	}

	private static void GetLayerRecursively(Transform obj)
	{
		layersList.Add(obj.gameObject.layer);
		for (int i = 0; i < obj.childCount; i++)
		{
			GetLayerRecursively(obj.GetChild(i));
		}
	}

	private static void SetLayerRecursively(Transform obj, ref int index)
	{
		obj.gameObject.layer = layersList[index++];
		for (int i = 0; i < obj.childCount; i++)
		{
			SetLayerRecursively(obj.GetChild(i), ref index);
		}
	}
}
