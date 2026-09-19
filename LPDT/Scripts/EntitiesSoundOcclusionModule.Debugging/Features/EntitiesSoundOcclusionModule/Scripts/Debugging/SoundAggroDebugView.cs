using System.Collections.Generic;
using Features.AudioServiceModule.Scripts;
using Features.Movement.Scripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.EntitiesSoundOcclusionModule.Scripts.Debugging
{
	public class SoundAggroDebugView : MonoBehaviour
	{
		private struct SoundGizmo
		{
			public Vector3 SoundPosition;

			public Vector3 ListenerPosition;

			public float Radius;

			public float ExpireTime;
		}

		[Header("Listener (virtual enemy)")]
		[SerializeField]
		private Transform _listenerOverride;

		[SerializeField]
		[Tooltip("In percents (1.0 = 100%). Matches SoundOcclusionMonoSystem hearing.")]
		private float _hearingStrength = 1f;

		[SerializeField]
		[Tooltip("Apply a sqrt curve to the bar height for readability. Off = raw soundDistance / MaxHearingDistance.")]
		private bool _useSqrtCurve;

		[Header("History / scroll")]
		[SerializeField]
		private int _columnCount = 48;

		[SerializeField]
		[Tooltip("Seconds each column represents. Smaller = faster scroll, shorter history.")]
		private float _secondsPerColumn = 0.05f;

		[Header("Bars")]
		[SerializeField]
		private float _barWidth = 8f;

		[SerializeField]
		private float _barSpacing = 2f;

		[SerializeField]
		private float _maxBarHeight = 180f;

		[SerializeField]
		private Vector2 _panelOffset = new Vector2(0f, 40f);

		[Header("Color")]
		[SerializeField]
		private Color _smallNoiseColor = new Color(0.3f, 0.9f, 0.4f);

		[SerializeField]
		private Color _bigNoiseColor = new Color(0.95f, 0.25f, 0.2f);

		[SerializeField]
		private Color _idleColor = new Color(1f, 1f, 1f, 0.15f);

		[Header("Gizmos (Scene/Game view, editor only)")]
		[SerializeField]
		[Tooltip("Draw a radius sphere at every triggered sound's position, regardless of whether we hear it.")]
		private bool _drawGizmos = true;

		[SerializeField]
		[Tooltip("How long (seconds) a sound's radius sphere stays drawn.")]
		private float _gizmoDuration = 1f;

		[SerializeField]
		private Color _gizmoColor = new Color(1f, 0.45f, 0.1f, 0.8f);

		private EntitiesSoundOcclusionModel _entitiesSoundOcclusionModel;

		private EntitiesSoundOcclusionConfiguration _entitiesSoundOcclusionConfiguration;

		private PlayerMovableModel _playerMovableModel;

		private readonly List<SoundGizmo> _soundGizmos = new List<SoundGizmo>();

		private RectTransform _content;

		private Image[] _bars;

		private RectTransform[] _barRects;

		private float[] _values;

		private Color[] _colors;

		private float _scrollTimer;

		private float _step;

		private int _slotCount;

		[Inject]
		public void InjectDependencies(EntitiesSoundOcclusionModel entitiesSoundOcclusionModel, EntitiesSoundOcclusionConfiguration entitiesSoundOcclusionConfiguration, PlayerMovableModel playerMovableModel)
		{
			_entitiesSoundOcclusionModel = entitiesSoundOcclusionModel;
			_entitiesSoundOcclusionConfiguration = entitiesSoundOcclusionConfiguration;
			_playerMovableModel = playerMovableModel;
			_entitiesSoundOcclusionModel.OnEntitiesTriggeredBySound += OnSoundTriggered;
		}

		private void Awake()
		{
			if (Debug.isDebugBuild)
			{
				BuildUi();
			}
		}

		private void OnDestroy()
		{
			if (_entitiesSoundOcclusionModel != null)
			{
				_entitiesSoundOcclusionModel.OnEntitiesTriggeredBySound -= OnSoundTriggered;
			}
		}

		private void Update()
		{
			_scrollTimer += Time.deltaTime;
			while (_scrollTimer >= _secondsPerColumn)
			{
				_scrollTimer -= _secondsPerColumn;
				ShiftLeft();
			}
			float num = _scrollTimer / _secondsPerColumn;
			_content.anchoredPosition = new Vector2((0f - num) * _step, 0f);
			Redraw();
			for (int num2 = _soundGizmos.Count - 1; num2 >= 0; num2--)
			{
				if (Time.time >= _soundGizmos[num2].ExpireTime)
				{
					_soundGizmos.RemoveAt(num2);
				}
			}
		}

		private void OnDrawGizmos()
		{
			if (!_drawGizmos)
			{
				return;
			}
			for (int i = 0; i < _soundGizmos.Count; i++)
			{
				SoundGizmo soundGizmo = _soundGizmos[i];
				float num = soundGizmo.ExpireTime - Time.time;
				if (!(num <= 0f))
				{
					float num2 = ((_gizmoDuration > 0f) ? Mathf.Clamp01(num / _gizmoDuration) : 1f);
					Color gizmoColor = _gizmoColor;
					gizmoColor.a *= num2;
					Gizmos.color = gizmoColor;
					Gizmos.DrawSphere(soundGizmo.SoundPosition, soundGizmo.Radius);
					Gizmos.DrawLine(soundGizmo.SoundPosition, soundGizmo.ListenerPosition);
				}
			}
		}

		private void OnSoundTriggered(Vector3 soundPosition, float soundDistance, ISoundSource soundSource, string soundPath)
		{
			Transform transform = ResolveListener();
			if (_drawGizmos)
			{
				_soundGizmos.Add(new SoundGizmo
				{
					SoundPosition = soundPosition,
					ListenerPosition = ((transform != null) ? transform.position : soundPosition),
					Radius = soundDistance,
					ExpireTime = Time.time + _gizmoDuration
				});
			}
			if (transform == null)
			{
				return;
			}
			float num = soundDistance * _hearingStrength;
			if (!(num <= 0f) && !(Vector3.Distance(soundPosition, transform.position) > num))
			{
				float num2 = Mathf.Clamp01(soundDistance / _entitiesSoundOcclusionConfiguration.MaxHearingDistance);
				if (_useSqrtCurve)
				{
					num2 = Mathf.Sqrt(num2);
				}
				int num3 = _slotCount - 1;
				if (num2 > _values[num3])
				{
					_values[num3] = num2;
					_colors[num3] = Color.Lerp(_smallNoiseColor, _bigNoiseColor, num2);
				}
			}
		}

		private void ShiftLeft()
		{
			for (int i = 0; i < _slotCount - 1; i++)
			{
				_values[i] = _values[i + 1];
				_colors[i] = _colors[i + 1];
			}
			_values[_slotCount - 1] = 0f;
			_colors[_slotCount - 1] = _idleColor;
		}

		private void Redraw()
		{
			for (int i = 0; i < _slotCount; i++)
			{
				float y = Mathf.Max(2f, _values[i] * _maxBarHeight);
				_barRects[i].sizeDelta = new Vector2(_barWidth, y);
				_bars[i].color = Color.Lerp(_idleColor, _colors[i], _values[i]);
			}
		}

		private Transform ResolveListener()
		{
			if (_listenerOverride != null)
			{
				return _listenerOverride;
			}
			if (!(_playerMovableModel.LocalMovable != null))
			{
				return null;
			}
			return _playerMovableModel.LocalMovable.CameraPositionTransform;
		}

		private void BuildUi()
		{
			_step = _barWidth + _barSpacing;
			_slotCount = _columnCount + 1;
			if (GetComponent<Canvas>() == null)
			{
				base.gameObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
				base.gameObject.AddComponent<CanvasScaler>();
				base.gameObject.AddComponent<GraphicRaycaster>();
			}
			RectTransform component = new GameObject("BarsPanel", typeof(RectTransform), typeof(RectMask2D)).GetComponent<RectTransform>();
			component.SetParent(base.transform, worldPositionStays: false);
			component.anchorMin = new Vector2(0.5f, 0f);
			component.anchorMax = new Vector2(0.5f, 0f);
			component.pivot = new Vector2(0.5f, 0f);
			component.sizeDelta = new Vector2((float)_columnCount * _step, _maxBarHeight);
			component.anchoredPosition = _panelOffset;
			GameObject gameObject = new GameObject("Content", typeof(RectTransform));
			_content = gameObject.GetComponent<RectTransform>();
			_content.SetParent(component, worldPositionStays: false);
			_content.anchorMin = new Vector2(0f, 0f);
			_content.anchorMax = new Vector2(0f, 0f);
			_content.pivot = new Vector2(0f, 0f);
			_content.sizeDelta = new Vector2((float)_slotCount * _step, _maxBarHeight);
			_content.anchoredPosition = Vector2.zero;
			_bars = new Image[_slotCount];
			_barRects = new RectTransform[_slotCount];
			_values = new float[_slotCount];
			_colors = new Color[_slotCount];
			for (int i = 0; i < _slotCount; i++)
			{
				GameObject obj = new GameObject($"Bar_{i}", typeof(RectTransform), typeof(Image));
				RectTransform component2 = obj.GetComponent<RectTransform>();
				component2.SetParent(_content, worldPositionStays: false);
				component2.anchorMin = new Vector2(0f, 0f);
				component2.anchorMax = new Vector2(0f, 0f);
				component2.pivot = new Vector2(0f, 0f);
				component2.sizeDelta = new Vector2(_barWidth, 2f);
				component2.anchoredPosition = new Vector2((float)i * _step, 0f);
				Image component3 = obj.GetComponent<Image>();
				component3.color = _idleColor;
				component3.raycastTarget = false;
				_bars[i] = component3;
				_barRects[i] = component2;
				_colors[i] = _idleColor;
			}
		}
	}
}
