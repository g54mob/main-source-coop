using CodeStage.AdvancedFPSCounter.CountersData;
using UnityEngine;
using UnityEngine.Rendering;

namespace CodeStage.AdvancedFPSCounter.Utils
{
	[DisallowMultipleComponent]
	public class AFPSRenderRecorder : MonoBehaviour
	{
		private static FPSCounterData currentListener;

		private static bool recording;

		private static float renderTime;

		public static void Add(FPSCounterData counter)
		{
			currentListener = counter;
			if (GraphicsSettings.defaultRenderPipeline == null)
			{
				Camera main = Camera.main;
				if (!(main == null) && !main.TryGetComponent<AFPSRenderRecorder>(out var _))
				{
					main.gameObject.AddComponent<AFPSRenderRecorder>();
				}
			}
			else
			{
				Remove();
				RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
				RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
			}
		}

		public static void Remove()
		{
			if (GraphicsSettings.defaultRenderPipeline == null)
			{
				Camera main = Camera.main;
				if (!(main == null) && main.TryGetComponent<AFPSRenderRecorder>(out var component))
				{
					Object.Destroy(component);
				}
			}
			else
			{
				RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
				RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
			}
		}

		private static void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (camera.cameraType == CameraType.Game)
			{
				BeginRecording();
			}
		}

		private static void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (camera.cameraType == CameraType.Game)
			{
				EndRecording();
			}
		}

		private static void BeginRecording()
		{
			if (!recording && currentListener != null)
			{
				recording = true;
				renderTime = Time.realtimeSinceStartup;
			}
		}

		private static void EndRecording()
		{
			if (recording && currentListener != null)
			{
				recording = false;
				renderTime = Time.realtimeSinceStartup - renderTime;
				currentListener.AddRenderTime(renderTime * 1000f);
			}
		}

		private void OnPreCull()
		{
			BeginRecording();
		}

		private void OnPostRender()
		{
			EndRecording();
		}
	}
}
