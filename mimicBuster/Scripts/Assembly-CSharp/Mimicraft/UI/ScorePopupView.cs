using DG.Tweening;
using Mimicraft.Localization;
using Mimicraft.Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.UI
{
	public class ScorePopupView : MonoBehaviour
	{
		[Tooltip("Left empty, this object's own TextMeshProUGUI is used.")]
		[SerializeField]
		private TextMeshProUGUI label;

		[Tooltip("Left empty, found in the scene.")]
		[SerializeField]
		private RoundManager roundManager;

		[Tooltip("Only pop for Hunters. A Modelci earns a point every couple of seconds, which as a stream of popups is noise rather than feedback - switch this off only if the survival payout becomes rare enough to be an event.")]
		[SerializeField]
		private bool hunterOnly = true;

		[Tooltip("How far the popup drifts up as it fades.")]
		[SerializeField]
		private float riseDistance = 40f;

		[SerializeField]
		[Min(0.1f)]
		private float duration = 0.9f;

		private Vector2 restPosition;

		private int lastScore = -1;

		private bool subscribed;

		private Sequence active;

		public static ScorePopupView Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
			if (label == null)
			{
				label = GetComponent<TextMeshProUGUI>();
			}
			if (roundManager == null)
			{
				roundManager = Object.FindFirstObjectByType<RoundManager>();
			}
			if (label == null)
			{
				base.enabled = false;
				return;
			}
			restPosition = ((RectTransform)label.transform).anchoredPosition;
			SetAlpha(0f);
		}

		private void OnDisable()
		{
			Unsubscribe();
		}

		private void Update()
		{
			if (!subscribed && roundManager != null && roundManager.IsSpawned)
			{
				subscribed = true;
				roundManager.Scores.OnListChanged += OnScoresChanged;
				lastScore = LocalScore();
			}
		}

		private void Unsubscribe()
		{
			if (subscribed && !(roundManager == null))
			{
				subscribed = false;
				roundManager.Scores.OnListChanged -= OnScoresChanged;
			}
		}

		private void OnScoresChanged(NetworkListEvent<PlayerScoreEntry> _)
		{
			if (hunterOnly && roundManager.LocalRole != PlayerRole.Hunter)
			{
				lastScore = LocalScore();
				return;
			}
			int num = LocalScore();
			int num2 = num - lastScore;
			lastScore = num;
			if (num2 > 0)
			{
				Play(num2);
			}
		}

		private int LocalScore()
		{
			if (NetworkManager.Singleton == null)
			{
				return 0;
			}
			ulong localClientId = NetworkManager.Singleton.LocalClientId;
			foreach (PlayerScoreEntry score in roundManager.Scores)
			{
				if (score.ClientId == localClientId)
				{
					return score.Score;
				}
			}
			return 0;
		}

		public void Pop(int amount)
		{
			if (label != null)
			{
				Play(amount);
			}
		}

		private void Play(int amount)
		{
			label.text = Loc.Format("ScoreGain", amount);
			RectTransform rectTransform = (RectTransform)label.transform;
			rectTransform.anchoredPosition = restPosition;
			SetAlpha(1f);
			active?.Kill();
			active = DOTween.Sequence().Append(rectTransform.DOAnchorPosY(restPosition.y + riseDistance, duration).SetEase(Ease.OutCubic)).Join(label.DOFade(0f, duration).SetEase(Ease.InQuad))
				.SetUpdate(isIndependentUpdate: true);
		}

		private void SetAlpha(float alpha)
		{
			Color color = label.color;
			color.a = alpha;
			label.color = color;
		}

		private void OnDestroy()
		{
			active?.Kill();
			if (Instance == this)
			{
				Instance = null;
			}
		}
	}
}
