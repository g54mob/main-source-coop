using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Enviro
{
	public static class EnviroHelper
	{
		public static string assetPath = "Assets/Enviro 3 - Sky and Weather";

		public static Vector3 PingPong(Vector3 value)
		{
			Vector3 result = value;
			if (result.x > 1f)
			{
				result.x = -1f;
			}
			else if (result.x < -1f)
			{
				result.x = 1f;
			}
			if (result.y > 1f)
			{
				result.y = -1f;
			}
			else if (result.y < -1f)
			{
				result.y = 1f;
			}
			if (result.z > 1f)
			{
				result.z = -1f;
			}
			else if (result.z < -1f)
			{
				result.z = 1f;
			}
			return result;
		}

		public static Vector2 PingPong(Vector2 value)
		{
			Vector2 result = value;
			if (result.x > 1f)
			{
				result.x = -1f;
			}
			else if (result.x < -1f)
			{
				result.x = 1f;
			}
			if (result.y > 1f)
			{
				result.y = -1f;
			}
			else if (result.y < -1f)
			{
				result.y = 1f;
			}
			return result;
		}

		public static float Remap(float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		public static void DestroyExtended(UnityEngine.Object obj)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(obj);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}

		public static bool CanRenderOnCamera(Camera cam)
		{
			if (EnviroManager.instance != null)
			{
				if (cam.cameraType == CameraType.SceneView || cam.cameraType == CameraType.Reflection)
				{
					return true;
				}
				if (cam == EnviroManager.instance.Camera)
				{
					return true;
				}
				if (EnviroManager.instance.Objects.globalReflectionProbe != null && cam == EnviroManager.instance.Objects.globalReflectionProbe.renderCam)
				{
					return true;
				}
				for (int i = 0; i < EnviroManager.instance.Cameras.Count; i++)
				{
					if (cam == EnviroManager.instance.Cameras[i].camera)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		public static Light GetDirectionalLight()
		{
			Light result = null;
			if (EnviroManager.instance.Lighting != null)
			{
				if (EnviroManager.instance.Lighting.Settings.lightingMode == EnviroLighting.LightingMode.Single)
				{
					if (EnviroManager.instance.Objects.directionalLight != null)
					{
						result = EnviroManager.instance.Objects.directionalLight;
					}
				}
				else if (!EnviroManager.instance.isNight)
				{
					if (EnviroManager.instance.Objects.directionalLight != null)
					{
						result = EnviroManager.instance.Objects.directionalLight;
					}
				}
				else if (EnviroManager.instance.Objects.additionalDirectionalLight != null)
				{
					result = EnviroManager.instance.Objects.additionalDirectionalLight;
				}
			}
			else
			{
				Light[] array = UnityEngine.Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].type == LightType.Directional && array[i].gameObject.activeSelf && array[i].enabled)
					{
						result = array[i];
						break;
					}
				}
			}
			return result;
		}

		public static void CreateBuffer(ref ComputeBuffer buffer, int count, int stride)
		{
			if (buffer == null || buffer.count != count)
			{
				if (buffer != null)
				{
					buffer.Release();
					buffer = null;
				}
				if (count > 0)
				{
					buffer = new ComputeBuffer(count, stride);
				}
			}
		}

		public static void ReleaseComputeBuffer(ref ComputeBuffer buffer)
		{
			if (buffer != null)
			{
				buffer.Release();
			}
			buffer = null;
		}

		public static Vector4 GetProjectionExtents(Camera camera)
		{
			return GetProjectionExtents(camera, 0f, 0f);
		}

		public static Vector4 GetProjectionExtents(Camera camera, float texelOffsetX, float texelOffsetY)
		{
			if (camera == null)
			{
				return Vector4.zero;
			}
			float num = (camera.orthographic ? camera.orthographicSize : Mathf.Tan((float)Math.PI / 360f * camera.fieldOfView));
			float num2 = num * camera.aspect;
			float num3 = num2 / (0.5f * (float)camera.pixelWidth);
			float num4 = num / (0.5f * (float)camera.pixelHeight);
			float z = num3 * texelOffsetX;
			float w = num4 * texelOffsetY;
			return new Vector4(num2, num, z, w);
		}

		public static Vector4 GetProjectionExtents(Camera camera, Camera.StereoscopicEye eye)
		{
			return GetProjectionExtents(camera, eye, 0f, 0f);
		}

		public static Vector4 GetProjectionExtents(Camera camera, Camera.StereoscopicEye eye, float texelOffsetX, float texelOffsetY)
		{
			Matrix4x4 matrix4x = ((!camera.stereoEnabled) ? Matrix4x4.Inverse(camera.projectionMatrix) : Matrix4x4.Inverse(camera.GetStereoProjectionMatrix(eye)));
			Vector3 vector = matrix4x.MultiplyPoint3x4(new Vector3(-1f, -1f, 0.95f));
			Vector3 vector2 = matrix4x.MultiplyPoint3x4(new Vector3(1f, 1f, 0.95f));
			vector /= 0f - vector.z;
			vector2 /= 0f - vector2.z;
			float num = 0.5f * (vector2.x - vector.x);
			float num2 = 0.5f * (vector2.y - vector.y);
			float num3 = num / (0.5f * (float)camera.pixelWidth);
			float num4 = num2 / (0.5f * (float)camera.pixelHeight);
			float z = 0.5f * (vector2.x + vector.x) + num3 * texelOffsetX;
			float w = 0.5f * (vector2.y + vector.y) + num4 * texelOffsetY;
			return new Vector4(num, num2, z, w);
		}

		public static EnviroQuality GetQualityForCamera(Camera cam)
		{
			if (EnviroManager.instance.Quality != null)
			{
				EnviroQuality result = EnviroManager.instance.Quality.Settings.defaultQuality;
				for (int i = 0; i < EnviroManager.instance.Cameras.Count; i++)
				{
					if (EnviroManager.instance.Cameras[i].camera != null && EnviroManager.instance.Cameras[i].camera == cam && EnviroManager.instance.Cameras[i].quality != null)
					{
						result = EnviroManager.instance.Cameras[i].quality;
						break;
					}
				}
				return result;
			}
			return null;
		}

		public static bool ResetMatrix(Camera cam)
		{
			for (int i = 0; i < EnviroManager.instance.Cameras.Count; i++)
			{
				if (EnviroManager.instance.Cameras[i].camera != null && EnviroManager.instance.Cameras[i].camera == cam)
				{
					return EnviroManager.instance.Cameras[i].resetMatrix;
				}
			}
			return false;
		}

		public static EnviroModule GetDefaultPreset(string name)
		{
			return null;
		}

		public static EnviroConfiguration GetConfig(string name)
		{
			return null;
		}

		public static VolumeProfile GetDefaultSkyAndFogProfile(string name)
		{
			return null;
		}
	}
}
