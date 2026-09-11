using Zorro.Core;

public class VideoExtractMachineStateMachine : StateMachine<VideoExtractMachineState>
{
	public void Update()
	{
		CurrentState.Update();
	}

	public void FixedUpdate()
	{
		CurrentState.FixedUpdate();
	}
}
