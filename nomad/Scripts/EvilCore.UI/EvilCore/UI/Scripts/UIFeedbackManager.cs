using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.Localization;
using UnityEngine;
using VContainer;

namespace EvilCore.UI.Scripts
{
	public class UIFeedbackManager : MonoBehaviour
	{
		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private IAudioManager _audioManager;

		[Header("Style")]
		[Tooltip("Per-severity sound / color / icon / entrance feel. Drives Info, Warning, Error and Success.")]
		[SerializeField]
		private FeedbackStyleConfig styleConfig;

		[Header("Floating Message")]
		[SerializeField]
		private FloatingMessage floatingMessagePrefab;

		[SerializeField]
		private Transform floatingMessageContainer;

		[SerializeField]
		private float floatingMessageYOffset = 250f;

		[Tooltip("Base anchored Y the first visible message settles at. Stacked messages settle above it.")]
		[SerializeField]
		private float floatingMessageBaseY;

		[Tooltip("Vertical gap between simultaneously visible messages so they rise in parallel lanes instead of overlapping.")]
		[SerializeField]
		private float floatingMessageSpacing = 70f;

		[Tooltip("How many floating messages to pre-instantiate into the pool on Awake.")]
		[SerializeField]
		private int floatingMessagePrewarm = 4;

		private readonly Queue<FloatingMessage> _pool = new Queue<FloatingMessage>();

		private readonly List<FloatingMessage> _active = new List<FloatingMessage>();

		private Transform _poolParent;

		private void Awake()
		{
			CreatePoolParent();
			Prewarm();
		}

		public void CreateFloatingMessage(string message, FeedbackType type = FeedbackType.Info)
		{
			ShowFloatingMessage(message, type, null, null, null);
		}

		public void CreateFloatingMessage(string message, FeedbackType type, float onScreenDuration, float movementDuration, float fadeDuration)
		{
			ShowFloatingMessage(message, type, onScreenDuration, movementDuration, fadeDuration);
		}

		public void CreateFloatingMessage(string message, float onScreenDuration, float movementDuration = 0.5f, float fadeDuration = 0.4f)
		{
			ShowFloatingMessage(message, FeedbackType.Info, onScreenDuration, movementDuration, fadeDuration);
		}

		private void ShowFloatingMessage(string message, FeedbackType type, float? onScreenOverride, float? movementOverride, float? fadeOverride)
		{
			if (!(floatingMessagePrefab == null))
			{
				string message2 = _localizationService?.Localize(message) ?? message;
				FeedbackStyle feedbackStyle = ((styleConfig != null) ? styleConfig.Get(type) : null);
				PlayFeedbackSound(feedbackStyle);
				float onScreen = onScreenOverride ?? feedbackStyle?.OnScreenDuration ?? 0.6f;
				float rise = movementOverride ?? feedbackStyle?.RiseDuration ?? 0.5f;
				float fade = fadeOverride ?? feedbackStyle?.FadeDuration ?? 0.4f;
				float riseDistance = feedbackStyle?.RiseDistance ?? floatingMessageYOffset;
				FloatingMessage fromPool = GetFromPool();
				float settleY = floatingMessageBaseY + (float)_active.Count * floatingMessageSpacing;
				_active.Add(fromPool);
				fromPool.Play(message2, feedbackStyle, onScreen, rise, fade, riseDistance, settleY, ReturnToPool);
			}
		}

		private void PlayFeedbackSound(FeedbackStyle style)
		{
			if (style != null && style.Sound.IsValid())
			{
				_audioManager?.PlayOneShotUI(style.Sound);
			}
		}

		private void CreatePoolParent()
		{
			GameObject gameObject = new GameObject("FloatingMessagePool");
			_poolParent = gameObject.transform;
			Transform parent = ((floatingMessageContainer != null) ? floatingMessageContainer : base.transform);
			_poolParent.SetParent(parent, worldPositionStays: false);
			gameObject.SetActive(value: false);
		}

		private void Prewarm()
		{
			if (!(floatingMessagePrefab == null))
			{
				for (int i = 0; i < floatingMessagePrewarm; i++)
				{
					_pool.Enqueue(CreatePooledInstance());
				}
			}
		}

		private FloatingMessage CreatePooledInstance()
		{
			FloatingMessage floatingMessage = Object.Instantiate(floatingMessagePrefab, _poolParent);
			floatingMessage.gameObject.SetActive(value: false);
			return floatingMessage;
		}

		private FloatingMessage GetFromPool()
		{
			FloatingMessage obj = ((_pool.Count > 0) ? _pool.Dequeue() : CreatePooledInstance());
			obj.transform.SetParent(floatingMessageContainer, worldPositionStays: false);
			obj.gameObject.SetActive(value: true);
			return obj;
		}

		private void ReturnToPool(FloatingMessage instance)
		{
			_active.Remove(instance);
			instance.gameObject.SetActive(value: false);
			instance.transform.SetParent(_poolParent, worldPositionStays: false);
			_pool.Enqueue(instance);
		}
	}
}
