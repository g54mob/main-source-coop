using UnityEngine;

public class NoneState : ConnectionState
{
	public override void Enter()
	{
		base.Enter();
		Debug.Log("Entered None State");
	}

	public override void Exit()
	{
		base.Exit();
		Debug.Log("Exited None State");
	}
}
