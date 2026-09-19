using Fusion;
using UnityEngine;

namespace Features.CartUpgradesModule.Scripts.Core
{
	public interface ICartTierCarrier
	{
		int Tier { get; }

		NetworkObject NetworkObject { get; }

		Transform Transform { get; }
	}
}
