using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Drawing
{
	[ExecuteAlways]
	[AddComponentMenu("")]
	[HelpURL("http://arongranberg.com/aline/documentation/stable/drawingmanager.html")]
	public class DrawingManager : MonoBehaviour
	{
		private struct GizmoDrawerGroup
		{
			public Type type;

			public ProfilerMarker profilerMarker;

			public List<IDrawGizmos> drawers;

			public bool enabled;
		}

		public DrawingData gizmos;

		private static List<GizmoDrawerGroup> gizmoDrawers = new List<GizmoDrawerGroup>();

		private static List<(Type, IDrawGizmos)> pendingGizmoDrawers = new List<(Type, IDrawGizmos)>();

		private static Dictionary<Type, int> gizmoDrawerIndices = new Dictionary<Type, int>();

		private static bool ignoreAllDrawing;

		private static DrawingManager _instance;

		private bool framePassed;

		private int lastFrameCount = -2147483648;

		private float lastFrameTime = -1f / 0f;

		private int lastFilterFrame;

		[SerializeField]
		private bool actuallyEnabled;

		private RedrawScope previousFrameRedrawScope;

		public static bool allowRenderToRenderTextures = false;

		public static bool drawToAllCameras = false;

		public static float lineWidthMultiplier = 1f;

		private CommandBuffer commandBuffer;

		[NonSerialized]
		private DetectedRenderPipeline detectedRenderPipeline;

		private CustomPass hdrpGlobalPass;

		private static readonly ProfilerMarker MarkerALINE = new ProfilerMarker("ALINE");

		private static readonly ProfilerMarker MarkerCommandBuffer = new ProfilerMarker("Executing command buffer");

		private static readonly ProfilerMarker MarkerFrameTick = new ProfilerMarker("Frame Tick");

		private static readonly ProfilerMarker MarkerFilterDestroyedObjects = new ProfilerMarker("Filter destroyed objects");

		internal static readonly ProfilerMarker MarkerRefreshSelectionCache = new ProfilerMarker("Refresh Selection Cache");

		private static readonly ProfilerMarker MarkerGizmosAllowed = new ProfilerMarker("GizmosAllowed");

		private static readonly ProfilerMarker MarkerDrawGizmos = new ProfilerMarker("DrawGizmos");

		private static readonly ProfilerMarker MarkerSubmitGizmos = new ProfilerMarker("Submit Gizmos");

		private const float NO_DRAWING_TIMEOUT_SECS = 10f;

		public static DrawingManager instance
		{
			get
			{
				if (_instance == null)
				{
					Init();
				}
				return _instance;
			}
		}

		public static void Init()
		{
			if (!(_instance != null))
			{
				GameObject gameObject = new GameObject("RetainedGizmos")
				{
					hideFlags = (HideFlags.HideAndDontSave | HideFlags.HideInInspector)
				};
				_instance = gameObject.AddComponent<DrawingManager>();
				if (Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				if (Application.isBatchMode)
				{
					ignoreAllDrawing = true;
					gizmoDrawers.Clear();
					pendingGizmoDrawers.Clear();
					gizmoDrawerIndices.Clear();
				}
			}
		}

		private void RefreshRenderPipelineMode()
		{
			if (((RenderPipelineManager.currentPipeline != null) ? RenderPipelineManager.currentPipeline.GetType() : null) == typeof(HDRenderPipeline))
			{
				if (detectedRenderPipeline != DetectedRenderPipeline.HDRP)
				{
					detectedRenderPipeline = DetectedRenderPipeline.HDRP;
					hdrpGlobalPass = new AlineHDRPCustomPass();
					CustomPassVolume.RegisterGlobalCustomPass(CustomPassInjectionPoint.AfterPostProcess, hdrpGlobalPass);
					HDRenderPipelineAsset hDRenderPipelineAsset = GraphicsSettings.defaultRenderPipeline as HDRenderPipelineAsset;
					if (hDRenderPipelineAsset != null && !hDRenderPipelineAsset.currentPlatformRenderPipelineSettings.supportCustomPass)
					{
						Debug.LogWarning("ALINE: The current render pipeline has custom pass support disabled. ALINE will not be able to render anything. Please enable custom pass support on your HDRenderPipelineAsset.", hDRenderPipelineAsset);
					}
				}
			}
			else
			{
				if (hdrpGlobalPass != null)
				{
					CustomPassVolume.UnregisterGlobalCustomPass(CustomPassInjectionPoint.AfterPostProcess, hdrpGlobalPass);
					hdrpGlobalPass = null;
				}
				detectedRenderPipeline = DetectedRenderPipeline.BuiltInOrCustom;
			}
		}

		private void OnEnable()
		{
			if (_instance == null)
			{
				_instance = this;
			}
			if (!(_instance != this))
			{
				actuallyEnabled = true;
				if (gizmos == null)
				{
					gizmos = new DrawingData();
				}
				gizmos.frameRedrawScope = new RedrawScope(gizmos);
				Draw.builder = gizmos.GetBuiltInBuilder();
				Draw.ingame_builder = gizmos.GetBuiltInBuilder(renderInGame: true);
				commandBuffer = new CommandBuffer();
				commandBuffer.name = "ALINE Gizmos";
				detectedRenderPipeline = DetectedRenderPipeline.BuiltInOrCustom;
				Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(PostRender));
				RenderPipelineManager.beginContextRendering += BeginContextRendering;
				RenderPipelineManager.beginCameraRendering += BeginCameraRendering;
				RenderPipelineManager.endCameraRendering += EndCameraRendering;
			}
		}

		private void BeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
		{
			RefreshRenderPipelineMode();
		}

		private void BeginFrameRendering(ScriptableRenderContext context, Camera[] cameras)
		{
			RefreshRenderPipelineMode();
		}

		private void BeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
		}

		private void OnDisable()
		{
			if (actuallyEnabled)
			{
				actuallyEnabled = false;
				commandBuffer.Dispose();
				commandBuffer = null;
				Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(PostRender));
				RenderPipelineManager.beginContextRendering -= BeginContextRendering;
				RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
				RenderPipelineManager.endCameraRendering -= EndCameraRendering;
				if (gizmos != null)
				{
					Draw.builder.DiscardAndDisposeInternal();
					Draw.ingame_builder.DiscardAndDisposeInternal();
					gizmos.ClearData();
				}
				if (hdrpGlobalPass != null)
				{
					CustomPassVolume.UnregisterGlobalCustomPass(CustomPassInjectionPoint.AfterPostProcess, hdrpGlobalPass);
					hdrpGlobalPass = null;
				}
			}
		}

		private void OnEditorUpdate()
		{
			framePassed = true;
			CleanupIfNoCameraRendered();
		}

		private void Update()
		{
			if (actuallyEnabled)
			{
				CleanupIfNoCameraRendered();
			}
		}

		private void CleanupIfNoCameraRendered()
		{
			if (Time.frameCount > lastFrameCount + 1)
			{
				CheckFrameTicking();
				gizmos.PostRenderCleanup();
			}
			if (Time.realtimeSinceStartup - lastFrameTime > 10f)
			{
				Draw.builder.DiscardAndDisposeInternal();
				Draw.ingame_builder.DiscardAndDisposeInternal();
				Draw.builder = gizmos.GetBuiltInBuilder();
				Draw.ingame_builder = gizmos.GetBuiltInBuilder(renderInGame: true);
				lastFrameTime = Time.realtimeSinceStartup;
				AddPendingGizmoDrawers();
				RemoveDestroyedGizmoDrawers();
			}
			if (lastFilterFrame - Time.frameCount > 5)
			{
				lastFilterFrame = Time.frameCount;
				AddPendingGizmoDrawers();
				RemoveDestroyedGizmoDrawers();
			}
		}

		internal void ExecuteCustomRenderPass(ScriptableRenderContext context, Camera camera)
		{
			commandBuffer.Clear();
			SubmitFrame(camera, new DrawingData.CommandBufferWrapper
			{
				cmd = commandBuffer
			}, usingRenderPipeline: true);
			context.ExecuteCommandBuffer(commandBuffer);
		}

		private void EndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (detectedRenderPipeline == DetectedRenderPipeline.BuiltInOrCustom)
			{
				ExecuteCustomRenderPass(context, camera);
			}
		}

		private void PostRender(Camera camera)
		{
			commandBuffer.Clear();
			SubmitFrame(camera, new DrawingData.CommandBufferWrapper
			{
				cmd = commandBuffer
			}, usingRenderPipeline: false);
			Graphics.ExecuteCommandBuffer(commandBuffer);
		}

		private void CheckFrameTicking()
		{
			if (Time.frameCount != lastFrameCount)
			{
				framePassed = true;
				lastFrameCount = Time.frameCount;
				lastFrameTime = Time.realtimeSinceStartup;
				previousFrameRedrawScope = gizmos.frameRedrawScope;
				gizmos.frameRedrawScope = new RedrawScope(gizmos);
				Draw.builder.DisposeInternal();
				Draw.ingame_builder.DisposeInternal();
				Draw.builder = gizmos.GetBuiltInBuilder();
				Draw.ingame_builder = gizmos.GetBuiltInBuilder(renderInGame: true);
			}
			else if (framePassed && Application.isPlaying)
			{
				previousFrameRedrawScope.Draw();
			}
			if (framePassed)
			{
				gizmos.TickFramePreRender();
				framePassed = false;
			}
		}

		internal void SubmitFrame(Camera camera, DrawingData.CommandBufferWrapper cmd, bool usingRenderPipeline)
		{
			bool flag = false;
			bool allowCameraDefault = allowRenderToRenderTextures || drawToAllCameras || camera.targetTexture == null || flag;
			CheckFrameTicking();
			Submit(camera, cmd, usingRenderPipeline, allowCameraDefault);
			gizmos.PostRenderCleanup();
		}

		private bool ShouldDrawGizmos(UnityEngine.Object obj)
		{
			return true;
		}

		private static void AddPendingGizmoDrawers()
		{
			for (int i = 0; i < pendingGizmoDrawers.Count; i++)
			{
				(Type, IDrawGizmos) tuple = pendingGizmoDrawers[i];
				Type item = tuple.Item1;
				IDrawGizmos item2 = tuple.Item2;
				int gizmoDrawerIndex = GetGizmoDrawerIndex(item);
				if (gizmoDrawerIndex != -1 && (!(item2 is MonoBehaviour monoBehaviour) || (bool)monoBehaviour))
				{
					gizmoDrawers[gizmoDrawerIndex].drawers.Add(item2);
				}
			}
			pendingGizmoDrawers.Clear();
			if (pendingGizmoDrawers.Capacity > 1024)
			{
				pendingGizmoDrawers.Capacity = 4;
			}
		}

		private static void RemoveDestroyedGizmoDrawers()
		{
			for (int i = 0; i < gizmoDrawers.Count; i++)
			{
				GizmoDrawerGroup gizmoDrawerGroup = gizmoDrawers[i];
				int num = 0;
				for (int j = 0; j < gizmoDrawerGroup.drawers.Count; j++)
				{
					IDrawGizmos drawGizmos = gizmoDrawerGroup.drawers[j];
					if ((bool)(drawGizmos as MonoBehaviour) || (!(drawGizmos is MonoBehaviour) && drawGizmos.Exists))
					{
						gizmoDrawerGroup.drawers[num] = drawGizmos;
						num++;
					}
				}
				gizmoDrawerGroup.drawers.RemoveRange(num, gizmoDrawerGroup.drawers.Count - num);
			}
		}

		public static bool ShouldDrawGizmos(Type type)
		{
			int gizmoDrawerIndex = GetGizmoDrawerIndex(type);
			if (gizmoDrawerIndex == -1)
			{
				return false;
			}
			return gizmoDrawers[gizmoDrawerIndex].enabled;
		}

		private void Submit(Camera camera, DrawingData.CommandBufferWrapper cmd, bool usingRenderPipeline, bool allowCameraDefault)
		{
			bool allowGizmos = false;
			Draw.builder.DisposeInternal();
			Draw.ingame_builder.DisposeInternal();
			gizmos.Render(camera, allowGizmos, cmd, allowCameraDefault);
			Draw.builder = gizmos.GetBuiltInBuilder();
			Draw.ingame_builder = gizmos.GetBuiltInBuilder(renderInGame: true);
		}

		public static void Register(IDrawGizmos item)
		{
			Register(item, item.GetType());
		}

		public static void Register(IDrawGizmos item, Type overrideType)
		{
			if (!ignoreAllDrawing)
			{
				pendingGizmoDrawers.Add((overrideType, item));
			}
		}

		private static int GetGizmoDrawerIndex(Type tp)
		{
			if (!gizmoDrawerIndices.TryGetValue(tp, out var value))
			{
				BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
				MethodInfo obj = tp.GetMethod("DrawGizmos", bindingAttr) ?? tp.GetMethod("Pathfinding.Drawing.IDrawGizmos.DrawGizmos", bindingAttr) ?? tp.GetMethod("Drawing.IDrawGizmos.DrawGizmos", bindingAttr);
				if (obj == null)
				{
					throw new Exception("Could not find the DrawGizmos method in type " + tp.Name);
				}
				if (obj.DeclaringType != typeof(MonoBehaviourGizmos))
				{
					int num = (gizmoDrawerIndices[tp] = gizmoDrawers.Count);
					value = num;
					gizmoDrawers.Add(new GizmoDrawerGroup
					{
						type = tp,
						enabled = true,
						drawers = new List<IDrawGizmos>(),
						profilerMarker = new ProfilerMarker(ProfilerCategory.Render, "Gizmos for " + tp.Name)
					});
				}
				else
				{
					int num = (gizmoDrawerIndices[tp] = -1);
					value = num;
				}
			}
			return value;
		}

		public static CommandBuilder GetBuilder(bool renderInGame = false)
		{
			return instance.gizmos.GetBuilder(renderInGame);
		}

		public static CommandBuilder GetBuilder(RedrawScope redrawScope, bool renderInGame = false)
		{
			return instance.gizmos.GetBuilder(redrawScope, renderInGame);
		}

		public static CommandBuilder GetBuilder(DrawingData.Hasher hasher, RedrawScope redrawScope = default(RedrawScope), bool renderInGame = false)
		{
			return instance.gizmos.GetBuilder(hasher, redrawScope, renderInGame);
		}

		public static bool TryDrawHasher(DrawingData.Hasher hasher, RedrawScope redrawScope = default(RedrawScope))
		{
			return instance.gizmos.Draw(hasher, redrawScope);
		}

		public static RedrawScope GetRedrawScope(GameObject associatedGameObject = null)
		{
			RedrawScope result = new RedrawScope(instance.gizmos);
			result.DrawUntilDispose(associatedGameObject);
			return result;
		}
	}
}
