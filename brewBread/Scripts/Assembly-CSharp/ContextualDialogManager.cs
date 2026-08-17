using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

[RequireComponent(typeof(Dialoger))]
public class ContextualDialogManager : MonoBehaviour
{
	[Serializable]
	private enum ContextualType
	{
		InOrder = 0,
		Random = 1,
		SkipFirst = 2
	}

	[Header("Texts")]
	[SerializeField]
	private ContextualDialog _contextualDialog;

	[SerializeField]
	private ContextualType _type;

	[Space(10f)]
	[Header("Logic")]
	[SerializeField]
	private DialogCollider _effectArea;

	[SerializeField]
	private LocalizeStringEvent _localizeString;

	[Space(10f)]
	[Header("Animator")]
	[SerializeField]
	private Animator _popUpAnimator;

	private Dialoger _dialoger;

	private int _count;

	private LocalizedString[] _dialogs;

	private void Awake()
	{
		_dialoger = GetComponent<Dialoger>();
		_count = 0;
		_dialogs = _contextualDialog?.Texts;
		if (_type == ContextualType.Random)
		{
			_dialogs.Shuffle();
		}
	}

	private void Start()
	{
		_effectArea.OnTriggerEnter.AddListener(ShowPopUp);
		_effectArea.OnTriggerExit.AddListener(HidePopUp);
	}

	private void Update()
	{
		if (_dialoger.CurrentState.HasFlag(Dialoger.DialogState.InDialog))
		{
			HidePopUp();
		}
	}

	private void ShowPopUp()
	{
		if (_dialoger.CurrentState.HasFlag(Dialoger.DialogState.ContextualPopUp))
		{
			return;
		}
		switch (_type)
		{
		case ContextualType.InOrder:
			ShowNextDialog(_count);
			if (_count < _dialogs.Length - 1)
			{
				_count++;
			}
			break;
		case ContextualType.Random:
			ShowNextDialog(_count);
			if (_count < _dialogs.Length - 1)
			{
				_count++;
			}
			break;
		case ContextualType.SkipFirst:
			if (_count > 0)
			{
				ShowNextDialog(_count - 1);
			}
			if (_count < _dialogs.Length)
			{
				_count++;
			}
			break;
		}
	}

	private void ShowNextDialog(int count)
	{
		_localizeString.StringReference = _dialogs[count];
		_popUpAnimator.Play("DialogPopUp");
		_dialoger.CurrentState |= Dialoger.DialogState.ContextualPopUp;
	}

	private void HidePopUp()
	{
		if (_dialoger.CurrentState.HasFlag(Dialoger.DialogState.ContextualPopUp))
		{
			_popUpAnimator.Play("DialogPopDown");
			_dialoger.CurrentState &= ~Dialoger.DialogState.ContextualPopUp;
		}
	}
}
