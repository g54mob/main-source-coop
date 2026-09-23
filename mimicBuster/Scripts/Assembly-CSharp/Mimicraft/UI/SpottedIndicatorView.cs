using DG.Tweening;
using Mimicraft.Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.UI
{
	public class SpottedIndicatorView : MonoBehaviour
	{
		[Tooltip("Left empty, this object's own TextMeshProUGUI is used.")]
		[SerializeField]
		private TextMeshProUGUI label;

		[Tooltip("Shown while a Hunter has line of sight. {0} is the score multiplier.")]
		[SerializeField]
		private string format = "Görülüyorsun ×{0:0.#}";

		[Tooltip("Must match RoundManager's Los Score Multiplier - this only displays the number, it does not decide it.")]
		[SerializeField]
		[Min(1f)]
		private float displayedMultiplier = 2f;

		[Tooltip("Left empty, found in the scene.")]
		[SerializeField]
		private RoundManager roundManager;

		private PlayerSpotState spotState;

		private bool appliedSpotted;

		private bool appliedOnce;

		private Tween pulse;

		private void Awake()
		{
			if (label == null)
			{
				label = GetComponent<TextMeshProUGUI>();
			}
			if (label == null)
			{
				base.enabled = false;
			}
			else
			{
				label.gameObject.SetActive(value: false);
			}
		}

		private void OnDestroy()
		{
			pulse?.Kill();
		}

		private void Update()
		{
			bool flag = IsLiveHider() && TryResolveState() && spotState.IsSpotted;
			if (!appliedOnce || flag != appliedSpotted)
			{
				appliedOnce = true;
				appliedSpotted = flag;
				label.gameObject.SetActive(flag);
				if (!flag)
				{
					pulse?.Kill();
					return;
				}
				label.text = string.Format(format, displayedMultiplier);
				pulse?.Kill();
				label.transform.localScale = Vector3.one;
				pulse = label.transform.DOScale(1.08f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine)
					.SetUpdate(isIndependentUpdate: true);
			}
		}

		private bool IsLiveHider()
		{
			if (roundManager == null)
			{
				roundManager = GameModeController.Current as RoundManager;
				if (roundManager == null)
				{
					return false;
				}
			}
			if (roundManager.IsSpawned && roundManager.CurrentPhase.Value == RoundPhase.Hunt)
			{
				return roundManager.LocalRole == PlayerRole.Hider;
			}
			return false;
		}

		private bool TryResolveState()
		{
			if (spotState != null)
			{
				return true;
			}
			NetworkManager singleton = NetworkManager.Singleton;
			if (singleton == null || !singleton.IsClient || singleton.LocalClient.PlayerObject == null)
			{
				return false;
			}
			spotState = singleton.LocalClient.PlayerObject.GetComponent<PlayerSpotState>();
			return spotState != null;
		}
	}
}
