using System;
using System.Collections.Generic;

namespace Features.PlayersEyeFocusModule.Scripts
{
	public class PlayerEyesTargetsModel
	{
		private readonly List<EyesTargetData> _eyesTargets = new List<EyesTargetData>();

		public List<EyesTargetData> EyesTargets => _eyesTargets;

		public event Action<EyesTargetData> OnEyesTargetAdded;

		public event Action<EyesTargetData> OnEyesTargetRemoved;

		public void RegisterTarget(EyesTargetData data)
		{
			_eyesTargets.Add(data);
			this.OnEyesTargetAdded?.Invoke(data);
		}

		public void UnregisterTarget(EyesTargetData data)
		{
			_eyesTargets.Remove(data);
			this.OnEyesTargetRemoved?.Invoke(data);
		}
	}
}
