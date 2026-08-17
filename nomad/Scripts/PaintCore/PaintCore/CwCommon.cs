using System;
using System.Collections.Generic;
using CW.Common;
using UnityEngine;
using UnityEngine.Rendering;

namespace PaintCore
{
	public static class CwCommon
	{
		public const string HelpUrlPrefix = "https://carloswilkes.com/Documentation/PaintCore#";

		public const string ComponentMenuPrefix = "CW/Paint Core/CW ";

		public const string ComponentHitMenuPrefix = "CW/Paint Core/Hit/CW ";

		public static Action<Camera> OnCameraDraw;

		private static int _Coord;

		private static Mesh sphereMesh;

		private static bool sphereMeshSet;

		private static Mesh quadMesh;

		private static bool quadMeshSet;

		private static Texture2D tempReadTexture;

		private static bool invertReadPixels;

		static CwCommon()
		{
			_Coord = Shader.PropertyToID("_Coord");
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, (Camera.CameraCallback)delegate(Camera camera)
			{
				if (OnCameraDraw != null)
				{
					OnCameraDraw(camera);
				}
			});
			RenderPipelineManager.beginCameraRendering += delegate(ScriptableRenderContext context, Camera camera)
			{
				if (OnCameraDraw != null)
				{
					OnCameraDraw(camera);
				}
			};
		}

		public static float RatioToPercentage(float ratio01, int decimalPlaces)
		{
			double num = (double)Mathf.Clamp01(ratio01) * 100.0;
			double num2 = 1.0;
			if (decimalPlaces >= 0)
			{
				num2 = Math.Pow(10.0, decimalPlaces);
			}
			return (float)(Math.Truncate(num * num2) / num2);
		}

		public static RenderTexture GetRenderTexture(RenderTexture other)
		{
			return GetRenderTexture(other.descriptor, other);
		}

		public static RenderTexture GetRenderTexture(RenderTextureDescriptor desc, RenderTexture other)
		{
			RenderTexture renderTexture = GetRenderTexture(desc);
			renderTexture.filterMode = other.filterMode;
			renderTexture.anisoLevel = other.anisoLevel;
			renderTexture.wrapModeU = other.wrapModeU;
			renderTexture.wrapModeV = other.wrapModeV;
			return renderTexture;
		}

		public static RenderTexture GetRenderTexture(RenderTextureDescriptor desc)
		{
			return GetRenderTexture(desc, QualitySettings.activeColorSpace == ColorSpace.Gamma);
		}

		public static RenderTexture GetRenderTexture(RenderTextureDescriptor desc, bool sRGB)
		{
			desc.sRGB = sRGB;
			return CwRenderTextureManager.GetTemporary(desc, "CwCommon GetRenderTexture");
		}

		public static RenderTexture ReleaseRenderTexture(RenderTexture renderTexture)
		{
			return CwRenderTextureManager.ReleaseTemporary(renderTexture);
		}

		public static Quaternion NormalToCameraRotation(Vector3 normal, Camera optionalCamera = null)
		{
			Vector3 up = Vector3.up;
			Camera camera = CwHelper.GetCamera(optionalCamera);
			if (camera != null)
			{
				up = camera.transform.up;
			}
			return Quaternion.LookRotation(-normal, up);
		}

		public static Vector3 GetCameraUp(Camera camera = null)
		{
			camera = CwHelper.GetCamera(camera);
			if (!(camera != null))
			{
				return Vector3.up;
			}
			return camera.transform.up;
		}

		public static bool CanReadPixels(TextureFormat format)
		{
			if (format == TextureFormat.RGBA32 || format == TextureFormat.ARGB32 || format == TextureFormat.RGB24 || format == TextureFormat.RGBAFloat || format == TextureFormat.RGBAHalf)
			{
				return true;
			}
			return false;
		}

		public static void ReadPixelsLinearGamma(Texture2D texture2D, RenderTexture renderTexture)
		{
			if (renderTexture != null)
			{
				CwHelper.BeginActive(renderTexture);
				Texture2D texture2D2 = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, mipChain: false, linear: true);
				texture2D2.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
				CwHelper.EndActive();
				Color[] pixels = texture2D2.GetPixels();
				for (int num = pixels.Length - 1; num >= 0; num--)
				{
					pixels[0] = pixels[0].gamma;
				}
				UnityEngine.Object.DestroyImmediate(texture2D2);
				texture2D.SetPixels(pixels);
				texture2D.Apply();
			}
		}

		public static void ReadPixels(Texture2D texture2D, RenderTexture renderTexture)
		{
			if (renderTexture != null)
			{
				CwHelper.BeginActive(renderTexture);
				if (CanReadPixels(texture2D.format))
				{
					texture2D.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
					CwHelper.EndActive();
					texture2D.Apply();
					return;
				}
				Texture2D texture2D2 = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, mipChain: false);
				texture2D2.ReadPixels(new Rect(0f, 0f, renderTexture.width, renderTexture.height), 0, 0);
				CwHelper.EndActive();
				Color32[] pixels = texture2D2.GetPixels32();
				UnityEngine.Object.DestroyImmediate(texture2D2);
				texture2D.SetPixels32(pixels);
				texture2D.Apply();
			}
		}

		public static bool Downsample(RenderTexture renderTexture, int steps, ref RenderTexture temporary)
		{
			if (steps > 0 && renderTexture != null)
			{
				RenderTexture active = RenderTexture.active;
				RenderTextureDescriptor desc = new RenderTextureDescriptor(renderTexture.width / 2, renderTexture.height / 2, renderTexture.format, 0);
				RenderTexture renderTexture2 = GetRenderTexture(desc);
				CwCommandReplace.BlitFast(renderTexture2, renderTexture, Color.white);
				for (int i = 1; i < steps; i++)
				{
					desc.width /= 2;
					desc.height /= 2;
					renderTexture = renderTexture2;
					renderTexture2 = GetRenderTexture(desc);
					Graphics.Blit(renderTexture, renderTexture2);
					ReleaseRenderTexture(renderTexture);
				}
				temporary = renderTexture2;
				RenderTexture.active = active;
				return true;
			}
			return false;
		}

		public static bool HasMipMaps(Texture texture)
		{
			if (texture != null)
			{
				Texture2D texture2D = texture as Texture2D;
				if (texture2D != null)
				{
					return texture2D.mipmapCount > 0;
				}
				RenderTexture renderTexture = texture as RenderTexture;
				if (renderTexture != null)
				{
					return renderTexture.useMipMap;
				}
			}
			return false;
		}

		public static Mesh GetSphereMesh()
		{
			if (!sphereMeshSet)
			{
				GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				sphereMeshSet = true;
				sphereMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
			return sphereMesh;
		}

		public static Mesh GetMesh(GameObject root, Mesh mesh = null)
		{
			if (mesh == null)
			{
				MeshFilter component = root.GetComponent<MeshFilter>();
				if (component != null)
				{
					return component.sharedMesh;
				}
				SkinnedMeshRenderer component2 = root.GetComponent<SkinnedMeshRenderer>();
				if (component2 != null)
				{
					return component2.sharedMesh;
				}
			}
			return mesh;
		}

		public static Mesh GetQuadMesh()
		{
			if (!quadMeshSet)
			{
				GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
				quadMeshSet = true;
				quadMesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
			return quadMesh;
		}

		public static Color GetPixel(RenderTexture renderTexture, Vector2 uv, bool mipMaps = false)
		{
			if (renderTexture != null)
			{
				if (tempReadTexture == null)
				{
					tempReadTexture = new Texture2D(2, 2, TextureFormat.ARGB32, mipMaps, QualitySettings.activeColorSpace == ColorSpace.Linear);
					tempReadTexture.SetPixel(0, 0, Color.clear);
					tempReadTexture.SetPixel(1, 0, Color.clear);
					tempReadTexture.SetPixel(0, 1, Color.white);
					tempReadTexture.SetPixel(1, 1, Color.white);
					tempReadTexture.Apply();
					RenderTexture temporary = RenderTexture.GetTemporary(2, 2, 0, RenderTextureFormat.ARGB32);
					Graphics.Blit(tempReadTexture, temporary);
					CwHelper.BeginActive(temporary);
					tempReadTexture.ReadPixels(new Rect(0f, 0f, 1f, 1f), 0, 0);
					CwHelper.EndActive();
					RenderTexture.ReleaseTemporary(temporary);
					tempReadTexture.Apply();
					invertReadPixels = tempReadTexture.GetPixel(0, 0) != Color.clear;
				}
				if (invertReadPixels)
				{
					uv.y = 1f - uv.y;
				}
				float x = uv.x * (float)renderTexture.width;
				float y = uv.y * (float)renderTexture.height;
				CwHelper.BeginActive(renderTexture);
				tempReadTexture.ReadPixels(new Rect(x, y, 1f, 1f), 0, 0);
				CwHelper.EndActive();
				tempReadTexture.Apply();
				return CwHelper.ToGamma(tempReadTexture.GetPixel(0, 0));
			}
			return default(Color);
		}

		public static Texture2D GetReadableCopy(Texture texture, TextureFormat format = TextureFormat.ARGB32, bool mipMaps = false, int width = 0, int height = 0)
		{
			Texture2D texture2D = null;
			if (texture != null)
			{
				if (width <= 0)
				{
					width = texture.width;
				}
				if (height <= 0)
				{
					height = texture.height;
				}
				if (CanReadPixels(format))
				{
					RenderTexture renderTexture = GetRenderTexture(new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 0), sRGB: true);
					texture2D = new Texture2D(width, height, format, mipMaps, linear: false);
					CwHelper.BeginActive(renderTexture);
					Graphics.Blit(texture, renderTexture);
					texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
					CwHelper.EndActive();
					ReleaseRenderTexture(renderTexture);
					texture2D.Apply();
				}
			}
			return texture2D;
		}

		public static void SaveBytes(string saveName, byte[] data, bool save = true)
		{
			string value = null;
			if (data != null)
			{
				value = Convert.ToBase64String(data);
			}
			PlayerPrefs.SetString(saveName, value);
			if (save)
			{
				PlayerPrefs.Save();
			}
		}

		public static byte[] LoadBytes(string saveName)
		{
			string text = PlayerPrefs.GetString(saveName);
			if (!string.IsNullOrEmpty(text))
			{
				return Convert.FromBase64String(text);
			}
			return null;
		}

		public static bool SaveExists(string saveName)
		{
			return PlayerPrefs.HasKey(saveName);
		}

		public static void ClearSave(string saveName, bool save = true)
		{
			if (PlayerPrefs.HasKey(saveName))
			{
				PlayerPrefs.DeleteKey(saveName);
				if (save)
				{
					PlayerPrefs.Save();
				}
			}
		}

		public static Vector3 GetPosition(Vector3 position, Vector3 endPosition)
		{
			return (position + endPosition) / 2f;
		}

		public static Vector3 GetPosition(Vector3 positionA, Vector3 positionB, Vector3 positionC)
		{
			return (positionA + positionB + positionC) / 3f;
		}

		public static Vector3 GetPosition(Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2)
		{
			return (position + position2 + endPosition + endPosition2) / 4f;
		}

		public static float GetRadius(Vector3 size)
		{
			return Mathf.Sqrt(Vector3.Dot(size, size));
		}

		public static float GetRadius(Vector3 size, Vector3 position, Vector3 endPosition)
		{
			size.x = Math.Abs(size.x) + Math.Abs(endPosition.x - position.x);
			size.y = Math.Abs(size.y) + Math.Abs(endPosition.y - position.y);
			size.z = Math.Abs(size.z) + Math.Abs(endPosition.z - position.z);
			return GetRadius(size);
		}

		public static float GetRadius(Vector3 size, Vector3 positionA, Vector3 positionB, Vector3 positionC)
		{
			float num = Math.Min(Math.Min(positionA.x, positionB.x), positionC.x);
			float num2 = Math.Max(Math.Max(positionA.x, positionB.x), positionC.x);
			float num3 = Math.Min(Math.Min(positionA.y, positionB.y), positionC.y);
			float num4 = Math.Max(Math.Max(positionA.y, positionB.y), positionC.y);
			float num5 = Math.Min(Math.Min(positionA.z, positionB.z), positionC.z);
			float num6 = Math.Max(Math.Max(positionA.z, positionB.z), positionC.z);
			size.x = Math.Abs(size.x) + Math.Abs(num2 - num);
			size.y = Math.Abs(size.y) + Math.Abs(num4 - num3);
			size.z = Math.Abs(size.z) + Math.Abs(num6 - num5);
			return GetRadius(size);
		}

		public static float GetRadius(Vector3 size, Vector3 position, Vector3 endPosition, Vector3 position2, Vector3 endPosition2)
		{
			float num = Math.Min(Math.Min(position.x, position2.x), Math.Min(endPosition.x, endPosition2.x));
			float num2 = Math.Max(Math.Max(position.x, position2.x), Math.Max(endPosition.x, endPosition2.x));
			float num3 = Math.Min(Math.Min(position.y, position2.y), Math.Min(endPosition.y, endPosition2.y));
			float num4 = Math.Max(Math.Max(position.y, position2.y), Math.Max(endPosition.y, endPosition2.y));
			float num5 = Math.Min(Math.Min(position.z, position2.z), Math.Min(endPosition.z, endPosition2.z));
			float num6 = Math.Max(Math.Max(position.z, position2.z), Math.Max(endPosition.z, endPosition2.z));
			size.x = Math.Abs(size.x) + Math.Abs(num2 - num);
			size.y = Math.Abs(size.y) + Math.Abs(num4 - num3);
			size.z = Math.Abs(size.z) + Math.Abs(num6 - num5);
			return GetRadius(size);
		}

		public static Vector3 ScaleAspect(Vector3 size, float aspect)
		{
			if (aspect > 1f)
			{
				size.y /= aspect;
			}
			else
			{
				size.x *= aspect;
			}
			return size;
		}

		public static float GetAspect(Texture textureA, Texture textureB = null)
		{
			if (textureA != null)
			{
				return (float)textureA.width / (float)textureA.height;
			}
			if (textureB != null)
			{
				return (float)textureB.width / (float)textureB.height;
			}
			return 1f;
		}

		public static void Blit(RenderTexture renderTexture, Texture other)
		{
			RenderTexture active = RenderTexture.active;
			Graphics.Blit(other, renderTexture);
			RenderTexture.active = active;
		}

		public static void Blit(RenderTexture renderTexture, Material material, int pass)
		{
			CwHelper.BeginActive(renderTexture);
			Draw(material, pass);
			CwHelper.EndActive();
		}

		public static Vector4 IndexToVector(int index)
		{
			return index switch
			{
				0 => new Vector4(1f, 0f, 0f, 0f), 
				1 => new Vector4(0f, 1f, 0f, 0f), 
				2 => new Vector4(0f, 0f, 1f, 0f), 
				3 => new Vector4(0f, 0f, 0f, 1f), 
				_ => default(Vector4), 
			};
		}

		public static void Draw(Material material, int pass, Mesh mesh, Matrix4x4 matrix, int subMesh, CwCoord coord)
		{
			material.SetVector(_Coord, IndexToVector((int)coord));
			if (material.SetPass(pass))
			{
				Graphics.DrawMeshNow(mesh, matrix, subMesh);
			}
		}

		public static void Draw(Material material, int pass)
		{
			if (material.SetPass(pass))
			{
				Graphics.DrawMeshNow(GetQuadMesh(), Matrix4x4.identity, 0);
			}
		}

		public static Texture2D CreateTexture(int width, int height, TextureFormat format, bool mipMaps)
		{
			if (width > 0 && height > 0)
			{
				return new Texture2D(width, height, format, mipMaps);
			}
			return null;
		}

		public static Material GetMaterial(Renderer renderer, int materialIndex = 0)
		{
			if (renderer != null && materialIndex >= 0)
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				if (materialIndex < sharedMaterials.Length)
				{
					return sharedMaterials[materialIndex];
				}
			}
			return null;
		}

		public static Material CloneMaterial(GameObject gameObject, int materialIndex = 0)
		{
			if (gameObject != null && materialIndex >= 0)
			{
				Renderer component = gameObject.GetComponent<Renderer>();
				if (component != null)
				{
					Material[] sharedMaterials = component.sharedMaterials;
					if (materialIndex < sharedMaterials.Length)
					{
						Material original = sharedMaterials[materialIndex];
						original = (sharedMaterials[materialIndex] = UnityEngine.Object.Instantiate(original));
						component.sharedMaterials = sharedMaterials;
						return original;
					}
				}
			}
			return null;
		}

		public static Material AddMaterial(Renderer renderer, Shader shader, int materialIndex = -1)
		{
			if (renderer != null)
			{
				List<Material> list = new List<Material>(renderer.sharedMaterials);
				Material material = new Material(shader);
				if (materialIndex <= 0)
				{
					materialIndex = list.Count;
				}
				list.Insert(materialIndex, material);
				renderer.sharedMaterials = list.ToArray();
				return material;
			}
			return null;
		}

		public static Shader LoadShader(string shaderName)
		{
			Shader shader = Shader.Find(shaderName);
			if (shader == null)
			{
				throw new Exception("Failed to find shader called: " + shaderName);
			}
			return shader;
		}

		public static Material BuildMaterial(Shader shader)
		{
			return new Material(shader);
		}

		public static Material BuildMaterial(string shaderName, string keyword = null)
		{
			Material material = BuildMaterial(LoadShader(shaderName));
			material.name = shaderName + keyword;
			if (!string.IsNullOrEmpty(keyword))
			{
				material.EnableKeyword(keyword);
			}
			return material;
		}
	}
}
