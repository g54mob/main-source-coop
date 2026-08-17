using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharlieTutorialController : MonoBehaviour
{
	[Serializable]
	private struct TutorialText
	{
		public GameObject _animation;

		public TextMeshProUGUI[] _texts;

		public TextAnimatorPlayer[] _textAnimator;

		public GameObject _inputs;
	}

	[SerializeField]
	private Animator _charlieAnimator;

	[SerializeField]
	private GameObject _canvasObject;

	private Canvas _canvas;

	[SerializeField]
	private List<TutorialText> _dialogs;

	private int _currentDialog;

	private int _currentText;

	private Animator _animator;

	private bool _tutorialShown;

	private Player _player;

	[SerializeField]
	private Image _circleLoading;

	private void Start()
	{
		_canvasObject.SetActive(value: false);
		_animator = GetComponent<Animator>();
		_canvas = _canvasObject.GetComponent<Canvas>();
		_tutorialShown = false;
		_player = ReInput.players.GetPlayer(3);
		_player.AddInputEventDelegate(Action, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, 18);
		_player.AddInputEventDelegate(SkipTutorial, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, 19);
		_player.AddInputEventDelegate(CancelSkiping, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased, 19);
		_currentDialog = -1;
		_currentText = -1;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 9)
		{
			StartTutorial();
		}
	}

	private void StartTutorial()
	{
		if (!_tutorialShown)
		{
			StaticInstance<Pause>.Instance.SetEnablePause(v: false);
			StaticInstance<CameraController>.Instance.SetBlur(value: true);
			_animator.SetTrigger("Start");
			_tutorialShown = true;
			StaticInstance<Bread>.Instance.DisableInput();
			StaticInstance<Fred>.Instance.DisableInput();
			_canvasObject.SetActive(value: true);
			NextDialog();
			StaticInstance<GameManager>.Instance.SetBlur(value: true);
		}
	}

	private void EndTutorial()
	{
		_animator.SetTrigger("End");
		_player.RemoveInputEventDelegate(CancelSkiping);
		_player.RemoveInputEventDelegate(SkipTutorial);
		_player.RemoveInputEventDelegate(Action);
		if (_currentDialog > 0)
		{
			if ((bool)_dialogs[_currentDialog - 1]._animation)
			{
				_dialogs[_currentDialog - 1]._animation.SetActive(value: false);
			}
		}
		else if ((bool)_dialogs[0]._animation)
		{
			_dialogs[0]._animation.SetActive(value: false);
		}
		StaticInstance<Bread>.Instance.EnableInput();
		StaticInstance<Fred>.Instance.EnableInput();
		StaticInstance<CameraController>.Instance.SetBlur(value: false);
	}

	private void Action(InputActionEventData obj)
	{
		if (_tutorialShown)
		{
			if (_currentText <= _dialogs[_currentDialog]._texts.Length - 1)
			{
				_dialogs[_currentDialog]._textAnimator[_currentText].SkipTypewriter();
				NextText();
			}
			else
			{
				NextDialog();
			}
		}
	}

	private void NextDialog()
	{
		GameObject gameObject = null;
		if (_currentDialog >= 0)
		{
			TextMeshProUGUI[] texts = _dialogs[_currentDialog]._texts;
			for (int i = 0; i < texts.Length; i++)
			{
				texts[i].gameObject.SetActive(value: false);
			}
			if (_dialogs[_currentDialog]._inputs != null)
			{
				_dialogs[_currentDialog]._inputs.SetActive(value: false);
			}
			gameObject = _dialogs[_currentDialog]._animation;
		}
		_currentDialog++;
		_currentText = -1;
		if (_currentDialog >= _dialogs.Count)
		{
			EndTutorial();
			return;
		}
		NextText();
		if (_dialogs[_currentDialog]._animation != gameObject)
		{
			if ((bool)gameObject)
			{
				gameObject.SetActive(value: false);
			}
			if ((bool)_dialogs[_currentDialog]._animation)
			{
				_dialogs[_currentDialog]._animation.SetActive(value: true);
			}
		}
	}

	private void NextText()
	{
		_currentText++;
		if (_currentText > 0)
		{
			_dialogs[_currentDialog]._textAnimator[_currentText - 1].onTextShowed.RemoveAllListeners();
			if (_dialogs[_currentDialog]._inputs != null)
			{
				_dialogs[_currentDialog]._inputs.SetActive(value: true);
			}
		}
		if (_currentText < _dialogs[_currentDialog]._texts.Length)
		{
			_dialogs[_currentDialog]._texts[_currentText].gameObject.SetActive(value: true);
			_dialogs[_currentDialog]._textAnimator[_currentText].onTextShowed.AddListener(NextText);
			_dialogs[_currentDialog]._textAnimator[_currentText].ShowText(_dialogs[_currentDialog]._texts[_currentText].text);
		}
	}

	public void SkipTutorial(InputActionEventData obj)
	{
		if (_tutorialShown)
		{
			StopAllCoroutines();
			StartCoroutine(SkipHold());
		}
	}

	private void CancelSkiping(InputActionEventData obj)
	{
		if (_tutorialShown)
		{
			StopAllCoroutines();
			StartCoroutine(ReleaseHold());
		}
	}

	private IEnumerator ReleaseHold()
	{
		float t = 1f - _circleLoading.fillAmount;
		while (!(t > 1f))
		{
			_circleLoading.fillAmount = Mathf.Lerp(1f, 0f, t);
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		_circleLoading.fillAmount = Mathf.Lerp(1f, 0f, 1f);
	}

	private IEnumerator SkipHold()
	{
		float t = _circleLoading.fillAmount;
		while (!(t > 1f))
		{
			_circleLoading.fillAmount = Mathf.Lerp(0f, 1f, t);
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		_circleLoading.fillAmount = Mathf.Lerp(0f, 1f, 1f);
		EndTutorial();
	}

	public void DisableTutorial()
	{
		CharlieLeaveAnimation();
		_canvasObject.SetActive(value: false);
		StaticInstance<Pause>.Instance.SetEnablePause(v: true);
	}

	private void CharlieLeaveAnimation()
	{
		_charlieAnimator.SetTrigger("Leave");
		Invoke("DisableObject", 4f);
	}

	private void DisableObject()
	{
		base.gameObject.SetActive(value: false);
	}
}
