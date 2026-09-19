using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using UnityEngine;

namespace Features.StrechArmsModule.Scripts
{
	public class ArmStateModel : ISessionCleanup
	{
		private readonly Dictionary<Arm, StrechArmController> _armControllers = new Dictionary<Arm, StrechArmController>();

		private readonly Dictionary<Arm, ArmState> _armStates = new Dictionary<Arm, ArmState>();

		private readonly Dictionary<Arm, float> _armProgress = new Dictionary<Arm, float>();

		public event Action<Arm, ArmState> OnArmStateChanged;

		public event Action<Arm, float> OnArmProgressChanged;

		public event Action<Arm, bool> OnEnemyGrabbedChanged;

		public event Action<Arm, StrechArmController> OnArmControllerRegistered;

		public void RegisterArmController(Arm armOrientation, StrechArmController controller)
		{
			_armControllers[armOrientation] = controller;
			if (_armStates.TryAdd(armOrientation, ArmState.ReadyToThrow))
			{
				_armProgress[armOrientation] = 0f;
				this.OnArmControllerRegistered?.Invoke(armOrientation, controller);
			}
		}

		public void SetArmState(Arm armOrientation, ArmState state)
		{
			if (_armStates.ContainsKey(armOrientation) && _armStates[armOrientation] != state)
			{
				_armStates[armOrientation] = state;
				this.OnArmStateChanged?.Invoke(armOrientation, state);
			}
		}

		public void SetArmProgress(Arm armOrientation, float progress)
		{
			if (_armProgress.ContainsKey(armOrientation))
			{
				_armProgress[armOrientation] = Mathf.Clamp01(progress);
				this.OnArmProgressChanged?.Invoke(armOrientation, _armProgress[armOrientation]);
			}
		}

		public ArmState GetArmState(Arm armOrientation)
		{
			return _armStates.GetValueOrDefault(armOrientation, ArmState.ReadyToThrow);
		}

		public float GetArmProgress(Arm armOrientation)
		{
			return _armProgress.GetValueOrDefault(armOrientation, 0f);
		}

		public StrechArmController GetArmController(Arm armOrientation)
		{
			return _armControllers.GetValueOrDefault(armOrientation);
		}

		public void OnEnemyGrabbedChangedInvoke(Arm armOrientation, bool value)
		{
			this.OnEnemyGrabbedChanged?.Invoke(armOrientation, value);
		}

		public void Cleanup()
		{
			_armControllers.Clear();
			_armStates.Clear();
			_armProgress.Clear();
		}
	}
}
