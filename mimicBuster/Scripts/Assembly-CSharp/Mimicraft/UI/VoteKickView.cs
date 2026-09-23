using System.Collections.Generic;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public class VoteKickView : MonoBehaviour
	{
		[Header("Açma")]
		[Tooltip("Duraklat menüsündeki 'Oyuncu At' butonu. Listeyi açar.\n\nOylama açılamayacak durumdayken (Steam yok, üç kişiden az, zaten bir oylama sürüyor) kendiliğinden tıklanamaz olur. Boş bırakılabilir - o zaman listeyi OpenList() çağıran başka bir şey açar.")]
		[SerializeField]
		private Button openListButton;

		[Header("Oyuncu listesi")]
		[Tooltip("Oyuncu listesi paneli. Sahnede açık bırakılmış olsa bile ilk karede kapanır; yalnızca butona basılınca açılır.")]
		[SerializeField]
		private GameObject listPanel;

		[Tooltip("Oyuncu satırlarının dizileceği boş obje. Boş bırakılırsa şablonun kendi parent'ı.")]
		[SerializeField]
		private RectTransform listParent;

		[Tooltip("Satır şablonu: üzerinde Button olan bir obje, altında da adı yazacak bir TextMeshProUGUI. Sahnede İNAKTİF bırak - kopyalanır, kendisi hiç gösterilmez.")]
		[SerializeField]
		private Button rowTemplate;

		[Tooltip("Listeyi kapatan buton. İsteğe bağlı.")]
		[SerializeField]
		private Button closeListButton;

		[Header("Oylama paneli")]
		[Tooltip("Oylama SÜRERKEN açılan panel. Sunucu bir oylama başlattığında kendiliğinden açılır, oylama bitince kendiliğinden kapanır - elle açılmaz.")]
		[SerializeField]
		private GameObject votePanel;

		[Tooltip("'X atılsın mı?' yazısı.")]
		[SerializeField]
		private TextMeshProUGUI questionLabel;

		[Tooltip("'3 / 5 evet - 42 sn' gibi sayaç yazısı.")]
		[SerializeField]
		private TextMeshProUGUI tallyLabel;

		[SerializeField]
		private Button yesButton;

		[SerializeField]
		private Button noButton;

		[Tooltip("Evet butonunun yazısı. Kısayol tuşu buraya yazılır - 'Evet (F1)'. Boş bırakılırsa butonun altındaki ilk TextMeshProUGUI kullanılır.")]
		[SerializeField]
		private TextMeshProUGUI yesLabel;

		[Tooltip("Hayır butonunun yazısı. Aynı mantık - 'Hayır (F2)'.")]
		[SerializeField]
		private TextMeshProUGUI noLabel;

		[Header("Durum")]
		[Tooltip("Oylama açılamadığında sebebini yazan satır. İsteğe bağlı.")]
		[SerializeField]
		private TextMeshProUGUI statusLabel;

		private readonly List<Button> rows = new List<Button>();

		private readonly List<ulong> rowClientIds = new List<ulong>();

		private bool answered;

		private ulong answeredFor;

		private bool voteShown;

		private GameModeController Mode => GameModeController.Current;

		private VoteKickState Vote
		{
			get
			{
				GameModeController mode = Mode;
				if (!(mode != null) || !mode.IsSpawned)
				{
					return default(VoteKickState);
				}
				return mode.VoteKick.Value;
			}
		}

		private bool CanStartVote
		{
			get
			{
				GameModeController mode = Mode;
				if (mode != null && mode.IsSpawned && mode.VoteKickAvailable)
				{
					return !Vote.Active;
				}
				return false;
			}
		}

		private void Awake()
		{
			if (rowTemplate != null)
			{
				rowTemplate.gameObject.SetActive(value: false);
			}
			if (yesButton != null)
			{
				yesButton.onClick.AddListener(delegate
				{
					Answer(yes: true);
				});
			}
			if (noButton != null)
			{
				noButton.onClick.AddListener(delegate
				{
					Answer(yes: false);
				});
			}
			if (openListButton != null)
			{
				openListButton.onClick.AddListener(OpenList);
			}
			if (closeListButton != null)
			{
				closeListButton.onClick.AddListener(CloseList);
			}
			Show(listPanel, shown: false);
			Show(votePanel, shown: false);
			RefreshHotkeyLabels();
			Loc.Changed += RefreshHotkeyLabels;
		}

		private void OnDestroy()
		{
			Loc.Changed -= RefreshHotkeyLabels;
		}

		private void OnDisable()
		{
			CloseList();
		}

		public void OpenList()
		{
			if (CanStartVote)
			{
				DrawList();
				Show(listPanel, shown: true);
				GameMenuState.SetMenuOpen(this, open: true);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		public void CloseList()
		{
			Show(listPanel, shown: false);
			GameMenuState.SetMenuOpen(this, open: false);
		}

		private void DrawList()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			GameModeController mode = Mode;
			if (singleton == null || mode == null)
			{
				return;
			}
			ulong localClientId = singleton.LocalClientId;
			rowClientIds.Clear();
			foreach (ulong connectedClientsId in singleton.ConnectedClientsIds)
			{
				if (connectedClientsId != localClientId && connectedClientsId != 0L)
				{
					rowClientIds.Add(connectedClientsId);
				}
			}
			EnsureRows(rowClientIds.Count);
			for (int i = 0; i < rows.Count; i++)
			{
				bool flag = i < rowClientIds.Count;
				if (rows[i].gameObject.activeSelf != flag)
				{
					rows[i].gameObject.SetActive(flag);
				}
				if (flag)
				{
					TextMeshProUGUI componentInChildren = rows[i].GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
					if (componentInChildren != null)
					{
						componentInChildren.text = mode.GetPlayerName(rowClientIds[i]);
					}
				}
			}
		}

		private void EnsureRows(int needed)
		{
			if (rowTemplate == null)
			{
				return;
			}
			Transform parent = ((listParent != null) ? listParent : rowTemplate.transform.parent);
			while (rows.Count < needed)
			{
				Button button = Object.Instantiate(rowTemplate, parent);
				button.gameObject.name = $"KickRow{rows.Count}";
				int index = rows.Count;
				button.onClick.AddListener(delegate
				{
					StartVote(index);
				});
				rows.Add(button);
			}
		}

		private void StartVote(int index)
		{
			GameModeController mode = Mode;
			if (!(mode == null) && index < rowClientIds.Count && CanStartVote)
			{
				answered = false;
				mode.StartVoteKickServerRpc(rowClientIds[index]);
				CloseList();
			}
		}

		private void Update()
		{
			VoteKickState vote = Vote;
			if (vote.Active != voteShown)
			{
				voteShown = vote.Active;
				Show(votePanel, vote.Active);
				if (vote.Active)
				{
					CloseList();
				}
			}
			if (openListButton != null)
			{
				openListButton.interactable = CanStartVote;
			}
			if (vote.Active)
			{
				DrawVote(vote);
				ReadHotkeys(vote);
			}
			else if (listPanel != null && listPanel.activeSelf)
			{
				DrawList();
				SetStatus((rowClientIds.Count == 0) ? Loc.Get("VoteKick.NoTargets") : "");
			}
		}

		private void DrawVote(VoteKickState state)
		{
			NetworkManager singleton = NetworkManager.Singleton;
			GameModeController mode = Mode;
			if (!(singleton == null) && !(mode == null))
			{
				bool flag = singleton.LocalClientId == state.TargetClientId;
				if (answeredFor != state.TargetClientId)
				{
					answeredFor = state.TargetClientId;
					answered = state.StartedByClientId == singleton.LocalClientId;
				}
				if (questionLabel != null)
				{
					questionLabel.text = (flag ? Loc.Get("VoteKick.AboutYou") : Loc.Format("VoteKick.Question", mode.GetPlayerName(state.TargetClientId), mode.GetPlayerName(state.StartedByClientId)));
				}
				if (tallyLabel != null)
				{
					double num = state.EndsAtServerTime - singleton.ServerTime.Time;
					int num2 = Mathf.Max(0, Mathf.CeilToInt((float)num));
					tallyLabel.text = Loc.Format("VoteKick.Tally", state.Yes, state.Eligible, num2);
				}
				bool interactable = CanAnswer(state);
				if (yesButton != null)
				{
					yesButton.interactable = interactable;
				}
				if (noButton != null)
				{
					noButton.interactable = interactable;
				}
				SetStatus("");
			}
		}

		private bool CanAnswer(VoteKickState state)
		{
			if (NetworkManager.Singleton != null && NetworkManager.Singleton.LocalClientId != state.TargetClientId)
			{
				return !answered;
			}
			return false;
		}

		private void ReadHotkeys(VoteKickState state)
		{
			if (CanAnswer(state))
			{
				if (GameInput.VoteYes.WasPressedThisFrame())
				{
					Answer(yes: true);
				}
				else if (GameInput.VoteNo.WasPressedThisFrame())
				{
					Answer(yes: false);
				}
			}
		}

		private void Answer(bool yes)
		{
			GameModeController mode = Mode;
			if (!(mode == null) && !answered && Vote.Active)
			{
				answered = true;
				mode.CastVoteKickServerRpc(yes);
			}
		}

		private void RefreshHotkeyLabels()
		{
			TextMeshProUGUI textMeshProUGUI = ((yesLabel != null) ? yesLabel : ((yesButton != null) ? yesButton.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true) : null));
			TextMeshProUGUI textMeshProUGUI2 = ((noLabel != null) ? noLabel : ((noButton != null) ? noButton.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true) : null));
			if (textMeshProUGUI != null)
			{
				textMeshProUGUI.text = Loc.Get("VoteKick.Yes");
			}
			if (textMeshProUGUI2 != null)
			{
				textMeshProUGUI2.text = Loc.Get("VoteKick.No");
			}
		}

		private static void Show(GameObject target, bool shown)
		{
			if (target != null && target.activeSelf != shown)
			{
				target.SetActive(shown);
			}
		}

		private void SetStatus(string text)
		{
			if (statusLabel != null)
			{
				statusLabel.text = text;
			}
		}
	}
}
