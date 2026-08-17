using UnityEngine;

[CreateAssetMenu(fileName = "PenguinPullRope", menuName = "Actions/PullRope", order = 1)]
public class PullingRopeAction : PenguinAction
{
	protected float _originalDst;

	[SerializeField]
	protected float _pullRopeTime;

	protected float _pullRopeTimeAux;

	public override void Initialize()
	{
		base.Initialize();
		_originalDst = Controller.DistanceJoint2D.distance;
	}

	public override void Enter()
	{
		base.Enter();
		Controller.SetPlayerMobility(value: false);
		_pullRopeTimeAux = _pullRopeTime;
	}

	public override void LogicUpdate()
	{
		base.LogicUpdate();
		if (Controller.DistanceJoint2D.distance > 0.05f)
		{
			Controller.DistanceJoint2D.distance -= Time.deltaTime;
		}
		else
		{
			Vector3 position = Controller.CompanionTransform.position;
			position.y += 0.1f;
			Controller.CompanionTransform.position = position;
		}
		if (_controllerTransform.position.y > Controller.CompanionTransform.position.y)
		{
			Controller.CompanionTransform.position += new Vector3(0f, Time.deltaTime, 0f);
		}
		if (_pullRopeTimeAux <= 0f)
		{
			Controller.CompanionTransform.position = _controllerTransform.position;
		}
		else
		{
			_pullRopeTimeAux -= Time.deltaTime;
		}
	}

	public override void Exit()
	{
		base.Exit();
		Controller.SetPlayerMobility(value: true);
		Controller.DistanceJoint2D.distance = _originalDst;
	}
}
