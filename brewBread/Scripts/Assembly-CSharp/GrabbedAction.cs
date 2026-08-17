using UnityEngine;

[CreateAssetMenu(fileName = "Grabbed", menuName = "Actions/Grabbed", order = 1)]
public class GrabbedAction : PenguinAction
{
	[SerializeField]
	private AudioClip _launchClip;

	private AudioSource _audioSource;

	private float _prevSpeed;

	public override void Initialize()
	{
		base.Initialize();
		_audioSource = Controller.GetComponent<AudioSource>();
	}

	public override void Enter()
	{
		base.Enter();
		_prevSpeed = Controller.Companion.Speed;
		Controller.Companion.Speed = _prevSpeed / 5f;
		Controller.SetPlayerMobility(value: false);
		Controller.Companion.EnableToJump = false;
		Controller.Companion.SetAnimationInt("WalkState", 3);
		Controller.Companion.SetAnimationInt("IdleState", 3);
	}

	public override void LogicUpdate()
	{
		Controller.Companion.SetAnimationInt("IdleState", 3);
		base.LogicUpdate();
	}

	public override void Exit()
	{
		base.Exit();
		Controller.SetPlayerMobility(value: true);
		Controller.Companion.Speed = _prevSpeed;
		Controller.Companion.EnableToJump = true;
		_controllerTransform.localScale = Controller.CompanionTransform.localScale;
		Controller.Companion.SetAnimationInt("WalkState", 0);
		Controller.Companion.SetAnimationInt("IdleState", 0);
		_audioSource.PlayOneShot(_launchClip);
	}
}
