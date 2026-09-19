using Features.WorldTokenModule.Scripts.Views;
using UnityEngine;

namespace Features.WorldTokenModule.Scripts
{
	public interface IWorldTokenService
	{
		bool IsReady { get; }

		WorldTokenPresenter CreateWorldToken(WorldTokenType worldTokenType, Vector3 position);

		BigButtWorldTokenPresenter CreateBigButtWWorldToken(WorldTokenType worldTokenType, Vector3 position);
	}
}
