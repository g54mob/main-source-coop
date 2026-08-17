using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	public PenguinActionsController[] _penguins;

	[SerializeField]
	private float _blendtime;

	[SerializeField]
	private CinemachineVirtualCamera _virtualCamera;

	[SerializeField]
	private Vector2 _originalDamping;

	private PenguinActions[] _currentStates;

	private CinemachineTargetGroup.Target[] _group;

	private int _cameramove;

	private CinemachineTransposer _transposer;

	private bool _changingDamping;

	public void Initialize()
	{
		_currentStates = new PenguinActions[_penguins.Length];
		for (int i = 0; i < _penguins.Length; i++)
		{
			_currentStates[i] = _penguins[i].CurrentState.Type;
			_penguins[i].OnStateChanged.AddListener(ChangedState);
		}
		_group = GetComponent<CinemachineTargetGroup>().m_Targets;
		_transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
	}

	public void OnDestroy()
	{
		for (int i = 0; i < _penguins.Length; i++)
		{
			_penguins[i].OnStateChanged?.RemoveListener(ChangedState);
		}
	}

	private void ChangedState(PenguinActions newState, PenguinActionsController controller)
	{
		for (int i = 0; i < _penguins.Length; i++)
		{
			if (_penguins[i] == controller)
			{
				_currentStates[i] = controller.CurrentState.Type;
			}
		}
		if (newState == PenguinActions.Swing)
		{
			PlayerSwing(controller.transform);
		}
		else if (!_currentStates.Any((PenguinActions n) => n == PenguinActions.Swing))
		{
			PlayerStopSwing();
		}
		if (_currentStates.All((PenguinActions n) => n == PenguinActions.Fall || n == PenguinActions.FallHard))
		{
			if (!_changingDamping)
			{
				_changingDamping = true;
				StopCoroutine("ChangeCameraDamping");
				StartCoroutine(ChangeCameraDamping(new Vector2(_transposer.m_XDamping, _transposer.m_YDamping), Vector2.zero));
			}
		}
		else if (_changingDamping)
		{
			_changingDamping = false;
			StopCoroutine("ChangeCameraDamping");
			StartCoroutine(ChangeCameraDamping(new Vector2(_transposer.m_XDamping, _transposer.m_YDamping), _originalDamping));
		}
	}

	private void PlayerSwing(Transform transform)
	{
		if (_group[0].target == transform)
		{
			_cameramove = 0;
		}
		if (_group[1].target == transform)
		{
			_cameramove = 1;
		}
		StopCoroutine("MoveCamera");
		StartCoroutine(ChangeTarget(_group[_cameramove].weight, 0f));
	}

	private void PlayerStopSwing()
	{
		StopCoroutine("MoveCamera");
		StartCoroutine(ChangeTarget(_group[0].weight, 1f));
		StartCoroutine(ChangeTarget(_group[1].weight, 1f));
	}

	private IEnumerator ChangeTarget(float origin, float target)
	{
		float time = 0f;
		while (time <= _blendtime)
		{
			_group[_cameramove].weight = Mathf.Lerp(origin, target, time);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		_group[_cameramove].weight = target;
	}

	private IEnumerator ChangeCameraDamping(Vector2 origin, Vector2 target)
	{
		float time = 0f;
		while (time <= _blendtime)
		{
			_transposer.m_XDamping = Mathf.Lerp(origin.x, target.x, time);
			_transposer.m_YDamping = Mathf.Lerp(origin.y, target.y, time);
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		_transposer.m_XDamping = target.x;
		_transposer.m_YDamping = target.y;
	}
}
