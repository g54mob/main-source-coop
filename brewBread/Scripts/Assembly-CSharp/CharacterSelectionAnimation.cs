using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelectionAnimation : MonoBehaviour
{
	public GameObject fredJoinText;

	public GameObject breadJoinText;

	[Header("Game Start")]
	[SerializeField]
	private GameObject[] _waitingText;

	[SerializeField]
	private TextMeshProUGUI _timer;

	[Space(5f)]
	[Header("Animators")]
	[SerializeField]
	private Animator _generalAnimator;

	[Space(5f)]
	[Header("Animators")]
	[SerializeField]
	private Animator _breadAnimator;

	[Space(5f)]
	[Header("Animators")]
	[SerializeField]
	private Animator _fredAnimator;

	private bool _loading;

	public void PlayerJoined(int playerJoined)
	{
		switch (playerJoined)
		{
		case 1:
			fredJoinText.SetActive(value: true);
			_breadAnimator.Play("Appear");
			break;
		case 2:
			fredJoinText.SetActive(value: false);
			_fredAnimator.Play("Appear");
			break;
		}
	}

	public void PlayerLeaves(int playerLeft)
	{
		StopAllCoroutines();
		_timer.gameObject.SetActive(value: false);
		_loading = false;
		switch (playerLeft)
		{
		case 1:
			fredJoinText.SetActive(value: false);
			_breadAnimator.Play("Disappear");
			break;
		case 2:
			_fredAnimator.Play("Disappear");
			fredJoinText.SetActive(value: true);
			break;
		}
	}

	public void AllPlayersJoined()
	{
		if (!_loading)
		{
			_loading = true;
			StartCoroutine(ChangeScene());
		}
	}

	public void QuitCharacterSelection()
	{
		_generalAnimator.Play("CharacterSelectionDisappear");
	}

	public void UnloadScene()
	{
		StaticInstance<TransitionSystem>.Instance.UnloadScene(Scenes.CharacterSelection);
		StaticInstance<UIManager>.Instance.SendEvent(UIStateMachineEvents.PlayMenu);
	}

	public void LoadSceneAsync(string sceneName)
	{
		ScenesLoadManager.instance.LoadScene(sceneName);
		EventSystem[] array = Object.FindObjectsOfType<EventSystem>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].gameObject.scene == base.gameObject.scene)
			{
				array[i].enabled = false;
			}
		}
	}

	private IEnumerator ChangeScene()
	{
		AudioSource source = GetComponent<AudioSource>();
		for (int i = 0; i < _waitingText.Length; i++)
		{
			_waitingText[i].SetActive(value: false);
		}
		_timer.gameObject.SetActive(value: true);
		_timer.text = "3";
		source.Play();
		yield return new WaitForSeconds(1f);
		_timer.text = "2";
		source.Play();
		yield return new WaitForSeconds(1f);
		_timer.text = "1";
		source.Play();
		yield return new WaitForSeconds(1f);
		StaticInstance<UIManager>.Instance.SendEvent(UIStateMachineEvents.Game);
		yield return null;
	}
}
