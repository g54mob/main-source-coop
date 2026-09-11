using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Portningsbolaget.Platforms;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using Zorro.ControllerSupport;
using Zorro.Core;
using pworld.Scripts.Extensions;

public class PlayerCustomizer : MonoBehaviour
{
	public InputActionReference m_exitButton;

	public InputActionReference m_openKeyboardButton;

	public ProceduralImage headColor;

	public GameObject colorsRoot;

	public GameObject colorSelectorPrefab;

	public Button applyButton;

	public Button quitButton;

	public List<Color> colorsToPickFrom = new List<Color>();

	public Player playerInTerminal;

	public TextMeshProUGUI faceText;

	public Button rotateLeftButton;

	public Button rotateRightButton;

	public Button smallerFaceButton;

	public Button biggerFaceButton;

	public int faceSizeStepCount;

	public Vector2 faceSizeMinMax;

	public Vector2 visorFaceSizeMinMax = new Vector2(0.025f, 0.035f);

	public float startFaceSize;

	public float startFaceRotation;

	public SFX_Instance enterSound;

	public SFX_Instance leaveSound;

	public SFX_Instance clickSound;

	public SFX_Instance typeSound;

	public SFX_Instance backSound;

	public SFX_Instance applySound;

	public SFX_Instance rotateSound;

	public SFX_Instance sizeSound;

	public Button leftHatButton;

	public Button rightHatButton;

	public Button clearHatButton;

	public TextMeshProUGUI hatNameText;

	public Button pastButton;

	public GameObject glyphIcon;

	private Hat hatAtStart;

	private ColorSelector selectedColor;

	private Hat selectedHat;

	private int startColorIndex;

	private string startFaceText;

	private Color startHeadColor;

	private List<int> unlockedHats;

	private PhotonView view_g;

	private bool m_navigationActive;

	private bool m_isTyping;

	private const float BLOCKED_ROTATION = 270f;

	private const string BLOCKED_FACE = ":)";

	public ColorSelector SelectedColor
	{
		get
		{
			return selectedColor;
		}
		set
		{
			if (!(value == selectedColor))
			{
				view_g.RPC("RPCA_PickColor", RpcTarget.All, value.transform.GetSiblingIndex());
			}
		}
	}

	public bool HasPlayerInTerminal => playerInTerminal != null;

	public float FaceRotation
	{
		get
		{
			return faceText.transform.localEulerAngles.z;
		}
		set
		{
			VerboseDebug.Log("about to set face rotation to " + value);
			int num = Mathf.RoundToInt(value / 90f) * 90;
			num %= 360;
			VerboseDebug.Log("Set face rotation to " + value);
			faceText.transform.localEulerAngles = new Vector3(0f, 0f, num);
		}
	}

	public Hat SelectedHat
	{
		get
		{
			return selectedHat;
		}
		set
		{
			if (value == selectedHat)
			{
				if (value == null)
				{
					hatNameText.text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.NoHat);
				}
				else
				{
					hatNameText.text = value.GetName();
				}
			}
			else
			{
				Debug.Log($"calling RPCA_PickHat to select {value}");
				view_g.RPC("RPCA_PickHat", RpcTarget.All, (value != null) ? HatDatabase.instance.GetIndexOfHat(value) : (-1));
				rotateSound.Play(base.transform.position);
			}
		}
	}

	public float FaceSize
	{
		get
		{
			return faceText.transform.localScale.x;
		}
		set
		{
			faceText.transform.localScale = new Vector3(value, value, 1f);
		}
	}

	private void Awake()
	{
		view_g = GetComponent<PhotonView>();
		applyButton.onClick.AddListener(OnApply);
		quitButton.onClick.AddListener(OnQuit);
		rotateLeftButton.onClick.AddListener(delegate
		{
			OnRotate(right: false);
		});
		rotateRightButton.onClick.AddListener(delegate
		{
			OnRotate(right: true);
		});
		smallerFaceButton.onClick.AddListener(delegate
		{
			OnChangeFaceSize(smaller: true);
		});
		biggerFaceButton.onClick.AddListener(delegate
		{
			OnChangeFaceSize(smaller: false);
		});
		leftHatButton.onClick.AddListener(delegate
		{
			OnChangeHat(right: false);
		});
		rightHatButton.onClick.AddListener(delegate
		{
			OnChangeHat(right: true);
		});
		clearHatButton.onClick.AddListener(delegate
		{
			SelectedHat = null;
		});
		pastButton.onClick.AddListener(delegate
		{
			Debug.Log("incopy " + GUIUtility.systemCopyBuffer);
			typeSound.Play(base.transform.position);
			view_g.RPC("RCP_SetFaceText", RpcTarget.All, GUIUtility.systemCopyBuffer);
		});
		SpawnColors();
		glyphIcon.SetActive(value: false);
	}

	private void Update()
	{
		if (playerInTerminal != null && playerInTerminal.refs.view.IsMine)
		{
			RunTerminal();
		}
		else if (m_navigationActive)
		{
			DisableNavigation();
		}
		else if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(base.transform))
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}

	private void EnableNavigation()
	{
		m_navigationActive = true;
		Button[] componentsInChildren = GetComponentsInChildren<Button>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].navigation = new Navigation
			{
				mode = Navigation.Mode.Automatic,
				wrapAround = false
			};
		}
		if (PlatformManager.Platform is SteamRuntimeManager steamRuntimeManager && (steamRuntimeManager.UsingBigPictureMode || steamRuntimeManager.OnSteamDeck))
		{
			glyphIcon.SetActive(InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad);
		}
	}

	private void DisableNavigation()
	{
		m_navigationActive = false;
		Button[] componentsInChildren = GetComponentsInChildren<Button>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].navigation = new Navigation
			{
				mode = Navigation.Mode.None,
				wrapAround = false
			};
		}
		if (PlatformManager.Platform is SteamRuntimeManager steamRuntimeManager)
		{
			steamRuntimeManager.CloseDialog();
			glyphIcon.SetActive(value: false);
		}
	}

	private void EnableInteraction()
	{
		Button[] componentsInChildren = GetComponentsInChildren<Button>();
		foreach (Button obj in componentsInChildren)
		{
			obj.navigation = new Navigation
			{
				mode = Navigation.Mode.Automatic,
				wrapAround = false
			};
			obj.interactable = true;
		}
	}

	private void DisableInteraction()
	{
		Button[] componentsInChildren = GetComponentsInChildren<Button>();
		foreach (Button obj in componentsInChildren)
		{
			obj.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
			obj.interactable = false;
		}
	}

	private void OnDestroy()
	{
		applyButton.onClick.RemoveListener(OnApply);
		quitButton.onClick.RemoveListener(OnQuit);
		smallerFaceButton.onClick.RemoveListener(delegate
		{
			OnChangeFaceSize(smaller: true);
		});
		biggerFaceButton.onClick.RemoveListener(delegate
		{
			OnChangeFaceSize(smaller: false);
		});
		leftHatButton.onClick.RemoveListener(delegate
		{
			OnChangeHat(right: false);
		});
		rightHatButton.onClick.RemoveListener(delegate
		{
			OnChangeHat(right: true);
		});
	}

	private void RemoveNavigation()
	{
		Selectable[] componentsInChildren = GetComponentsInChildren<Selectable>();
		foreach (Selectable obj in componentsInChildren)
		{
			obj.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
			PExt.SaveObj(obj);
		}
	}

	public void OnChangeHat(bool right)
	{
		VerboseDebug.Log("OnChangeHat " + right);
		if (unlockedHats.Count <= 0)
		{
			return;
		}
		if (SelectedHat == null)
		{
			VerboseDebug.Log("SelectedHat == null");
			Hat arg = HatDatabase.instance.hats[unlockedHats[0]];
			VerboseDebug.Log($"pickin first hat {unlockedHats[0]} , {arg}");
			SelectedHat = arg;
			return;
		}
		int num = unlockedHats.IndexOf(HatDatabase.instance.GetIndexOfHat(SelectedHat));
		int num2 = num;
		num += (right ? 1 : (-1));
		num = num.PLoopMe(0, unlockedHats.Count);
		if (num2 != num)
		{
			SelectedHat = HatDatabase.instance.hats[unlockedHats[num]];
		}
	}

	public void OnChangeFaceSize(bool smaller)
	{
		view_g.RPC("RPCA_ChangeFaceSize", RpcTarget.All, smaller);
		sizeSound.Play(base.transform.position);
	}

	[PunRPC]
	public void RPCA_ChangeFaceSize(bool smaller)
	{
		if (!(playerInTerminal == null))
		{
			float x = faceText.transform.localScale.x;
			float num = (faceSizeMinMax.y - faceSizeMinMax.x) / (float)faceSizeStepCount;
			float value = x + (smaller ? (0f - num) : num);
			value = (FaceSize = Mathf.Clamp(value, faceSizeMinMax.x, faceSizeMinMax.y));
			float faceSize = FaceSizeUiToVisor(value);
			playerInTerminal.refs.visor.FaceSize = faceSize;
		}
	}

	public float FaceSizeVisorToUi(float visorSize)
	{
		float t = Mathf.InverseLerp(visorFaceSizeMinMax.x, visorFaceSizeMinMax.y, visorSize);
		return Mathf.Lerp(faceSizeMinMax.x, faceSizeMinMax.y, t);
	}

	public float FaceSizeUiToVisor(float uiSize)
	{
		float t = Mathf.InverseLerp(faceSizeMinMax.x, faceSizeMinMax.y, uiSize);
		return Mathf.Lerp(visorFaceSizeMinMax.x, visorFaceSizeMinMax.y, t);
	}

	public void OnRotate(bool right)
	{
		VerboseDebug.Log("OnRotate " + right);
		view_g.RPC("RPCA_RotateFaceText", RpcTarget.All, right);
		rotateSound.Play(base.transform.position);
	}

	[PunRPC]
	public void RPCA_RotateFaceText(bool right)
	{
		if (!(playerInTerminal == null))
		{
			FaceRotation += (right ? 90 : (-90));
			playerInTerminal.refs.visor.FaceRotation = faceText.transform.localEulerAngles.z;
		}
	}

	private void OnApply()
	{
		InputHandler.AddInputBlock();
		view_g.RPC("RPCA_PlayerLeftTerminal", RpcTarget.All, true);
		applySound.Play(base.transform.position);
		PlatformManager.UnlockAchievement(Achievements.ACH_FACE);
	}

	private void OnQuit()
	{
		InputHandler.AddInputBlock();
		view_g.RPC("RPCA_PlayerLeftTerminal", RpcTarget.All, false);
		leaveSound.Play(base.transform.position);
	}

	private void RunTerminal()
	{
		if (InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad && !Singleton<EscapeMenu>.Instance.Open)
		{
			if (!m_navigationActive)
			{
				EnableNavigation();
			}
			if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.transform.IsChildOf(base.transform))
			{
				EventSystem.current.SetSelectedGameObject(applyButton.gameObject);
			}
		}
		else
		{
			if (m_navigationActive)
			{
				DisableNavigation();
			}
			if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(base.transform))
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
		}
		if (m_exitButton.action.WasPressedThisFrame())
		{
			backSound.Play(base.transform.position);
			view_g.RPC("RPCA_PlayerLeftTerminal", RpcTarget.All, false);
		}
		else if (PlatformManager.Platform is SteamRuntimeManager steamRuntimeManager && (steamRuntimeManager.UsingBigPictureMode || steamRuntimeManager.OnSteamDeck) && InputHandler.GetCurrentUsedInputScheme() == InputScheme.Gamepad)
		{
			TypeWithGamepad();
		}
		else
		{
			TypeWithKeyboard();
		}
	}

	private void TypeWithKeyboard()
	{
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			backSound.Play(base.transform.position);
			VerboseDebug.Log("Backspace + ");
			if (faceText.text.Length != 0)
			{
				string text = faceText.text.Substring(0, faceText.text.Length - 1);
				VerboseDebug.Log("Backspace " + text);
				view_g.RPC("RCP_SetFaceText", RpcTarget.All, text);
			}
		}
		else
		{
			if (faceText.text.Length >= 3)
			{
				return;
			}
			string inputString = Input.inputString;
			if (inputString.Length >= 1)
			{
				if (inputString.Length > 0)
				{
					typeSound.Play(base.transform.position);
				}
				view_g.RPC("RCP_SetFaceText", RpcTarget.All, faceText.text + inputString[0]);
			}
		}
	}

	private void TypeWithGamepad()
	{
		if (!m_openKeyboardButton.action.WasPressedThisFrame() || m_isTyping)
		{
			return;
		}
		m_isTyping = true;
		DisableInteraction();
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.TypeAFace);
		PlatformManager.Platform.OpenDialog(localizedString, DialogType.Console, delegate(string input, DialogueResult result)
		{
			if (result == DialogueResult.Succeeded)
			{
				view_g.RPC("RCP_SetFaceText", RpcTarget.All, input);
			}
			EnableInteraction();
			m_isTyping = false;
		});
	}

	[PunRPC]
	private void RCP_SetFaceText(string text)
	{
		Debug.Log("FaceText Arrived in RPC: " + text);
		if ((bool)playerInTerminal)
		{
			playerInTerminal.refs.visor.SetVisorText(text, delegate(string result)
			{
				Debug.Log("Set Visor FaceText: " + result);
				faceText.text = result;
			});
		}
	}

	[PunRPC]
	public void RPCA_PickColor(int childNumber)
	{
		VerboseDebug.Log("RPCA_PickColor");
		ColorSelector component = colorsRoot.transform.GetChild(childNumber).GetComponent<ColorSelector>();
		if (selectedColor != null)
		{
			selectedColor.UnSelect();
		}
		selectedColor = component;
		selectedColor.Select();
		headColor.color = selectedColor.color;
		if (!(playerInTerminal == null))
		{
			playerInTerminal.refs.visor.ApplyVisorColor(headColor.color);
			playerInTerminal.refs.visor.visorColorIndex = SelectedColor.transform.GetSiblingIndex();
			clickSound.Play(base.transform.position);
		}
	}

	public void EnterTerminal(PhotonView playerView)
	{
		VerboseDebug.Log("EnterTerminal " + playerView);
		view_g.RPC("RPCM_RequestEnterTerminal", RpcTarget.MasterClient, playerView.ViewID);
		enterSound.Play(base.transform.position);
	}

	[PunRPC]
	public void RPCM_RequestEnterTerminal(int playerId)
	{
		VerboseDebug.Log("RPCM_RequestEnterTerminal " + playerId + "playerinTermainal " + playerInTerminal);
		if (playerInTerminal == null)
		{
			view_g.RPC("RPCA_EnterTerminal", RpcTarget.All, playerId);
		}
	}

	[PunRPC]
	public void RPCA_EnterTerminal(int playerId)
	{
		if (!(playerInTerminal != null))
		{
			VerboseDebug.Log("RPCA_EnterTerminal");
			playerInTerminal = PlayerHandler.instance.TryGetPlayerFromViewID(playerId);
			if (playerInTerminal.refs.view.IsMine)
			{
				playerInTerminal.data.isInCostomizeTerminal = true;
				unlockedHats = MetaProgressionHandler.GetUnlockedHats().ToList();
				MetaProgressionHandler.CheckIfUnlockedAllHats();
				VerboseDebug.Log($"unlocked {unlockedHats.Count} hats");
			}
			startColorIndex = playerInTerminal.refs.visor.visorColorIndex;
			startHeadColor = playerInTerminal.refs.visor.visorColor.Value;
			if (playerInTerminal.data.currentHat != null)
			{
				hatAtStart = HatDatabase.instance.hats[playerInTerminal.data.currentHat.runtimeHatIndex];
				SelectedHat = hatAtStart;
			}
			else
			{
				hatAtStart = null;
				SelectedHat = null;
			}
			if (startColorIndex < 0)
			{
				startColorIndex = 0;
			}
			int childCount = colorsRoot.transform.childCount;
			if (startColorIndex >= childCount)
			{
				startColorIndex = 0;
			}
			SelectedColor = colorsRoot.transform.GetChild(startColorIndex).GetComponent<ColorSelector>();
			startFaceText = playerInTerminal.refs.visor.visorFaceText.text;
			faceText.text = startFaceText;
			float num = FaceSizeVisorToUi(playerInTerminal.refs.visor.visorFaceText.transform.localScale.x);
			startFaceSize = num;
			FaceSize = startFaceSize;
			startFaceRotation = playerInTerminal.refs.visor.FaceRotation;
			FaceRotation = startFaceRotation;
			if (playerInTerminal.refs.visor.visorColorIndex < 0)
			{
				SelectedColor = colorsRoot.transform.GetChild(0).GetComponent<ColorSelector>();
			}
		}
	}

	[PunRPC]
	public void RPCA_PickHat(int hatIndex)
	{
		if (!(playerInTerminal == null))
		{
			selectedHat = ((hatIndex >= 0) ? HatDatabase.instance.hats[hatIndex] : null);
			playerInTerminal.RPCA_EquipHat(hatIndex);
			hatNameText.text = ((selectedHat != null) ? selectedHat.GetName() : LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.NoHat));
		}
	}

	[PunRPC]
	public void RPCA_SyncEverything(int playerId, int colorIndex, string faceText, float faceRotation, float faceSize, int hatIndex)
	{
		if (PlayerHandler.instance.TryGetPlayerFromOwnerID(playerId, out var o))
		{
			ColorSelector component = colorsRoot.transform.GetChild(colorIndex).GetComponent<ColorSelector>();
			if (IsBlocked(o))
			{
				faceText = ":)";
				faceRotation = 270f;
			}
			else
			{
				float faceSize2 = FaceSizeUiToVisor(faceSize);
				o.refs.visor.SetAllFaceSettings(PlayerVisor.GetHueFromColor(component.color), colorIndex, faceText, faceRotation, faceSize2);
			}
			this.faceText.text = faceText;
			FaceSize = faceSize;
			FaceRotation = faceRotation;
			SelectedColor = component;
			SelectedHat = ((hatIndex >= 0) ? HatDatabase.instance.hats[hatIndex] : null);
		}
	}

	private bool IsBlocked(Player player)
	{
		if (player.TryGetGlobalPlayerData(out var d))
		{
			return d.isBlocked;
		}
		return true;
	}

	[PunRPC]
	public void RPCA_PlayerLeftTerminal(bool apply)
	{
		if (playerInTerminal == null)
		{
			return;
		}
		VerboseDebug.Log("RPCA_PlayerLeftTerminal");
		playerInTerminal.refs.visor.ApplyVisorColor(apply ? headColor.color : startHeadColor);
		playerInTerminal.refs.visor.visorColorIndex = (apply ? SelectedColor.transform.GetSiblingIndex() : startColorIndex);
		playerInTerminal.refs.visor.visorFaceText.text = (apply ? faceText.text : startFaceText);
		if (!apply)
		{
			SelectedColor = colorsRoot.transform.GetChild(startColorIndex).GetComponent<ColorSelector>();
			faceText.text = startFaceText;
			FaceSize = startFaceSize;
			FaceRotation = startFaceRotation;
			SelectedHat = hatAtStart;
		}
		if (playerInTerminal.refs.view.IsMine)
		{
			playerInTerminal.data.isInCostomizeTerminal = false;
			if (apply)
			{
				playerInTerminal.refs.visor.SaveFaceToPlayerPrefs();
			}
			view_g.RPC("RPCA_SyncEverything", RpcTarget.All, playerInTerminal.refs.view.OwnerActorNr, SelectedColor.transform.GetSiblingIndex(), faceText.text, FaceRotation, FaceSize, (SelectedHat != null) ? HatDatabase.instance.GetIndexOfHat(SelectedHat) : (-1));
		}
		playerInTerminal = null;
	}

	public void SpawnColors()
	{
		VerboseDebug.Log("SpawnColors");
		colorsRoot.transform.KillAllChildren(destroyImmediate: true);
		foreach (Color item in colorsToPickFrom)
		{
			Object.Instantiate(colorSelectorPrefab, colorsRoot.transform).GetComponent<ColorSelector>().color = item;
		}
	}
}
