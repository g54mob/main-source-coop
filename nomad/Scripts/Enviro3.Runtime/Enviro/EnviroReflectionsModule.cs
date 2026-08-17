using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Enviro
{
	[Serializable]
	[ExecuteInEditMode]
	public class EnviroReflectionsModule : EnviroModule
	{
		public EnviroReflections Settings;

		public EnviroReflectionsModule preset;

		public bool showReflectionControls;

		public float lastReflectionUpdate;

		public Vector3 lastReflectionUpdatePos;

		private Coroutine renderReflectionCoroutine;

		private Coroutine waitForProbeCoroutine;

		private Coroutine copyDefaultReflectionCoroutine;

		public override void Enable()
		{
			if (!(EnviroManager.instance == null))
			{
				Setup();
				if (EnviroManager.instance.Objects.globalReflectionProbe != null && Settings.globalReflections)
				{
					EnviroManager.instance.StartCoroutine(WaitToRefreshReflection());
				}
			}
		}

		public override void Disable()
		{
			if (!(EnviroManager.instance == null))
			{
				Cleanup();
			}
		}

		private void Cleanup()
		{
			if (!(EnviroManager.instance == null) && EnviroManager.instance.Objects.globalReflectionProbe != null)
			{
				UnityEngine.Object.DestroyImmediate(EnviroManager.instance.Objects.globalReflectionProbe.gameObject);
			}
		}

		private IEnumerator WaitToRefreshReflection()
		{
			yield return null;
			RenderGlobalReflectionProbe(forced: true);
			UpdateDefaultReflectionTextureMode();
		}

		private void Setup()
		{
			if (EnviroManager.instance.Objects.globalReflectionProbe == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "Global Reflection Probe";
				gameObject.transform.SetParent(EnviroManager.instance.transform);
				gameObject.transform.localPosition = Vector3.zero;
				EnviroManager.instance.Objects.globalReflectionProbe = gameObject.AddComponent<EnviroReflectionProbe>();
			}
		}

		public override void UpdateModule()
		{
			if (!(EnviroManager.instance == null) && EnviroManager.instance.Objects.globalReflectionProbe != null)
			{
				UpdateReflection();
			}
		}

		private void UpdateReflection()
		{
			if (!Settings.globalReflections)
			{
				EnviroManager.instance.Objects.globalReflectionProbe.myProbe.enabled = false;
				UpdateDefaultReflectionTextureMode();
				return;
			}
			EnviroManager.instance.Objects.globalReflectionProbe.myProbe.enabled = true;
			EnviroReflectionProbe globalReflectionProbe = EnviroManager.instance.Objects.globalReflectionProbe;
			SetupProbeSettings(globalReflectionProbe);
			if (EnviroManager.instance.Time != null && (lastReflectionUpdate < EnviroManager.instance.Time.Settings.timeOfDay || lastReflectionUpdate > EnviroManager.instance.Time.Settings.timeOfDay + (Settings.globalReflectionsTimeTreshold + 0.01f)) && Settings.globalReflectionsUpdateOnGameTime)
			{
				RenderGlobalReflectionProbe(forced: false, Settings.customRenderingTimeSlicing);
				lastReflectionUpdate = EnviroManager.instance.Time.Settings.timeOfDay + Settings.globalReflectionsTimeTreshold;
			}
			if ((globalReflectionProbe.transform.position.magnitude > lastReflectionUpdatePos.magnitude + Settings.globalReflectionsPositionTreshold || globalReflectionProbe.transform.position.magnitude < lastReflectionUpdatePos.magnitude - Settings.globalReflectionsPositionTreshold) && Settings.globalReflectionsUpdateOnPosition)
			{
				RenderGlobalReflectionProbe(forced: false, Settings.customRenderingTimeSlicing);
				lastReflectionUpdatePos = globalReflectionProbe.transform.position;
			}
			UpdateDefaultReflectionTextureMode();
		}

		public void RenderGlobalReflectionProbe(bool forced = false, bool timeslice = false)
		{
			EnviroReflectionProbe globalReflectionProbe = EnviroManager.instance.Objects.globalReflectionProbe;
			if (!(globalReflectionProbe == null))
			{
				if (renderReflectionCoroutine != null)
				{
					EnviroManager.instance.StopCoroutine(renderReflectionCoroutine);
					renderReflectionCoroutine = null;
				}
				renderReflectionCoroutine = EnviroManager.instance.StartCoroutine(RenderGlobalReflectionProbeTimed(globalReflectionProbe, timeslice));
			}
		}

		private void CopyDefaultReflectionCubemap(EnviroReflectionProbe probe)
		{
			if (Settings.defaultSkyReflectionTex == null || Settings.defaultSkyReflectionTex.height != probe.myProbe.texture.height || Settings.defaultSkyReflectionTex.width != probe.myProbe.texture.width)
			{
				if (Settings.defaultSkyReflectionTex != null)
				{
					UnityEngine.Object.DestroyImmediate(Settings.defaultSkyReflectionTex);
				}
				Settings.defaultSkyReflectionTex = new Cubemap(probe.myProbe.resolution, probe.myProbe.hdr ? TextureFormat.RGBAHalf : TextureFormat.RGBA32, mipChain: true);
				Settings.defaultSkyReflectionTex.name = "Enviro Default Sky Reflection";
			}
			if (probe.myProbe.texture != null)
			{
				Graphics.CopyTexture(probe.myProbe.texture, Settings.defaultSkyReflectionTex);
			}
		}

		public void UpdateDefaultReflectionTextureMode()
		{
			if (Settings.updateDefaultEnvironmentReflections && Settings.globalReflections)
			{
				RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
				RenderSettings.customReflectionTexture = EnviroManager.instance.Objects.globalReflectionProbe.myProbe.texture;
			}
			else
			{
				RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
			}
		}

		private void SetupProbeSettings(EnviroReflectionProbe probe)
		{
			int resolution = 128;
			switch (Settings.globalReflectionResolution)
			{
			case EnviroReflections.GlobalReflectionResolution.R16:
				resolution = 16;
				break;
			case EnviroReflections.GlobalReflectionResolution.R32:
				resolution = 32;
				break;
			case EnviroReflections.GlobalReflectionResolution.R64:
				resolution = 64;
				break;
			case EnviroReflections.GlobalReflectionResolution.R128:
				resolution = 128;
				break;
			case EnviroReflections.GlobalReflectionResolution.R256:
				resolution = 256;
				break;
			case EnviroReflections.GlobalReflectionResolution.R512:
				resolution = 512;
				break;
			case EnviroReflections.GlobalReflectionResolution.R1024:
				resolution = 1024;
				break;
			case EnviroReflections.GlobalReflectionResolution.R2048:
				resolution = 2048;
				break;
			}
			probe.customRendering = false;
			probe.myProbe.resolution = resolution;
			if (probe.hdprobe != null)
			{
				probe.hdprobe.settingsRaw.cameraSettings.culling.cullingMask = Settings.globalReflectionLayers;
				probe.hdprobe.settingsRaw.influence.boxSize = new Vector3(Settings.globalReflectionsScale, Settings.globalReflectionsScale, Settings.globalReflectionsScale);
				probe.hdprobe.settingsRaw.influence.sphereRadius = Settings.globalReflectionsScale;
				probe.hdprobe.settingsRaw.lighting.multiplier = Settings.globalReflectionsIntensity;
			}
		}

		private IEnumerator CopyDefaultReflectionCustom(EnviroReflectionProbe probe, bool timeslice)
		{
			if (timeslice)
			{
				for (int i = 0; i < 8; i++)
				{
					yield return null;
				}
				CopyDefaultReflectionCubemap(probe);
			}
			else
			{
				yield return null;
				yield return null;
				CopyDefaultReflectionCubemap(probe);
			}
		}

		private void CopyDefaultReflectionUnity(EnviroReflectionProbe probe)
		{
			if (probe.renderId == -1 || probe.myProbe.IsFinishedRendering(probe.renderId))
			{
				CopyDefaultReflectionCubemap(probe);
				return;
			}
			if (waitForProbeCoroutine != null)
			{
				EnviroManager.instance.StopCoroutine(waitForProbeCoroutine);
				waitForProbeCoroutine = null;
			}
			waitForProbeCoroutine = EnviroManager.instance.StartCoroutine(WaitForUnityProbe(probe));
		}

		private IEnumerator WaitForUnityProbe(EnviroReflectionProbe probe)
		{
			yield return null;
			CopyDefaultReflectionUnity(probe);
		}

		private IEnumerator RenderGlobalReflectionProbeTimed(EnviroReflectionProbe probe, bool timeslice)
		{
			if (EnviroManager.instance.Lighting.Settings.setDirectLighting)
			{
				EnviroManager.instance.Lighting.UpdateDirectLightingHDRP();
			}
			if (EnviroManager.instance.Lighting.Settings.setAmbientLighting)
			{
				EnviroManager.instance.Lighting.UpdateAmbientLightingHDRP();
			}
			yield return null;
			probe.RefreshReflection(timeslice);
			yield return null;
			EnviroManager.instance.Lighting.UpdateExposureHDRP();
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroReflections>(JsonUtility.ToJson(preset.Settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroReflectionsModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroReflections>(JsonUtility.ToJson(Settings));
		}
	}
}
