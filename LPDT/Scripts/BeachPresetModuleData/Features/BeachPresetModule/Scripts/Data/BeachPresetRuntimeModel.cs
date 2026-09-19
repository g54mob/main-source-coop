using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Data
{
	public class BeachPresetRuntimeModel
	{
		private Transform _runtimeAnchor;

		public Transform RuntimeAnchor => _runtimeAnchor;

		public void RegisterRuntimeAnchor(Transform runtimeAnchor)
		{
			_runtimeAnchor = runtimeAnchor;
		}

		public void ClearRuntimeAnchor(Transform runtimeAnchor)
		{
			if (_runtimeAnchor == runtimeAnchor)
			{
				_runtimeAnchor = null;
			}
		}
	}
}
