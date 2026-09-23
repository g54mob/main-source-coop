using System.Collections.Generic;
using Mimicraft.Gameplay;
using Mimicraft.Localization;
using Mimicraft.Networking;
using Mimicraft.Settings;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mimicraft.UI
{
	public class WeaponChoiceView : MonoBehaviour
	{
		[Tooltip("Açılıp kapanan panel - butonların hepsi bunun içinde. Sahnede KAPALI bırak; N ile açılır.")]
		[SerializeField]
		private GameObject panel;

		[Tooltip("Paneldeki silah butonları. Boş bırakılırsa panelin altındaki bütün WeaponChoiceButton'lar kullanılır.")]
		[SerializeField]
		private List<WeaponChoiceButton> buttons = new List<WeaponChoiceButton>();

		[Tooltip("Maç sırasında seçilen silah bir sonraki doğuşta verilir - bu metin 'Sonraki doğuşta: M4' yazar. İsteğe bağlı; bekleyen seçim yokken gizlenir.")]
		[SerializeField]
		private TextMeshProUGUI pendingLabel;

		private WeaponDefinition pending;

		private DeathmatchRoundManager mode;

		private bool open;

		private bool forced;

		private bool wired;

		private bool shown;

		private bool Shown
		{
			get
			{
				if (!open)
				{
					return forced;
				}
				return true;
			}
		}

		private void Awake()
		{
			if (panel != null && panel.activeSelf)
			{
				panel.SetActive(value: false);
			}
		}

		private void Update()
		{
			if (mode == null)
			{
				mode = GameModeController.Current as DeathmatchRoundManager;
				if (mode == null)
				{
					return;
				}
			}
			WireButtons();
			bool canChooseWeapon = mode.CanChooseWeapon;
			if (open && !canChooseWeapon)
			{
				SetOpen(value: false);
			}
			if (canChooseWeapon && GameInput.WeaponMenu.WasPressedThisFrame())
			{
				SetOpen(!open);
			}
			forced = canChooseWeapon && mode.IsLocalDead;
			ApplyVisibility();
			if (canChooseWeapon)
			{
				ReadHotkeys();
			}
			if (Shown)
			{
				RefreshSelection();
			}
		}

		private void ReadHotkeys()
		{
			InputAction[] array = new InputAction[3]
			{
				GameInput.WeaponSlot1,
				GameInput.WeaponSlot2,
				GameInput.WeaponSlot3
			};
			for (int i = 0; i < array.Length && i < buttons.Count; i++)
			{
				if (array[i] != null && array[i].WasPressedThisFrame())
				{
					Choose(buttons[i]);
				}
			}
		}

		private void ApplyVisibility()
		{
			if (shown != Shown)
			{
				shown = Shown;
				if (panel != null && panel.activeSelf != shown)
				{
					panel.SetActive(shown);
				}
				if (shown)
				{
					RefreshPreviews();
				}
			}
		}

		private void RefreshPreviews()
		{
			string[] array = new string[3] { "Human/WeaponSlot1", "Human/WeaponSlot2", "Human/WeaponSlot3" };
			for (int i = 0; i < buttons.Count; i++)
			{
				if (!(buttons[i] == null))
				{
					buttons[i].RefreshPreview();
					buttons[i].SetHotkey((i < array.Length) ? GameInput.DisplayFor(array[i]) : "");
				}
			}
		}

		private void OnDisable()
		{
			forced = false;
			if (open)
			{
				SetOpen(value: false);
			}
			else
			{
				ApplyVisibility();
			}
		}

		private void WireButtons()
		{
			if (wired)
			{
				return;
			}
			wired = true;
			if (buttons.Count == 0 && panel != null)
			{
				buttons.AddRange(panel.GetComponentsInChildren<WeaponChoiceButton>(includeInactive: true));
			}
			foreach (WeaponChoiceButton button in buttons)
			{
				if (!(button == null))
				{
					WeaponChoiceButton captured = button;
					button.Bind(delegate
					{
						Choose(captured);
					});
				}
			}
		}

		private void SetOpen(bool value)
		{
			open = value;
			ApplyVisibility();
			GameMenuState.SetMenuOpen(this, open);
			if (mode != null)
			{
				mode.WeaponMenuOpen = open;
			}
			if (open)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				RefreshSelection();
			}
		}

		private void Choose(WeaponChoiceButton entry)
		{
			if (!(mode == null) && !(entry == null) && !(entry.Weapon == null))
			{
				WeaponDefinition weaponDefinition = LocalEquipped();
				pending = ((mode.WeaponChoiceAppliesNow || entry.Weapon == weaponDefinition) ? null : entry.Weapon);
				mode.RequestWeapon(entry.Weapon.WeaponId);
				RefreshSelection();
			}
		}

		private static WeaponDefinition LocalEquipped()
		{
			NetworkManager singleton = NetworkManager.Singleton;
			PlayerWeapons playerWeapons = ((singleton != null && singleton.LocalClient != null && singleton.LocalClient.PlayerObject != null) ? singleton.LocalClient.PlayerObject.GetComponent<PlayerWeapons>() : null);
			if (!(playerWeapons != null))
			{
				return null;
			}
			return playerWeapons.Equipped;
		}

		private void RefreshSelection()
		{
			WeaponDefinition weaponDefinition = LocalEquipped();
			if (pending != null && pending == weaponDefinition)
			{
				pending = null;
			}
			WeaponDefinition weaponDefinition2 = ((pending != null) ? pending : weaponDefinition);
			foreach (WeaponChoiceButton button in buttons)
			{
				if (button != null)
				{
					button.SetSelected(button.Weapon != null && button.Weapon == weaponDefinition2);
				}
			}
			if (!(pendingLabel != null))
			{
				return;
			}
			bool flag = pending != null;
			if (flag)
			{
				string text = string.Format(Loc.Get("Deathmatch.WeaponNextSpawn"), pending.DisplayName);
				if (pendingLabel.text != text)
				{
					pendingLabel.text = text;
				}
			}
			if (pendingLabel.gameObject.activeSelf != flag)
			{
				pendingLabel.gameObject.SetActive(flag);
			}
		}
	}
}
