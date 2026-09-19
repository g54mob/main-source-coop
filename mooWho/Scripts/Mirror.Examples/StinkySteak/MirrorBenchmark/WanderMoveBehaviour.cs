using Mirror;
using StinkySteak.NetcodeBenchmark;
using UnityEngine;

namespace StinkySteak.MirrorBenchmark
{
	public class WanderMoveBehaviour : NetworkBehaviour
	{
		[SerializeField]
		private BehaviourConfig _config;

		private WanderMoveWrapper _wrapper;

		public override void OnStartServer()
		{
			if (!base.isClient)
			{
				_config.ApplyConfig(ref _wrapper);
				_wrapper.NetworkStart(base.transform);
			}
		}

		private void FixedUpdate()
		{
			if (!base.isClient)
			{
				_wrapper.NetworkUpdate(base.transform);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
