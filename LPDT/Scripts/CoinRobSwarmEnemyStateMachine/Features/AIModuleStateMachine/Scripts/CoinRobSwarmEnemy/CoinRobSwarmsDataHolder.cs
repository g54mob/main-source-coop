using System.Collections.Generic;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy
{
	public class CoinRobSwarmsDataHolder
	{
		public List<ICoinRobSwarmHost> ActiveCoinRobSwarms { get; } = new List<ICoinRobSwarmHost>();
	}
}
