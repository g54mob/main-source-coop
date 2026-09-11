public class ExtractMachineClosedState : VideoExtractMachineState
{
	public ExtractMachineClosedState(ExtractVideoMachine machine)
		: base(machine)
	{
	}

	public override void Enter()
	{
		base.Enter();
		base.Machine.Hatch.Close();
	}

	public override void Update()
	{
		base.Update();
	}
}
