using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class SpectatorHudView : MonoBehaviour
	{
		[SerializeField]
		private GameObject panelRoot;

		[SerializeField]
		private TextMeshProUGUI targetLabel;

		[SerializeField]
		private TextMeshProUGUI healthLabel;

		[SerializeField]
		private TextMeshProUGUI hintLabel;

		private SpectatorController spectator;

		private RoundManager roundManager;

		private ulong lastTargetId;

		private int lastHealth = int.MinValue;

		private bool lastVisible;

		private bool lastFreeCam;

		private bool appliedOnce;

		public static SpectatorHudView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "SpectatorHud");
			rectTransform.anchorMin = new Vector2(0.5f, 0f);
			rectTransform.anchorMax = new Vector2(0.5f, 0f);
			rectTransform.pivot = new Vector2(0.5f, 0f);
			rectTransform.anchoredPosition = new Vector2(0f, 28f);
			rectTransform.sizeDelta = new Vector2(340f, 78f);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.08f, 0.08f, 0.08f, 0.82f);
			image.raycastTarget = false;
			TextMeshProUGUI textMeshProUGUI = UIFactory.CreateLabel(rectTransform, "TargetLabel", "", 18);
			RectTransform obj = (RectTransform)textMeshProUGUI.transform;
			obj.anchorMin = new Vector2(0f, 1f);
			obj.anchorMax = new Vector2(1f, 1f);
			obj.pivot = new Vector2(0.5f, 1f);
			obj.anchoredPosition = new Vector2(0f, -8f);
			obj.sizeDelta = new Vector2(0f, 24f);
			TextMeshProUGUI textMeshProUGUI2 = UIFactory.CreateLabel(rectTransform, "TargetHealthLabel", "", 15);
			RectTransform obj2 = (RectTransform)textMeshProUGUI2.transform;
			obj2.anchorMin = new Vector2(0f, 1f);
			obj2.anchorMax = new Vector2(1f, 1f);
			obj2.pivot = new Vector2(0.5f, 1f);
			obj2.anchoredPosition = new Vector2(0f, -34f);
			obj2.sizeDelta = new Vector2(0f, 20f);
			TextMeshProUGUI textMeshProUGUI3 = UIFactory.CreateLabel(rectTransform, "HintLabel", "", 12);
			RectTransform obj3 = (RectTransform)textMeshProUGUI3.transform;
			obj3.anchorMin = new Vector2(0f, 1f);
			obj3.anchorMax = new Vector2(1f, 1f);
			obj3.pivot = new Vector2(0.5f, 1f);
			obj3.anchoredPosition = new Vector2(0f, -56f);
			obj3.sizeDelta = new Vector2(0f, 18f);
			textMeshProUGUI3.color = new Color(0.72f, 0.72f, 0.72f);
			SpectatorHudView spectatorHudView = rectTransform.gameObject.AddComponent<SpectatorHudView>();
			spectatorHudView.panelRoot = rectTransform.gameObject;
			spectatorHudView.targetLabel = textMeshProUGUI;
			spectatorHudView.healthLabel = textMeshProUGUI2;
			spectatorHudView.hintLabel = textMeshProUGUI3;
			rectTransform.gameObject.SetActive(value: false);
			return spectatorHudView;
		}

		private void Update()
		{
			if (!ResolveReferences())
			{
				SetVisible(visible: false);
				return;
			}
			bool isSpectating = spectator.IsSpectating;
			SetVisible(isSpectating);
			if (!isSpectating)
			{
				return;
			}
			bool isFreeCam = spectator.IsFreeCam;
			NetworkObject currentTarget = spectator.CurrentTarget;
			ulong num = ((currentTarget != null) ? currentTarget.OwnerClientId : ulong.MaxValue);
			int num2 = ResolveHealth(currentTarget);
			if (!appliedOnce || isFreeCam != lastFreeCam || num != lastTargetId || num2 != lastHealth)
			{
				appliedOnce = true;
				lastFreeCam = isFreeCam;
				lastTargetId = num;
				lastHealth = num2;
				if (isFreeCam)
				{
					targetLabel.text = Loc.Get("Spectator.FreeCamera");
					healthLabel.text = "";
					hintLabel.text = Loc.Get("Spectator.FreeCameraHint");
				}
				else if (currentTarget == null)
				{
					targetLabel.text = Loc.Get("Spectator.NoPlayer");
					healthLabel.text = "";
					hintLabel.text = Loc.Get("Spectator.FreeCameraKey");
				}
				else
				{
					targetLabel.text = Loc.Format("Spectator.Watching", roundManager.GetPlayerName(num));
					healthLabel.text = ((num2 >= 0) ? Loc.Format("Spectator.Health", num2) : "");
					hintLabel.text = Loc.Get("Spectator.SwitchHint");
				}
			}
		}

		private bool ResolveReferences()
		{
			if (roundManager == null)
			{
				roundManager = GameModeController.Current as RoundManager;
			}
			if (spectator == null && NetworkManager.Singleton != null)
			{
				NetworkObject networkObject = NetworkManager.Singleton.LocalClient?.PlayerObject;
				if (networkObject != null)
				{
					spectator = networkObject.GetComponent<SpectatorController>();
				}
			}
			if (spectator != null)
			{
				return roundManager != null;
			}
			return false;
		}

		private static int ResolveHealth(NetworkObject target)
		{
			if (target == null)
			{
				return -1;
			}
			PlayerHealth component = target.GetComponent<PlayerHealth>();
			if (!(component != null))
			{
				return -1;
			}
			return component.Health.Value;
		}

		private void SetVisible(bool visible)
		{
			if (visible != lastVisible || !appliedOnce)
			{
				lastVisible = visible;
				if (panelRoot != null && panelRoot.activeSelf != visible)
				{
					panelRoot.SetActive(visible);
				}
				if (!visible)
				{
					appliedOnce = false;
				}
			}
		}
	}
}
