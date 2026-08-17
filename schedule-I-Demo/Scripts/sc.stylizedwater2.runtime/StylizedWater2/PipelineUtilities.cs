using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace StylizedWater2
{
	public static class PipelineUtilities
	{
		private const string renderDataListFieldName = "m_RendererDataList";

		private const string renderFeaturesListFieldName = "m_RendererFeatures";

		private static GUIContent[] _rendererDisplayList;

		private static int[] _rendererIndexList;

		public static GUIContent[] rendererDisplayList
		{
			get
			{
				if (_rendererDisplayList == null)
				{
					RefreshRendererList();
				}
				return _rendererDisplayList;
			}
		}

		public static int[] rendererIndexList
		{
			get
			{
				if (_rendererIndexList == null)
				{
					RefreshRendererList();
				}
				return _rendererIndexList;
			}
		}

		private static ScriptableRendererData[] GetRenderDataList(UniversalRenderPipelineAsset asset)
		{
			FieldInfo field = typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field != null)
			{
				return (ScriptableRendererData[])field.GetValue(asset);
			}
			throw new Exception("Reflection failed on field \"m_RendererDataList\" from class \"UniversalRenderPipelineAsset\". URP API likely changed");
		}

		public static void RefreshRendererList()
		{
			if (UniversalRenderPipeline.asset == null)
			{
				Debug.LogError("No pipeline is active, do not display UI that uses this function if it isn't!");
			}
			ScriptableRendererData[] renderDataList = GetRenderDataList(UniversalRenderPipeline.asset);
			_rendererDisplayList = new GUIContent[renderDataList.Length + 1];
			int defaultRendererIndex = GetDefaultRendererIndex(UniversalRenderPipeline.asset);
			_rendererDisplayList[0] = new GUIContent("Default (" + renderDataList[defaultRendererIndex].name + ")");
			for (int i = 1; i < _rendererDisplayList.Length; i++)
			{
				if (renderDataList[i - 1] != null)
				{
					_rendererDisplayList[i] = new GUIContent(i - 1 + ": " + renderDataList[i - 1].name);
				}
				else
				{
					_rendererDisplayList[i] = new GUIContent("(Missing)");
				}
			}
			_rendererIndexList = new int[renderDataList.Length + 1];
			for (int j = 0; j < _rendererIndexList.Length; j++)
			{
				_rendererIndexList[j] = j - 1;
			}
		}

		public static int ValidateRenderer(int index)
		{
			if ((bool)UniversalRenderPipeline.asset)
			{
				int defaultRendererIndex = GetDefaultRendererIndex(UniversalRenderPipeline.asset);
				ScriptableRendererData[] renderDataList = GetRenderDataList(UniversalRenderPipeline.asset);
				if (index == -1)
				{
					index = defaultRendererIndex;
				}
				if (index >= renderDataList.Length || !(renderDataList[index] != null))
				{
					Debug.LogWarning("Renderer at <b>index " + index + "</b> is missing, falling back to Default Renderer. <b>" + renderDataList[defaultRendererIndex].name + "</b>", UniversalRenderPipeline.asset);
					return defaultRendererIndex;
				}
				return index;
			}
			Debug.LogError("No Universal Render Pipeline is currently active.");
			return 0;
		}

		public static bool IsRendererAdded(ScriptableRendererData renderer)
		{
			if (renderer == null)
			{
				Debug.LogError("Pass is null");
				return false;
			}
			if ((bool)UniversalRenderPipeline.asset)
			{
				ScriptableRendererData[] renderDataList = GetRenderDataList(UniversalRenderPipeline.asset);
				bool result = false;
				for (int i = 0; i < renderDataList.Length; i++)
				{
					if (renderDataList[i] == renderer)
					{
						result = true;
					}
				}
				return result;
			}
			Debug.LogError("No Universal Render Pipeline is currently active.");
			return false;
		}

		private static int AddRendererToPipeline(ScriptableRendererData renderer)
		{
			if (renderer == null)
			{
				return -1;
			}
			if ((bool)UniversalRenderPipeline.asset)
			{
				ScriptableRendererData[] renderDataList = GetRenderDataList(UniversalRenderPipeline.asset);
				List<ScriptableRendererData> list = new List<ScriptableRendererData>();
				for (int i = 0; i < renderDataList.Length; i++)
				{
					list.Add(renderDataList[i]);
				}
				list.Add(renderer);
				int result = list.Count - 1;
				typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(UniversalRenderPipeline.asset, list.ToArray());
				RefreshRendererList();
				return result;
			}
			Debug.LogError("No Universal Render Pipeline is currently active.");
			return -1;
		}

		private static int GetDefaultRendererIndex(UniversalRenderPipelineAsset asset)
		{
			return (int)typeof(UniversalRenderPipelineAsset).GetField("m_DefaultRendererIndex", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(asset);
		}

		public static ScriptableRendererData GetDefaultRenderer(UniversalRenderPipelineAsset asset = null)
		{
			if (asset == null)
			{
				asset = UniversalRenderPipeline.asset;
			}
			if ((bool)asset)
			{
				ScriptableRendererData[] renderDataList = GetRenderDataList(asset);
				int defaultRendererIndex = GetDefaultRendererIndex(asset);
				return renderDataList[defaultRendererIndex];
			}
			throw new Exception("No Universal Render Pipeline is currently active.");
		}

		public static ScriptableRendererFeature GetRenderFeature<T>()
		{
			ScriptableRendererData defaultRenderer = GetDefaultRenderer();
			if ((bool)defaultRenderer)
			{
				foreach (ScriptableRendererFeature rendererFeature in defaultRenderer.rendererFeatures)
				{
					if ((bool)rendererFeature && rendererFeature.GetType() == typeof(T))
					{
						return rendererFeature;
					}
				}
			}
			return null;
		}

		public static bool RenderFeatureAdded<T>(ScriptableRendererData renderer = null, bool addIfMissing = false)
		{
			if (renderer == null)
			{
				renderer = GetDefaultRenderer();
			}
			bool flag = false;
			foreach (ScriptableRendererFeature rendererFeature in renderer.rendererFeatures)
			{
				if (!(rendererFeature == null) && rendererFeature.GetType() == typeof(T))
				{
					flag = true;
				}
			}
			if (!flag && addIfMissing)
			{
				AddRenderFeature<T>(renderer);
			}
			return flag;
		}

		public static bool RenderFeatureMissing<T>(out ScriptableRendererData[] renderers)
		{
			List<ScriptableRendererData> list = new List<ScriptableRendererData>();
			RenderPipelineAsset[] allConfiguredRenderPipelines = GraphicsSettings.allConfiguredRenderPipelines;
			for (int i = 0; i < allConfiguredRenderPipelines.Length; i++)
			{
				ScriptableRendererData defaultRenderer = GetDefaultRenderer((UniversalRenderPipelineAsset)allConfiguredRenderPipelines[i]);
				if (!RenderFeatureAdded<T>(defaultRenderer))
				{
					list.Add(defaultRenderer);
				}
			}
			renderers = list.ToArray();
			return list.Count > 0;
		}

		public static void SetupRenderFeature<T>(string name = "")
		{
			RenderPipelineAsset[] allConfiguredRenderPipelines = GraphicsSettings.allConfiguredRenderPipelines;
			for (int i = 0; i < allConfiguredRenderPipelines.Length; i++)
			{
				ScriptableRendererData defaultRenderer = GetDefaultRenderer((UniversalRenderPipelineAsset)allConfiguredRenderPipelines[i]);
				if (!RenderFeatureAdded<T>(defaultRenderer))
				{
					AddRenderFeature<T>(defaultRenderer, name);
				}
			}
		}

		public static ScriptableRendererFeature AddRenderFeature<T>(ScriptableRendererData renderer = null, string name = "")
		{
			if (renderer == null)
			{
				renderer = GetDefaultRenderer();
			}
			ScriptableRendererFeature scriptableRendererFeature = (ScriptableRendererFeature)ScriptableObject.CreateInstance(typeof(T).ToString());
			scriptableRendererFeature.name = ((name == string.Empty) ? typeof(T).ToString() : name);
			FieldInfo field = typeof(ScriptableRendererData).GetField("m_RendererFeatures", BindingFlags.Instance | BindingFlags.NonPublic);
			List<ScriptableRendererFeature> list = (List<ScriptableRendererFeature>)field.GetValue(renderer);
			list.Add(scriptableRendererFeature);
			field.SetValue(renderer, list);
			typeof(ScriptableRendererData).GetMethod("OnValidate", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(renderer, null);
			Debug.Log("<b>" + scriptableRendererFeature.name + "</b> was added to the <i>" + renderer.name + "</i> renderer");
			return scriptableRendererFeature;
		}

		public static bool IsRenderFeatureEnabled<T>(ScriptableRendererData forwardRenderer = null, bool autoEnable = false)
		{
			if (!UniversalRenderPipeline.asset)
			{
				return true;
			}
			if (forwardRenderer == null)
			{
				forwardRenderer = GetDefaultRenderer();
			}
			foreach (ScriptableRendererFeature item in (List<ScriptableRendererFeature>)typeof(ScriptableRendererData).GetField("m_RendererFeatures", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(forwardRenderer))
			{
				if ((bool)item && item.GetType() == typeof(T))
				{
					if (!item.isActive && autoEnable)
					{
						item.SetActive(active: true);
					}
					return item.isActive;
				}
			}
			return true;
		}

		public static void ToggleRenderFeature<T>(bool state)
		{
			foreach (ScriptableRendererFeature rendererFeature in GetDefaultRenderer().rendererFeatures)
			{
				if ((bool)rendererFeature && rendererFeature.GetType() == typeof(T))
				{
					rendererFeature.SetActive(state);
				}
			}
		}

		public static void CreateAndAssignNewRenderer(out int index, out string path)
		{
			GetDefaultRenderer();
			path = string.Empty;
			ScriptableRendererData renderer = CreateEmptyRenderer("Planar Reflections Renderer", path);
			index = AddRendererToPipeline(renderer);
		}

		public static UniversalRendererData CreateEmptyRenderer(string name = "", string folder = "")
		{
			ScriptableRendererData defaultRenderer = GetDefaultRenderer();
			UniversalRendererData universalRendererData = ScriptableObject.CreateInstance<UniversalRendererData>();
			_ = (UniversalRendererData)defaultRenderer;
			universalRendererData.name = name;
			universalRendererData.rendererFeatures.Clear();
			return universalRendererData;
		}

		public static void RemoveRendererFromPipeline(ScriptableRendererData renderer)
		{
			if (renderer == null)
			{
				return;
			}
			if ((bool)UniversalRenderPipeline.asset)
			{
				BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
				List<ScriptableRendererData> list = new List<ScriptableRendererData>(GetRenderDataList(UniversalRenderPipeline.asset));
				if (list.Contains(renderer))
				{
					list.Remove(renderer);
					typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", bindingAttr).SetValue(UniversalRenderPipeline.asset, list.ToArray());
				}
			}
			else
			{
				Debug.LogError("No Universal Render Pipeline is currently active.");
			}
		}

		public static void AssignRendererToCamera(UniversalAdditionalCameraData camData, ScriptableRendererData renderer)
		{
			if ((bool)UniversalRenderPipeline.asset)
			{
				if (!renderer)
				{
					return;
				}
				ScriptableRendererData[] renderDataList = GetRenderDataList(UniversalRenderPipeline.asset);
				for (int i = 0; i < renderDataList.Length; i++)
				{
					if (renderDataList[i] == renderer)
					{
						camData.SetRenderer(i);
					}
				}
			}
			else
			{
				Debug.LogError("No Universal Render Pipeline is currently active.");
			}
		}

		public static bool IsDepthTextureOptionDisabledAnywhere()
		{
			bool flag = false;
			for (int i = 0; i < GraphicsSettings.allConfiguredRenderPipelines.Length; i++)
			{
				if (!(GraphicsSettings.allConfiguredRenderPipelines[i].GetType() != typeof(UniversalRenderPipelineAsset)))
				{
					UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.allConfiguredRenderPipelines[i];
					flag |= !universalRenderPipelineAsset.supportsCameraDepthTexture;
				}
			}
			return flag;
		}

		public static void SetDepthTextureOnAllAssets(bool state)
		{
			for (int i = 0; i < GraphicsSettings.allConfiguredRenderPipelines.Length; i++)
			{
				if (!(GraphicsSettings.allConfiguredRenderPipelines[i].GetType() != typeof(UniversalRenderPipelineAsset)))
				{
					((UniversalRenderPipelineAsset)GraphicsSettings.allConfiguredRenderPipelines[i]).supportsCameraDepthTexture = state;
				}
			}
		}

		public static bool IsOpaqueTextureOptionDisabledAnywhere()
		{
			bool result = false;
			for (int i = 0; i < GraphicsSettings.allConfiguredRenderPipelines.Length; i++)
			{
				if (!(GraphicsSettings.allConfiguredRenderPipelines[i].GetType() != typeof(UniversalRenderPipelineAsset)) && !((UniversalRenderPipelineAsset)GraphicsSettings.allConfiguredRenderPipelines[i]).supportsCameraOpaqueTexture)
				{
					return true;
				}
			}
			return result;
		}

		public static void SetOpaqueTextureOnAllAssets(bool state)
		{
			for (int i = 0; i < GraphicsSettings.allConfiguredRenderPipelines.Length; i++)
			{
				if (!(GraphicsSettings.allConfiguredRenderPipelines[i].GetType() != typeof(UniversalRenderPipelineAsset)))
				{
					((UniversalRenderPipelineAsset)GraphicsSettings.allConfiguredRenderPipelines[i]).supportsCameraOpaqueTexture = state;
				}
			}
		}

		public static bool TransparentShadowsEnabled()
		{
			if (!UniversalRenderPipeline.asset)
			{
				return false;
			}
			UniversalRendererData universalRendererData = (UniversalRendererData)GetDefaultRenderer();
			if (!universalRendererData)
			{
				return false;
			}
			return universalRendererData.shadowTransparentReceive;
		}

		public static bool IsDepthAfterTransparents()
		{
			if (!UniversalRenderPipeline.asset)
			{
				return false;
			}
			return ((UniversalRendererData)GetDefaultRenderer()).copyDepthMode == CopyDepthMode.AfterTransparents;
		}

		public static bool VREnabled()
		{
			return XRGraphics.enabled;
		}
	}
}
