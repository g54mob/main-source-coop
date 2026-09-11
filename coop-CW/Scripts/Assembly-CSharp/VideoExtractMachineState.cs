using Zorro.Core;

public abstract class VideoExtractMachineState : StateMachineState
{
	public ExtractVideoMachine Machine { get; private set; }

	public VideoExtractMachineState(ExtractVideoMachine machine)
	{
		Machine = machine;
	}

	public virtual void Update()
	{
	}

	public virtual void FixedUpdate()
	{
	}
}
