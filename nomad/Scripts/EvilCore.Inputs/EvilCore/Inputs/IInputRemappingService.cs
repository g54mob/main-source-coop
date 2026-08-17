using System;
using System.Collections.Generic;
using Rewired;

namespace EvilCore.Inputs
{
	public interface IInputRemappingService
	{
		bool IsListening { get; }

		bool CanRevert { get; }

		event Action OnListeningStarted;

		event Action<InputRemappingResult> OnInputRemapped;

		event Action<InputRemappingResult> OnRemapOutcome;

		event Action OnListeningStopped;

		List<InputRemappingActionData> GetActionsForCategory(int mapCategoryId);

		Dictionary<int, List<InputRemappingActionData>> GetAllActions();

		void StartKeyboardMouseRemapping(int actionId, AxisRange axisRange, int actionElementMapToReplaceId);

		void StartJoystickRemapping(int actionId, AxisRange axisRange, int actionElementMapToReplaceId);

		void CancelListening();

		void SaveBindings();

		void LoadBindings();

		void ResetToDefaults();

		void RevertLastRemap();

		string GetCategoryDisplayName(int mapCategoryId);

		bool IsActionProtected(int actionId);
	}
}
