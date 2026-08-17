using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Enviro
{
	[AddComponentMenu("Enviro 3/Reflection Probe")]
	[RequireComponent(typeof(ReflectionProbe))]
	[ExecuteInEditMode]
	public class EnviroReflectionProbe : MonoBehaviour
	{
		public bool standalone;

		public bool updateReflectionOnGameTime = true;

		public float reflectionsUpdateTreshhold = 0.025f;

		public bool useTimeSlicing = true;

		public Camera renderCam;

		[HideInInspector]
		public ReflectionProbe myProbe;

		public bool customRendering;

		public bool useFog;

		public Camera bakingCam;

		public int renderId = -1;

		private bool currentMode;

		private int currentRes;

		private RenderTexture cubemap;

		private RenderTexture finalCubemap;

		private RenderTexture mirrorTexture;

		private RenderTexture renderTexture;

		private GameObject renderCamObj;

		private Material mirror;

		private Material bakeMat;

		private Material convolutionMat;

		private Coroutine refreshing;

		private int renderID;

		public HDProbe hdprobe;

		private static Quaternion[] orientations = new Quaternion[6]
		{
			Quaternion.LookRotation(Vector3.right, Vector3.down),
			Quaternion.LookRotation(Vector3.left, Vector3.down),
			Quaternion.LookRotation(Vector3.up, Vector3.forward),
			Quaternion.LookRotation(Vector3.down, Vector3.back),
			Quaternion.LookRotation(Vector3.forward, Vector3.down),
			Quaternion.LookRotation(Vector3.back, Vector3.down)
		};

		private double lastRelfectionUpdate;

		private void OnEnable()
		{
			myProbe = GetComponent<ReflectionProbe>();
			if ((EnviroManager.instance != null && EnviroManager.instance.Reflections != null && !EnviroManager.instance.Reflections.Settings.globalReflections && !standalone) || !(EnviroManager.instance != null))
			{
				return;
			}
			hdprobe = GetComponent<HDProbe>();
			if (!standalone && myProbe != null)
			{
				myProbe.enabled = true;
			}
			if (customRendering)
			{
				if (hdprobe != null)
				{
					hdprobe.mode = ProbeSettings.Mode.Custom;
					CreateCubemap();
					CreateTexturesAndMaterial();
					CreateRenderCamera();
					currentRes = myProbe.resolution;
					StartCoroutine(RefreshFirstTime());
				}
			}
			else if (hdprobe != null)
			{
				hdprobe.mode = ProbeSettings.Mode.Realtime;
				hdprobe.realtimeMode = ProbeSettings.RealtimeMode.OnDemand;
			}
		}

		private void OnDisable()
		{
			Cleanup();
			if (!standalone && myProbe != null)
			{
				myProbe.enabled = false;
			}
			RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
		}

		private void Cleanup()
		{
			if (refreshing != null)
			{
				StopCoroutine(refreshing);
			}
			if (cubemap != null)
			{
				if (renderCam != null)
				{
					renderCam.targetTexture = null;
				}
				Object.DestroyImmediate(cubemap);
			}
			if (renderCamObj != null)
			{
				Object.DestroyImmediate(renderCamObj);
			}
			if (mirrorTexture != null)
			{
				Object.DestroyImmediate(mirrorTexture);
			}
			if (renderTexture != null)
			{
				Object.DestroyImmediate(renderTexture);
			}
		}

		private void CreateRenderCamera()
		{
			if (renderCamObj == null)
			{
				renderCamObj = new GameObject();
				renderCamObj.name = "Reflection Probe Cam";
				renderCamObj.hideFlags = HideFlags.HideAndDontSave;
				renderCam = renderCamObj.AddComponent<Camera>();
				renderCam.gameObject.SetActive(value: true);
				renderCam.cameraType = CameraType.Reflection;
				renderCam.fieldOfView = 90f;
				renderCam.farClipPlane = myProbe.farClipPlane;
				renderCam.nearClipPlane = myProbe.nearClipPlane;
				renderCam.clearFlags = (CameraClearFlags)myProbe.clearFlags;
				renderCam.backgroundColor = myProbe.backgroundColor;
				renderCam.allowHDR = myProbe.hdr;
				renderCam.targetTexture = cubemap;
				renderCam.enabled = false;
			}
		}

		private void UpdateCameraSettings()
		{
			if (renderCam != null)
			{
				renderCam.cullingMask = myProbe.cullingMask;
			}
		}

		private Camera CreateBakingCamera()
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "Reflection Probe Cam";
			Camera camera = gameObject.AddComponent<Camera>();
			camera.enabled = false;
			camera.gameObject.SetActive(value: true);
			camera.cameraType = CameraType.Reflection;
			camera.fieldOfView = 90f;
			camera.farClipPlane = myProbe.farClipPlane;
			camera.nearClipPlane = myProbe.nearClipPlane;
			camera.cullingMask = myProbe.cullingMask;
			camera.clearFlags = (CameraClearFlags)myProbe.clearFlags;
			camera.backgroundColor = myProbe.backgroundColor;
			camera.allowHDR = myProbe.hdr;
			camera.targetTexture = cubemap;
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			return camera;
		}

		private void CreateCubemap()
		{
			if (!(cubemap != null) || myProbe.resolution != currentRes)
			{
				if (cubemap != null)
				{
					cubemap.Release();
					Object.DestroyImmediate(cubemap);
				}
				if (finalCubemap != null)
				{
					finalCubemap.Release();
					Object.DestroyImmediate(finalCubemap);
				}
				int num = (currentRes = myProbe.resolution);
				RenderTextureFormat format = (myProbe.hdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32);
				cubemap = new RenderTexture(num, num, 16, format, RenderTextureReadWrite.Linear);
				cubemap.dimension = TextureDimension.Cube;
				cubemap.useMipMap = true;
				cubemap.autoGenerateMips = false;
				cubemap.name = "Enviro Reflection Temp Cubemap";
				cubemap.filterMode = FilterMode.Trilinear;
				cubemap.Create();
				finalCubemap = new RenderTexture(num, num, 16, format, RenderTextureReadWrite.Linear);
				finalCubemap.dimension = TextureDimension.Cube;
				finalCubemap.useMipMap = true;
				finalCubemap.autoGenerateMips = false;
				finalCubemap.name = "Enviro Reflection Final Cubemap";
				finalCubemap.filterMode = FilterMode.Trilinear;
				finalCubemap.Create();
			}
		}

		private void CreateTexturesAndMaterial()
		{
			if (mirror == null)
			{
				mirror = new Material(Shader.Find("Hidden/Enviro/ReflectionProbe"));
			}
			if (convolutionMat == null)
			{
				convolutionMat = new Material(Shader.Find("Hidden/EnviroCubemapBlur"));
			}
			int resolution = myProbe.resolution;
			RenderTextureFormat format = (myProbe.hdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32);
			if (mirrorTexture == null || mirrorTexture.width != resolution || mirrorTexture.height != resolution)
			{
				if (mirrorTexture != null)
				{
					Object.DestroyImmediate(mirrorTexture);
				}
				mirrorTexture = new RenderTexture(resolution, resolution, 16, format, RenderTextureReadWrite.Linear);
				mirrorTexture.useMipMap = true;
				mirrorTexture.autoGenerateMips = false;
				mirrorTexture.name = "Enviro Reflection Mirror Texture";
				mirrorTexture.Create();
			}
			if (renderTexture == null || renderTexture.width != resolution || renderTexture.height != resolution)
			{
				if (renderTexture != null)
				{
					Object.DestroyImmediate(renderTexture);
				}
				renderTexture = new RenderTexture(resolution, resolution, 16, format, RenderTextureReadWrite.Linear);
				renderTexture.useMipMap = true;
				renderTexture.autoGenerateMips = false;
				renderTexture.name = "Enviro Reflection Target Texture";
				renderTexture.Create();
			}
		}

		public void RefreshReflection(bool timeSlice = false)
		{
			if (customRendering)
			{
				if (refreshing != null)
				{
					return;
				}
				CreateTexturesAndMaterial();
				if (renderCam == null)
				{
					CreateRenderCamera();
				}
				UpdateCameraSettings();
				renderCam.transform.position = base.transform.position;
				renderCam.targetTexture = renderTexture;
				if (Application.isPlaying)
				{
					if (!timeSlice)
					{
						refreshing = StartCoroutine(RefreshInstant(renderTexture, mirrorTexture));
					}
					else
					{
						refreshing = StartCoroutine(RefreshOvertime(renderTexture, mirrorTexture));
					}
				}
				else
				{
					refreshing = StartCoroutine(RefreshInstant(renderTexture, mirrorTexture));
				}
			}
			else if (hdprobe != null)
			{
				hdprobe.RequestRenderNextUpdate();
			}
		}

		private IEnumerator RefreshFirstTime()
		{
			yield return null;
			RefreshReflection();
			RefreshReflection();
		}

		public IEnumerator RefreshUnity()
		{
			yield return null;
			renderId = myProbe.RenderProbe();
		}

		public IEnumerator RefreshInstant(RenderTexture renderTex, RenderTexture mirrorTex)
		{
			CreateCubemap();
			yield return null;
			for (int i = 0; i < 6; i++)
			{
				renderCam.transform.rotation = orientations[i];
				renderCam.Render();
				if (mirrorTex != null)
				{
					Graphics.Blit(renderTex, mirrorTex, mirror);
					Graphics.CopyTexture(mirrorTex, 0, 0, cubemap, i, 0);
				}
			}
			ConvolutionCubemap();
			if (hdprobe != null)
			{
				hdprobe.SetTexture(ProbeSettings.Mode.Custom, finalCubemap);
			}
			refreshing = null;
		}

		public IEnumerator RefreshOvertime(RenderTexture renderTex, RenderTexture mirrorTex)
		{
			CreateCubemap();
			for (int face = 0; face < 6; face++)
			{
				yield return null;
				renderCam.transform.rotation = orientations[face];
				renderCam.Render();
				if (mirrorTex != null)
				{
					Graphics.Blit(renderTex, mirrorTex, mirror);
					Graphics.CopyTexture(mirrorTex, 0, 0, cubemap, face, 0);
				}
			}
			ConvolutionCubemap();
			if (hdprobe != null)
			{
				hdprobe.SetTexture(ProbeSettings.Mode.Custom, finalCubemap);
			}
			refreshing = null;
		}

		public RenderTexture BakeCubemapFace(int face, int res)
		{
			if (bakeMat == null)
			{
				bakeMat = new Material(Shader.Find("Hidden/Enviro/BakeCubemap"));
			}
			if (bakingCam == null)
			{
				bakingCam = CreateBakingCamera();
			}
			bakingCam.transform.rotation = orientations[face];
			RenderTexture temporary = RenderTexture.GetTemporary(res, res, 0, RenderTextureFormat.ARGBFloat);
			bakingCam.targetTexture = temporary;
			bakingCam.Render();
			RenderTexture renderTexture = new RenderTexture(res, res, 0, RenderTextureFormat.ARGBFloat);
			Graphics.Blit(temporary, renderTexture, bakeMat);
			RenderTexture.ReleaseTemporary(temporary);
			return renderTexture;
		}

		private void ClearTextures()
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = renderTexture;
			GL.Clear(clearDepth: true, clearColor: true, Color.clear);
			RenderTexture.active = mirrorTexture;
			GL.Clear(clearDepth: true, clearColor: true, Color.clear);
			RenderTexture.active = active;
		}

		private void ConvolutionCubemap()
		{
			int num = 7;
			GL.PushMatrix();
			GL.LoadOrtho();
			cubemap.GenerateMips();
			float num2 = 1f;
			switch (finalCubemap.width)
			{
			case 16:
				num2 = 1f;
				break;
			case 32:
				num2 = 1f;
				break;
			case 64:
				num2 = 2f;
				break;
			case 128:
				num2 = 4f;
				break;
			case 256:
				num2 = 8f;
				break;
			case 512:
				num2 = 14f;
				break;
			case 1024:
				num2 = 30f;
				break;
			case 2048:
				num2 = 60f;
				break;
			}
			float num3 = finalCubemap.width;
			for (int i = 0; i < num + 1; i++)
			{
				Graphics.CopyTexture(cubemap, 0, i, finalCubemap, 0, i);
				Graphics.CopyTexture(cubemap, 1, i, finalCubemap, 1, i);
				Graphics.CopyTexture(cubemap, 2, i, finalCubemap, 2, i);
				Graphics.CopyTexture(cubemap, 3, i, finalCubemap, 3, i);
				Graphics.CopyTexture(cubemap, 4, i, finalCubemap, 4, i);
				Graphics.CopyTexture(cubemap, 5, i, finalCubemap, 5, i);
				int num4 = i + 1;
				if (num4 == num)
				{
					break;
				}
				float value = num2 * (float)num4 / num3;
				convolutionMat.SetTexture("_MainTex", finalCubemap);
				convolutionMat.SetFloat("_Texel", value);
				convolutionMat.SetFloat("_Level", i);
				convolutionMat.SetPass(0);
				num3 *= 0.75f;
				Graphics.SetRenderTarget(cubemap, num4, CubemapFace.PositiveX);
				GL.Begin(7);
				GL.TexCoord3(1f, 1f, 1f);
				GL.Vertex3(0f, 0f, 1f);
				GL.TexCoord3(1f, -1f, 1f);
				GL.Vertex3(0f, 1f, 1f);
				GL.TexCoord3(1f, -1f, -1f);
				GL.Vertex3(1f, 1f, 1f);
				GL.TexCoord3(1f, 1f, -1f);
				GL.Vertex3(1f, 0f, 1f);
				GL.End();
				Graphics.SetRenderTarget(cubemap, num4, CubemapFace.NegativeX);
				GL.Begin(7);
				GL.TexCoord3(-1f, 1f, -1f);
				GL.Vertex3(0f, 0f, 1f);
				GL.TexCoord3(-1f, -1f, -1f);
				GL.Vertex3(0f, 1f, 1f);
				GL.TexCoord3(-1f, -1f, 1f);
				GL.Vertex3(1f, 1f, 1f);
				GL.TexCoord3(-1f, 1f, 1f);
				GL.Vertex3(1f, 0f, 1f);
				GL.End();
				Graphics.SetRenderTarget(cubemap, num4, CubemapFace.PositiveY);
				GL.Begin(7);
				GL.TexCoord3(-1f, 1f, -1f);
				GL.Vertex3(0f, 0f, 1f);
				GL.TexCoord3(-1f, 1f, 1f);
				GL.Vertex3(0f, 1f, 1f);
				GL.TexCoord3(1f, 1f, 1f);
				GL.Vertex3(1f, 1f, 1f);
				GL.TexCoord3(1f, 1f, -1f);
				GL.Vertex3(1f, 0f, 1f);
				GL.End();
				Graphics.SetRenderTarget(cubemap, num4, CubemapFace.NegativeY);
				GL.Begin(7);
				GL.TexCoord3(-1f, -1f, 1f);
				GL.Vertex3(0f, 0f, 1f);
				GL.TexCoord3(-1f, -1f, -1f);
				GL.Vertex3(0f, 1f, 1f);
				GL.TexCoord3(1f, -1f, -1f);
				GL.Vertex3(1f, 1f, 1f);
				GL.TexCoord3(1f, -1f, 1f);
				GL.Vertex3(1f, 0f, 1f);
				GL.End();
				Graphics.SetRenderTarget(cubemap, num4, CubemapFace.PositiveZ);
				GL.Begin(7);
				GL.TexCoord3(-1f, 1f, 1f);
				GL.Vertex3(0f, 0f, 1f);
				GL.TexCoord3(-1f, -1f, 1f);
				GL.Vertex3(0f, 1f, 1f);
				GL.TexCoord3(1f, -1f, 1f);
				GL.Vertex3(1f, 1f, 1f);
				GL.TexCoord3(1f, 1f, 1f);
				GL.Vertex3(1f, 0f, 1f);
				GL.End();
				Graphics.SetRenderTarget(cubemap, num4, CubemapFace.NegativeZ);
				GL.Begin(7);
				GL.TexCoord3(1f, 1f, -1f);
				GL.Vertex3(0f, 0f, 1f);
				GL.TexCoord3(1f, -1f, -1f);
				GL.Vertex3(0f, 1f, 1f);
				GL.TexCoord3(-1f, -1f, -1f);
				GL.Vertex3(1f, 1f, 1f);
				GL.TexCoord3(-1f, 1f, -1f);
				GL.Vertex3(1f, 0f, 1f);
				GL.End();
			}
			GL.PopMatrix();
		}

		private void UpdateStandaloneReflection()
		{
			if ((EnviroManager.instance.Time.GetDateInHours() > lastRelfectionUpdate + (double)reflectionsUpdateTreshhold || EnviroManager.instance.Time.GetDateInHours() < lastRelfectionUpdate - (double)reflectionsUpdateTreshhold) && updateReflectionOnGameTime)
			{
				lastRelfectionUpdate = EnviroManager.instance.Time.GetDateInHours();
				RefreshReflection(!useTimeSlicing);
			}
		}

		private void Update()
		{
			if (currentMode != customRendering)
			{
				currentMode = customRendering;
				if (customRendering)
				{
					OnEnable();
				}
				else
				{
					OnEnable();
					Cleanup();
				}
			}
			if (EnviroManager.instance != null && standalone)
			{
				UpdateStandaloneReflection();
			}
		}
	}
}
