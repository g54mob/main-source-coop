using Features.SessionManagementModule.Models;
using Features.StoreModule.Scripts;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class ShopVotePersistenceAdapter : IShopVotePersistence
	{
		private readonly ISessionPlayerPresence _sessionPlayerPresence;

		private readonly SessionStateMachine _sessionStateMachine;

		public ShopVotePersistenceAdapter(ISessionPlayerPresence sessionPlayerPresence, SessionStateMachine sessionStateMachine)
		{
			_sessionPlayerPresence = sessionPlayerPresence;
			_sessionStateMachine = sessionStateMachine;
		}

		public void SaveVote(bool hasVoted)
		{
			_sessionPlayerPresence.SaveShopVote(_sessionStateMachine.CurrentEpoch, hasVoted);
		}

		public bool HasVoteForCurrentShop()
		{
			return _sessionPlayerPresence.HasShopVoteForEpoch(_sessionStateMachine.CurrentEpoch);
		}
	}
}
