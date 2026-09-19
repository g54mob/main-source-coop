using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CartoonFX
{
	[RequireComponent(typeof(ParticleSystem))]
	[DisallowMultipleComponent]
	public class CFXR_Effect : MonoBehaviour
	{
		[Serializable]
		public class CameraShake
		{
			public enum ShakeSpace
			{
				Screen = 0,
				World = 1
			}

			public static bool editorPreview = true;

			public bool enabled;

			[Space]
			public bool useMainCamera = true;

			public List<Camera> cameras = new List<Camera>();

			[Space]
			public float delay;

			public float duration = 1f;

			public ShakeSpace shakeSpace;

			public Vector3 shakeStrength = new Vector3(0.1f, 0.1f, 0.1f);

			public AnimationCurve shakeCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

			[Space]
			[Range(0f, 0.1f)]
			public float shakesDelay;

			[NonSerialized]
			public bool isShaking;

			private Dictionary<Camera, Vector3> camerasPreRenderPosition = new Dictionary<Camera, Vector3>();

			private Vector3 shakeVector;

			private float delaysTimer;

			private static bool s_CallbackRegistered;

			private static List<CameraShake> s_CameraShakes = new List<CameraShake>();

			private static void OnPreRenderCamera_Static_URP(ScriptableRenderContext context, Camera cam)
			{
				OnPreRenderCamera_Static(cam);
			}

			private static void OnPostRenderCamera_Static_URP(ScriptableRenderContext context, Camera cam)
			{
				OnPostRenderCamera_Static(cam);
			}

			private static void OnPreRenderCamera_Static(Camera cam)
			{
				int count = s_CameraShakes.Count;
				for (int i = 0; i < count; i++)
				{
					s_CameraShakes[i].onPreRenderCamera(cam);
				}
			}

			private static void OnPostRenderCamera_Static(Camera cam)
			{
				for (int num = s_CameraShakes.Count - 1; num >= 0; num--)
				{
					s_CameraShakes[num].onPostRenderCamera(cam);
				}
			}

			private static void RegisterStaticCallback(CameraShake cameraShake)
			{
				s_CameraShakes.Add(cameraShake);
				if (!s_CallbackRegistered)
				{
					if (GraphicsSettings.currentRenderPipeline == null)
					{
						Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(OnPreRenderCamera_Static));
						Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(OnPostRenderCamera_Static));
					}
					else
					{
						RenderPipelineManager.beginCameraRendering += OnPreRenderCamera_Static_URP;
						RenderPipelineManager.endCameraRendering += OnPostRenderCamera_Static_URP;
					}
					s_CallbackRegistered = true;
				}
			}

			private static void UnregisterStaticCallback(CameraShake cameraShake)
			{
				s_CameraShakes.Remove(cameraShake);
				if (s_CallbackRegistered && s_CameraShakes.Count == 0)
				{
					if (GraphicsSettings.currentRenderPipeline == null)
					{
						Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(OnPreRenderCamera_Static));
						Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(OnPostRenderCamera_Static));
					}
					else
					{
						RenderPipelineManager.beginCameraRendering -= OnPreRenderCamera_Static_URP;
						RenderPipelineManager.endCameraRendering -= OnPostRenderCamera_Static_URP;
					}
					s_CallbackRegistered = false;
				}
			}

			private void onPreRenderCamera(Camera cam)
			{
				if (!isShaking || !camerasPreRenderPosition.ContainsKey(cam))
				{
					return;
				}
				camerasPreRenderPosition[cam] = cam.transform.localPosition;
				if (!(Time.timeScale <= 0f))
				{
					switch (shakeSpace)
					{
					case ShakeSpace.Screen:
						cam.transform.localPosition += cam.transform.rotation * shakeVector;
						break;
					case ShakeSpace.World:
						cam.transform.localPosition += shakeVector;
						break;
					}
				}
			}

			private void onPostRenderCamera(Camera cam)
			{
				if (camerasPreRenderPosition.ContainsKey(cam))
				{
					cam.transform.localPosition = camerasPreRenderPosition[cam];
				}
			}

			public void fetchCameras()
			{
				foreach (Camera camera in cameras)
				{
					if (!(camera == null))
					{
						camerasPreRenderPosition.Remove(camera);
					}
				}
				cameras.Clear();
				if (useMainCamera && Camera.main != null)
				{
					cameras.Add(Camera.main);
				}
				foreach (Camera camera2 in cameras)
				{
					if (!(camera2 == null) && !camerasPreRenderPosition.ContainsKey(camera2))
					{
						camerasPreRenderPosition.Add(camera2, Vector3.zero);
					}
				}
			}

			public void StartShake()
			{
				if (isShaking)
				{
					StopShake();
				}
				isShaking = true;
				RegisterStaticCallback(this);
			}

			public void StopShake()
			{
				isShaking = false;
				shakeVector = Vector3.zero;
				UnregisterStaticCallback(this);
			}

			public void animate(float time)
			{
				float num = duration + delay;
				if (time < num)
				{
					if (time < delay)
					{
						return;
					}
					if (!isShaking)
					{
						StartShake();
					}
					float time2 = Mathf.Clamp01(time / num);
					if (shakesDelay > 0f)
					{
						delaysTimer += Time.deltaTime;
						if (delaysTimer < shakesDelay)
						{
							return;
						}
						while (delaysTimer >= shakesDelay)
						{
							delaysTimer -= shakesDelay;
						}
					}
					Vector3 vector = Vector3.Scale(new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value), shakeStrength) * ((!(UnityEngine.Random.value > 0.5f)) ? 1 : (-1));
					shakeVector = vector * shakeCurve.Evaluate(time2) * 1f;
				}
				else if (isShaking)
				{
					StopShake();
				}
			}
		}

		public enum ClearBehavior
		{
			None = 0,
			Disable = 1,
			Destroy = 2
		}

		[Serializable]
		public class AnimatedLight
		{
			public static bool editorPreview = true;

			public Light light;

			public bool loop;

			public bool animateIntensity;

			public float intensityStart = 8f;

			public float intensityEnd;

			public float intensityDuration = 0.5f;

			public AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

			public bool perlinIntensity;

			public float perlinIntensitySpeed = 1f;

			public bool fadeIn;

			public float fadeInDuration = 0.5f;

			public bool fadeOut;

			public float fadeOutDuration = 0.5f;

			public bool animateRange;

			public float rangeStart = 8f;

			public float rangeEnd;

			public float rangeDuration = 0.5f;

			public AnimationCurve rangeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

			public bool perlinRange;

			public float perlinRangeSpeed = 1f;

			public bool animateColor;

			public Gradient colorGradient;

			public float colorDuration = 0.5f;

			public AnimationCurve colorCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

			public bool perlinColor;

			public float perlinColorSpeed = 1f;

			public void animate(float time)
			{
				if (!(light != null))
				{
					return;
				}
				if (animateIntensity)
				{
					float time2 = (loop ? Mathf.Clamp01(time % intensityDuration / intensityDuration) : Mathf.Clamp01(time / intensityDuration));
					time2 = (perlinIntensity ? Mathf.PerlinNoise(Time.time * perlinIntensitySpeed, 0f) : intensityCurve.Evaluate(time2));
					light.intensity = Mathf.LerpUnclamped(intensityEnd, intensityStart, time2);
					if (fadeIn && time < fadeInDuration)
					{
						light.intensity *= Mathf.Clamp01(time / fadeInDuration);
					}
				}
				if (animateRange)
				{
					float time3 = (loop ? Mathf.Clamp01(time % rangeDuration / rangeDuration) : Mathf.Clamp01(time / rangeDuration));
					time3 = (perlinRange ? Mathf.PerlinNoise(Time.time * perlinRangeSpeed, 10f) : rangeCurve.Evaluate(time3));
					light.range = Mathf.LerpUnclamped(rangeEnd, rangeStart, time3);
				}
				if (animateColor)
				{
					float time4 = (loop ? Mathf.Clamp01(time % colorDuration / colorDuration) : Mathf.Clamp01(time / colorDuration));
					time4 = (perlinColor ? Mathf.PerlinNoise(Time.time * perlinColorSpeed, 0f) : colorCurve.Evaluate(time4));
					light.color = colorGradient.Evaluate(time4);
				}
			}

			public void animateFadeOut(float time)
			{
				if (fadeOut && light != null)
				{
					light.intensity *= 1f - Mathf.Clamp01(time / fadeOutDuration);
				}
			}

			public void reset()
			{
				if (light != null)
				{
					if (animateIntensity)
					{
						light.intensity = ((fadeIn || fadeOut) ? 0f : intensityEnd);
					}
					if (animateRange)
					{
						light.range = rangeEnd;
					}
					if (animateColor)
					{
						light.color = colorGradient.Evaluate(1f);
					}
				}
			}
		}

		private const float GLOBAL_CAMERA_SHAKE_MULTIPLIER = 1f;

		private static readonly int _GameObjectWorldPosition = Shader.PropertyToID("_GameObjectWorldPosition");

		public static bool GlobalDisableCameraShake;

		public static bool GlobalDisableLights;

		[Tooltip("Defines an action to execute when the Particle System has completely finished playing and emitting particles.")]
		public ClearBehavior clearBehavior = ClearBehavior.Destroy;

		[Space]
		public CameraShake cameraShake;

		[Space]
		public AnimatedLight[] animatedLights;

		[Tooltip("Defines which Particle System to track to trigger light fading out.\nLeave empty if not using fading out.")]
		public ParticleSystem fadeOutReference;

		private float time;

		private ParticleSystem rootParticleSystem;

		[NonSerialized]
		private MaterialPropertyBlock materialPropertyBlock;

		[NonSerialized]
		private Renderer particleRenderer;

		private const int CHECK_EVERY_N_FRAME = 20;

		private static int GlobalStartFrameOffset;

		private int startFrameOffset;

		private bool isFadingOut;

		private float fadingOutStartTime;

		public void ResetState()
		{
			time = 0f;
			fadingOutStartTime = 0f;
			isFadingOut = false;
			if (animatedLights != null)
			{
				AnimatedLight[] array = animatedLights;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].reset();
				}
			}
			if (cameraShake != null && cameraShake.enabled)
			{
				cameraShake.StopShake();
			}
		}

		private void Awake()
		{
			if (cameraShake != null && cameraShake.enabled)
			{
				cameraShake.fetchCameras();
			}
			startFrameOffset = GlobalStartFrameOffset++;
			particleRenderer = GetComponent<ParticleSystemRenderer>();
			if (particleRenderer.sharedMaterial != null && particleRenderer.sharedMaterial.IsKeywordEnabled("_CFXR_LIGHTING_WPOS_OFFSET"))
			{
				materialPropertyBlock = new MaterialPropertyBlock();
			}
			if (!GraphicsSettings.lightsUseLinearIntensity && animatedLights != null)
			{
				AnimatedLight[] array = animatedLights;
				foreach (AnimatedLight obj in array)
				{
					obj.intensityStart = Mathf.LinearToGammaSpace(obj.intensityStart);
					obj.intensityEnd = Mathf.LinearToGammaSpace(obj.intensityEnd);
				}
			}
		}

		private void OnEnable()
		{
			AnimatedLight[] array = animatedLights;
			foreach (AnimatedLight animatedLight in array)
			{
				if (animatedLight.light != null)
				{
					animatedLight.light.enabled = !GlobalDisableLights;
				}
			}
		}

		private void OnDisable()
		{
			ResetState();
		}

		private void Update()
		{
			time += Time.deltaTime;
			Animate(time);
			if (fadeOutReference != null && !fadeOutReference.isEmitting && (fadeOutReference.isPlaying || isFadingOut))
			{
				FadeOut(time);
			}
			if (clearBehavior != ClearBehavior.None)
			{
				if (rootParticleSystem == null)
				{
					rootParticleSystem = GetComponent<ParticleSystem>();
				}
				if ((Time.renderedFrameCount + startFrameOffset) % 20 == 0 && !rootParticleSystem.IsAlive(withChildren: true))
				{
					if (clearBehavior == ClearBehavior.Destroy)
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
					else
					{
						base.gameObject.SetActive(value: false);
					}
				}
			}
			if (materialPropertyBlock != null)
			{
				particleRenderer.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetVector(_GameObjectWorldPosition, base.transform.position);
				particleRenderer.SetPropertyBlock(materialPropertyBlock);
			}
		}

		public void Animate(float _time)
		{
			if (animatedLights != null && !GlobalDisableLights)
			{
				AnimatedLight[] array = animatedLights;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].animate(_time);
				}
			}
			if (cameraShake != null && cameraShake.enabled && !GlobalDisableCameraShake)
			{
				cameraShake.animate(_time);
			}
		}

		public void FadeOut(float _time)
		{
			if (animatedLights != null)
			{
				if (!isFadingOut)
				{
					isFadingOut = true;
					fadingOutStartTime = _time;
				}
				AnimatedLight[] array = animatedLights;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].animateFadeOut(_time - fadingOutStartTime);
				}
			}
		}
	}
}
