using System;
using System.Collections.Generic;

namespace Features.LevelGatesModule.Scripts
{
	public class LevelGatesModel
	{
		private readonly List<IGate> _levelGates = new List<IGate>();

		private readonly List<IGate> _exitGates = new List<IGate>();

		private bool _isLocalPlayerInsideGate;

		public IReadOnlyList<IGate> LevelGates => _levelGates;

		public IReadOnlyList<IGate> ExitGates => _exitGates;

		public bool IsLocalPlayerInsideGate
		{
			get
			{
				return _isLocalPlayerInsideGate;
			}
			set
			{
				bool isLocalPlayerInsideGate = _isLocalPlayerInsideGate;
				_isLocalPlayerInsideGate = value;
				if (isLocalPlayerInsideGate != _isLocalPlayerInsideGate)
				{
					this.OnLocalPlayerInsideGateChanged?.Invoke(_isLocalPlayerInsideGate);
				}
			}
		}

		public event Action<IGate> OnGateRegistered;

		public event Action<IGate> OnGateUnRegistered;

		public event Action<bool> OnLocalPlayerInsideGateChanged;

		public void RegisterGate(IGate levelTipGate)
		{
			_levelGates.Add(levelTipGate);
			if (levelTipGate.GateType == GateType.Exit)
			{
				_exitGates.Add(levelTipGate);
			}
			this.OnGateRegistered?.Invoke(levelTipGate);
		}

		public void UnregisterGate(IGate levelTipGate)
		{
			_levelGates.Remove(levelTipGate);
			_exitGates.Remove(levelTipGate);
			this.OnGateUnRegistered?.Invoke(levelTipGate);
		}
	}
}
