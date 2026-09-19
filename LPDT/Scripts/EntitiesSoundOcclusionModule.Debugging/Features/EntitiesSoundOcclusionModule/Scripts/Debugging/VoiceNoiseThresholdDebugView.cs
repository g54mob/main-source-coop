using System;
using System.Globalization;
using Features.VoiceSpeakersModule.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.EntitiesSoundOcclusionModule.Scripts.Debugging
{
	public class VoiceNoiseThresholdDebugView : MonoBehaviour
	{
		private readonly struct VoiceNoiseSample
		{
			public int PlayerId { get; }

			public float Loudness { get; }

			public float Threshold { get; }

			public float Radius { get; }

			public float MaxRadius { get; }

			public bool WouldEmit { get; }

			public VoiceNoiseSample(int playerId, float loudness, float threshold, float radius, float maxRadius, bool wouldEmit)
			{
				PlayerId = playerId;
				Loudness = loudness;
				Threshold = threshold;
				Radius = radius;
				MaxRadius = maxRadius;
				WouldEmit = wouldEmit;
			}
		}

		[Header("History")]
		[SerializeField]
		private int _columnCount = 96;

		[SerializeField]
		private float _secondsPerColumn = 0.05f;

		[Header("Layout")]
		[SerializeField]
		private float _barWidth = 5f;

		[SerializeField]
		private float _barSpacing = 1f;

		[SerializeField]
		private float _maxBarHeight = 180f;

		[SerializeField]
		private Vector2 _panelOffset = new Vector2(0f, 100f);

		[Header("Threshold")]
		[SerializeField]
		private float _thresholdStep = 0.01f;

		[Header("Color")]
		[SerializeField]
		private Color _blockedColor = new Color(0.35f, 0.75f, 1f, 0.9f);

		[SerializeField]
		private Color _emittedColor = new Color(1f, 0.28f, 0.16f, 0.95f);

		[SerializeField]
		private Color _idleColor = new Color(1f, 1f, 1f, 0.15f);

		[SerializeField]
		private Color _thresholdColor = new Color(1f, 0.9f, 0.1f, 1f);

		private EntitiesSoundOcclusionConfiguration _configuration;

		private PlayerLipSyncController[] _lipSyncControllers = Array.Empty<PlayerLipSyncController>();

		private RectTransform _content;

		private Image[] _bars;

		private RectTransform[] _barRects;

		private float[] _values;

		private bool[] _emitted;

		private RectTransform _thresholdLine;

		private Text _statusText;

		private Text _thresholdText;

		private Slider _thresholdSlider;

		private float _scrollTimer;

		private float _step;

		private int _slotCount;

		private float _initialThreshold;

		private VoiceNoiseSample _latestSample;

		private bool _hasSample;

		private bool _isBuilt;

		private bool _isInjected;

		private float _controllerRefreshTimer;

		[Inject]
		public void InjectDependencies(EntitiesSoundOcclusionConfiguration configuration)
		{
			AssignDependencies(configuration);
		}

		private void Awake()
		{
			TryResolveDependencies();
			if (_isInjected)
			{
				BuildUi();
			}
		}

		private void OnEnable()
		{
			TryResolveDependencies();
			if (_isInjected && !_isBuilt)
			{
				BuildUi();
			}
		}

		private void Update()
		{
			if (_isBuilt)
			{
				RefreshControllersIfNeeded();
				SampleVoice();
				_scrollTimer += Time.deltaTime;
				while (_scrollTimer >= _secondsPerColumn)
				{
					_scrollTimer -= _secondsPerColumn;
					ShiftLeft();
				}
				float num = _scrollTimer / _secondsPerColumn;
				_content.anchoredPosition = new Vector2((0f - num) * _step, 0f);
				Redraw();
				UpdateThresholdControls();
				UpdateStatusText();
			}
		}

		private void OnVoiceSampled(VoiceNoiseSample sample)
		{
			_latestSample = sample;
			_hasSample = true;
			int num = _slotCount - 1;
			if (sample.Loudness > _values[num])
			{
				_values[num] = sample.Loudness;
				_emitted[num] = sample.WouldEmit;
			}
		}

		private void AssignDependencies(EntitiesSoundOcclusionConfiguration configuration)
		{
			_configuration = configuration;
			_initialThreshold = configuration.MinVoiceLoudnessForEntityOcclusion;
			_isInjected = true;
		}

		private void TryResolveDependencies()
		{
			if (_isInjected)
			{
				return;
			}
			SceneContext[] array = UnityEngine.Object.FindObjectsByType<SceneContext>(FindObjectsSortMode.None);
			foreach (SceneContext sceneContext in array)
			{
				if (TryResolveFromContainer(sceneContext.Container))
				{
					return;
				}
			}
			if (ProjectContext.Instance != null)
			{
				TryResolveFromContainer(ProjectContext.Instance.Container);
			}
		}

		private bool TryResolveFromContainer(DiContainer container)
		{
			if (container == null)
			{
				return false;
			}
			if (!container.HasBinding<EntitiesSoundOcclusionConfiguration>())
			{
				return false;
			}
			AssignDependencies(container.Resolve<EntitiesSoundOcclusionConfiguration>());
			return true;
		}

		private void BuildUi()
		{
			if (!_isBuilt)
			{
				_step = _barWidth + _barSpacing;
				_slotCount = _columnCount + 1;
				if (GetComponent<Canvas>() == null)
				{
					base.gameObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
					base.gameObject.AddComponent<CanvasScaler>();
					base.gameObject.AddComponent<GraphicRaycaster>();
				}
				GameObject obj = new GameObject("VoiceNoisePanel", typeof(RectTransform), typeof(Image));
				RectTransform component = obj.GetComponent<RectTransform>();
				component.SetParent(base.transform, worldPositionStays: false);
				component.anchorMin = new Vector2(0.5f, 0f);
				component.anchorMax = new Vector2(0.5f, 0f);
				component.pivot = new Vector2(0.5f, 0f);
				component.sizeDelta = new Vector2((float)_columnCount * _step + 24f, _maxBarHeight + 96f);
				component.anchoredPosition = _panelOffset;
				obj.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.65f);
				RectTransform component2 = new GameObject("Graph", typeof(RectTransform), typeof(RectMask2D)).GetComponent<RectTransform>();
				component2.SetParent(component, worldPositionStays: false);
				component2.anchorMin = new Vector2(0f, 0f);
				component2.anchorMax = new Vector2(0f, 0f);
				component2.pivot = new Vector2(0f, 0f);
				component2.sizeDelta = new Vector2((float)_columnCount * _step, _maxBarHeight);
				component2.anchoredPosition = new Vector2(12f, 56f);
				GameObject gameObject = new GameObject("Content", typeof(RectTransform));
				_content = gameObject.GetComponent<RectTransform>();
				_content.SetParent(component2, worldPositionStays: false);
				_content.anchorMin = Vector2.zero;
				_content.anchorMax = Vector2.zero;
				_content.pivot = Vector2.zero;
				_content.sizeDelta = new Vector2((float)_slotCount * _step, _maxBarHeight);
				_thresholdLine = CreateImage("ThresholdLine", component2, _thresholdColor).rectTransform;
				_thresholdLine.anchorMin = new Vector2(0f, 0f);
				_thresholdLine.anchorMax = new Vector2(0f, 0f);
				_thresholdLine.pivot = new Vector2(0f, 0.5f);
				_thresholdLine.sizeDelta = new Vector2((float)_columnCount * _step, 2f);
				_bars = new Image[_slotCount];
				_barRects = new RectTransform[_slotCount];
				_values = new float[_slotCount];
				_emitted = new bool[_slotCount];
				for (int i = 0; i < _slotCount; i++)
				{
					Image image = CreateImage($"Bar_{i}", _content, _idleColor);
					RectTransform rectTransform = image.rectTransform;
					rectTransform.anchorMin = Vector2.zero;
					rectTransform.anchorMax = Vector2.zero;
					rectTransform.pivot = Vector2.zero;
					rectTransform.sizeDelta = new Vector2(_barWidth, 2f);
					rectTransform.anchoredPosition = new Vector2((float)i * _step, 0f);
					image.raycastTarget = false;
					_bars[i] = image;
					_barRects[i] = rectTransform;
				}
				_statusText = CreateText("Status", component, new Vector2(12f, 12f), new Vector2((float)_columnCount * _step, 18f), 13);
				_thresholdText = CreateText("Threshold", component, new Vector2(12f, _maxBarHeight + 60f), new Vector2(180f, 20f), 13);
				_thresholdSlider = CreateSlider(component, new Vector2(200f, _maxBarHeight + 62f), new Vector2((float)_columnCount * _step - 200f, 16f));
				_thresholdSlider.SetValueWithoutNotify(_configuration.MinVoiceLoudnessForEntityOcclusion);
				_thresholdSlider.onValueChanged.AddListener(SetThreshold);
				CreateButton(component, "-", new Vector2(12f, _maxBarHeight + 30f), delegate
				{
					AdjustThreshold(0f - _thresholdStep);
				});
				CreateButton(component, "+", new Vector2(56f, _maxBarHeight + 30f), delegate
				{
					AdjustThreshold(_thresholdStep);
				});
				CreateButton(component, "Reset", new Vector2(100f, _maxBarHeight + 30f), delegate
				{
					SetThreshold(_initialThreshold);
				}, new Vector2(64f, 22f));
				_isBuilt = true;
			}
		}

		private void RefreshControllersIfNeeded()
		{
			_controllerRefreshTimer -= Time.deltaTime;
			if (!(_controllerRefreshTimer > 0f))
			{
				_controllerRefreshTimer = 0.5f;
				_lipSyncControllers = UnityEngine.Object.FindObjectsByType<PlayerLipSyncController>(FindObjectsSortMode.None);
			}
		}

		private void SampleVoice()
		{
			if (_lipSyncControllers == null || _lipSyncControllers.Length == 0)
			{
				return;
			}
			VoiceNoiseSample sample = default(VoiceNoiseSample);
			bool flag = false;
			float minVoiceLoudnessForEntityOcclusion = _configuration.MinVoiceLoudnessForEntityOcclusion;
			float maxVoiceHearingDistance = _configuration.MaxVoiceHearingDistance;
			for (int i = 0; i < _lipSyncControllers.Length; i++)
			{
				PlayerLipSyncController playerLipSyncController = _lipSyncControllers[i];
				if (!(playerLipSyncController == null) && !(playerLipSyncController.Object == null))
				{
					float num = 1f - Mathf.Clamp01((float)playerLipSyncController.CurrentVolume / 100f);
					float num2 = num * maxVoiceHearingDistance;
					VoiceNoiseSample voiceNoiseSample = new VoiceNoiseSample(playerLipSyncController.Object.InputAuthority.PlayerId, num, minVoiceLoudnessForEntityOcclusion, num2, maxVoiceHearingDistance, num >= minVoiceLoudnessForEntityOcclusion && num2 > 0f);
					if (!flag || voiceNoiseSample.Loudness > sample.Loudness)
					{
						sample = voiceNoiseSample;
						flag = true;
					}
				}
			}
			if (flag)
			{
				OnVoiceSampled(sample);
			}
		}

		private void SetThreshold(float value)
		{
			_configuration.SetMinVoiceLoudnessForEntityOcclusion(value);
		}

		private void AdjustThreshold(float delta)
		{
			SetThreshold(_configuration.MinVoiceLoudnessForEntityOcclusion + delta);
		}

		private void ShiftLeft()
		{
			for (int i = 0; i < _slotCount - 1; i++)
			{
				_values[i] = _values[i + 1];
				_emitted[i] = _emitted[i + 1];
			}
			_values[_slotCount - 1] = 0f;
			_emitted[_slotCount - 1] = false;
		}

		private void Redraw()
		{
			for (int i = 0; i < _slotCount; i++)
			{
				float num = Mathf.Clamp01(_values[i]);
				float y = Mathf.Max(2f, num * _maxBarHeight);
				_barRects[i].sizeDelta = new Vector2(_barWidth, y);
				_bars[i].color = Color.Lerp(_idleColor, _emitted[i] ? _emittedColor : _blockedColor, Mathf.Max(num, 0.2f));
			}
			float y2 = Mathf.Clamp01(_configuration.MinVoiceLoudnessForEntityOcclusion) * _maxBarHeight;
			_thresholdLine.anchoredPosition = new Vector2(0f, y2);
		}

		private void UpdateThresholdControls()
		{
			float minVoiceLoudnessForEntityOcclusion = _configuration.MinVoiceLoudnessForEntityOcclusion;
			_thresholdSlider.SetValueWithoutNotify(minVoiceLoudnessForEntityOcclusion);
			_thresholdText.text = "Voice threshold: " + minVoiceLoudnessForEntityOcclusion.ToString("0.00", CultureInfo.InvariantCulture);
		}

		private void UpdateStatusText()
		{
			if (!_hasSample)
			{
				_statusText.text = "Waiting for voice samples";
				return;
			}
			string arg = (_latestSample.WouldEmit ? "EMIT" : "BLOCK");
			_statusText.text = $"P{_latestSample.PlayerId} {arg}  loudness {_latestSample.Loudness:0.00}  " + $"radius {_latestSample.Radius:0.0}/{_latestSample.MaxRadius:0.0}";
		}

		private Image CreateImage(string objectName, Transform parent, Color color)
		{
			GameObject obj = new GameObject(objectName, typeof(RectTransform), typeof(Image));
			obj.transform.SetParent(parent, worldPositionStays: false);
			Image component = obj.GetComponent<Image>();
			component.color = color;
			return component;
		}

		private Text CreateText(string objectName, Transform parent, Vector2 position, Vector2 size, int fontSize)
		{
			GameObject obj = new GameObject(objectName, typeof(RectTransform), typeof(Text));
			RectTransform component = obj.GetComponent<RectTransform>();
			component.SetParent(parent, worldPositionStays: false);
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.zero;
			component.pivot = Vector2.zero;
			component.anchoredPosition = position;
			component.sizeDelta = size;
			Text component2 = obj.GetComponent<Text>();
			component2.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component2.fontSize = fontSize;
			component2.color = Color.white;
			component2.alignment = TextAnchor.MiddleLeft;
			component2.raycastTarget = false;
			return component2;
		}

		private Slider CreateSlider(Transform parent, Vector2 position, Vector2 size)
		{
			GameObject gameObject = new GameObject("ThresholdSlider", typeof(RectTransform), typeof(Slider));
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(parent, worldPositionStays: false);
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.zero;
			component.pivot = Vector2.zero;
			component.anchoredPosition = position;
			component.sizeDelta = size;
			Slider component2 = gameObject.GetComponent<Slider>();
			component2.minValue = 0f;
			component2.maxValue = 1f;
			component2.wholeNumbers = false;
			component2.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
			RectTransform rectTransform = CreateImage("Background", gameObject.transform, new Color(1f, 1f, 1f, 0.18f)).rectTransform;
			rectTransform.anchorMin = new Vector2(0f, 0.35f);
			rectTransform.anchorMax = new Vector2(1f, 0.65f);
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			RectTransform component3 = new GameObject("Fill Area", typeof(RectTransform)).GetComponent<RectTransform>();
			component3.SetParent(gameObject.transform, worldPositionStays: false);
			component3.anchorMin = Vector2.zero;
			component3.anchorMax = Vector2.one;
			component3.offsetMin = Vector2.zero;
			component3.offsetMax = Vector2.zero;
			RectTransform rectTransform2 = CreateImage("Fill", component3, _thresholdColor).rectTransform;
			rectTransform2.anchorMin = new Vector2(0f, 0.35f);
			rectTransform2.anchorMax = new Vector2(1f, 0.65f);
			rectTransform2.offsetMin = Vector2.zero;
			rectTransform2.offsetMax = Vector2.zero;
			Image image = CreateImage("Handle", gameObject.transform, Color.white);
			RectTransform rectTransform3 = image.rectTransform;
			rectTransform3.sizeDelta = new Vector2(10f, 18f);
			component2.fillRect = rectTransform2;
			component2.handleRect = rectTransform3;
			component2.targetGraphic = image;
			component2.direction = Slider.Direction.LeftToRight;
			return component2;
		}

		private void CreateButton(Transform parent, string label, Vector2 position, Action action, Vector2? size = null)
		{
			GameObject gameObject = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button));
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(parent, worldPositionStays: false);
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.zero;
			component.pivot = Vector2.zero;
			component.anchoredPosition = position;
			component.sizeDelta = size ?? new Vector2(36f, 22f);
			gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.16f);
			Button component2 = gameObject.GetComponent<Button>();
			component2.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
			component2.onClick.AddListener(delegate
			{
				action();
			});
			Text text = CreateText("Text", gameObject.transform, Vector2.zero, component.sizeDelta, 12);
			text.text = label;
			text.alignment = TextAnchor.MiddleCenter;
		}
	}
}
