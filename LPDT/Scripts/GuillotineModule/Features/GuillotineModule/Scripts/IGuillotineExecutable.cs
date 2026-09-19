using Fusion;
using UnityEngine;

namespace Features.GuillotineModule.Scripts
{
	public interface IGuillotineExecutable
	{
		NetworkObject NetworkObject { get; }

		bool ShouldQuitExecution { get; set; }

		bool IsRuntime { get; }

		bool CanBeExecuted { get; }

		void PrepareForExecution(Transform poseBlueprint);

		void Execute();

		void QuitExecution();

		void PlayExecutionSound();
	}
}
