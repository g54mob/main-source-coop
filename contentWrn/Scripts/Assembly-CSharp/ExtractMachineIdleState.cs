public class ExtractMachineIdleState : VideoExtractMachineState
{
	public ExtractMachineIdleState(ExtractVideoMachine machine)
		: base(machine)
	{
	}

	public override void Enter()
	{
		base.Machine.Hatch.Open();
	}

	public override void Update()
	{
		base.Update();
		if (base.Machine.CheckForItems().Count > 0)
		{
			base.Machine.StateMachine.SwitchState<ExtractMachineCheckItemState>();
		}
	}
}
