using System.Collections.Generic;
using Febucci.UI;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

[RequireComponent(typeof(Dialoger))]
public class DialogManager : MonoBehaviour
{
	[Header("Dialogs")]
	[SerializeField]
	private List<NPCDialog> _dialogBlocks = new List<NPCDialog>();

	[Space(10f)]
	[Header("Text Components")]
	[SerializeField]
	private LocalizeStringEvent _localizeString;

	[SerializeField]
	private TextAnimatorPlayer _textAnimator;

	[SerializeField]
	private TextMeshPro _textMeshPro;

	[SerializeField]
	private Animator _portrait;

	[SerializeField]
	private Animator _popUpDialog;

	[Space(10f)]
	[Header("Penguin Interactions")]
	[SerializeField]
	private SpriteRenderer _interactSprite;

	[SerializeField]
	private DialogCollider _effectArea;

	[Space(10f)]
	[Header("Zoom")]
	[SerializeField]
	private float _zoomSize;

	[SerializeField]
	private float _zoomTime;

	private int _currentBlockIndex;

	private int _currentDialogIndex;

	private Player _playerUI;

	private Player _player1;

	private Player _player2;

	private Player _player3;

	private bool _textShown;

	private Dialoger _dialoger;

	private NPCDialog _currentDialogBlock;

	private Dialog _currentDialog;

	private void Awake()
	{
		_playerUI = ReInput.players.GetPlayer(3);
		_player1 = ReInput.players.GetPlayer(0);
		_player2 = ReInput.players.GetPlayer(1);
		_player3 = ReInput.players.GetPlayer(2);
		_popUpDialog.transform.localScale = Vector2.zero;
		_dialoger = GetComponent<Dialoger>();
	}

	private void Start()
	{
		_effectArea.OnTriggerEnter.AddListener(InRange);
		_effectArea.OnTriggerExit.AddListener(OutOfRange);
		_interactSprite.enabled = false;
		_currentBlockIndex = 0;
		_currentDialogIndex = 0;
		_dialoger.CurrentState |= Dialoger.DialogState.DialogEnabled;
		_textAnimator.onTextShowed.AddListener(TextShown);
	}

	private void OnDestroy()
	{
		_textAnimator.onTextShowed.RemoveListener(TextShown);
	}

	private void Update()
	{
		if (CheckStartDialog())
		{
			CheckAction();
		}
	}

	private void CheckAction()
	{
		if (_playerUI.GetButtonDown(18))
		{
			SkipDialog();
		}
	}

	private bool CheckStartDialog()
	{
		if (!_dialoger.CurrentState.HasFlag(Dialoger.DialogState.InRange))
		{
			return false;
		}
		if (_dialoger.CurrentState.HasFlag(Dialoger.DialogState.InDialog))
		{
			return true;
		}
		if (_player1.GetButtonDown(21) || _player2.GetButtonDown(21) || _player3.GetButtonDown(21))
		{
			StartDialog();
		}
		return false;
	}

	private void StartDialog()
	{
		if (_currentBlockIndex <= _dialogBlocks.Count - 1)
		{
			StaticInstance<Pause>.Instance.SetEnablePause(v: false);
			_currentDialogIndex = 0;
			_localizeString.StringReference = _dialogBlocks[_currentBlockIndex].Dialogs[_currentDialogIndex].Text;
			_dialoger.CurrentState |= Dialoger.DialogState.InDialog;
			HideIcon();
			StaticInstance<Bread>.Instance.DisableInput();
			StaticInstance<Fred>.Instance.DisableInput();
			_popUpDialog.Play("DialogAppear");
			_portrait.Play(_dialogBlocks[_currentBlockIndex].Dialogs[_currentDialogIndex].TalkExpresion.name);
			StaticInstance<CameraController>.Instance.CameraZoom(_zoomSize, _zoomTime);
		}
	}

	private void EndDialog()
	{
		StaticInstance<Pause>.Instance.SetEnablePause(v: true);
		_currentBlockIndex++;
		if (_currentBlockIndex >= _dialogBlocks.Count)
		{
			HideIcon();
			_dialoger.CurrentState &= ~Dialoger.DialogState.DialogEnabled;
		}
		else
		{
			ShowIcon();
		}
		_dialoger.CurrentState &= ~Dialoger.DialogState.InDialog;
		StaticInstance<Bread>.Instance.EnableInput();
		StaticInstance<Fred>.Instance.EnableInput();
		_popUpDialog.Play("DialogDisappear");
		StaticInstance<CameraController>.Instance.RestoreCameraZoom(_zoomTime);
	}

	private void SkipDialog()
	{
		if (!_textShown)
		{
			_textAnimator.SkipTypewriter();
		}
		else
		{
			NextDialog();
		}
	}

	private void NextDialog()
	{
		_textShown = false;
		_currentDialogIndex++;
		if (_currentDialogIndex >= _dialogBlocks[_currentBlockIndex].Dialogs.Count)
		{
			EndDialog();
			return;
		}
		_portrait.Play(_dialogBlocks[_currentBlockIndex].Dialogs[_currentDialogIndex].TalkExpresion.name);
		_localizeString.StringReference = _dialogBlocks[_currentBlockIndex].Dialogs[_currentDialogIndex].Text;
	}

	private void InRange()
	{
		if (_dialoger.CurrentState.HasFlag(Dialoger.DialogState.DialogEnabled))
		{
			_dialoger.CurrentState |= Dialoger.DialogState.InRange;
			ShowIcon();
		}
	}

	private void OutOfRange()
	{
		_dialoger.CurrentState &= ~Dialoger.DialogState.InRange;
		HideIcon();
	}

	private void ShowIcon()
	{
		_interactSprite.enabled = true;
	}

	private void HideIcon()
	{
		_interactSprite.enabled = false;
	}

	private void TextShown()
	{
		_portrait.Play(_dialogBlocks[_currentBlockIndex].Dialogs[_currentDialogIndex].IdleExpresion.name);
		_textShown = true;
	}
}
