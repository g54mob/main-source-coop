using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

namespace EvilCore.UI.Scripts
{
	public class UIStaggeredGroup : MonoBehaviour
	{
		[Header("Stagger")]
		[SerializeField]
		private float staggerDelay = 0.06f;

		[SerializeField]
		private float startDelay;

		[Header("Animation")]
		[SerializeField]
		private bool useFade = true;

		[SerializeField]
		private bool useSlide = true;

		[SerializeField]
		private AppearDirection direction = AppearDirection.Down;

		[SerializeField]
		private float slideDistance = 30f;

		[SerializeField]
		private float duration = 0.3f;

		[SerializeField]
		private Ease ease = Ease.OutCubic;

		[SerializeField]
		private bool playOnEnable = true;

		private readonly List<CanvasGroup> _groups = new List<CanvasGroup>();

		private readonly List<RectTransform> _rects = new List<RectTransform>();

		private readonly List<Vector2> _basePositions = new List<Vector2>();

		private bool _cached;

		private void Awake()
		{
			Cache();
		}

		private void Cache()
		{
			if (_cached)
			{
				return;
			}
			_groups.Clear();
			_rects.Clear();
			_basePositions.Clear();
			foreach (RectTransform item in base.transform)
			{
				CanvasGroup canvasGroup = item.GetComponent<CanvasGroup>();
				if (canvasGroup == null)
				{
					canvasGroup = item.gameObject.AddComponent<CanvasGroup>();
				}
				_groups.Add(canvasGroup);
				_rects.Add(item);
				_basePositions.Add(item.anchoredPosition);
			}
			_cached = true;
		}

		private void OnEnable()
		{
			if (playOnEnable)
			{
				Play();
			}
		}

		public void Play()
		{
			Cache();
			Vector2 offset = GetOffset();
			for (int i = 0; i < _rects.Count; i++)
			{
				CanvasGroup canvasGroup = _groups[i];
				RectTransform rectTransform = _rects[i];
				float num = startDelay + staggerDelay * (float)i;
				Tween.CompleteAll(canvasGroup);
				Tween.CompleteAll(rectTransform);
				if (useFade)
				{
					canvasGroup.alpha = 0f;
					Tween.Alpha(canvasGroup, 1f, duration, ease, 1, CycleMode.Restart, num, 0f, useUnscaledTime: true);
				}
				if (useSlide)
				{
					rectTransform.anchoredPosition = _basePositions[i] + offset;
					Tween.UIAnchoredPosition(rectTransform, _basePositions[i], duration, ease, 1, CycleMode.Restart, num, 0f, useUnscaledTime: true);
				}
			}
		}

		private Vector2 GetOffset()
		{
			return direction switch
			{
				AppearDirection.Up => new Vector2(0f, 0f - slideDistance), 
				AppearDirection.Down => new Vector2(0f, slideDistance), 
				AppearDirection.Left => new Vector2(slideDistance, 0f), 
				AppearDirection.Right => new Vector2(0f - slideDistance, 0f), 
				_ => Vector2.zero, 
			};
		}

		private void OnDisable()
		{
			if (!_cached)
			{
				return;
			}
			for (int i = 0; i < _rects.Count; i++)
			{
				if (_groups[i] != null)
				{
					Tween.CompleteAll(_groups[i]);
					_groups[i].alpha = 1f;
				}
				if (_rects[i] != null)
				{
					Tween.CompleteAll(_rects[i]);
					_rects[i].anchoredPosition = _basePositions[i];
				}
			}
		}
	}
}
