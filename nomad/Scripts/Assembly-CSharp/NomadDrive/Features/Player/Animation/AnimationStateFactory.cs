using NomadDrive.Features.Player.Animation.States;
using NomadDrive.Features.Player.PlayerStateMachine;

namespace NomadDrive.Features.Player.Animation
{
	public class AnimationStateFactory
	{
		public AnimationState CreateState(PlayerState state)
		{
			return state switch
			{
				PlayerState.Idle => new IdleState(), 
				PlayerState.Walk => new WalkState(), 
				PlayerState.Sprint => new SprintState(), 
				PlayerState.CrouchedIdle => new CrouchedIdleState(), 
				PlayerState.CrouchedWalk => new CrouchedWalkState(), 
				PlayerState.CrouchedSprint => new CrouchedSprintState(), 
				PlayerState.Sit => new SittingState(), 
				PlayerState.Jump => new JumpState(), 
				PlayerState.Falling => new FallingState(), 
				PlayerState.Landing => new LandingState(), 
				PlayerState.Laying => new LayingState(), 
				PlayerState.DownedLying => new DownedLyingState(), 
				PlayerState.DownedSitting => new DownedSittingState(), 
				_ => new IdleState(), 
			};
		}
	}
}
