using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace StylizedGrass
{
	public static class PipelineUtilities
	{
		private const string renderDataListFieldName = "m_RendererDataList";

		public static UniversalRendererData GetRenderer(string guid)
		{
			Debug.LogError("StylizedGrass.PipelineUtilities.GetRenderer() cannot be called in a build, it requires AssetDatabase. References to renderers should be saved beforehand!");
			return null;
		}

		public static void ValidatePipelineRenderers(ScriptableRendererData pass)
		{
			if (pass == null)
			{
				Debug.LogError("Pass is null");
				return;
			}
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
			ScriptableRendererData[] array = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", bindingAttr).GetValue(UniversalRenderPipeline.asset);
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == pass)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				AddRendererToPipeline(pass);
			}
		}

		private static void AddRendererToPipeline(ScriptableRendererData pass)
		{
			if (!(pass == null))
			{
				BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
				ScriptableRendererData[] array = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", bindingAttr).GetValue(UniversalRenderPipeline.asset);
				List<ScriptableRendererData> list = new List<ScriptableRendererData>();
				for (int i = 0; i < array.Length; i++)
				{
					list.Add(array[i]);
				}
				list.Add(pass);
				typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", bindingAttr).SetValue(UniversalRenderPipeline.asset, list.ToArray());
			}
		}

		public static void RemoveRendererFromPipeline(ScriptableRendererData pass)
		{
			if (!(pass == null))
			{
				BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
				List<ScriptableRendererData> list = new List<ScriptableRendererData>((ScriptableRendererData[])typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", bindingAttr).GetValue(UniversalRenderPipeline.asset));
				if (list.Contains(pass))
				{
					list.Remove(pass);
				}
				typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", bindingAttr).SetValue(UniversalRenderPipeline.asset, list.ToArray());
			}
		}

		private static int GetDefaultRendererIndex(UniversalRenderPipelineAsset asset)
		{
			return (int)typeof(UniversalRenderPipelineAsset).GetField("m_DefaultRendererIndex", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(asset);
		}

		public static ScriptableRendererData GetDefaultRenderer()
		{
			if ((bool)UniversalRenderPipeline.asset)
			{
				ScriptableRendererData[] obj = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(UniversalRenderPipeline.asset);
				int defaultRendererIndex = GetDefaultRendererIndex(UniversalRenderPipeline.asset);
				return obj[defaultRendererIndex];
			}
			Debug.LogError("No Universal Render Pipeline is currently active.");
			return null;
		}

		public static bool RenderFeatureAdded<T>(bool addIfMissing = false)
		{
			ScriptableRendererData defaultRenderer = GetDefaultRenderer();
			bool flag = false;
			foreach (ScriptableRendererFeature rendererFeature in defaultRenderer.rendererFeatures)
			{
				if (!(rendererFeature == null) && rendererFeature.GetType() == typeof(T))
				{
					flag = true;
				}
			}
			if (!flag && addIfMissing)
			{
				AddRenderFeature<T>(defaultRenderer);
			}
			return flag;
		}

		public static void AddRenderFeature<T>(ScriptableRendererData forwardRenderer = null, bool persistent = true)
		{
			if (forwardRenderer == null)
			{
				forwardRenderer = GetDefaultRenderer();
			}
			ScriptableRendererFeature scriptableRendererFeature = (ScriptableRendererFeature)ScriptableObject.CreateInstance(typeof(T).ToString());
			scriptableRendererFeature.name = typeof(T).ToString();
			FieldInfo field = typeof(ScriptableRendererData).GetField("m_RendererFeatures", BindingFlags.Instance | BindingFlags.NonPublic);
			List<ScriptableRendererFeature> list = (List<ScriptableRendererFeature>)field.GetValue(forwardRenderer);
			list.Add(scriptableRendererFeature);
			field.SetValue(forwardRenderer, list);
			if (persistent)
			{
				typeof(ScriptableRendererData).GetMethod("OnValidate", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(forwardRenderer, null);
			}
			if (persistent)
			{
				Debug.Log("<b>" + scriptableRendererFeature.name + "</b> was added to the " + forwardRenderer.name + " renderer");
			}
		}

		public static void AssignRendererToCamera(UniversalAdditionalCameraData camData, ScriptableRendererData pass)
		{
			if ((bool)UniversalRenderPipeline.asset)
			{
				if (!pass)
				{
					return;
				}
				ScriptableRendererData[] array = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(UniversalRenderPipeline.asset);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == pass)
					{
						camData.SetRenderer(i);
					}
				}
			}
			else
			{
				Debug.LogError("[StylizedGrassRenderer] No Universal Render Pipeline is currently active.");
			}
		}
	}
}
