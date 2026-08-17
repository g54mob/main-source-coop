using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace CW.Common
{
	public static class CwHelper
	{
		private static Stack<UnityEngine.Random.State> seedStates;

		public static List<Material> tempMaterials;

		public static List<MaterialPropertyBlock> tempProperties;

		private static Stack<RenderTexture> actives;

		private static int uniqueSeed;

		private static Mesh quadMesh;

		private static bool quadMeshSet;

		private static List<Material> materials;

		public static bool IsSRP => GraphicsSettings.currentRenderPipeline != null;

		public static bool IsBIRP => GraphicsSettings.currentRenderPipeline == null;

		public static bool IsURP
		{
			get
			{
				RenderPipelineAsset currentRenderPipeline = GraphicsSettings.currentRenderPipeline;
				if (currentRenderPipeline != null && currentRenderPipeline.GetType().ToString().Contains("Universal"))
				{
					return true;
				}
				return false;
			}
		}

		public static bool IsHDRP
		{
			get
			{
				RenderPipelineAsset currentRenderPipeline = GraphicsSettings.currentRenderPipeline;
				if (currentRenderPipeline != null && currentRenderPipeline.GetType().ToString().Contains("HighDefinition"))
				{
					return true;
				}
				return false;
			}
		}

		public static event Action<Camera> OnCameraPreRender;

		public static event Action<Camera> OnCameraPostRender;

		static CwHelper()
		{
			seedStates = new Stack<UnityEngine.Random.State>();
			tempMaterials = new List<Material>();
			tempProperties = new List<MaterialPropertyBlock>();
			actives = new Stack<RenderTexture>();
			materials = new List<Material>();
			Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, (Camera.CameraCallback)delegate(Camera camera)
			{
				if (CwHelper.OnCameraPreRender != null)
				{
					CwHelper.OnCameraPreRender(camera);
				}
			});
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, (Camera.CameraCallback)delegate(Camera camera)
			{
				if (CwHelper.OnCameraPostRender != null)
				{
					CwHelper.OnCameraPostRender(camera);
				}
			});
			RenderPipelineManager.beginCameraRendering += delegate(ScriptableRenderContext context, Camera camera)
			{
				if (CwHelper.OnCameraPreRender != null)
				{
					CwHelper.OnCameraPreRender(camera);
				}
			};
			RenderPipelineManager.endCameraRendering += delegate(ScriptableRenderContext context, Camera camera)
			{
				if (CwHelper.OnCameraPostRender != null)
				{
					CwHelper.OnCameraPostRender(camera);
				}
			};
		}

		public static T FindAnyObjectByType<T>(bool includeInactive = false) where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindAnyObjectByType<T>(includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude);
		}

		public static T[] FindObjectsByType<T>() where T : UnityEngine.Object
		{
			return UnityEngine.Object.FindObjectsByType<T>(FindObjectsSortMode.None);
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

		public static T GetOrAddComponent<T>(GameObject gameObject, bool recordUndo = true) where T : Component
		{
			if (gameObject != null)
			{
				T val = gameObject.GetComponent<T>();
				if (val == null)
				{
					val = AddComponent<T>(gameObject, recordUndo);
				}
				return val;
			}
			return null;
		}

		public static T AddComponent<T>(GameObject gameObject, bool recordUndo = true) where T : Component
		{
			if (gameObject != null)
			{
				return gameObject.AddComponent<T>();
			}
			return null;
		}

		public static bool IndexInMask(int index, int mask)
		{
			return ((1 << index) & mask) != 0;
		}

		public static Camera GetCamera(Camera currentCamera, GameObject gameObject = null)
		{
			if (currentCamera == null)
			{
				if (gameObject != null)
				{
					currentCamera = gameObject.GetComponent<Camera>();
				}
				if (currentCamera == null)
				{
					currentCamera = Camera.main;
				}
			}
			return currentCamera;
		}

		public static Vector3 GetObserverPosition(Transform observer)
		{
			if (observer != null)
			{
				return observer.position;
			}
			Camera main = Camera.main;
			if (main != null)
			{
				return main.transform.position;
			}
			return Vector3.zero;
		}

		public static bool Enabled(Behaviour b)
		{
			if (b != null)
			{
				return b.isActiveAndEnabled;
			}
			return false;
		}

		public static void BeginSeed()
		{
			uniqueSeed += UnityEngine.Random.Range(-2147483648, 2147483647);
			BeginSeed(uniqueSeed);
		}

		public static void BeginSeed(int newSeed)
		{
			seedStates.Push(UnityEngine.Random.state);
			UnityEngine.Random.InitState(newSeed);
		}

		public static void EndSeed()
		{
			UnityEngine.Random.state = seedStates.Pop();
		}

		public static Color Brighten(Color color, float brightness, bool convertToGamma = true)
		{
			if (convertToGamma)
			{
				color = ToGamma(color);
			}
			color.r *= brightness;
			color.g *= brightness;
			color.b *= brightness;
			return color;
		}

		public static Color Premultiply(Color color)
		{
			color.r *= color.a;
			color.g *= color.a;
			color.b *= color.a;
			return color;
		}

		public static float Saturate(float c)
		{
			if (c >= 0f && c <= 1f)
			{
				return c;
			}
			if (!(c < 0.5f))
			{
				return 1f;
			}
			return 0f;
		}

		public static Color Saturate(Color c)
		{
			c.r = Saturate(c.r);
			c.g = Saturate(c.g);
			c.b = Saturate(c.b);
			c.a = Saturate(c.a);
			return c;
		}

		public static void Resize<T>(List<T> list, int size)
		{
			if (list.Count > size)
			{
				list.RemoveRange(size, list.Count - size);
				return;
			}
			list.Capacity = size;
			for (int i = list.Count; i < size; i++)
			{
				list.Add(default(T));
			}
		}

		public static float Sharpness(float a, float p)
		{
			if (p >= 0f)
			{
				return Mathf.Pow(a, p);
			}
			return 1f - Mathf.Pow(1f - a, 0f - p);
		}

		public static Color ToLinear(Color gamma)
		{
			if (QualitySettings.activeColorSpace == ColorSpace.Linear)
			{
				return gamma.linear;
			}
			return gamma;
		}

		public static float ToLinear(float gamma)
		{
			if (QualitySettings.activeColorSpace == ColorSpace.Linear)
			{
				return Mathf.Pow(gamma, 0.45454544f);
			}
			return gamma;
		}

		public static Color ToGamma(Color linear)
		{
			if (QualitySettings.activeColorSpace == ColorSpace.Linear)
			{
				return linear.gamma;
			}
			return linear;
		}

		public static float ToGamma(float linear)
		{
			if (QualitySettings.activeColorSpace == ColorSpace.Linear)
			{
				return Mathf.Pow(linear, 2.2f);
			}
			return linear;
		}

		public static float UniformScale(Vector3 scale)
		{
			return Math.Max(Math.Max(scale.x, scale.y), scale.z);
		}

		public static void BeginActive(RenderTexture renderTexture)
		{
			actives.Push(RenderTexture.active);
			RenderTexture.active = renderTexture;
		}

		public static void EndActive()
		{
			RenderTexture.active = actives.Pop();
		}

		public static void SetTempMaterial(Material material)
		{
			tempMaterials.Clear();
			tempProperties.Clear();
			tempMaterials.Add(material);
		}

		public static void SetTempMaterial(Material material1, Material material2)
		{
			tempMaterials.Clear();
			tempProperties.Clear();
			tempMaterials.Add(material1);
			tempMaterials.Add(material2);
		}

		public static void SetTempMaterial(List<Material> materials)
		{
			tempMaterials.Clear();
			tempProperties.Clear();
			if (materials != null)
			{
				tempMaterials.AddRange(materials);
			}
		}

		public static void SetTempMaterial(MaterialPropertyBlock properties)
		{
			tempMaterials.Clear();
			tempProperties.Clear();
			tempProperties.Add(properties);
		}

		public static void AddMaterial(Renderer r, Material m)
		{
			if (!(r != null) || !(m != null))
			{
				return;
			}
			Material[] sharedMaterials = r.sharedMaterials;
			materials.Clear();
			Material[] array = sharedMaterials;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == m)
				{
					return;
				}
			}
			array = sharedMaterials;
			foreach (Material material in array)
			{
				if (material != null)
				{
					materials.Add(material);
				}
			}
			materials.Add(m);
			r.sharedMaterials = materials.ToArray();
			materials.Clear();
		}

		public static void ReplaceMaterial(Renderer r, Material m)
		{
			if (!(r != null) || !(m != null))
			{
				return;
			}
			Material[] sharedMaterials = r.sharedMaterials;
			Material[] array = sharedMaterials;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == m)
				{
					return;
				}
			}
			array = sharedMaterials;
			foreach (Material material in array)
			{
				if (material != null && material.shader != m.shader)
				{
					materials.Add(material);
				}
			}
			materials.Add(m);
			r.sharedMaterials = materials.ToArray();
			materials.Clear();
		}

		public static void RemoveMaterial(Renderer r, Material m)
		{
			if (!(r != null))
			{
				return;
			}
			Material[] sharedMaterials = r.sharedMaterials;
			materials.Clear();
			Material[] array = sharedMaterials;
			foreach (Material material in array)
			{
				if (material != null && material != m)
				{
					materials.Add(material);
				}
			}
			r.sharedMaterials = materials.ToArray();
			materials.Clear();
		}

		public static Texture2D CreateTempTexture2D(string name, int width, int height, TextureFormat format = TextureFormat.ARGB32, bool mips = false, bool linear = false)
		{
			return new Texture2D(width, height, format, mips, linear)
			{
				name = name,
				hideFlags = HideFlags.DontSave
			};
		}

		public static Material CreateTempMaterial(string materialName, string shaderName)
		{
			Shader shader = Shader.Find(shaderName);
			if (shader == null)
			{
				Debug.LogError("Failed to find shader: " + shaderName);
			}
			return CreateTempMaterial(materialName, shader);
		}

		public static Material CreateTempMaterial(string materialName, Shader shader)
		{
			return new Material(shader)
			{
				name = materialName,
				hideFlags = HideFlags.HideAndDontSave
			};
		}

		public static Material CreateTempMaterial(string materialName, Material source)
		{
			return new Material(source)
			{
				name = materialName,
				hideFlags = HideFlags.HideAndDontSave
			};
		}

		public static T Destroy<T>(T o) where T : UnityEngine.Object
		{
			if (o != null)
			{
				UnityEngine.Object.Destroy(o);
			}
			return null;
		}

		public static GameObject CreateGameObject(string name, int layer, Transform parent = null, string recordUndo = null)
		{
			return CreateGameObject(name, layer, parent, Vector3.zero, Quaternion.identity, Vector3.one, recordUndo);
		}

		public static GameObject CreateGameObject(string name, int layer, Transform parent, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, string recordUndo = null)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.layer = layer;
			gameObject.transform.SetParent(parent, worldPositionStays: false);
			gameObject.transform.localPosition = localPosition;
			gameObject.transform.localRotation = localRotation;
			gameObject.transform.localScale = localScale;
			return gameObject;
		}

		public static T CreateElement<T>(Transform parent) where T : Component
		{
			GameObject gameObject = new GameObject(typeof(T).Name);
			T val = gameObject.AddComponent<T>();
			if (parent == null || parent.GetComponentInParent<Canvas>() == null)
			{
				Canvas canvas = FindAnyObjectByType<Canvas>();
				if (canvas == null)
				{
					canvas = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)).GetComponent<Canvas>();
					canvas.gameObject.layer = LayerMask.NameToLayer("UI");
					canvas.renderMode = RenderMode.ScreenSpaceOverlay;
					if (EventSystem.current == null)
					{
						new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
					}
				}
				parent = canvas.transform;
			}
			gameObject.layer = parent.gameObject.layer;
			val.transform.SetParent(parent, worldPositionStays: false);
			return val;
		}

		public static float Reciprocal(float v)
		{
			if (v == 0f)
			{
				return 0f;
			}
			return 1f / v;
		}

		public static double Reciprocal(double v)
		{
			if (v == 0.0)
			{
				return 0.0;
			}
			return 1.0 / v;
		}

		public static float Divide(float a, float b)
		{
			if (b == 0f)
			{
				return 0f;
			}
			return a / b;
		}

		public static double Divide(double a, double b)
		{
			if (b == 0.0)
			{
				return 0.0;
			}
			return a / b;
		}

		public static float Acos(float v)
		{
			if (v >= -1f && v <= 1f)
			{
				return (float)Math.Acos(v);
			}
			return 0f;
		}

		public static double Acos(double v)
		{
			if (v >= -1.0 && v <= 1.0)
			{
				return Math.Acos(v);
			}
			return 0.0;
		}

		public static float DampenFactor(float speed, float elapsed)
		{
			if (speed < 0f)
			{
				return 1f;
			}
			return 1f - Mathf.Pow((float)Math.E, (0f - speed) * elapsed);
		}

		public static float DampenFactor(float damping, float deltaTime, float linear)
		{
			return Mathf.Clamp01(DampenFactor(damping, deltaTime) + linear * deltaTime);
		}

		public static float Atan2(Vector2 xy)
		{
			return Mathf.Atan2(xy.x, xy.y);
		}

		public static int Mod(int a, int b)
		{
			int num = a % b;
			if (num < 0)
			{
				return num + b;
			}
			return num;
		}

		public static float Mod(float a, float b)
		{
			float num = a % b;
			if (num < 0f)
			{
				return num + b;
			}
			return num;
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
				RenderTexture temporary = CwRenderTextureManager.GetTemporary(new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 0), "CwHelper GetReadableCopy");
				texture2D = new Texture2D(width, height, format, mipMaps, linear: false);
				BeginActive(temporary);
				Graphics.Blit(texture, temporary);
				texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
				EndActive();
				CwRenderTextureManager.ReleaseTemporary(temporary);
				texture2D.Apply();
			}
			return texture2D;
		}
	}
}
