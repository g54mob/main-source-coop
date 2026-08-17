using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : StaticInstance<CheckpointController>
{
	private Animator _animator;

	private int _animationState;

	[SerializeField]
	private List<PenguinActionsController> _penguins;

	[SerializeField]
	private GameObject _flagAssist;

	[SerializeField]
	private GameObject _targetPoint;

	[SerializeField]
	private LineRenderer _rope;

	private bool _savedCheckpointThisGame;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_animationState = 0;
	}

	public void SaveGame()
	{
		SpawnFlag();
		_savedCheckpointThisGame = true;
	}

	public void LoadGame()
	{
		if (_savedCheckpointThisGame)
		{
			StartCoroutine(LoadCheckpoint());
			for (int i = 0; i < _penguins.Count; i++)
			{
				_penguins[i].SetPlayerMobility(value: false);
			}
		}
	}

	private void SpawnFlag()
	{
		SaveSystem.SaveData(StaticInstance<TransitionSystem>.Instance.GetActiveScene() == Scenes.SinglePlayer.ToString());
		Object.Instantiate(_flagAssist, _targetPoint.transform.position, Quaternion.identity);
	}

	public void SetAnimationState(int value)
	{
		_animationState = value;
	}

	private IEnumerator LoadCheckpoint()
	{
		_penguins.ForEach(delegate(PenguinActionsController n)
		{
			n.EnableInput(value: false);
		});
		_animator.Play("PickCharacters");
		yield return new WaitUntil(() => _animationState == 1);
		StaticInstance<Bread>.Instance.SetSpriteRendererActive(value: false);
		StaticInstance<Fred>.Instance.SetSpriteRendererActive(value: false);
		_rope.enabled = false;
		yield return new WaitUntil(() => _animationState == 2);
		SaveSystem.LoadData(StaticInstance<TransitionSystem>.Instance.GetActiveScene() == Scenes.SinglePlayer.ToString());
		StaticInstance<CameraController>.Instance.GetDamping();
		StaticInstance<CameraController>.Instance.SetDamping(Vector2.zero);
		_animator.Play("ReleaseCharacters");
		yield return new WaitUntil(() => _animationState == 3);
		StaticInstance<CameraController>.Instance.SetDamping(Vector2.one);
		StaticInstance<Bread>.Instance.SetSpriteRendererActive(value: true);
		StaticInstance<Fred>.Instance.SetSpriteRendererActive(value: true);
		_rope.enabled = true;
		yield return new WaitUntil(() => _animationState == 4);
		_penguins.ForEach(delegate(PenguinActionsController n)
		{
			n.EnableInput(value: true);
		});
		_penguins.ForEach(delegate(PenguinActionsController n)
		{
			n.SetPlayerMobility(value: true);
		});
		_animationState = 0;
	}
}
